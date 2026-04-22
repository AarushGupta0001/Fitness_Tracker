import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { api } from '../utils/api'
import { setAuth } from '../utils/auth'

export default function Register() {
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
      const result = await api.register({ username, password })
      setAuth({ token: result.token, username: result.username })
      navigate('/')
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-[#0d0d0d] px-4">
      <div className="w-full max-w-md bg-[#131313] border border-[#2a2a2a] rounded-xl p-8 text-[#e5e2e1]">
        <h1 className="text-2xl font-semibold text-center">Create Account</h1>
        <p className="mt-2 text-sm text-center text-[#9ca3af]">Pick a unique username and password.</p>

        <form className="mt-6 space-y-4" onSubmit={handleSubmit}>
          <div>
            <label className="block text-sm mb-1" htmlFor="username">Username</label>
            <input
              id="username"
              type="text"
              className="w-full rounded-md bg-[#0e0e0e] border border-[#2a2a2a] px-3 py-2 text-sm text-white focus:outline-none focus:ring-2 focus:ring-red-500"
              value={username}
              onChange={(event) => setUsername(event.target.value)}
              required
            />
          </div>
          <div>
            <label className="block text-sm mb-1" htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              className="w-full rounded-md bg-[#0e0e0e] border border-[#2a2a2a] px-3 py-2 text-sm text-white focus:outline-none focus:ring-2 focus:ring-red-500"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              required
            />
          </div>

          {error && <div className="text-sm text-red-400">{error}</div>}

          <button
            type="submit"
            className="w-full rounded-md bg-red-500 px-4 py-2 text-sm font-semibold text-white hover:bg-red-600 transition-colors disabled:opacity-60"
            disabled={loading}
          >
            {loading ? 'Creating...' : 'Create Account'}
          </button>
        </form>

        <p className="mt-4 text-center text-sm text-[#9ca3af]">
          Already have an account?{' '}
          <Link className="text-red-400 hover:text-red-300" to="/login">Log in</Link>
        </p>
      </div>
    </div>
  )
}
