import { useState, useEffect, useRef } from 'react'
import { Link, useNavigate, useLocation } from 'react-router-dom'
import { clearAuth, getUsername, isGuest } from '../utils/auth'
import '../styles/Navbar.css'

export default function Navbar({ selectedDate }) {
  const [isProfileOpen, setIsProfileOpen] = useState(false)
  const [scrolled, setScrolled] = useState(false)
  const profileRef = useRef(null)
  const navigate = useNavigate()
  const location = useLocation()
  const username = getUsername() || 'User'
  const guest = isGuest()

  // Shrink nav on scroll
  useEffect(() => {
    const onScroll = () => setScrolled(window.scrollY > 50)
    window.addEventListener('scroll', onScroll, { passive: true })
    return () => window.removeEventListener('scroll', onScroll)
  }, [])

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (e) => {
      if (profileRef.current && !profileRef.current.contains(e.target)) {
        setIsProfileOpen(false)
      }
    }
    document.addEventListener('mousedown', handleClickOutside)
    return () => document.removeEventListener('mousedown', handleClickOutside)
  }, [])

  const formatDate = (date) => {
    if (!date) return ''
    const d = new Date(date)
    return d.toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric' })
  }

  const navLinks = [
    { name: 'Home',     path: '/' },
    { name: 'Calendar', path: '/calendar' },
    { name: 'Logs',     path: '/logs' },
    { name: 'Progress', path: '/progress' },
  ]

  const handleSignOut = () => {
    clearAuth()
    navigate('/login')
  }

  const isActive = (path) => location.pathname === path

  return (
    <nav className={`app-nav ${scrolled ? 'app-nav--scrolled' : ''}`}>
      <div className="app-nav__inner">
        {/* Logo */}
        <Link to="/" className="app-nav__logo">
          <span className="app-nav__logo-mark">⚡</span>
          <span className="app-nav__logo-text">
            FITNESS<span className="app-nav__logo-accent">TRACK</span>
          </span>
        </Link>

        {/* Nav Links */}
        <ul className="app-nav__links">
          {navLinks.map((link) => (
            <li key={link.name}>
              <Link
                to={link.path}
                className={`app-nav__link ${isActive(link.path) ? 'app-nav__link--active' : ''}`}
              >
                {link.name}
                <span className="app-nav__link-bar" />
              </Link>
            </li>
          ))}
        </ul>

        {/* Right side */}
        <div className="app-nav__right">
          {selectedDate && (
            <span className="app-nav__date">
              <span className="app-nav__date-dot" />
              {formatDate(selectedDate)}
            </span>
          )}

          {/* Profile */}
          <div className="app-nav__profile-wrap" ref={profileRef}>
            <button
              className="app-nav__avatar"
              onClick={() => setIsProfileOpen(!isProfileOpen)}
              aria-label="Profile menu"
              aria-expanded={isProfileOpen}
            >
              {username.charAt(0).toUpperCase()}
            </button>

            <div className={`app-nav__dropdown ${isProfileOpen ? 'app-nav__dropdown--open' : ''}`}>
              <div className="app-nav__dropdown-header">
                <span className="app-nav__dropdown-name">{username}</span>
                {guest && <span className="app-nav__guest-badge">Guest</span>}
              </div>
              {guest ? (
                <>
                  <Link
                    to="/login"
                    className="app-nav__dropdown-item"
                    onClick={() => setIsProfileOpen(false)}
                  >
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"
                      strokeLinecap="round" strokeLinejoin="round" width="14" height="14">
                      <path d="M15 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4"/>
                      <polyline points="10 17 15 12 10 7"/>
                      <line x1="15" y1="12" x2="3" y2="12"/>
                    </svg>
                    Sign In
                  </Link>
                  <Link
                    to="/register"
                    className="app-nav__dropdown-item"
                    onClick={() => setIsProfileOpen(false)}
                  >
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"
                      strokeLinecap="round" strokeLinejoin="round" width="14" height="14">
                      <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/>
                      <circle cx="9" cy="7" r="4"/>
                      <line x1="19" y1="8" x2="19" y2="14"/>
                      <line x1="22" y1="11" x2="16" y2="11"/>
                    </svg>
                    Create Account
                  </Link>
                </>
              ) : (
                <button className="app-nav__dropdown-item" onClick={handleSignOut}>
                  <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"
                    strokeLinecap="round" strokeLinejoin="round" width="14" height="14">
                    <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/>
                    <polyline points="16 17 21 12 16 7"/>
                    <line x1="21" y1="12" x2="9" y2="12"/>
                  </svg>
                  Sign Out
                </button>
              )}
            </div>
          </div>
        </div>
      </div>
    </nav>
  )
}
