<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { settingsApi } from '@/api/settings'
import type { HomepageData } from '@/types/api'

const data = ref<HomepageData | null>(null)

onMounted(async () => {
  try {
    const res = await settingsApi.getHomepage()
    if (res.data.success) data.value = res.data.data!
  } catch { /* 靜默 */ }
})
</script>

<template>
  <div>
    <!-- Hero Slider placeholder -->
    <section class="h-[600px] bg-dark flex items-center justify-center">
      <h1 class="text-gold font-serif text-4xl">星月大地景觀休閒園區</h1>
    </section>

    <!-- 內容區塊將在後續開發 -->
    <section class="py-16 text-center">
      <p class="text-gray-text">首頁內容建置中...</p>
    </section>
  </div>
</template>
