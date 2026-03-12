<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
const router = useRouter()
const route = useRoute()

const email = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)

const handleLogin = async () => {
  error.value = ''
  loading.value = true
  try {
    await authStore.login(email.value, password.value)
    const redirect = (route.query.redirect as string) || '/backstage'
    router.push(redirect)
  } catch (e: any) {
    error.value = e.message || '登入失敗'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="min-h-screen bg-dark flex items-center justify-center">
    <div class="bg-white rounded-lg shadow-xl p-8 w-full max-w-md">
      <h1 class="text-2xl font-bold text-center mb-2">星月大地</h1>
      <p class="text-center text-gray-text mb-6">後台管理系統</p>

      <form @submit.prevent="handleLogin">
        <div class="mb-4">
          <label class="block text-sm font-medium mb-1">Email</label>
          <input
            v-model="email"
            type="email"
            required
            class="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-gold"
            placeholder="admin@starmoonland.com"
          />
        </div>
        <div class="mb-4">
          <label class="block text-sm font-medium mb-1">密碼</label>
          <input
            v-model="password"
            type="password"
            required
            class="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-gold"
          />
        </div>
        <p v-if="error" class="text-red-accent text-sm mb-4">{{ error }}</p>
        <button
          type="submit"
          :disabled="loading"
          class="w-full bg-gold text-white py-2 rounded hover:bg-gold-dark transition disabled:opacity-50"
        >
          {{ loading ? '登入中...' : '登入' }}
        </button>
      </form>
    </div>
  </div>
</template>
