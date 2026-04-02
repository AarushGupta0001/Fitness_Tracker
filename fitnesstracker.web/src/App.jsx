import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Landing from './pages/Landing'
import Dashboard from './pages/Dashboard'
import Exercises from './pages/Exercises'
import './App.css'

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Landing />} />
        <Route path="/dashboard/:selectedDate" element={<Dashboard />} />
        <Route path="/exercises/:selectedDate" element={<Exercises />} />
      </Routes>
    </Router>
  )
}

export default App
