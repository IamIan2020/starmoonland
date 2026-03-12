import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authApi } from '@/api/auth'
import type { UserInfo } from '@/types/api'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<UserInfo | null>(null)
  const accessToken = ref<string | null>(null)

  const isAuthenticated = computed(() => !!accessToken.value)

  function init() {
    const storedToken = localStorage.getItem('accessToken')
    const storedUser = localStorage.getItem('user')
    if (storedToken) accessToken.value = storedToken
    if (storedUser) {
      try { user.value = JSON.parse(storedUser) } catch { /* ignore */ }
    }
  }

  async function login(email: string, password: string) {
    const { data } = await authApi.login({ email, password })
    if (data.success && data.data) {
      accessToken.value = data.data.accessToken
      user.value = data.data.user
      localStorage.setItem('accessToken', data.data.accessToken)
      localStorage.setItem('refreshToken', data.data.refreshToken)
      localStorage.setItem('user', JSON.stringify(data.data.user))
      return true
    }
    throw new Error(data.message || '登入失敗')
  }

  function logout() {
    user.value = null
    accessToken.value = null
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
    localStorage.removeItem('user')
  }

  init()

  return { user, accessToken, isAuthenticated, login, logout }
})
