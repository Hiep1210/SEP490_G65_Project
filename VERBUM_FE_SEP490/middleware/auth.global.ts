import { decodeToken } from '~/lib/auth/auth'

export default defineNuxtRouteMiddleware(async (to) => {
  const { isAuthenticated } = storeToRefs(useAuthStore())
  const access_token = useCookie('access_token')
  const unprotectedRoutes = ['/', '/login', '/signup']
  const isConfirmEmailRoute = to.path.startsWith('/confirm-email')

  // Check if access_token exists
  if (access_token?.value) {
    try {
      const user = decodeToken(access_token.value)
      if (user) {
        useAuthStore().setUser(user)
        // Redirect to /orders if already authenticated and accessing another route
        if (!isAuthenticated.value) {
          return navigateTo('/orders')
        }
      } else {
        // Invalid token, clear user and redirect to login
        useAuthStore().clearUser()
        return navigateTo('/login')
      }
    } catch (error) {
      console.error('Error decoding token:', error)
      useAuthStore().clearUser()
      return navigateTo('/login')
    }
  }

  if (
    !isAuthenticated.value &&
    !unprotectedRoutes.includes(to.path) &&
    !isConfirmEmailRoute
  ) {
    return navigateTo('/login')
  }

  // Handle authenticated user access
  if (isAuthenticated.value) {
    if (to.path === '/login') {
      return navigateTo('/orders')
    } else if (to.path === '/signup') {
      await useAuth().logout() // Ensure logout completes
      return navigateTo('/signup')
    }
  }
})
