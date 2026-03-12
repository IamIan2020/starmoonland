import { onMounted, onUnmounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useRouter } from 'vue-router'

export function useIdleTimeout(timeoutMs = 30 * 60 * 1000) {
  const authStore = useAuthStore()
  const router = useRouter()
  let timer: ReturnType<typeof setTimeout> | null = null

  const resetTimer = () => {
    if (timer) clearTimeout(timer)
    timer = setTimeout(() => {
      if (authStore.isAuthenticated) {
        authStore.logout()
        router.push({ name: 'backstageLogin' })
      }
    }, timeoutMs)
  }

  const events = ['mousedown', 'mousemove', 'keypress', 'scroll', 'touchstart']

  onMounted(() => {
    events.forEach((e) => document.addEventListener(e, resetTimer))
    resetTimer()
  })

  onUnmounted(() => {
    events.forEach((e) => document.removeEventListener(e, resetTimer))
    if (timer) clearTimeout(timer)
  })
}
