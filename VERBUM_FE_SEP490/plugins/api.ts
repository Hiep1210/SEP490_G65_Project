import { useToast } from '~/components/ui/toast'
const { toast } = useToast()

export default defineNuxtPlugin((nuxtApp) => {
  const config = useRuntimeConfig()
  const access_token = useCookie('access_token')
  const router = useRouter()
  const api = $fetch.create({
    baseURL: config.public.baseUrl + '/api',
    retry: 1,
    retryStatusCodes: [500, 502, 503, 504, 401],
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
        const oldToken = localStorage.getItem('access_token')
        const res = await fetch(
          `${config.public.baseUrl}/api/auth/refresh-token`,
          {
            method: 'POST',
            headers: {
              Authorization: `Bearer ${oldToken}`
            },
            credentials: 'include'
          }
        )
        if (res.ok) {
          const data = await res.json()
          localStorage.setItem('access_token', data.access_token)
        } else {
          await nuxtApp.runWithContext(() => {
            if (confirm('Your session has expired. Please login again.')) {
              useAuth().logout()
            }
          })
        }
      }
      if (response.status === 403) {
        await nuxtApp.runWithContext(() => {
          if (confirm('You are not authorized to access this resource.')) {
            router.back()
          }
        })
      }
      if (response.status === 404) {
        await nuxtApp.runWithContext(() => {
          toast({
            title: 'Not Found',
            description: 'The resource you are looking for does not exist.',
            variant: 'destructive'
          })
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
