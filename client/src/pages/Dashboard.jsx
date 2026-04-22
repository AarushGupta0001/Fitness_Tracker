import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import Navbar from '../components/Navbar'
import WorkoutCategoryBox from '../components/WorkoutCategoryBox'
import { api } from '../utils/api'
import { getUsername } from '../utils/auth'
import '../styles/Dashboard.css'

const CATEGORIES = ['Chest', 'Triceps', 'Back', 'Shoulder', 'Biceps', 'Legs']

export default function Dashboard() {
  const { selectedDate } = useParams()
  const navigate = useNavigate()
  const username = getUsername()
  const [selectedCategories, setSelectedCategories] = useState([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  useEffect(() => {
    if (!username) {
      navigate('/login')
    }
  }, [navigate, username])

  const handleSelectCategory = (category) => {
    setSelectedCategories((prev) =>
      prev.includes(category)
        ? prev.filter((c) => c !== category)
        : [...prev, category]
    )
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
      await api.createWorkoutSession(username, selectedMuscleGroups, selectedDate)

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

      </div>
    </div>
  )
}
