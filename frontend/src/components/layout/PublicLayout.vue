<script setup lang="ts">
import PublicHeader from './PublicHeader.vue'
import PublicFooter from './PublicFooter.vue'
import { useSettingsStore } from '@/stores/settings'

const settingsStore = useSettingsStore()
settingsStore.fetchSettings()
</script>

<template>
  <div class="min-h-screen flex flex-col">
    <PublicHeader />
    <main class="flex-1">
      <RouterView />
    </main>
    <PublicFooter />
  </div>
</template>
