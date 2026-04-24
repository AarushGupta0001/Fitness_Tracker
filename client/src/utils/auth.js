const TOKEN_KEY = 'fitness_tracker_token'
const USERNAME_KEY = 'fitness_tracker_username'
const GUEST_KEY = 'fitness_tracker_guest'

export const setAuth = ({ token, username }) => {
  localStorage.setItem(TOKEN_KEY, token)
  localStorage.setItem(USERNAME_KEY, username)
}

export const clearAuth = () => {
  localStorage.removeItem(TOKEN_KEY)
  localStorage.removeItem(USERNAME_KEY)
  localStorage.removeItem(GUEST_KEY)
}

export const setGuestMode = () => {
  localStorage.setItem(GUEST_KEY, 'true')
  localStorage.setItem(USERNAME_KEY, 'Guest')
}

export const isGuest = () => localStorage.getItem(GUEST_KEY) === 'true'

export const getToken = () => localStorage.getItem(TOKEN_KEY)

export const getUsername = () => localStorage.getItem(USERNAME_KEY) || ''

export const isAuthenticated = () => Boolean(getToken()) || isGuest()
