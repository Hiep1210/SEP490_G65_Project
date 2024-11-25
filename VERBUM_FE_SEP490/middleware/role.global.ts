export default defineNuxtRouteMiddleware((to) => {
  const { user, isAuthenticated } = useAuthStore()

  if (!isAuthenticated) {
    return navigateTo('/login')
  }
  if (user?.role.includes('MANAGER') && to.path.includes('orders'))
    return navigateTo('/works')
  else if (user?.role === 'LINGUIST' && to.path.includes('orders'))
    return navigateTo('/works')
  else if (user?.role.includes('ADMIN') && to.path.includes('orders'))
    return navigateTo('/works')
  else return
})
