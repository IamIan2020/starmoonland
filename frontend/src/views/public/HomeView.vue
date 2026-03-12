<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import { settingsApi } from '@/api/settings'
import type { HomepageData } from '@/types/api'
import HeroSlider from '@/components/public/HeroSlider.vue'
import { resolveImageUrl } from '@/utils/image'

const data = ref<HomepageData | null>(null)

onMounted(async () => {
  try {
    const res = await settingsApi.getHomepage()
    if (res.data.success && res.data.data) data.value = res.data.data
  } catch (err) { console.error(err) }
})

const services = [
  { title: '大地營區', slug: 'camp', desc: '遠眺美景．星月大地．親子野餐', img: '/_images/index/img-intro-01.png' },
  { title: '大地烤肉', slug: 'bbq', desc: '星月大地提供大台中微涼夜烤的新選擇', img: '/_images/index/img-intro-03.png' },
  { title: '大地風呂', slug: 'hotspring', desc: '露天冷熱湯池、多功能SPA池、檜木烤箱、蒸氣室、五星級衛浴', img: '/_images/index/img-intro-05.png' },
  { title: '星月文旅', slug: 'stay', desc: '旅途中放慢腳步、放鬆身心、享受在星月大地的「美」分「美」秒。', img: '/_images/index/img-intro-07.png' },
]
</script>

<template>
  <div>
    <!-- Hero -->
    <HeroSlider :slides="data?.slides || []" />

    <!-- 新聞預覽 -->
    <section class="py-16 bg-white">
      <div class="max-w-[1200px] mx-auto px-4">
        <div class="text-center mb-10">
          <p class="text-gold-light font-serif text-sm tracking-[0.3em] uppercase mb-2">News</p>
          <h2 class="text-2xl font-bold">星月訊息</h2>
        </div>
        <div class="grid md:grid-cols-3 gap-8">
          <RouterLink
            v-for="news in data?.latestNews"
            :key="news.id"
            :to="`/news/${news.id}`"
            class="group"
          >
            <div class="aspect-[4/3] overflow-hidden mb-3">
              <img
                :src="news.coverImage ? resolveImageUrl(news.coverImage) : '/_images/all/news_default.jpg'"
                :alt="news.title"
                class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
              />
            </div>
            <p class="text-xs text-gray-text mb-1">{{ new Date(news.publishDate).toLocaleDateString('zh-TW') }}</p>
            <h3 class="font-bold group-hover:text-gold transition-colors">{{ news.title }}</h3>
            <p v-if="news.summary" class="text-sm text-gray-text mt-1 line-clamp-2">{{ news.summary }}</p>
          </RouterLink>
        </div>
        <div class="text-center mt-8">
          <RouterLink to="/news" class="inline-block border border-gold text-gold px-8 py-2 hover:bg-gold hover:text-white transition">
            更多消息
          </RouterLink>
        </div>
      </div>
    </section>

    <!-- 服務四宮格 -->
    <section class="py-16 bg-gray-50">
      <div class="max-w-[1200px] mx-auto px-4">
        <div class="text-center mb-10">
          <p class="text-gold-light font-serif text-sm tracking-[0.3em] uppercase mb-2">Service</p>
          <h2 class="text-2xl font-bold">星月服務</h2>
        </div>
        <div class="space-y-12">
          <div
            v-for="(svc, idx) in services"
            :key="svc.slug"
            :class="['grid md:grid-cols-2 gap-8 items-center', idx % 2 === 1 ? 'md:direction-rtl' : '']"
          >
            <div :class="idx % 2 === 1 ? 'md:order-2' : ''">
              <img :src="svc.img" :alt="svc.title" class="w-full aspect-[16/10] object-cover" />
            </div>
            <div :class="['text-center md:text-left', idx % 2 === 1 ? 'md:order-1' : '']">
              <h3 class="text-xl font-bold mb-3">{{ svc.title }}</h3>
              <p class="text-gray-text mb-4">{{ svc.desc }}</p>
              <RouterLink :to="`/service/${svc.slug}`" class="inline-block border border-gold text-gold px-6 py-2 text-sm hover:bg-gold hover:text-white transition">
                了解更多
              </RouterLink>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- 相簿預覽 -->
    <section v-if="data?.featuredAlbums?.length" class="py-16 bg-dark">
      <div class="max-w-[1200px] mx-auto px-4">
        <div class="text-center mb-10">
          <p class="text-gold-light font-serif text-sm tracking-[0.3em] uppercase mb-2">Album</p>
          <h2 class="text-2xl font-bold text-white">大地景觀</h2>
        </div>
        <div class="grid grid-cols-2 md:grid-cols-3 gap-4">
          <RouterLink
            v-for="album in data.featuredAlbums"
            :key="album.id"
            :to="`/album/${album.id}`"
            class="group aspect-square overflow-hidden"
          >
            <img
              :src="album.coverImage ? resolveImageUrl(album.coverImage) : '/_images/all/album_default.jpg'"
              :alt="album.title"
              class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
            />
          </RouterLink>
        </div>
        <div class="text-center mt-8">
          <RouterLink to="/album" class="inline-block border border-gold text-gold px-8 py-2 hover:bg-gold hover:text-white transition">
            更多景觀
          </RouterLink>
        </div>
      </div>
    </section>
  </div>
</template>
