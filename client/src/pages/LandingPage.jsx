import { useNavigate } from 'react-router-dom'
import Navbar from '../components/Navbar'
import '../styles/Landing.css'

export default function LandingPage() {
  const navigate = useNavigate()

  return (
    <div className="landing-wrapper">
      <Navbar />
      <div className="landing-container">
        <div className="landing-header">
          <h1>Start Tracking</h1>
          <p>
            Pick a date, log your workout, and keep your progress organized.
          </p>
        </div>
        <button
          className="hero-button"
          onClick={() => navigate('/calendar')}
        >
          Start Tracking
        </button>
      </div>
    </div>
  )
}