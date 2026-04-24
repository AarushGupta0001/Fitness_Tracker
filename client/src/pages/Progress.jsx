import { useEffect, useMemo, useState } from 'react'
import Navbar from '../components/Navbar'
import { api } from '../utils/api'
import { getUsername } from '../utils/auth'
import '../styles/Progress.css'

const MUSCLE_GROUPS = ['Chest', 'Triceps', 'Back', 'Shoulder', 'Biceps', 'Legs']

export default function Progress() {
  const username = getUsername()
  const [activeSection, setActiveSection] = useState('progressive')
  const [selectedMuscle, setSelectedMuscle] = useState('Back')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')
  const [progressiveData, setProgressiveData] = useState(null)
  const [weeklyData, setWeeklyData] = useState(null)
  const [recoveryData, setRecoveryData] = useState(null)

  const maxTrendWeight = useMemo(() => {
    const values = (progressiveData?.trend || []).map((item) => Number(item.topWeight || 0))
    return values.length > 0 ? Math.max(...values) : 1
  }, [progressiveData])

  useEffect(() => {
    const loadSection = async () => {
      if (!username) {
        return
      }

      setLoading(true)
      setError('')

      try {
        if (activeSection === 'progressive') {
          const data = await api.getProgressiveOverloadInsight(selectedMuscle, username)
          setProgressiveData(data)
        }

        if (activeSection === 'weekly') {
          const data = await api.getWeeklyPerformanceInsight('', username)
          setWeeklyData(data)
        }

        if (activeSection === 'recovery') {
          const data = await api.getRecoveryInsight(14, username)
          setRecoveryData(data)
        }
      } catch (err) {
        setError(err.message)
      } finally {
        setLoading(false)
      }
    }

    loadSection()
  }, [activeSection, selectedMuscle, username])

  return (
    <div className="progress-page">
      <Navbar />

      <div className="progress-page__orb progress-page__orb--1" />
      <div className="progress-page__orb progress-page__orb--2" />

      <div className="progress-shell">
        <aside className="progress-sidebar">
          <h3>Progress</h3>
          <button
            className={`progress-nav-item ${activeSection === 'progressive' ? 'active' : ''}`}
            onClick={() => setActiveSection('progressive')}
          >
            Progressive Overload
          </button>
          <button
            className={`progress-nav-item ${activeSection === 'weekly' ? 'active' : ''}`}
            onClick={() => setActiveSection('weekly')}
          >
            Weekly Performance
          </button>
          <button
            className={`progress-nav-item ${activeSection === 'recovery' ? 'active' : ''}`}
            onClick={() => setActiveSection('recovery')}
          >
            Recovery Tracking
          </button>
        </aside>

        <main className="progress-main">
          {loading && <div className="progress-info">Loading insights...</div>}
          {error && <div className="progress-error">{error}</div>}

          {!loading && !error && activeSection === 'progressive' && (
            <section className="progress-section">
              <div className="section-header-row">
                <h2>Progressive Overload</h2>
                <select value={selectedMuscle} onChange={(event) => setSelectedMuscle(event.target.value)}>
                  {MUSCLE_GROUPS.map((group) => (
                    <option key={group} value={group}>{group}</option>
                  ))}
                </select>
              </div>

              <div className="insight-card">
                <h4>{progressiveData?.recommendationType?.toUpperCase() || 'ANALYSIS'}</h4>
                <p>{progressiveData?.recommendation || 'No data available yet.'}</p>
                {progressiveData?.recommendedIncrement != null && (
                  <span className="chip">Recommended increment: +{progressiveData.recommendedIncrement}</span>
                )}
              </div>

              <div className="trend-list">
                {(progressiveData?.trend || []).map((item) => (
                  <div key={item.workoutSessionId} className="trend-row">
                    <div className="trend-meta">
                      <strong>{new Date(item.date).toLocaleDateString()}</strong>
                      <span>Top: {item.topWeight}</span>
                    </div>
                    <div className="trend-bar-track">
                      <div
                        className="trend-bar-fill"
                        style={{ width: `${Math.max(8, (Number(item.topWeight || 0) / maxTrendWeight) * 100)}%` }}
                      />
                    </div>
                  </div>
                ))}
                {(progressiveData?.trend || []).length === 0 && (
                  <div className="progress-info">No trend points yet for this muscle group.</div>
                )}
              </div>
            </section>
          )}

          {!loading && !error && activeSection === 'weekly' && (
            <section className="progress-section">
              <h2>Weekly Performance</h2>

              <div className="weekly-grid">
                <div className="insight-card">
                  <h4>Days Trained</h4>
                  <p>{weeklyData?.daysTrained ?? 0}</p>
                </div>
                <div className="insight-card">
                  <h4>Total Sessions</h4>
                  <p>{weeklyData?.totalSessions ?? 0}</p>
                </div>
                <div className="insight-card">
                  <h4>New PRs</h4>
                  <p>{weeklyData?.prCount ?? 0}</p>
                </div>
              </div>

              <div className="insight-card">
                <h4>Muscle Frequency</h4>
                <div className="list-grid">
                  {(weeklyData?.muscleFrequency || []).map((item) => (
                    <div key={item.muscleGroup} className="list-row">
                      <span>{item.muscleGroup}</span>
                      <strong>{item.sessions}</strong>
                    </div>
                  ))}
                  {(weeklyData?.muscleFrequency || []).length === 0 && <span>No sessions this week.</span>}
                </div>
              </div>

              <div className="insight-card">
                <h4>Less Effective vs Last Week</h4>
                <div className="list-grid">
                  {(weeklyData?.lessEffectiveMuscles || []).map((item) => (
                    <div key={item.muscleGroup} className="list-row danger">
                      <span>{item.muscleGroup}</span>
                      <strong>{Number(item.delta).toFixed(2)}</strong>
                    </div>
                  ))}
                  {(weeklyData?.lessEffectiveMuscles || []).length === 0 && <span>No major decline detected.</span>}
                </div>
              </div>
            </section>
          )}

          {!loading && !error && activeSection === 'recovery' && (
            <section className="progress-section">
              <h2>Recovery Tracking</h2>

              <div className="weekly-grid">
                <div className="insight-card">
                  <h4>Fatigue</h4>
                  <p>{recoveryData?.fatigueLevel || 'Moderate'}</p>
                </div>
                <div className="insight-card">
                  <h4>Rest Days (Last 7)</h4>
                  <p>{recoveryData?.restDaysLast7 ?? 0}</p>
                </div>
                <div className="insight-card">
                  <h4>Consecutive Training Days</h4>
                  <p>{recoveryData?.consecutiveTrainingDays ?? 0}</p>
                </div>
              </div>

              <div className="insight-card">
                <h4>Recommendation</h4>
                <p>{recoveryData?.recommendation || 'Recovery recommendation will appear after workout activity.'}</p>
              </div>
            </section>
          )}
        </main>
      </div>
    </div>
  )
}
