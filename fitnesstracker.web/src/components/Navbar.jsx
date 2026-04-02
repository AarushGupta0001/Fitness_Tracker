import '../styles/Navbar.css'

export default function Navbar({ selectedDate }) {
  const username = 'Aarush'

  const formatDate = (date) => {
    const d = new Date(date)
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' }
    return d.toLocaleDateString('en-US', options)
  }

  return (
    <nav className="navbar">
      <div className="navbar-left">
        {selectedDate && <span className="date-display">{formatDate(selectedDate)}</span>}
      </div>
      <div className="navbar-center">
        <h1 className="navbar-title">Fitness Tracker</h1>
      </div>
      <div className="navbar-right">
        <span className="welcome">Welcome, {username}</span>
      </div>
    </nav>
  )
}
