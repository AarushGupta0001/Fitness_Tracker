import { getToken } from './auth'

const configuredApiBaseUrl = import.meta.env.VITE_API_BASE_URL

const API_BASE_URLS = configuredApiBaseUrl
  ? [configuredApiBaseUrl.replace(/\/$/, '')]
  : [
      'http://localhost:5179/api',
      'http://localhost:5180/api',
      'http://localhost:5181/api',
      '/api',
    ]

const withJsonHeaders = (options = {}) => ({
  ...options,
  headers: {
    'Content-Type': 'application/json',
    ...(options.headers || {}),
  },
})

const withAuthHeaders = (options = {}) => {
  const token = getToken()
  if (!token) {
    return options
  }

  return {
    ...options,
    headers: {
      ...(options.headers || {}),
      Authorization: `Bearer ${token}`,
    },
  }
}

const requestWithFallback = async (path, options = {}) => {
  let lastError = null

  for (const baseUrl of API_BASE_URLS) {
    try {
      const response = await fetch(`${baseUrl}${path}`, options)
      if (!response.ok) {
        const text = await response.text()
        throw new Error(text || `Request failed with status ${response.status}`)
      }

      return response
    } catch (error) {
      lastError = error

      // Retry only for network-level failures (server down, CORS/network unreachable).
      if (!(error instanceof TypeError)) {
        break
      }
    }
  }

  throw lastError || new Error('Failed to connect to backend API')
}

export const api = {
  login: async (payload) => {
    const response = await requestWithFallback(
      '/Auth/login',
      withJsonHeaders({
        method: 'POST',
        body: JSON.stringify(payload),
      })
    )
    return response.json()
  },
  register: async (payload) => {
    const response = await requestWithFallback(
      '/Auth/register',
      withJsonHeaders({
        method: 'POST',
        body: JSON.stringify(payload),
      })
    )
    return response.json()
  },
  // Workout Sessions
  createWorkoutSession: async (username, selectedMuscleGroups, date, fatigueLevel = null) => {
    const response = await requestWithFallback(
      '/WorkoutSessions',
      withAuthHeaders(withJsonHeaders({
        method: 'POST',
        body: JSON.stringify({ username, selectedMuscleGroups, date, fatigueLevel }),
      }))
    )
    return response.json()
  },

  getWorkoutSessionsByDate: async (date) => {
    const response = await requestWithFallback(
      `/WorkoutSessions/date/${date}`,
      withAuthHeaders()
    )
    return response.json()
  },

  // Exercises
  createExercise: async (exercise) => {
    const response = await requestWithFallback(
      '/Exercises',
      withAuthHeaders(withJsonHeaders({
        method: 'POST',
        body: JSON.stringify(exercise),
      }))
    )
    return response.json()
  },

  getExercisesByDate: async (date) => {
    const response = await requestWithFallback(
      `/Exercises/date/${date}`,
      withAuthHeaders()
    )
    return response.json()
  },

  getExercisesByDateAndMuscleGroup: async (date, muscleGroup) => {
    const response = await requestWithFallback(
      `/Exercises/date/${date}/musclegroup/${muscleGroup}`,
      withAuthHeaders()
    )
    return response.json()
  },

  getExerciseTemplates: async (muscleGroups = []) => {
    const query = muscleGroups.length > 0
      ? `?groups=${encodeURIComponent(muscleGroups.join(','))}`
      : ''

    const response = await requestWithFallback(`/ExerciseTemplates${query}`)
    return response.json()
  },

  getWeightCatalog: async () => {
    const response = await requestWithFallback('/Weights/catalog')
    return response.json()
  },

  deleteExercise: async (id) => {
    await requestWithFallback(`/Exercises/${id}`, withAuthHeaders({ method: 'DELETE' }))
  },

  createExerciseLogsBulk: async (payload) => {
    const response = await requestWithFallback(
      '/ExerciseLogs/bulk',
      withAuthHeaders(withJsonHeaders({
        method: 'POST',
        body: JSON.stringify(payload),
      }))
    )

    return response.json()
  },

  getWorkoutLogs: async (username = '') => {
    const query = username ? `?username=${encodeURIComponent(username)}` : ''
    const response = await requestWithFallback(
      `/WorkoutLogs${query}`,
      withAuthHeaders()
    )
    return response.json()
  },

  getLastSimilarWorkout: async (muscleGroup, username = '', excludeSessionId = null) => {
    const query = new URLSearchParams({ muscleGroup })
    if (username) {
      query.set('username', username)
    }
    if (excludeSessionId != null) {
      query.set('excludeSessionId', String(excludeSessionId))
    }

    const response = await requestWithFallback(
      `/WorkoutLogs/last-similar?${query.toString()}`,
      withAuthHeaders()
    )

    return response.json()
  },

  getProgressiveOverloadInsight: async (muscleGroup, username = '') => {
    const query = new URLSearchParams({ muscleGroup })
    if (username) {
      query.set('username', username)
    }

    const response = await requestWithFallback(
      `/Insights/progressive-overload?${query.toString()}`,
      withAuthHeaders()
    )

    return response.json()
  },

  getWeeklyPerformanceInsight: async (weekStart = '', username = '') => {
    const query = new URLSearchParams()
    if (weekStart) {
      query.set('weekStart', weekStart)
    }
    if (username) {
      query.set('username', username)
    }

    const suffix = query.toString() ? `?${query.toString()}` : ''
    const response = await requestWithFallback(
      `/Insights/weekly-performance${suffix}`,
      withAuthHeaders()
    )

    return response.json()
  },

  getRecoveryInsight: async (days = 14, username = '') => {
    const query = new URLSearchParams({ days: String(days) })
    if (username) {
      query.set('username', username)
    }

    const response = await requestWithFallback(
      `/Insights/recovery?${query.toString()}`,
      withAuthHeaders()
    )

    return response.json()
  },
}
