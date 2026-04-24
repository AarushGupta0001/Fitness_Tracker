import React, { useState, useEffect, useRef } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { clearAuth, getUsername } from '../utils/auth'

export default function Navbar({ selectedDate }) {
  const [isProfileOpen, setIsProfileOpen] = useState(false)
  const profileRef = useRef(null)
  const navigate = useNavigate()
  const username = getUsername() || 'User'

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
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' }
    return d.toLocaleDateString('en-US', options)
  }

  const navLinks = [
    { name: 'Home', path: '/' },
    { name: 'Calendar', path: '/calendar' },
    { name: 'Logs', path: '/logs' },
    { name: 'Progress', path: '/progress' }
  ]

  const handleSignOut = () => {
    clearAuth()
    navigate('/login')
  }

  return (
    <nav className="bg-[#131313] border-b border-[#2A2A2A] text-[#e5e2e1] px-6 py-4 flex items-center justify-between sticky top-0 z-50">
      <div className="flex items-center gap-8">
        <h1 className="text-2xl font-bold tracking-tighter text-red-500">
          VELOCITY<span className="text-[#e5e2e1]">.FIT</span>
        </h1>
        
        <ul className="hidden md:flex items-center gap-6">
          {navLinks.map((link) => (
            <li key={link.name}>
              <Link 
                to={link.path}
                className="group relative text-sm font-medium tracking-wide uppercase text-[#e5e2e1] hover:text-red-500 transition-colors duration-300"
              >
                {link.name}
                <span className="absolute -bottom-1 left-0 w-full h-[2px] bg-red-500 origin-left scale-x-0 group-hover:scale-x-100 transition-transform duration-300 ease-out"></span>
              </Link>
            </li>
          ))}
        </ul>
      </div>

      <div ref={profileRef} className="flex items-center gap-4 relative">
        {selectedDate && (
          <span className="text-xs font-mono text-[#ebbbb4] hidden lg:block tracking-widest uppercase mr-4">
            {formatDate(selectedDate)}
          </span>
        )}
        
        {/* Profile Button */}
        <button 
          onClick={() => setIsProfileOpen(!isProfileOpen)}
          className="w-10 h-10 rounded-full bg-red-500/10 flex items-center justify-center text-red-500 font-bold tracking-wider hover:bg-red-500 hover:text-[#131313] border-2 border-red-500 transition-all duration-300 focus:outline-none"
        >
          {username.charAt(0)}
        </button>

        {/* Profile Dropdown */}
        <div 
          className={`absolute top-14 right-0 w-40 bg-[#0E0E0E] rounded-md shadow-lg shadow-black/40 z-50 overflow-hidden transition-all duration-200 origin-top-right ${
            isProfileOpen ? 'opacity-100 scale-100 visible' : 'opacity-0 scale-95 invisible'
          }`}
        >
          <h2 className='py-2 text-center text-xs tracking-wider uppercase text-[#888] transition-colors cursor-default'>{username}</h2>
          <button 
            onClick={handleSignOut}
            className="w-full flex items-center gap-3 text-left px-4 py-2.5 text-xs tracking-wider uppercase text-[#888] hover:text-red-500 hover:bg-white/5 transition-colors outline-none"
          >
            <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"></path><polyline points="16 17 21 12 16 7"></polyline><line x1="21" y1="12" x2="9" y2="12"></line></svg>
            Sign Out
          </button>
        </div>
      </div>
    </nav>
  )
}
