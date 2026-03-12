<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useIdleTimeout } from '@/composables/useIdleTimeout'

const authStore = useAuthStore()
const router = useRouter()
const sidebarCollapsed = ref(false)

useIdleTimeout()

const menuItems = [
  { label: '儀表板', icon: 'pi pi-home', to: '/backstage' },
  { label: '頁面管理', icon: 'pi pi-file', to: '/backstage/pages' },
  { label: '新聞管理', icon: 'pi pi-megaphone', to: '/backstage/news' },
  { label: '相簿管理', icon: 'pi pi-images', to: '/backstage/albums' },
  { label: '聯絡表單', icon: 'pi pi-envelope', to: '/backstage/contacts' },
  { label: '首頁輪播', icon: 'pi pi-desktop', to: '/backstage/homepage' },
  { label: '交通資訊', icon: 'pi pi-car', to: '/backstage/traffic' },
  { label: '網站設定', icon: 'pi pi-cog', to: '/backstage/settings' },
  { label: '媒體庫', icon: 'pi pi-cloud-upload', to: '/backstage/media' },
]

const handleLogout = () => {
  authStore.logout()
  router.push({ name: 'backstageLogin' })
}
</script>

<template>
  <div class="min-h-screen flex bg-gray-100">
    <!-- Sidebar -->
    <aside
      :class="[
        'bg-dark text-white flex flex-col transition-all duration-300',
        sidebarCollapsed ? 'w-16' : 'w-60',
      ]"
    >
      <div class="p-4 border-b border-gold/20 flex items-center justify-between">
        <span v-if="!sidebarCollapsed" class="text-gold font-serif font-bold">星月大地管理</span>
        <button class="text-gold" @click="sidebarCollapsed = !sidebarCollapsed">
          <i :class="sidebarCollapsed ? 'pi pi-angle-right' : 'pi pi-angle-left'" />
        </button>
      </div>
      <nav class="flex-1 py-2">
        <RouterLink
          v-for="item in menuItems"
          :key="item.to"
          :to="item.to"
          class="flex items-center gap-3 px-4 py-3 text-sm hover:bg-gold/10 hover:text-gold transition-colors"
          active-class="bg-gold/20 text-gold border-r-2 border-gold"
        >
          <i :class="item.icon" />
          <span v-if="!sidebarCollapsed">{{ item.label }}</span>
        </RouterLink>
      </nav>
    </aside>

    <!-- Main Content -->
    <div class="flex-1 flex flex-col">
      <!-- Top Bar -->
      <header class="bg-white shadow-sm px-6 py-3 flex items-center justify-between">
        <h1 class="text-lg font-bold text-dark">後台管理</h1>
        <div class="flex items-center gap-4">
          <span class="text-sm text-gray-600">{{ authStore.user?.displayName }}</span>
          <button class="text-sm text-red-accent hover:underline" @click="handleLogout">
            登出
          </button>
        </div>
      </header>

      <!-- Content -->
      <main class="flex-1 p-6 overflow-auto">
        <RouterView />
      </main>
    </div>
  </div>
</template>
