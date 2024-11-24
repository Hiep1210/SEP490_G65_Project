/* eslint-disable @typescript-eslint/no-explicit-any */
import { useToast } from '~/components/ui/toast'
import { ref } from 'vue'
import { decodeToken } from '~/lib/auth/auth'
import type { User } from '~/types/user'
const { toast } = useToast()

export const useAuth = () => {
  const user = ref<User | null>(null)
  const accessToken = ref<string | null>(null)
  const refreshToken = ref<string | null>(null)
  const authStore = useAuthStore()

  const router = useRouter()
  const config = useRuntimeConfig()
  const login = async (credentials: any) => {
    try {
      const response = await fetch(`${config.public.baseUrl}/api/auth/login`, {
        method: 'POST',
        body: JSON.stringify(credentials),
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include'
      })

      if (!response.ok) {
        toast({
          title: 'Failed to login',
          description: 'An error occurred while logging in'
        })
        throw new Error('Failed to login')
      }

      accessToken.value =
        document.cookie
          .split('; ')
          .find((row) => row.startsWith('access_token'))
          ?.split('=')[1] || null

      if (!accessToken.value) {
        throw new Error('No access token found')
      }
      const decodedUser = decodeToken(accessToken.value as string)
      if (!decodedUser) {
        throw new Error('Invalid access token')
      }
      authStore.setUser(decodedUser)

      toast({
        title: 'Logged in',
        description: 'You have been logged in successfully'
      })
      setTimeout(() => {
        router.push('/orders')
      }, 2000)
    } catch (error) {
      console.error('Login error:', error)
      toast({
        title: 'Error',
        description: 'Something went wrong'
      })
    }
  }

  const signup = async (credentials: any) => {
    const response = await fetch(`${config.public.baseUrl}/api/auth/signup`, {
      method: 'POST',
      body: JSON.stringify(credentials),
      headers: { 'Content-Type': 'application/json' }
    })

    if (!response.ok) {
      toast({
        title: 'Failed to sign up',
        description: 'An error occurred while signing up'
      })
      throw new Error('Failed to sign up')
    }
    toast({
      title: 'Account created',
      description: 'Your account has been created successfully'
    })

    setTimeout(() => {
      router.push('/login')
    }, 2000)
  }

  const logout = async () => {
    user.value = null
    accessToken.value = null
    refreshToken.value = null
    authStore.clearUser()
    navigateTo('/login')
  }
   const refreshAccessToken = async () => {
    try {
      const response = await fetch(`${config.public.baseUrl}/api/auth/refresh-token`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include'
      })

      if (!response.ok) {
        if(confirm('Your session has expired. Please login again.')) {
          logout()
        }
        throw new Error('Failed to refresh token')
      }

      const data = await response.json()
      accessToken.value = data.accessToken
      refreshToken.value = data.refreshToken
      const decodedUser = decodeToken(accessToken.value as string)
    } catch (error) {
      console.error('Failed to refresh token:', error)
      toast({
        title: 'Error',
        description: 'Failed to refresh token'
      })
    }
  }

  const googleAuth = async () => {
    window.open(`${config.public.baseUrl}/auth/google-login`)
  }
  // Refresh token silently
  return {
    user: user.value,
    login,
    logout,
    signup,
    googleAuth,
    refreshAccessToken
  }
}
