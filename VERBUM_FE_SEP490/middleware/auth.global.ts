/* eslint-disable @typescript-eslint/no-unused-vars */
export default defineNuxtRouteMiddleware((to, from) => {
  const { isAuthenticated } = storeToRefs(useAuthStore())

  // Check if the route is not login, signup, or confirm-email (with dynamic params)
  const unprotectedRoutes = ['/login', '/signup']
  const isConfirmEmailRoute = to.path.startsWith('/confirm-email')

  if (
    !isAuthenticated.value &&
    !unprotectedRoutes.includes(to.path) &&
    !isConfirmEmailRoute
  ) {
    return navigateTo('/login')
  }

  // If user is authenticated and trying to access login or signup
  if (isAuthenticated.value) {
    if (to.path === '/login') {
      return navigateTo('/orders')
    } else if (to.path === '/signup') {
      useAuth().logout()
      return navigateTo('/signup')
    }
  }
})
