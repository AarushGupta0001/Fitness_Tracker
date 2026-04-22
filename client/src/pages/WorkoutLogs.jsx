import { useEffect, useMemo, useState } from 'react'
import Navbar from '../components/Navbar'
import { api } from '../utils/api'
import { getUsername } from '../utils/auth'
import '../styles/WorkoutLogs.css'

const formatLogDate = (dateString) => {
  if (!dateString) return ''
  const date = new Date(dateString)
  return date.toLocaleDateString('en-GB', {
    day: 'numeric',
    month: 'long',
    year: 'numeric',
  })
}

export default function WorkoutLogs() {
  const username = getUsername()
  const [logs, setLogs] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [filterDate, setFilterDate] = useState('')
  const [filterType, setFilterType] = useState('all')
  const [sortOrder, setSortOrder] = useState('latest')

  useEffect(() => {
    const loadLogs = async () => {
      setLoading(true)
      setError('')

      try {
        const data = await api.getWorkoutLogs(username)
        setLogs(data)
      } catch (err) {
        setError(err.message)
      } finally {
        setLoading(false)
      }
    }

    loadLogs()
  }, [username])

  const workoutTypes = useMemo(() => {
    const unique = new Set(logs.map((log) => log.workout).filter(Boolean))
    return ['all', ...Array.from(unique)]
  }, [logs])

  const rows = useMemo(() => {
    const filtered = logs.filter((log) => {
      const matchesType = filterType === 'all' || log.workout === filterType
      const matchesDate = !filterDate || log.date?.split('T')[0] === filterDate
      return matchesType && matchesDate
    })

    filtered.sort((a, b) => {
      const aTime = new Date(a.date).getTime()
      const bTime = new Date(b.date).getTime()
      return sortOrder === 'latest' ? bTime - aTime : aTime - bTime
    })

    return filtered.map((log, index) => ({
      id: `${log.workoutSessionId}-${index}`,
      sessionId: log.workoutSessionId,
      number: index + 1,
      date: formatLogDate(log.date),
      workout: log.workout,
      exerciseCount: log.exercises?.length ?? 0,
      exercises: log.exercises ?? [],
    }))
  }, [filterDate, filterType, logs, sortOrder])

  return (
    <div className="logs-page">
      <Navbar />

      <div className="logs-shell">
        <div className="logs-header">
          <div>
            <h2>Workout Logs</h2>
            <p>Review your saved workouts and exercise totals.</p>
          </div>
          <div className="logs-filters">
            <div className="filter-group">
              <label htmlFor="log-date">Date</label>
              <input
                id="log-date"
                type="date"
                value={filterDate}
                onChange={(event) => setFilterDate(event.target.value)}
              />
            </div>
            <div className="filter-group">
              <label htmlFor="log-type">Workout Type</label>
              <select
                id="log-type"
                value={filterType}
                onChange={(event) => setFilterType(event.target.value)}
              >
                {workoutTypes.map((type) => (
                  <option key={type} value={type}>
                    {type === 'all' ? 'All' : type}
                  </option>
                ))}
              </select>
            </div>
            <div className="filter-group">
              <label htmlFor="log-sort">Sort</label>
              <select
                id="log-sort"
                value={sortOrder}
                onChange={(event) => setSortOrder(event.target.value)}
              >
                <option value="latest">Latest</option>
                <option value="oldest">Oldest</option>
              </select>
            </div>
          </div>
        </div>

        {loading && <div className="logs-info">Loading logs...</div>}
        {error && <div className="logs-error">{error}</div>}

        {!loading && !error && rows.length === 0 && (
          <div className="logs-info">No logs yet. Complete a workout to see history.</div>
        )}

        {!loading && !error && rows.length > 0 && (
          <div className="logs-table">
            <div className="logs-table-header">
              <span>S.No</span>
              <span>Date</span>
              <span>Workout Type</span>
              <span>Exercises</span>
            </div>
            {rows.map((row) => (
              <div className="logs-table-row" key={row.id}>
                <span>{row.number}</span>
                <span>{row.date}</span>
                <span>{row.workout}</span>
                <span>{row.exerciseCount}</span>
                <div className="logs-detail">
                  <div className="logs-detail-header">
                    <span>Exercise</span>
                    <span>Muscle</span>
                    <span>Object</span>
                    <span>Sets</span>
                  </div>
                  {row.exercises.length === 0 && (
                    <div className="logs-detail-row empty">No exercise logs saved.</div>
                  )}
                  {row.exercises.map((exercise) => (
                    <div
                      key={`${row.sessionId}-${exercise.exerciseTemplateId}`}
                      className="logs-detail-row"
                    >
                      <span>{exercise.exerciseName}</span>
                      <span>{exercise.muscleGroup}</span>
                      <span>{exercise.object}</span>
                      <span>
                        {exercise.isSkipped
                          ? 'Skipped'
                          : exercise.sets.map((setItem) => `Set ${setItem.setNumber}: ${setItem.weight}`).join(', ')}
                      </span>
                    </div>
                  ))}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
