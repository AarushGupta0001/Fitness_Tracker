import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import Navbar from '../components/Navbar'
import { api } from '../utils/api'
import '../styles/Exercises.css'

const WEIGHT_OPTIONS = [2.5, 5, 7.5, 10, 12, 12.5, 14, 15, 16, 17.5, 20, 24]

export default function Exercises() {
  const { selectedDate } = useParams()
  const navigate = useNavigate()
  const [selectedCategories, setSelectedCategories] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [activeTab, setActiveTab] = useState(null)
  const [exerciseTemplates, setExerciseTemplates] = useState({})
  const [exerciseSelections, setExerciseSelections] = useState({})
  const [addingAll, setAddingAll] = useState(false)
  const [showAlert, setShowAlert] = useState(false)
  const [alertMessage, setAlertMessage] = useState('')

  useEffect(() => {
    const fetchData = async () => {
      try {
        const sessions = await api.getWorkoutSessionsByDate(selectedDate)
        if (sessions && sessions.length > 0) {
          const categories = sessions[0].selectedMuscleGroups
            .split(',')
            .map((c) => c.trim())
            .filter(Boolean)

          const templates = await api.getExerciseTemplates(categories)
          const groupedTemplates = templates.reduce((acc, template) => {
            if (!acc[template.muscleGroup]) {
              acc[template.muscleGroup] = []
            }

            acc[template.muscleGroup].push(template)
            return acc
          }, {})

          const initialSelections = templates.reduce((acc, template) => {
            acc[template.id] = {
              setsCount: 1,
              weights: [null],
              activeSetIndex: 0,
            }
            return acc
          }, {})

          setSelectedCategories(categories)
          setExerciseTemplates(groupedTemplates)
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
  }, [selectedDate])

  const handleAddExercise = (exerciseName) => {
    setAlertMessage(`${exerciseName} added successfully! ✓`)
    setShowAlert(true)
    setTimeout(() => setShowAlert(false), 2000)
  }

  const updateSetsCount = (exerciseId, nextCount) => {
    setExerciseSelections((prev) => {
      const current = prev[exerciseId] || { setsCount: 1, weights: [null], activeSetIndex: 0 }
      const resizedWeights = Array.from({ length: nextCount }, (_, index) => current.weights[index] ?? null)

      return {
        ...prev,
        [exerciseId]: {
          ...current,
          setsCount: nextCount,
          weights: resizedWeights,
          activeSetIndex: Math.min(current.activeSetIndex, nextCount - 1),
        },
      }
    })
  }

  const setActiveSet = (exerciseId, setIndex) => {
    setExerciseSelections((prev) => ({
      ...prev,
      [exerciseId]: {
        ...(prev[exerciseId] || { setsCount: 1, weights: [null], activeSetIndex: 0 }),
        activeSetIndex: setIndex,
      },
    }))
  }

  const assignWeightToActiveSet = (exerciseId, weight) => {
    setExerciseSelections((prev) => {
      const current = prev[exerciseId] || { setsCount: 1, weights: [null], activeSetIndex: 0 }
      const nextWeights = [...current.weights]
      nextWeights[current.activeSetIndex] = weight

      return {
        ...prev,
        [exerciseId]: {
          ...current,
          weights: nextWeights,
        },
      }
    })
  }

  const handleAddAllForMuscle = async () => {
    if (!activeTab) return

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

      const missingSet = selection.weights.findIndex((weight) => weight == null)
      if (missingSet !== -1) {
        setError(`Please select weight for ${exercise.name} - Set ${missingSet + 1}`)
        return
      }

      payloadExercises.push({
        exerciseName: exercise.name,
        weights: selection.weights,
      })
    }

    setError('')
    setAddingAll(true)

    try {
      const result = await api.createExerciseLogsBulk({
        username: 'Aarush',
        date: selectedDate,
        muscleGroup: activeTab,
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
          {error && <div className="error-message">{error}</div>}

          {showAlert && <div className="success-alert">{alertMessage}</div>}

          {activeTab && (
            <div className="exercises-section">
              <h2>{activeTab} Exercises</h2>
              <div className="exercises-grid">
                {(exerciseTemplates[activeTab] || []).map((exercise) => (
                  <div key={exercise.id} className="exercise-card">
                    <div className="exercise-card-content">
                      <h4>{exercise.name}</h4>
                      <div className="exercise-config-row">
                        <label htmlFor={`sets-${exercise.id}`}>Sets</label>
                        <select
                          id={`sets-${exercise.id}`}
                          value={exerciseSelections[exercise.id]?.setsCount || 1}
                          onChange={(event) => updateSetsCount(exercise.id, Number(event.target.value))}
                          className="sets-select"
                        >
                          <option value={1}>1</option>
                          <option value={2}>2</option>
                          <option value={3}>3</option>
                          <option value={4}>4</option>
                          <option value={5}>5</option>
                        </select>
                      </div>

                      <div className="set-tags">
                        {Array.from({ length: exerciseSelections[exercise.id]?.setsCount || 1 }).map((_, index) => {
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
                        {WEIGHT_OPTIONS.map((weight) => {
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
                    </div>
                  </div>
                ))}
              </div>
              {activeTab && (exerciseTemplates[activeTab] || []).length === 0 && (
                <div className="error-message">No exercises configured for {activeTab} yet.</div>
              )}
            </div>
          )}

          <div className="button-group">
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
      </div>
    </div>
  )
}
