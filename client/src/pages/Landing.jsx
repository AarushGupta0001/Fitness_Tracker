import { useNavigate } from 'react-router-dom'
import Calendar from '../components/Calendar'
import Navbar from '../components/Navbar'
import '../styles/Landing.css'

export default function Landing() {
  const navigate = useNavigate()

  const handleDateSelect = (date) => {
    // Format date as YYYY-MM-DD for URL
    const formattedDate = date.toISOString().split('T')[0]
    navigate(`/dashboard/${formattedDate}`)
  }

  return (
    <div className="min-h-screen bg-[#0e0e0e]">
      <Navbar />
      <div className="landing-container">
        <div className="landing-header">
          <h1>Fitness Tracker</h1>
          <p>Select a date to get started with your workout</p>
        </div>
        <Calendar onDateSelect={handleDateSelect} />
      </div>
    </div>
  )
}
