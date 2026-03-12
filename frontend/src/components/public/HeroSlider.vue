<script setup lang="ts">
import { ref, watch, onUnmounted } from 'vue'
import type { HomepageSlideDto } from '@/types/api'
import { resolveImageUrl } from '@/utils/image'

const props = defineProps<{
  slides: HomepageSlideDto[]
}>()

const current = ref(0)
let timer: ReturnType<typeof setInterval> | null = null

const startTimer = () => {
  if (timer) clearInterval(timer)
  if (props.slides.length > 1) {
    timer = setInterval(() => {
      current.value = (current.value + 1) % props.slides.length
    }, 5000)
  }
}

watch(() => props.slides.length, (len) => {
  if (len > 1) startTimer()
}, { immediate: true })

onUnmounted(() => {
  if (timer) clearInterval(timer)
})
</script>

<template>
  <section
    style="position: relative; overflow: hidden;"
    class="h-[500px] md:h-[600px] lg:h-[700px] bg-dark"
  >
    <!-- 輪播圖片 -->
    <div
      v-for="(slide, idx) in slides"
      :key="slide.id"
      style="position: absolute; inset: 0; transition: opacity 0.8s ease;"
      :style="{ opacity: idx === current ? 1 : 0 }"
    >
      <img
        :src="resolveImageUrl(slide.imageUrl)"
        :alt="slide.altText || '星月大地'"
        style="width: 100%; height: 100%; object-fit: cover;"
      />
      <div style="position: absolute; inset: 0; background: rgba(0,0,0,0.2);" />
    </div>

    <!-- 分頁點 -->
    <div
      v-if="slides.length > 1"
      style="position: absolute; bottom: 60px; left: 50%; transform: translateX(-50%); z-index: 10; display: flex; gap: 8px;"
    >
      <button
        v-for="(_, idx) in slides"
        :key="idx"
        style="width: 10px; height: 10px; border-radius: 50%; border: none; cursor: pointer; transition: background 0.3s;"
        :style="{ background: idx === current ? '#cda871' : 'rgba(255,255,255,0.5)' }"
        @click="current = idx"
      />
    </div>

    <!-- 無輪播圖 -->
    <div v-if="!slides.length" style="position: absolute; inset: 0; display: flex; align-items: center; justify-content: center;" class="bg-dark">
      <div style="text-align: center;">
        <h1 class="text-gold font-serif text-4xl md:text-5xl mb-4">星月大地</h1>
        <p class="text-gold-light text-lg">景觀休閒園區</p>
      </div>
    </div>

    <!-- 底部資訊覆蓋 -->
    <div style="position: absolute; bottom: 0; left: 0; right: 0; z-index: 10; padding: 16px 0; background: rgba(26,26,26,0.8);">
      <div class="max-w-[1200px] mx-auto px-4 flex flex-col sm:flex-row items-center justify-center gap-4 text-white text-sm">
        <span class="text-gold font-serif text-lg">星月大地景觀休閒園區</span>
        <span><i class="pi pi-phone mr-1" /> 04-26831671</span>
        <span><i class="pi pi-map-marker mr-1" /> 台中市后里區月眉北路486號</span>
      </div>
    </div>
  </section>
</template>
