import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import Navbar from '../components/Navbar'
import { api } from '../utils/api'
import { getUsername } from '../utils/auth'
import '../styles/Exercises.css'

export default function Exercises() {
  const { selectedDate } = useParams()
  const navigate = useNavigate()
  const username = getUsername()
  const [selectedCategories, setSelectedCategories] = useState([])
  const [workoutSessionId, setWorkoutSessionId] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [activeTab, setActiveTab] = useState(null)
  const [exerciseTemplates, setExerciseTemplates] = useState({})
  const [weightCatalog, setWeightCatalog] = useState({ dumbel: [], bar: [], machine: [] })
  const [exerciseSelections, setExerciseSelections] = useState({})
  const [addingAll, setAddingAll] = useState(false)
  const [showAlert, setShowAlert] = useState(false)
  const [alertMessage, setAlertMessage] = useState('')

  useEffect(() => {
    if (!username) {
      navigate('/login')
      return
    }

    const fetchData = async () => {
      try {
        const sessions = await api.getWorkoutSessionsByDate(selectedDate)
        if (sessions && sessions.length > 0) {
          const latestSession = sessions[0]
          const categories = sessions[0].selectedMuscleGroups
            .split(',')
            .map((c) => c.trim())
            .filter(Boolean)

          const [templates, catalog] = await Promise.all([
            api.getExerciseTemplates(categories),
            api.getWeightCatalog(),
          ])

          const groupedTemplates = templates.reduce((acc, template) => {
            if (!acc[template.muscleGroup]) {
              acc[template.muscleGroup] = []
            }

            acc[template.muscleGroup].push(template)
            return acc
          }, {})

          const initialSelections = templates.reduce((acc, template) => {
            acc[template.id] = {
              setsCount: 0,
              isSkipped: false,
              weights: [],
              activeSetIndex: 0,
            }
            return acc
          }, {})

          setWorkoutSessionId(latestSession.id)
          setSelectedCategories(categories)
          setExerciseTemplates(groupedTemplates)
          setWeightCatalog({
            dumbel: catalog.dumbel || [],
            bar: catalog.bar || [],
            machine: catalog.machine || [],
          })
          setExerciseSelections(initialSelections)
          setActiveTab(categories[0])
        } else {
          setError('No workout session found for this date. Please select workout groups first.')
        }
      } catch (err) {
        setError(err.message)
      } finally {
        setLoading(false)
      }
    }

    fetchData()
  }, [navigate, selectedDate, username])

  const handleAddExercise = (exerciseName) => {
    setAlertMessage(`${exerciseName} added successfully! ✓`)
    setShowAlert(true)
    setTimeout(() => setShowAlert(false), 2000)
  }

  const moveToNextExercise = (exerciseId) => {
    if (!activeTab) return

    const currentExercises = exerciseTemplates[activeTab] || []
    const currentIndex = currentExercises.findIndex((exercise) => exercise.id === exerciseId)
    const nextExercise = currentExercises[currentIndex + 1]

    if (nextExercise) {
      const nextCard = document.getElementById(`exercise-card-${nextExercise.id}`)
      if (nextCard) {
        nextCard.scrollIntoView({ behavior: 'smooth', block: 'center' })
      }
    }
  }

  const chooseSetPlan = (exerciseId, nextCount) => {
    setExerciseSelections((prev) => {
      const current = prev[exerciseId] || { setsCount: 0, isSkipped: false, weights: [], activeSetIndex: 0 }

      if (nextCount === 0) {
        return {
          ...prev,
          [exerciseId]: {
            ...current,
            setsCount: 0,
            isSkipped: true,
            weights: [],
            activeSetIndex: 0,
          },
        }
      }

      const resizedWeights = Array.from({ length: nextCount }, (_, index) => current.weights[index] ?? null)

      return {
        ...prev,
        [exerciseId]: {
          ...current,
          isSkipped: false,
          setsCount: nextCount,
          weights: resizedWeights,
          activeSetIndex: Math.min(current.activeSetIndex, nextCount - 1),
        },
      }
    })

    if (nextCount === 0) {
      moveToNextExercise(exerciseId)
    }
  }

  const setActiveSet = (exerciseId, setIndex) => {
    setExerciseSelections((prev) => ({
      ...prev,
      [exerciseId]: {
        ...(prev[exerciseId] || { setsCount: 0, isSkipped: false, weights: [], activeSetIndex: 0 }),
        activeSetIndex: setIndex,
      },
    }))
  }

  const assignWeightToActiveSet = (exerciseId, weight) => {
    setExerciseSelections((prev) => {
      const current = prev[exerciseId] || { setsCount: 0, isSkipped: false, weights: [], activeSetIndex: 0 }
      const nextWeights = [...current.weights]
      nextWeights[current.activeSetIndex] = weight

      return {
        ...prev,
        [exerciseId]: {
          ...current,
          isSkipped: false,
          weights: nextWeights,
        },
      }
    })
  }

  const weightsForObject = (objectName) => {
    const normalized = (objectName || '').toLowerCase()
    if (normalized === 'bar') return weightCatalog.bar
    if (normalized === 'machine') return weightCatalog.machine
    return weightCatalog.dumbel
  }

  const handleAddAllForMuscle = async () => {
    if (!activeTab || !workoutSessionId) return

    const exercisesInTab = exerciseTemplates[activeTab] || []
    if (exercisesInTab.length === 0) {
      setError(`No exercises configured for ${activeTab}.`)
      return
    }

    const payloadExercises = []

    for (const exercise of exercisesInTab) {
      const selection = exerciseSelections[exercise.id]
      if (!selection) {
        setError(`Missing selection state for ${exercise.name}`)
        return
      }

      if (!selection.isSkipped && selection.setsCount === 0) {
        setError(`Pick Set1-Set4 or Skip for ${exercise.name}`)
        return
      }

      if (!selection.isSkipped) {
        const missingSet = selection.weights.findIndex((weight) => weight == null)
        if (missingSet !== -1) {
          setError(`Please select weight for ${exercise.name} - Set ${missingSet + 1}`)
          return
        }
      }

      payloadExercises.push({
        exerciseTemplateId: exercise.id,
        isSkipped: selection.isSkipped,
        weights: selection.isSkipped ? [] : selection.weights,
      })
    }

    setError('')
    setAddingAll(true)

    try {
      const result = await api.createExerciseLogsBulk({
        workoutSessionId,
        username,
        date: selectedDate,
        exercises: payloadExercises,
      })

      handleAddExercise(`${result.createdCount} set logs`)
    } catch (err) {
      setError(err.message)
    } finally {
      setAddingAll(false)
    }
  }

  if (loading) {
    return (
      <div className="exercises-container">
        <Navbar selectedDate={selectedDate} />
        <div className="loading">Loading exercises...</div>
      </div>
    )
  }

  return (
    <div className="exercises-container">
      <Navbar selectedDate={selectedDate} />

      <div className="exercises-content-with-sidebar">
        {/* Sidebar */}
        <div className="exercises-sidebar">
          <h3>Muscle Groups</h3>
          <div className="sidebar-tabs">
            {selectedCategories.map((category) => (
              <button
                key={category}
                className={`sidebar-tab ${activeTab === category ? 'active' : ''}`}
                onClick={() => setActiveTab(category)}
              >
                {category}
              </button>
            ))}
          </div>
        </div>

        {/* Main Content */}
        <div className="exercises-main">
          <div className="exercises-toolbar">
            <div>
              <h2>{activeTab ? `${activeTab} Exercises` : 'Exercises'}</h2>
              <p>Select sets and weights, then save all exercises.</p>
            </div>
            <div className="exercises-actions">
              <button
                onClick={handleAddAllForMuscle}
                disabled={!activeTab || addingAll}
                className="add-all-button"
              >
                {addingAll ? 'Saving...' : `Add All ${activeTab || ''} Exercises`}
              </button>
              <button
                onClick={() => navigate('/')}
                className="back-button"
              >
                Back to Landing
              </button>
              <button
                onClick={() => navigate(`/dashboard/${selectedDate}`)}
                className="back-button"
              >
                Back to Dashboard
              </button>
            </div>
          </div>

          <div className="exercises-alerts">
            {error && <div className="error-message">{error}</div>}
            {showAlert && <div className="success-alert">{alertMessage}</div>}
          </div>

          <div className="exercises-scroll">
            {activeTab && (
              <div className="exercises-section">
                <div className="exercises-stack">
                {(exerciseTemplates[activeTab] || []).map((exercise) => (
                  <div key={exercise.id} className="exercise-card" id={`exercise-card-${exercise.id}`}>
                    <div className="exercise-card-content">
                      <div className="exercise-header-row">
                        <h4>{exercise.name}</h4>
                        <span className="exercise-object-chip">Object: {exercise.object}</span>
                      </div>

                      <div className="set-plan-grid">
                        {[1, 2, 3, 4].map((count) => (
                          <button
                            key={`${exercise.id}-count-${count}`}
                            className={`set-plan-button ${exerciseSelections[exercise.id]?.setsCount === count && !exerciseSelections[exercise.id]?.isSkipped ? 'selected' : ''}`}
                            onClick={() => chooseSetPlan(exercise.id, count)}
                          >
                            Set {count}
                          </button>
                        ))}
                        <button
                          className={`set-plan-button skip ${exerciseSelections[exercise.id]?.isSkipped ? 'selected' : ''}`}
                          onClick={() => chooseSetPlan(exercise.id, 0)}
                        >
                          Skip
                        </button>
                      </div>

                      {exerciseSelections[exercise.id]?.isSkipped && (
                        <div className="skip-note">This exercise is skipped. 0 sets will be stored.</div>
                      )}

                      {!exerciseSelections[exercise.id]?.isSkipped && exerciseSelections[exercise.id]?.setsCount > 0 && (
                        <>
                          <div className="set-tags">
                            {Array.from({ length: exerciseSelections[exercise.id]?.setsCount || 0 }).map((_, index) => {
                              const weight = exerciseSelections[exercise.id]?.weights?.[index]
                              const isActive = (exerciseSelections[exercise.id]?.activeSetIndex || 0) === index

                              return (
                                <button
                                  key={`${exercise.id}-set-${index + 1}`}
                                  className={`set-tag ${isActive ? 'active' : ''}`}
                                  onClick={() => setActiveSet(exercise.id, index)}
                                >
                                  Set {index + 1}: {weight ?? '-'}
                                </button>
                              )
                            })}
                          </div>

                          <div className="weight-grid">
                            {weightsForObject(exercise.object).map((weight) => {
                              const activeSet = exerciseSelections[exercise.id]?.activeSetIndex || 0
                              const selectedWeight = exerciseSelections[exercise.id]?.weights?.[activeSet]
                              const isSelected = selectedWeight === weight

                              return (
                                <button
                                  key={`${exercise.id}-weight-${weight}`}
                                  className={`weight-option ${isSelected ? 'selected' : ''}`}
                                  onClick={() => assignWeightToActiveSet(exercise.id, weight)}
                                >
                                  {weight}
                                </button>
                              )
                            })}
                          </div>
                        </>
                      )}
                    </div>
                  </div>
                ))}
                </div>
                {activeTab && (exerciseTemplates[activeTab] || []).length === 0 && (
                  <div className="error-message">No exercises configured for {activeTab} yet.</div>
                )}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  )
}
