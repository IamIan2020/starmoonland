<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import Breadcrumb from './Breadcrumb.vue'
import PageSlides from './PageSlides.vue'
import { pagesApi } from '@/api/pages'
import type { PageDto } from '@/types/api'
import { sanitizeHtml } from '@/utils/sanitize'

const props = defineProps<{
  categorySlug: string
  categoryLabel: string
  categoryLabelEn: string
}>()

const route = useRoute()
const router = useRouter()

const pages = ref<{ id: number; slug: string; title: string; subtitle?: string }[]>([])
const currentPage = ref<PageDto | null>(null)
const activeTab = ref(0)
const loading = ref(true)

const loadCategory = async () => {
  try {
    const { data } = await pagesApi.getCategory(props.categorySlug)
    if (data.success && data.data) {
      pages.value = (data.data as any).pages || []
      const slug = route.params.slug as string || pages.value[0]?.slug
      if (slug) await loadPage(slug)
    }
  } catch (err) { console.error(err) }
  loading.value = false
}

const loadPage = async (slug: string) => {
  try {
    const { data } = await pagesApi.getPage(props.categorySlug, slug)
    if (data.success && data.data) {
      currentPage.value = data.data as PageDto
      activeTab.value = 0
    }
  } catch (err) { console.error(err) }
}

const selectPage = (slug: string) => {
  if (currentPage.value?.slug === slug) return
  // 先清除當前頁面，確保子元件卸載
  currentPage.value = null
  router.replace(`/${props.categorySlug}/${slug}`)
  loadPage(slug)
}

watch(() => route.params.slug, (newSlug, oldSlug) => {
  if (newSlug && newSlug !== oldSlug && typeof newSlug === 'string' && newSlug !== currentPage.value?.slug)
    loadPage(newSlug)
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

      <!-- 輪播圖區域（獨立元件，避免 Swiper 狀態汙染） -->
      <PageSlides
        v-if="currentPage?.slides?.length"
        :key="currentPage.slug"
        :slides="currentPage.slides"
      />

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
        <div v-if="currentPage.tabs?.[activeTab]" class="prose max-w-none" v-html="sanitizeHtml(currentPage.tabs[activeTab]!.content ?? '')" />
      </div>

      <!-- 一般內容 -->
      <div v-else-if="currentPage?.content" class="prose max-w-none" v-html="sanitizeHtml(currentPage.content ?? '')" />
    </div>
  </div>
</template>
