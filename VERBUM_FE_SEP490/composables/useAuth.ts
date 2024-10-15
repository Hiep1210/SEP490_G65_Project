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
  let refreshInterval: number | null = null

  const router = useRouter()

  const login = async (credentials: any) => {
    try {
      const response = await fetch(`http://localhost:8000/api/auth/login`, {
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
    const response = await fetch(`http://localhost:8000/api/auth/signup`, {
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
    router.push('/login')
  }

  const googleAuth = async () => {
    window.open('http://localhost:8000/api/auth/google-login')
  }
  // Refresh token silently
  const silentTokenRefresh = async () => {
    if (!authStore.user) {
      console.log('No user logged in, skipping refresh')
      return // Skip if no user is logged in
    }

    const response = await fetch(
      'http://localhost:8000/api/auth/refresh-token',
      {
        method: 'POST',
        credentials: 'include'
      }
    )

    if (response.ok) {
      const data = await response.json()
      console.log(data)
    } else {
      useAuthStore().clearUser()
      router.push('/login')
    }
  }

  const handleVisibilityChange = () => {
    if (document.visibilityState === 'visible') {
      startSilentRefresh()
    } else {
      stopSilentRefresh()
    }
  }

  const startSilentRefresh = () => {
    if (!useAuthStore().user || refreshInterval !== null) return // Skip if no user or refresh already running
    // silentTokenRefresh() // Do an immediate refresh
    refreshInterval = window.setInterval(silentTokenRefresh, 60 * 60 * 1000) // Refresh every 60 minutes
  }

  const stopSilentRefresh = () => {
    if (refreshInterval !== null) {
      clearInterval(refreshInterval)
      refreshInterval = null
    }
  }

  return {
    user: user.value,
    login,
    logout,
    signup,
    googleAuth,
    handleVisibilityChange,
    startSilentRefresh,
    stopSilentRefresh
  }
}
