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
    }
  }

  throw lastError || new Error('Failed to connect to backend API')
}

export const api = {
  // Workout Sessions
  createWorkoutSession: async (username, selectedMuscleGroups, date) => {
    const response = await requestWithFallback(
      '/WorkoutSessions',
      withJsonHeaders({
        method: 'POST',
        body: JSON.stringify({ username, selectedMuscleGroups, date }),
      })
    )
    return response.json()
  },

  getWorkoutSessionsByDate: async (date) => {
    const response = await requestWithFallback(`/WorkoutSessions/date/${date}`)
    return response.json()
  },

  // Exercises
  createExercise: async (exercise) => {
    const response = await requestWithFallback(
      '/Exercises',
      withJsonHeaders({
        method: 'POST',
        body: JSON.stringify(exercise),
      })
    )
    return response.json()
  },

  getExercisesByDate: async (date) => {
    const response = await requestWithFallback(`/Exercises/date/${date}`)
    return response.json()
  },

  getExercisesByDateAndMuscleGroup: async (date, muscleGroup) => {
    const response = await requestWithFallback(`/Exercises/date/${date}/musclegroup/${muscleGroup}`)
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
    await requestWithFallback(`/Exercises/${id}`, { method: 'DELETE' })
  },

  createExerciseLogsBulk: async (payload) => {
    const response = await requestWithFallback(
      '/ExerciseLogs/bulk',
      withJsonHeaders({
        method: 'POST',
        body: JSON.stringify(payload),
      })
    )

    return response.json()
  },

  getWorkoutLogs: async (username = '') => {
    const query = username ? `?username=${encodeURIComponent(username)}` : ''
    const response = await requestWithFallback(`/WorkoutLogs${query}`)
    return response.json()
  },
}
