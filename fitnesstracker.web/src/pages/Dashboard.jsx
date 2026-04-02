import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import Navbar from '../components/Navbar'
import WorkoutCategoryBox from '../components/WorkoutCategoryBox'
import { api } from '../utils/api'
import '../styles/Dashboard.css'

const CATEGORIES = ['Chest', 'Triceps', 'Back', 'Shoulder', 'Biceps', 'Legs']

export default function Dashboard() {
  const { selectedDate } = useParams()
  const navigate = useNavigate()
  const [selectedCategories, setSelectedCategories] = useState([])
  const [loading, setLoading] = useState(false)
  const [logsLoading, setLogsLoading] = useState(false)
  const [error, setError] = useState('')
  const [logsError, setLogsError] = useState('')
  const [workoutLogs, setWorkoutLogs] = useState([])
  const [expandedWorkouts, setExpandedWorkouts] = useState({})
  const [expandedExercises, setExpandedExercises] = useState({})

  useEffect(() => {
    const loadLogs = async () => {
      setLogsLoading(true)
      setLogsError('')

      try {
        const logs = await api.getWorkoutLogs('Aarush')
        setWorkoutLogs(logs)
      } catch (err) {
        setLogsError(err.message)
      } finally {
        setLogsLoading(false)
      }
    }

    loadLogs()
  }, [])

  const handleSelectCategory = (category) => {
    setSelectedCategories((prev) =>
      prev.includes(category)
        ? prev.filter((c) => c !== category)
        : [...prev, category]
    )
  }

  const toggleWorkout = (workoutSessionId) => {
    setExpandedWorkouts((prev) => ({
      ...prev,
      [workoutSessionId]: !prev[workoutSessionId],
    }))
  }

  const toggleExercise = (workoutSessionId, exerciseTemplateId) => {
    const key = `${workoutSessionId}-${exerciseTemplateId}`
    setExpandedExercises((prev) => ({
      ...prev,
      [key]: !prev[key],
    }))
  }

  const handleSubmit = async () => {
    if (selectedCategories.length === 0) {
      setError('Please select at least one workout category')
      return
    }

    setLoading(true)
    setError('')

    try {
      // Create workout session with selected categories
      const selectedMuscleGroups = selectedCategories.join(',')
      await api.createWorkoutSession('Aarush', selectedMuscleGroups, selectedDate)

      // Navigate to exercises page
      navigate(`/exercises/${selectedDate}`)
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="dashboard-container">
      <Navbar selectedDate={selectedDate} />

      <div className="dashboard-content">
        <h2 className="workouts-heading">Workouts</h2>

        <div className="categories-grid">
          {CATEGORIES.map((category) => (
            <WorkoutCategoryBox
              key={category}
              category={category}
              isSelected={selectedCategories.includes(category)}
              onSelect={handleSelectCategory}
            />
          ))}
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="button-container">
          <button
            onClick={handleSubmit}
            disabled={loading || selectedCategories.length === 0}
            className="enter-button"
          >
            {loading ? 'Loading...' : 'Enter'}
          </button>
        </div>

        <div className="logs-section">
          <h3>Workout Logs</h3>

          {logsLoading && <div className="logs-info">Loading logs...</div>}
          {logsError && <div className="error-message">{logsError}</div>}

          {!logsLoading && !logsError && workoutLogs.length === 0 && (
            <div className="logs-info">No logs yet. Add workouts to see history.</div>
          )}

          {!logsLoading && !logsError && workoutLogs.length > 0 && (
            <div className="workout-log-list">
              <div className="workout-log-header">
                <span>S.No</span>
                <span>Date</span>
                <span>Workout</span>
              </div>

              {workoutLogs.map((log, index) => (
                <div className="workout-log-item" key={log.workoutSessionId}>
                  <div className="workout-log-row">
                    <span>{index + 1}</span>
                    <span>{new Date(log.date).toLocaleDateString()}</span>
                    <button className="workout-link" onClick={() => toggleWorkout(log.workoutSessionId)}>
                      {log.workout}
                    </button>
                  </div>

                  {expandedWorkouts[log.workoutSessionId] && (
                    <div className="exercise-log-list">
                      {log.exercises.length === 0 && (
                        <div className="logs-info">No exercise sets saved for this workout yet.</div>
                      )}

                      {log.exercises.map((exercise) => {
                        const key = `${log.workoutSessionId}-${exercise.exerciseTemplateId}`

                        return (
                          <div className="exercise-log-item" key={key}>
                            <button
                              className="exercise-log-button"
                              onClick={() => toggleExercise(log.workoutSessionId, exercise.exerciseTemplateId)}
                            >
                              {exercise.exerciseName} ({exercise.muscleGroup}) - {exercise.object}
                            </button>

                            {expandedExercises[key] && (
                              <div className="set-log-list">
                                {exercise.isSkipped && <div className="set-log-row skipped">Skipped (0 sets)</div>}

                                {!exercise.isSkipped && exercise.sets.map((setItem) => (
                                  <div className="set-log-row" key={`${key}-set-${setItem.setNumber}`}>
                                    Set {setItem.setNumber}: {setItem.weight}
                                  </div>
                                ))}
                              </div>
                            )}
                          </div>
                        )
                      })}
                    </div>
                  )}
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  )
}
