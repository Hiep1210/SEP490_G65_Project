import { useToast } from '~/components/ui/toast'
const { toast } = useToast()

export default defineNuxtPlugin((nuxtApp) => {
  const config = useRuntimeConfig()
  const access_token = useCookie('access_token')

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
      switch (response.status) {
        case 401:
          await nuxtApp.runWithContext(async () => {
            const res = await fetch(
              `${config.public.baseUrl}/api/auth/refresh-token`,
              {
                method: 'POST',
                headers: {
                  Authorization: `Bearer ${access_token.value}`
                },
                credentials: 'include'
              }
            )
            if (res.ok) {
              const data = await res.json()
              console.log('New access token:', data.access_token)
              access_token.value = data.access_token
            } else {
              toast({
                title: 'Unauthorized',
                description: 'Please log in again',
                variant: 'destructive'
              })
              access_token.value = null
              if (confirm('Session expired. Please log in again.')) {
                useAuth().logout()
              }
            }
          })
          break
        case 403:
          await nuxtApp.runWithContext(() => {
            toast({
              title: 'Access Denied',
              description:
                'You do not have permission to access this resource.',
              variant: 'destructive'
            })
          })
          break
        case 404:
          toast({
            title: 'Not Found',
            description: 'The resource you are looking for does not exist.',
            variant: 'destructive'
          })
          break
        default:
          toast({
            title: 'Error',
            description: `An error occurred: ${response.statusText}`,
            variant: 'destructive'
          })
          break
      }
    }
  })
  return {
    provide: {
      api
    }
  }
})
