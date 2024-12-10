import { decodeToken, isTokenExpired } from "~/lib/auth/auth"
export default defineNuxtRouteMiddleware(async (to) => {
  const { isAuthenticated } = storeToRefs(useAuthStore())
  let access_token = useCookie('access_token').value
  const user = access_token ? decodeToken(access_token) : null
  const unprotectedRoutes = ['/', '/login', '/signup', '/resend', '/confirm-email']

  if (unprotectedRoutes.some(route => to.path.startsWith(route))) {
    return
  }

  // If access_token exists and user is decoded
  if (access_token && user) {
    try {
      if (isTokenExpired(access_token)) {
        const config = useRuntimeConfig()
        const res = await fetch(`${config.public.baseUrl}/api/auth/refresh-token`, {
          method: 'POST',
          headers: { Authorization: `Bearer ${access_token}` },
          credentials: 'include'
        })

        if (!res.ok) {
          console.log('Token refresh failed. Redirecting to login.')
          useAuthStore().clearUser()
          localStorage.removeItem('access_token')
          return navigateTo('/login')
        }

        const data = await res.json()
        access_token = data.access_token
      }

      // role-based navigation
      if (isAuthenticated.value) {
        const roleRoutes = {
          CLIENT: { routes: ['/orders', '/issues', '/receipts'], default: '/orders' },
          MANAGER: { routes: ['/works', '/jobs', '/issues'], default: '/works' },
          LINGUIST: { routes: ['/works', '/jobs', '/issues'], default: '/works' },
          ADMIN: { routes: ['/users', '/languages', '/categories'], default: '/languages' },
          DIRECTOR: { routes: ['/orders', '/discounts'], default: '/orders' },
          STAFF: { routes: ['/orders'], default: '/orders' }
        }

        const userRole = Object.keys(roleRoutes).find(role => user.role.includes(role))
        console.log('User Role:', userRole)
        if (userRole) {
          const { routes, default: defaultRoute } = roleRoutes[userRole]
          if (!routes.some(route => to.path.startsWith(route))) {
            console.log('Redirecting to role default route:', defaultRoute)
            return navigateTo(defaultRoute)
          }
        }
      }
      return
    } catch (error) {
      console.error('Error in auth middleware:', error)
    }
  }

  // Redirect to login if not authenticated
  console.log('Not authenticated. Redirecting to login.')
  useAuthStore().clearUser()
  localStorage.removeItem('access_token')
  return navigateTo('/login')
})
