/* eslint-disable @typescript-eslint/no-explicit-any */
import { useToast } from '~/components/ui/toast'
import { ref } from 'vue'
import { decodeToken } from '~/lib/auth/auth'
import type { User } from '~/types/user'
const { toast } = useToast()

export const useAuth = () => {
  const user = ref<User | null>(null)
  const accessToken = ref<string | null | undefined>(null)
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
        if (response.status === 400) {
          toast({
            title: 'Invalid credentials',
            description: 'Please check your email and password'
          })
        } else {
          toast({
            title: 'Login error',
            description: 'An error occurred while logging in'
          })
        }
      }

      accessToken.value = useCookie('access_token').value

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
    accessToken.value = null
    refreshToken.value = null
    authStore.clearUser()
    useRouter().push('/login')
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
    googleAuth
  }
}
