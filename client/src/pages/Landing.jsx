import { useEffect, useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import { gsap } from 'gsap'
import Calendar from '../components/Calendar'
import Navbar from '../components/Navbar'
import '../styles/CalendarPage.css'

export default function Landing() {
  const navigate = useNavigate()
  const headingRef = useRef(null)
  const subRef = useRef(null)
  const calRef = useRef(null)
  const tipsRef = useRef(null)

  const handleDateSelect = (date) => {
    const formattedDate = date.toISOString().split('T')[0]
    navigate(`/dashboard/${formattedDate}`)
  }

  useEffect(() => {
    // Stagger entrance animations
    const tl = gsap.timeline({ defaults: { ease: 'power3.out' } })

    tl.fromTo(headingRef.current,
      { y: 40, opacity: 0 },
      { y: 0, opacity: 1, duration: 0.7 }
    )
    .fromTo(subRef.current,
      { y: 30, opacity: 0 },
      { y: 0, opacity: 1, duration: 0.6 },
      '-=0.4'
    )
    .fromTo(calRef.current,
      { y: 50, opacity: 0, scale: 0.97 },
      { y: 0, opacity: 1, scale: 1, duration: 0.7 },
      '-=0.3'
    )
    .fromTo(tipsRef.current,
      { y: 30, opacity: 0 },
      { y: 0, opacity: 1, duration: 0.6 },
      '-=0.4'
    )
  }, [])

  const tips = [
    { icon: '📅', text: 'Select any past date to review or log a previous workout' },
    { icon: '🎯', text: 'Pick today to start a new training session' },
    { icon: '📊', text: 'Dates with workouts will show on your Progress page' },
  ]

  return (
    <div className="cal-page">
      <Navbar />

      {/* Background orbs */}
      <div className="cal-page__orb cal-page__orb--1" />
      <div className="cal-page__orb cal-page__orb--2" />

      <div className="cal-page__content">
        {/* Left column — heading + tips */}
        <div className="cal-page__left">
          <div ref={headingRef}>
            <div className="cal-page__tag">Training Session</div>
            <h1 className="cal-page__title">
              Pick a Date.<br />
              <span className="cal-page__title-accent">Start Training.</span>
            </h1>
          </div>

          <p className="cal-page__sub" ref={subRef}>
            Select a date from the calendar to log your workout, track exercises,
            and monitor your performance over time.
          </p>

          {/* Quick tips */}
          <div className="cal-page__tips" ref={tipsRef}>
            <p className="cal-page__tips-label">Quick Tips</p>
            {tips.map((tip, i) => (
              <div key={i} className="cal-page__tip">
                <span className="cal-page__tip-icon">{tip.icon}</span>
                <span className="cal-page__tip-text">{tip.text}</span>
              </div>
            ))}
          </div>

          {/* Stats strip */}
          <div className="cal-page__stats">
            {[
              { value: '50+', label: 'Exercise Types' },
              { value: '∞', label: 'Workouts' },
              { value: '24/7', label: 'Tracking' },
            ].map((s) => (
              <div key={s.label} className="cal-page__stat">
                <span className="cal-page__stat-val">{s.value}</span>
                <span className="cal-page__stat-lab">{s.label}</span>
              </div>
            ))}
          </div>
        </div>

        {/* Right column — calendar */}
        <div className="cal-page__right" ref={calRef}>
          <div className="cal-page__cal-wrap">
            <div className="cal-page__cal-label">
              <span className="cal-page__cal-dot" />
              <span>Choose your training date</span>
            </div>
            <Calendar onDateSelect={handleDateSelect} />
          </div>
        </div>
      </div>
    </div>
  )
}
