import { defineStore } from 'pinia'
import type { User } from '~/types/user'

export const useAuthStore = defineStore({
  id: 'AuthStore',
  state: () => ({
    user: null as User | null,
    isAuthenticated: false
  }),
  actions: {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    setUser(user: any) {
      this.user = user
      this.isAuthenticated = true
    },
    clearUser() {
      this.user = null
      this.isAuthenticated = false
    }
  },
  persist: {
    storage: piniaPluginPersistedstate.cookies()
  }
})
