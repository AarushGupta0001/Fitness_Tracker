import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Landing from './pages/Landing'
import Dashboard from './pages/Dashboard'
import Exercises from './pages/Exercises'
import LandingPage from './pages/LandingPage'
import Navbar from './components/Navbar'
import './App.css'

function App() {
  return (
    <Router>
      <Navbar/>
      <Routes>
        <Route path="/landing" element={<Landing />} />
        <Route path="/" element={<LandingPage />} />
        <Route path="/dashboard/:selectedDate" element={<Dashboard />} />
        <Route path="/exercises/:selectedDate" element={<Exercises />} />
      </Routes>
    </Router>
  )
}

export default App
