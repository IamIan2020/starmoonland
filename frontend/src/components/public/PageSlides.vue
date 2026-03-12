<script setup lang="ts">
import { ref, watch, onUnmounted } from 'vue'

const props = defineProps<{
  slides: { id: number; imageUrl: string; title?: string; description?: string }[]
}>()

const activeIndex = ref(0)
let timer: ReturnType<typeof setInterval> | null = null

const resolveImageUrl = (url: string) => {
  if (url.startsWith('http://') || url.startsWith('https://') || url.startsWith('/')) return url
  return `/_images/${url}`
}

const goToSlide = (idx: number) => {
  activeIndex.value = idx
  restartTimer()
}

const startTimer = () => {
  if (timer) clearInterval(timer)
  if (props.slides.length > 1) {
    timer = setInterval(() => {
      activeIndex.value = (activeIndex.value + 1) % props.slides.length
    }, 5000)
  }
}

const restartTimer = () => {
  startTimer()
}

watch(() => props.slides.length, (len) => {
  if (len > 1) startTimer()
}, { immediate: true })

onUnmounted(() => { if (timer) clearInterval(timer) })
</script>

<template>
  <div class="mb-8">
    <div class="flex flex-col lg:flex-row">
      <!-- 左側金色文字面板 -->
      <div class="w-full lg:w-[390px] shrink-0" style="background-color: #c9a063;">
        <div class="flex flex-col justify-center h-[280px] lg:h-[420px] px-6 py-8 relative">
          <!-- 目前 slide 的文字 -->
          <div
            v-for="(slide, idx) in slides"
            :key="slide.id"
            class="absolute inset-0 flex flex-col justify-center px-8 py-6 transition-opacity duration-500"
            :style="{ opacity: activeIndex === idx ? 1 : 0, pointerEvents: activeIndex === idx ? 'auto' : 'none' }"
          >
            <h3
              v-if="slide.title"
              class="text-white text-center text-2xl lg:text-[30px] leading-relaxed mb-2"
            >
              {{ slide.title }}
            </h3>
            <div
              v-if="slide.title"
              class="mx-auto mb-6"
              style="width: 60px; height: 3px; background-color: #ffd18c;"
            />
            <p
              v-if="slide.description"
              class="text-white text-sm leading-relaxed max-h-[150px] overflow-hidden"
            >
              {{ slide.description }}
            </p>
          </div>

          <!-- 分頁圓點 -->
          <div v-if="slides.length > 1" class="absolute bottom-4 left-0 right-0 flex justify-center gap-2">
            <button
              v-for="(_, idx) in slides"
              :key="idx"
              :class="[
                'w-2.5 h-2.5 rounded-full transition-all',
                activeIndex === idx ? 'bg-white scale-125' : 'bg-[#ffd18c]'
              ]"
              @click="goToSlide(idx)"
            />
          </div>
        </div>
      </div>

      <!-- 右側圖片 -->
      <div class="flex-1 relative overflow-hidden h-[280px] lg:h-[420px]">
        <div
          v-for="(slide, idx) in slides"
          :key="slide.id"
          class="absolute inset-0 transition-opacity duration-500"
          :style="{ opacity: activeIndex === idx ? 1 : 0 }"
        >
          <img
            :src="resolveImageUrl(slide.imageUrl)"
            :alt="slide.title || ''"
            class="w-full h-full object-cover"
          />
        </div>
      </div>
    </div>
  </div>
</template>
