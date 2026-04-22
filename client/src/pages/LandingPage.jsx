import { useNavigate } from 'react-router-dom'
import Navbar from '../components/Navbar'

export default function LandingPage() {
  const navigate = useNavigate()

  return (
    <div className="min-h-screen bg-[#0e0e0e] text-white">
      <Navbar />
      <div className="flex items-center justify-center h-[calc(100vh-72px)] px-4">
        <div className="text-center max-w-lg">
          <h1 className="text-4xl font-semibold">Start Tracking</h1>
          <p className="mt-3 text-sm text-[#9ca3af]">
            Pick a date, log your workout, and keep your progress organized.
          </p>
          <button
            className="mt-6 rounded-md bg-red-500 px-6 py-3 text-sm font-semibold text-white hover:bg-red-600 transition-colors"
            onClick={() => navigate('/calendar')}
          >
            Start Tracking
          </button>
        </div>
      </div>
    </div>
  )
}