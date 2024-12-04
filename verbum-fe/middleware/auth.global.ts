import { decodeToken, isTokenExpired } from '~/lib/auth/auth'

export default defineNuxtRouteMiddleware(async (to) => {
  const { isAuthenticated } = storeToRefs(useAuthStore())
  const access_token = useCookie('access_token')
  const user = decodeToken(access_token.value)
  const unprotectedRoutes = ['/', '/login', '/signup']
  const employeeRoutes = ['/works', '/jobs', '/issues']
  const adminRoutes = ['/users', '/languages', '/discounts', '/categories ']
  const clientRoutes = ['/orders', '/issues', '/receipts']
  const directorRoutes = ['/orders']
  const staffRoutes = ['/orders']
  const redirectPath = '/redirect'
  const isConfirmEmailRoute = to.path.startsWith('/confirm-email')


  if (access_token?.value ) {
    try {
      if (isTokenExpired(access_token?.value)) {
        const config = useRuntimeConfig()
        const res = await fetch(`${config.public.baseUrl}/api/auth/refresh-token`, {
          method: 'POST',
          headers: { Authorization: `Bearer ${access_token.value}` },
          credentials: 'include'
        });
        if (res.ok) {
          const data = await res.json();
          access_token.value = data.access_token;
        } else {
          abortNavigation()
          useAuth().logout();
        }
      }
      if (user) {
        useAuthStore().setUser(user)
        if (user?.role.includes('CLIENT') && !clientRoutes.some(route => to.path.includes(route)))
          return navigateTo('/orders')
        if (user?.role.includes('MANAGER') && !employeeRoutes.some(route => to.path.includes(route)))
          return navigateTo('/works')
        else if (user?.role === 'LINGUIST' && !employeeRoutes.some(route => to.path.includes(route)))
          return navigateTo('/works')
        else if (user?.role.includes('ADMINISTRATOR') && !adminRoutes.some(route => to.path.includes(route)))
          return navigateTo('/users')
        else if (user?.role.includes('DIRECTOR') && (!directorRoutes.some(route => to.path.includes(route)) || to.path.includes('/create')))
          return navigateTo('/orders')
        else if (user?.role.includes('STAFF') && !staffRoutes.some(route => to.path.includes(route)))
          return navigateTo('/order')
        else if (to.path.includes(redirectPath))
          return navigateTo('/')
        else return
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
