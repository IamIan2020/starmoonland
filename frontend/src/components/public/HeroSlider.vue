<script setup lang="ts">
import { Swiper, SwiperSlide } from 'swiper/vue'
import { Autoplay, Pagination, EffectFade } from 'swiper/modules'
import 'swiper/swiper-bundle.css'
import type { HomepageSlideDto } from '@/types/api'

defineProps<{
  slides: HomepageSlideDto[]
}>()

const modules = [Autoplay, Pagination, EffectFade]
</script>

<template>
  <section class="relative h-[500px] md:h-[600px] lg:h-[700px]">
    <Swiper
      :modules="modules"
      :autoplay="{ delay: 5000, disableOnInteraction: false }"
      :pagination="{ clickable: true }"
      effect="fade"
      :loop="true"
      class="h-full"
    >
      <SwiperSlide v-for="slide in slides" :key="slide.id">
        <div class="relative h-full">
          <img
            :src="`/uploads/${slide.imageUrl}`"
            :alt="slide.altText || '星月大地'"
            class="w-full h-full object-cover"
          />
          <div class="absolute inset-0 bg-black/20" />
        </div>
      </SwiperSlide>
    </Swiper>

    <!-- 底部資訊覆蓋 -->
    <div class="absolute bottom-0 left-0 right-0 z-10 bg-dark/80 py-4">
      <div class="max-w-[1200px] mx-auto px-4 flex flex-col sm:flex-row items-center justify-center gap-4 text-white text-sm">
        <span class="text-gold font-serif text-lg">星月大地景觀休閒園區</span>
        <span><i class="pi pi-phone mr-1" /> 04-26831671</span>
        <span><i class="pi pi-map-marker mr-1" /> 台中市后里區月眉北路486號</span>
      </div>
    </div>

    <!-- 預設畫面（無輪播圖時） -->
    <div v-if="!slides.length" class="absolute inset-0 bg-dark flex items-center justify-center">
      <div class="text-center">
        <h1 class="text-gold font-serif text-4xl md:text-5xl mb-4">星月大地</h1>
        <p class="text-gold-light text-lg">景觀休閒園區</p>
      </div>
    </div>
  </section>
</template>
