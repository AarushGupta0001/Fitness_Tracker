import { useState } from 'react'
import '../styles/Calendar.css'

const MONTHS = [
  'January','February','March','April','May','June',
  'July','August','September','October','November','December',
]
const WEEKDAYS = ['Sun','Mon','Tue','Wed','Thu','Fri','Sat']

export default function Calendar({ onDateSelect }) {
  const [currentDate, setCurrentDate] = useState(new Date())
  const [selectedDate, setSelectedDate] = useState(null)

  const year  = currentDate.getFullYear()
  const month = currentDate.getMonth()

  const daysInMonth   = new Date(year, month + 1, 0).getDate()
  const firstDayOfWeek = new Date(year, month, 1).getDay()

  const handlePrev = () => setCurrentDate(new Date(year, month - 1))
  const handleNext = () => setCurrentDate(new Date(year, month + 1))

  const handleDayClick = (day) => {
    if (!day) return
    const picked = new Date(year, month, day)
    setSelectedDate(picked)
    onDateSelect(picked)
  }

  const isToday = (day) => {
    if (!day) return false
    const t = new Date()
    return day === t.getDate() && month === t.getMonth() && year === t.getFullYear()
  }

  const isSelected = (day) => {
    if (!day || !selectedDate) return false
    return (
      day === selectedDate.getDate() &&
      month === selectedDate.getMonth() &&
      year === selectedDate.getFullYear()
    )
  }

  const isFuture = (day) => {
    if (!day) return false
    const d = new Date(year, month, day)
    const today = new Date(); today.setHours(0,0,0,0)
    return d > today
  }

  // Build day cells: leading nulls + actual days
  const cells = [
    ...Array(firstDayOfWeek).fill(null),
    ...Array.from({ length: daysInMonth }, (_, i) => i + 1),
  ]

  return (
    <div className="cal">
      {/* Header */}
      <div className="cal__header">
        <button className="cal__nav-btn" onClick={handlePrev} aria-label="Previous month">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"
            strokeLinecap="round" strokeLinejoin="round">
            <polyline points="15 18 9 12 15 6" />
          </svg>
        </button>

        <div className="cal__month-wrap">
          <span className="cal__month">{MONTHS[month]}</span>
          <span className="cal__year">{year}</span>
        </div>

        <button className="cal__nav-btn" onClick={handleNext} aria-label="Next month">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"
            strokeLinecap="round" strokeLinejoin="round">
            <polyline points="9 18 15 12 9 6" />
          </svg>
        </button>
      </div>

      {/* Weekday labels */}
      <div className="cal__weekdays">
        {WEEKDAYS.map((d) => (
          <div key={d} className="cal__weekday">{d}</div>
        ))}
      </div>

      {/* Day grid */}
      <div className="cal__grid">
        {cells.map((day, idx) => (
          <button
            key={idx}
            className={[
              'cal__day',
              !day         ? 'cal__day--empty'    : '',
              isToday(day) ? 'cal__day--today'    : '',
              isSelected(day) ? 'cal__day--selected' : '',
              isFuture(day) ? 'cal__day--future' : '',
            ].join(' ')}
            onClick={() => handleDayClick(day)}
            disabled={!day}
            aria-label={day ? `${MONTHS[month]} ${day}, ${year}` : undefined}
            aria-pressed={isSelected(day)}
            tabIndex={day ? 0 : -1}
          >
            {day && <span className="cal__day-num">{day}</span>}
            {isToday(day) && <span className="cal__today-dot" />}
          </button>
        ))}
      </div>

      {/* Selected date CTA */}
      {selectedDate && (
        <div className="cal__selected-banner">
          <div className="cal__selected-left">
            <span className="cal__selected-label">Selected</span>
            <span className="cal__selected-date">
              {selectedDate.toLocaleDateString('en-US', {
                weekday: 'long', month: 'long', day: 'numeric', year: 'numeric',
              })}
            </span>
          </div>
          <div className="cal__selected-right">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8"
              strokeLinecap="round" strokeLinejoin="round" className="cal__check">
              <polyline points="20 6 9 17 4 12" />
            </svg>
          </div>
        </div>
      )}
    </div>
  )
}
