export default defineNuxtPlugin((nuxtApp) => {
  const config = useRuntimeConfig()
  const access_token = useCookie('access_token')
  const router = useRouter()
  const api = $fetch.create({
    baseURL: config.public.baseUrl + '/api',
    onRequest({ options }) {
      if (access_token?.value) {
        const headers = new Headers(options.headers)
        headers.set('Authorization', `Bearer ${access_token?.value}`)
        options.headers = headers
        options.credentials = 'include'
      }
    },
    async onResponseError({ response }) {
      if (response.status === 401) {
        await nuxtApp.runWithContext(() => {
          if (confirm('Your session has expired. Please login again.')) {
            useAuth().logout()
          }
        })
      }
      if (response.status === 403) {
        await nuxtApp.runWithContext(() => {
          if (confirm('You are not authorized to access this resource.')) {
            router.back()
          }
        })
      }
    }
  })
  return {
    provide: {
      api
    }
  }
})
