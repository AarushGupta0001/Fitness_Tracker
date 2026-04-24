import { useEffect, useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import { gsap } from 'gsap'
import { ScrollTrigger } from 'gsap/ScrollTrigger'
import { TextPlugin } from 'gsap/TextPlugin'
import Navbar from '../components/Navbar'
import '../styles/LandingPage.css'

gsap.registerPlugin(ScrollTrigger, TextPlugin)

// ── SVG Icon Components ───────────────────────────────────────────────
const FlameIcon = () => (
  <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5"
    strokeLinecap="round" strokeLinejoin="round" className="feature-icon-svg">
    <path d="M12 2C9 6 4 8 4 13a8 8 0 0016 0c0-5-5-7-5-13z"/>
    <path d="M12 22c-2 0-4-1.5-4-4 0-2.5 2-4 4-5.5 2 1.5 4 3 4 5.5 0 2.5-2 4-4 4z"/>
  </svg>
)

const ChartIcon = () => (
  <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5"
    strokeLinecap="round" strokeLinejoin="round" className="feature-icon-svg">
    <polyline points="22 12 18 12 15 21 9 3 6 12 2 12"/>
  </svg>
)

const CalendarIcon = () => (
  <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5"
    strokeLinecap="round" strokeLinejoin="round" className="feature-icon-svg">
    <rect x="3" y="4" width="18" height="18" rx="2"/>
    <line x1="16" y1="2" x2="16" y2="6"/>
    <line x1="8" y1="2" x2="8" y2="6"/>
    <line x1="3" y1="10" x2="21" y2="10"/>
    <circle cx="8" cy="15" r="1" fill="currentColor" stroke="none"/>
    <circle cx="12" cy="15" r="1" fill="currentColor" stroke="none"/>
    <circle cx="16" cy="15" r="1" fill="currentColor" stroke="none"/>
  </svg>
)

const TargetIcon = () => (
  <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5"
    strokeLinecap="round" strokeLinejoin="round" className="feature-icon-svg">
    <circle cx="12" cy="12" r="10"/>
    <circle cx="12" cy="12" r="6"/>
    <circle cx="12" cy="12" r="2" fill="currentColor" stroke="none"/>
  </svg>
)

// ── Animated Counter ──────────────────────────────────────────────────
function AnimatedStat({ value, label, suffix = '' }) {
  const numRef = useRef(null)

  useEffect(() => {
    const el = numRef.current
    if (!el) return
    gsap.fromTo(
      el,
      { textContent: 0 },
      {
        textContent: parseInt(value),
        duration: 2,
        ease: 'power2.out',
        snap: { textContent: 1 },
        scrollTrigger: {
          trigger: el,
          start: 'top 85%',
          once: true,
        },
        onUpdate: function () {
          el.textContent = Math.round(this.targets()[0].textContent).toLocaleString() + suffix
        },
      }
    )
  }, [value, suffix])

  return (
    <div className="stat-item">
      <span className="stat-number" ref={numRef}>0{suffix}</span>
      <span className="stat-label">{label}</span>
    </div>
  )
}

// ── Particle Dots (decorative) ────────────────────────────────────────
function ParticleField() {
  const canvasRef = useRef(null)

  useEffect(() => {
    const canvas = canvasRef.current
    if (!canvas) return
    const ctx = canvas.getContext('2d')
    let animId
    const resize = () => {
      canvas.width = canvas.offsetWidth
      canvas.height = canvas.offsetHeight
    }
    resize()
    window.addEventListener('resize', resize)

    const particles = Array.from({ length: 60 }, () => ({
      x: Math.random() * canvas.width,
      y: Math.random() * canvas.height,
      vx: (Math.random() - 0.5) * 0.4,
      vy: (Math.random() - 0.5) * 0.4,
      r: Math.random() * 1.5 + 0.5,
      opacity: Math.random() * 0.4 + 0.1,
    }))

    const draw = () => {
      ctx.clearRect(0, 0, canvas.width, canvas.height)
      particles.forEach(p => {
        p.x += p.vx; p.y += p.vy
        if (p.x < 0) p.x = canvas.width
        if (p.x > canvas.width) p.x = 0
        if (p.y < 0) p.y = canvas.height
        if (p.y > canvas.height) p.y = 0
        ctx.beginPath()
        ctx.arc(p.x, p.y, p.r, 0, Math.PI * 2)
        ctx.fillStyle = `rgba(229,25,26,${p.opacity})`
        ctx.fill()
      })
      animId = requestAnimationFrame(draw)
    }
    draw()
    return () => {
      cancelAnimationFrame(animId)
      window.removeEventListener('resize', resize)
    }
  }, [])

  return <canvas ref={canvasRef} className="particle-canvas" />
}

// ── Main Component ────────────────────────────────────────────────────
export default function LandingPage() {
  const navigate = useNavigate()
  const heroRef = useRef(null)
  const navRef = useRef(null) // kept for GSAP nav entrance on .app-nav
  const headlineRef = useRef(null)
  const subRef = useRef(null)
  const ctaRef = useRef(null)
  const statsCardRef = useRef(null)
  const featuresRef = useRef(null)
  const statsRef = useRef(null)
  const ctaSectionRef = useRef(null)
  const scrollLineRef = useRef(null)

  useEffect(() => {
    const ctx = gsap.context(() => {
      // ── Nav entrance (target shared Navbar) ───────────────────────
      gsap.fromTo('.app-nav',
        { y: -80, opacity: 0 },
        { y: 0, opacity: 1, duration: 0.8, ease: 'power3.out', delay: 0.1 }
      )

      // ── Hero headline stagger ─────────────────────────────────────
      gsap.fromTo('.hero-word',
        { y: 120, opacity: 0, rotateX: -40 },
        {
          y: 0, opacity: 1, rotateX: 0,
          duration: 0.9,
          ease: 'power4.out',
          stagger: 0.12,
          delay: 0.4,
        }
      )

      // ── Subheadline ───────────────────────────────────────────────
      gsap.fromTo(subRef.current,
        { y: 30, opacity: 0 },
        { y: 0, opacity: 1, duration: 0.8, ease: 'power3.out', delay: 1.0 }
      )

      // ── CTA buttons ───────────────────────────────────────────────
      gsap.fromTo('.cta-btn',
        { y: 30, opacity: 0, scale: 0.9 },
        { y: 0, opacity: 1, scale: 1, duration: 0.7, ease: 'back.out(1.5)', stagger: 0.12, delay: 1.2 }
      )

      // ── Stats card floating animation ─────────────────────────────
      gsap.fromTo(statsCardRef.current,
        { x: 80, opacity: 0, rotateY: 15 },
        { x: 0, opacity: 1, rotateY: 0, duration: 1.0, ease: 'power3.out', delay: 0.8 }
      )

      // ── Continuous floating ───────────────────────────────────────
      gsap.to(statsCardRef.current, {
        y: '-=12', duration: 2.5, ease: 'sine.inOut', yoyo: true, repeat: -1
      })

      // ── Scroll line grow ──────────────────────────────────────────
      gsap.fromTo(scrollLineRef.current,
        { scaleY: 0 },
        { scaleY: 1, duration: 1.2, ease: 'power2.inOut', delay: 1.8, transformOrigin: 'top center' }
      )

      // ── Feature cards scroll-in ───────────────────────────────────
      gsap.fromTo('.feature-card',
        { y: 80, opacity: 0, scale: 0.95 },
        {
          y: 0, opacity: 1, scale: 1,
          duration: 0.8,
          ease: 'power3.out',
          stagger: 0.15,
          scrollTrigger: {
            trigger: featuresRef.current,
            start: 'top 75%',
          }
        }
      )

      gsap.fromTo('.features-tag, .features-title, .features-subtitle',
        { y: 40, opacity: 0 },
        {
          y: 0, opacity: 1, duration: 0.7, ease: 'power3.out', stagger: 0.1,
          scrollTrigger: { trigger: featuresRef.current, start: 'top 80%' }
        }
      )

      // ── Stats section ─────────────────────────────────────────────
      gsap.fromTo('.stats-section-inner',
        { y: 60, opacity: 0 },
        {
          y: 0, opacity: 1, duration: 0.8, ease: 'power3.out',
          scrollTrigger: { trigger: statsRef.current, start: 'top 80%' }
        }
      )

      // ── CTA section ───────────────────────────────────────────────
      gsap.fromTo('.cta-section-content',
        { y: 60, opacity: 0, scale: 0.97 },
        {
          y: 0, opacity: 1, scale: 1, duration: 0.9, ease: 'power3.out',
          scrollTrigger: { trigger: ctaSectionRef.current, start: 'top 75%' }
        }
      )

      // ── Parallax hero bg ──────────────────────────────────────────
      ScrollTrigger.create({
        trigger: heroRef.current,
        start: 'top top',
        end: 'bottom top',
        onUpdate: (self) => {
          gsap.set('.hero-bg-orb-1', { y: self.progress * 80 })
          gsap.set('.hero-bg-orb-2', { y: self.progress * -50 })
        }
      })

      // ── Navbar scroll class handled inside Navbar component ───────

    })

    return () => ctx.revert()
  }, [])

  return (
    <div className="lp-wrapper">
      {/* ── NAVBAR (shared) ────────────────────────────────────── */}
      <Navbar />

      {/* ── HERO ──────────────────────────────────────────────────── */}
      <section className="lp-hero" ref={heroRef}>
        <ParticleField />
        <div className="hero-bg-orb hero-bg-orb-1" />
        <div className="hero-bg-orb hero-bg-orb-2" />

        <div className="hero-content">
          <div className="hero-left">
            <div className="hero-badge">
              <span className="badge-dot" />
              <span>KINETIC PRECISION ENGINE</span>
            </div>

            <h1 className="hero-headline" aria-label="Track. Train. Dominate.">
              <span className="hero-word hw-track">TRACK.</span>{' '}
              <span className="hero-word hw-train">TRAIN.</span>{' '}
              <span className="hero-word hw-dom">
                <span className="dom-glow">DOMINATE.</span>
              </span>
            </h1>

            <p className="hero-sub" ref={subRef}>
              Your ultimate performance tracker — log workouts, track progress,
              and crush your goals with precision-engineered analytics.
            </p>

            <div className="hero-ctas" ref={ctaRef}>
              <button
                id="hero-start-btn"
                className="cta-btn cta-primary"
                onClick={() => navigate('/calendar')}
              >
                <span>Start Tracking</span>
                <svg viewBox="0 0 20 20" fill="currentColor" className="btn-arrow">
                  <path fillRule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clipRule="evenodd"/>
                </svg>
              </button>
              <a href="#features" className="cta-btn cta-ghost">
                See How It Works
              </a>
            </div>

            <div className="hero-proof">
              <div className="proof-avatars">
                {['A','B','C','D'].map((l,i) => (
                  <div key={i} className="proof-avatar" style={{ '--i': i }}>{l}</div>
                ))}
              </div>
              <p className="proof-text"><strong>5,000+</strong> athletes already tracking</p>
            </div>
          </div>

          <div className="hero-right">
            <div className="stats-card-3d" ref={statsCardRef}>
              <div className="stats-card-header">
                <span className="card-live-dot" />
                <span className="card-live-text">LIVE METRICS</span>
                <span className="card-date">Today</span>
              </div>
              <div className="stats-card-ring">
                <svg viewBox="0 0 120 120" className="ring-svg">
                  <circle cx="60" cy="60" r="52" className="ring-track"/>
                  <circle cx="60" cy="60" r="52" className="ring-progress"/>
                  <text x="60" y="55" className="ring-label-sm">PROGRESS</text>
                  <text x="60" y="72" className="ring-big">78%</text>
                </svg>
              </div>
              <div className="stats-card-metrics">
                {[
                  { icon: '👟', label: 'STEPS', value: '12,402' },
                  { icon: '🔥', label: 'KCAL', value: '840' },
                  { icon: '❤️', label: 'BPM', value: '142' },
                ].map((m) => (
                  <div key={m.label} className="metric-row">
                    <span className="metric-icon">{m.icon}</span>
                    <div className="metric-info">
                      <span className="metric-label">{m.label}</span>
                      <span className="metric-value">{m.value}</span>
                    </div>
                    <div className="metric-bar">
                      <div className="metric-bar-fill" style={{ width: m.label === 'STEPS' ? '82%' : m.label === 'KCAL' ? '70%' : '90%' }} />
                    </div>
                  </div>
                ))}
              </div>
              <div className="card-glow" />
            </div>
          </div>
        </div>

        <div className="hero-scroll-indicator">
          <div className="scroll-line" ref={scrollLineRef} />
          <span className="scroll-label">SCROLL</span>
        </div>
      </section>

      {/* ── FEATURES ──────────────────────────────────────────────── */}
      <section className="lp-features" id="features" ref={featuresRef}>
        <div className="container">
          <div className="features-tag">PERFORMANCE SUITE</div>
          <h2 className="features-title">Why Athletes Choose Us</h2>
          <p className="features-subtitle">
            Precision metrics for those who refuse to settle for second best.
          </p>
          <div className="features-grid">
            {[
              {
                icon: <FlameIcon />,
                tag: 'INTENSITY',
                title: 'Intensity Monitoring',
                desc: 'Real-time calorie tracking and effort analysis that keeps you in the optimal zone for maximum gains.',
                color: '#E5191A',
              },
              {
                icon: <ChartIcon />,
                tag: 'PROGRESS',
                title: 'Elite Progress Analytics',
                desc: 'Deep-dive telemetrics and historical data visualization to ensure your growth never plateaus.',
                color: '#E5191A',
              },
              {
                icon: <CalendarIcon />,
                tag: 'SCHEDULING',
                title: 'Smart Scheduling',
                desc: 'Integrated calendar and recovery tools to balance high-intensity output with necessary recovery.',
                color: '#E5191A',
              },
              {
                icon: <TargetIcon />,
                tag: 'GOALS',
                title: 'Goal Engineering',
                desc: 'Set precision targets, get automated micro-milestone breakdowns and stay locked on your objective.',
                color: '#E5191A',
              },
            ].map((f, i) => (
              <div key={i} className="feature-card" id={`feature-card-${i}`}>
                <div className="feature-icon-wrap" style={{ '--accent': f.color }}>
                  {f.icon}
                </div>
                <div className="feature-tag-label">{f.tag}</div>
                <h3 className="feature-title">{f.title}</h3>
                <p className="feature-desc">{f.desc}</p>
                <div className="feature-arrow">
                  <svg viewBox="0 0 20 20" fill="currentColor">
                    <path fillRule="evenodd" d="M10.293 3.293a1 1 0 011.414 0l6 6a1 1 0 010 1.414l-6 6a1 1 0 01-1.414-1.414L14.586 11H3a1 1 0 110-2h11.586l-4.293-4.293a1 1 0 010-1.414z" clipRule="evenodd"/>
                  </svg>
                </div>
                <div className="feature-card-glow" />
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* ── STATS SECTION ─────────────────────────────────────────── */}
      <section className="lp-stats" id="stats" ref={statsRef}>
        <div className="stats-section-inner">
          <div className="stats-divider" />
          <div className="stats-grid">
            <AnimatedStat value="5000" label="Workouts Logged Every Hour" suffix="+" />
            <div className="stats-vdivider" />
            <AnimatedStat value="98" label="Athlete Satisfaction Rate" suffix="%" />
            <div className="stats-vdivider" />
            <AnimatedStat value="50" label="Specialized Exercise Categories" suffix="+" />
          </div>
          <div className="stats-divider" />
        </div>
      </section>

      {/* ── PROCESS SECTION ───────────────────────────────────────── */}
      <section className="lp-process">
        <div className="container">
          <div className="features-tag">HOW IT WORKS</div>
          <h2 className="features-title" style={{ marginBottom: '3rem' }}>Three Steps to Domination</h2>
          <div className="process-steps">
            {[
              { num: '01', title: 'Pick a Date', desc: 'Open the performance calendar and select any training day to begin your session.' },
              { num: '02', title: 'Log Your Workout', desc: 'Browse 50+ exercise categories, set reps, sets, and intensity — all in one sharp interface.' },
              { num: '03', title: 'Track Progress', desc: 'Visualize your performance data, spot trends, and optimize your training over time.' },
            ].map((step, i) => (
              <div key={i} className="process-step process-step-animated">
                <div className="step-num">{step.num}</div>
                <div className="step-connector" />
                <div className="step-content">
                  <h3 className="step-title">{step.title}</h3>
                  <p className="step-desc">{step.desc}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* ── CTA SECTION ───────────────────────────────────────────── */}
      <section className="lp-cta-section" id="cta" ref={ctaSectionRef}>
        <div className="cta-section-content">
          <div className="cta-orb-1" />
          <div className="cta-orb-2" />
          <div className="features-tag" style={{ color: '#E5191A' }}>READY TO START</div>
          <h2 className="cta-headline">
            Pick a Date.<br />
            <span className="cta-headline-red">Start Your Journey.</span>
          </h2>
          <p className="cta-sub">
            Join thousands of athletes who track smarter, not just harder.
          </p>
          <button
            id="cta-open-tracker-btn"
            className="cta-mega-btn"
            onClick={() => navigate('/calendar')}
          >
            <span>Open Tracker</span>
            <div className="cta-btn-shine" />
          </button>
        </div>
      </section>

      {/* ── FOOTER ────────────────────────────────────────────────── */}
      <footer className="lp-footer">
        <div className="footer-inner">
          <div className="footer-logo">
            <span className="logo-mark">⚡</span>
            <span className="logo-text">FITNESS<span className="logo-accent">TRACK</span></span>
          </div>
          <div className="footer-links">
            <a href="#" className="footer-link">Privacy</a>
            <a href="#" className="footer-link">Terms</a>
            <a href="#" className="footer-link">Support</a>
          </div>
          <p className="footer-copy">© 2026 FitnessTrack. All rights reserved.</p>
        </div>
      </footer>
    </div>
  )
}