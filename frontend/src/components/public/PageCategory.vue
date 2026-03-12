<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { Swiper, SwiperSlide } from 'swiper/vue'
import { Navigation, Thumbs } from 'swiper/modules'
import type SwiperType from 'swiper'
import 'swiper/swiper-bundle.css'
import Breadcrumb from './Breadcrumb.vue'
import { pagesApi } from '@/api/pages'
import type { PageDto } from '@/types/api'

const props = defineProps<{
  categorySlug: string
  categoryLabel: string
  categoryLabelEn: string
}>()

const route = useRoute()
const router = useRouter()
const modules = [Navigation, Thumbs]

const pages = ref<{ id: number; slug: string; title: string; subtitle?: string }[]>([])
const currentPage = ref<PageDto | null>(null)
const activeTab = ref(0)
const thumbsSwiper = ref<SwiperType | null>(null)
const loading = ref(true)

const setThumbsSwiper = (swiper: SwiperType) => {
  thumbsSwiper.value = swiper
}

const loadCategory = async () => {
  try {
    const { data } = await pagesApi.getCategory(props.categorySlug)
    if (data.success && data.data) {
      pages.value = (data.data as any).pages || []
      // 載入指定或第一個子頁面
      const slug = route.params.slug as string || pages.value[0]?.slug
      if (slug) await loadPage(slug)
    }
  } catch { /* 靜默 */ }
  loading.value = false
}

const loadPage = async (slug: string) => {
  try {
    const { data } = await pagesApi.getPage(props.categorySlug, slug)
    if (data.success && data.data) {
      currentPage.value = data.data as PageDto
      activeTab.value = 0
    }
  } catch { /* 靜默 */ }
}

const selectPage = (slug: string) => {
  router.push(`/${props.categorySlug}/${slug}`)
  loadPage(slug)
}

watch(() => route.params.slug, (newSlug) => {
  if (newSlug && typeof newSlug === 'string') loadPage(newSlug)
})

onMounted(loadCategory)
</script>

<template>
  <div>
    <Breadcrumb :items="[
      { label: categoryLabelEn, to: `/${categorySlug}` },
      ...(currentPage ? [{ label: currentPage.title }] : [])
    ]" />

    <!-- 頁面標題 -->
    <div class="bg-dark text-center py-10">
      <p class="text-gold-light font-serif text-sm tracking-[0.3em] uppercase mb-2">{{ categoryLabelEn }}</p>
      <h2 class="text-white text-2xl md:text-3xl font-bold">{{ categoryLabel }}</h2>
    </div>

    <div v-if="loading" class="py-20 text-center text-gray-text">載入中...</div>

    <div v-else class="max-w-[1200px] mx-auto px-4 py-10">
      <!-- 子頁面頁籤 -->
      <div class="flex flex-wrap gap-2 mb-8 border-b border-gold/30 pb-4">
        <button
          v-for="p in pages"
          :key="p.slug"
          :class="[
            'px-5 py-2 text-sm border transition-colors',
            currentPage?.slug === p.slug
              ? 'bg-gold text-white border-gold'
              : 'border-gold/30 text-dark-light hover:border-gold hover:text-gold'
          ]"
          @click="selectPage(p.slug)"
        >
          {{ p.title }}
        </button>
      </div>

      <!-- 輪播圖區域 -->
      <div v-if="currentPage?.slides?.length" class="mb-8">
        <div class="grid grid-cols-1 lg:grid-cols-3 gap-4">
          <!-- 文字導覽 -->
          <div class="lg:col-span-1">
            <Swiper
              :modules="modules"
              :slides-per-view="3"
              :space-between="8"
              direction="vertical"
              class="h-[300px] lg:h-[400px]"
              @swiper="setThumbsSwiper"
            >
              <SwiperSlide
                v-for="slide in currentPage.slides"
                :key="slide.id"
                class="cursor-pointer"
              >
                <div class="p-3 border border-gold/20 hover:border-gold transition h-full flex items-center">
                  <div>
                    <p v-if="slide.title" class="text-sm font-bold text-dark">{{ slide.title }}</p>
                    <p v-if="slide.description" class="text-xs text-gray-text mt-1">{{ slide.description }}</p>
                  </div>
                </div>
              </SwiperSlide>
            </Swiper>
          </div>
          <!-- 大圖 -->
          <div class="lg:col-span-2">
            <Swiper
              :modules="modules"
              :thumbs="{ swiper: thumbsSwiper }"
              :navigation="true"
              class="h-[300px] lg:h-[400px]"
            >
              <SwiperSlide v-for="slide in currentPage.slides" :key="slide.id">
                <img :src="`/uploads/${slide.imageUrl}`" :alt="slide.title || ''" class="w-full h-full object-cover" />
              </SwiperSlide>
            </Swiper>
          </div>
        </div>
      </div>

      <!-- Tab 內容 -->
      <div v-if="currentPage?.tabs?.length">
        <div class="flex gap-1 border-b border-gray-200 mb-6">
          <button
            v-for="(tab, idx) in currentPage.tabs"
            :key="tab.id"
            :class="[
              'px-5 py-3 text-sm font-medium transition-colors border-b-2 -mb-px',
              activeTab === idx
                ? 'border-gold text-gold'
                : 'border-transparent text-gray-text hover:text-gold'
            ]"
            @click="activeTab = idx"
          >
            {{ tab.title }}
          </button>
        </div>
        <div v-if="currentPage.tabs?.[activeTab]" class="prose max-w-none" v-html="currentPage.tabs[activeTab]!.content" />
      </div>

      <!-- 一般內容 -->
      <div v-else-if="currentPage?.content" class="prose max-w-none" v-html="currentPage.content" />
    </div>
  </div>
</template>
