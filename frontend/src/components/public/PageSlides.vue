<script setup lang="ts">
import { ref } from 'vue'
import { Swiper, SwiperSlide } from 'swiper/vue'
import { Navigation } from 'swiper/modules'
import type SwiperType from 'swiper'
import 'swiper/swiper-bundle.css'

defineProps<{
  slides: { id: number; imageUrl: string; title?: string; description?: string }[]
}>()

const modules = [Navigation]
const mainSwiper = ref<SwiperType | null>(null)
const activeIndex = ref(0)

const setMainSwiper = (swiper: SwiperType) => {
  mainSwiper.value = swiper
  swiper.on('slideChange', () => {
    activeIndex.value = swiper.activeIndex
  })
}

const goToSlide = (idx: number) => {
  mainSwiper.value?.slideTo(idx)
  activeIndex.value = idx
}
</script>

<template>
  <div class="mb-8">
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-4">
      <!-- 文字導覽 -->
      <div class="lg:col-span-1">
        <div class="flex flex-col gap-2 h-[300px] lg:h-[400px] overflow-y-auto">
          <button
            v-for="(slide, idx) in slides"
            :key="slide.id"
            :class="[
              'p-3 border text-left transition h-auto flex items-center',
              activeIndex === idx
                ? 'border-gold bg-gold/10'
                : 'border-gold/20 hover:border-gold'
            ]"
            @click="goToSlide(idx)"
          >
            <div>
              <p v-if="slide.title" class="text-sm font-bold text-dark">{{ slide.title }}</p>
              <p v-if="slide.description" class="text-xs text-gray-text mt-1">{{ slide.description }}</p>
            </div>
          </button>
        </div>
      </div>
      <!-- 大圖 -->
      <div class="lg:col-span-2">
        <Swiper
          :modules="modules"
          :navigation="true"
          class="h-[300px] lg:h-[400px]"
          @swiper="setMainSwiper"
        >
          <SwiperSlide v-for="slide in slides" :key="slide.id">
            <img :src="`/uploads/${slide.imageUrl}`" :alt="slide.title || ''" class="w-full h-full object-cover" />
          </SwiperSlide>
        </Swiper>
      </div>
    </div>
  </div>
</template>
