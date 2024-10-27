export default defineNuxtPlugin((nuxtApp) => {
  const access_token = useCookie('access_token')

  const api = $fetch.create({
    baseURL: 'http://localhost:8000/api',
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
    }
  })
  return {
    provide: {
      api
    }
  }
})
