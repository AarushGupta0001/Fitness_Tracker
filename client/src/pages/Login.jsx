import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { api } from '../utils/api'
import { setAuth, setGuestMode } from '../utils/auth'
import '../styles/Auth.css'

export default function Login() {
  const navigate = useNavigate()
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (event) => {
    event.preventDefault()
    setError('')
    setLoading(true)

    try {
      const result = await api.login({ username, password })
      setAuth({ token: result.token, username: result.username })
      navigate('/')
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  const handleGuestLogin = () => {
    setGuestMode()
    navigate('/')
  }

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h1>Log In</h1>
        <p className="auth-subtitle">Use your username to access your workouts.</p>

        <form className="auth-form" onSubmit={handleSubmit}>
          <div className="auth-group">
            <label htmlFor="username">Username</label>
            <input
              id="username"
              type="text"
              className="auth-input"
              value={username}
              onChange={(event) => setUsername(event.target.value)}
              required
            />
          </div>
          <div className="auth-group">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              className="auth-input"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              required
            />
          </div>

          {error && <div className="auth-error">{error}</div>}

          <button
            type="submit"
            className="auth-button"
            disabled={loading}
          >
            {loading ? 'Signing in...' : 'Sign In'}
          </button>
        </form>

        <p className="auth-footer">
          New here?{' '}
          <Link className="auth-link" to="/register">Create an account</Link>
        </p>

        <div className="auth-divider">
          <span>or</span>
        </div>

        <button
          id="guest-login-btn"
          type="button"
          className="auth-guest-button"
          onClick={handleGuestLogin}
        >
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" className="guest-icon">
            <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
            <circle cx="12" cy="7" r="4"/>
          </svg>
          Continue as Guest
        </button>
      </div>
    </div>
  )
}
