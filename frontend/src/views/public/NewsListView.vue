<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { RouterLink } from 'vue-router'
import Breadcrumb from '@/components/public/Breadcrumb.vue'
import { newsApi } from '@/api/news'
import type { NewsDto, NewsCategoryDto } from '@/types/api'

const news = ref<NewsDto[]>([])
const categories = ref<NewsCategoryDto[]>([])
const currentCategory = ref('')
const currentPage = ref(1)
const totalPages = ref(1)
const loading = ref(true)

const loadNews = async () => {
  loading.value = true
  try {
    const { data } = await newsApi.getList({ page: currentPage.value, pageSize: 9, category: currentCategory.value || undefined })
    if (data.success) {
      news.value = data.data
      totalPages.value = data.totalPages
    }
  } catch (err) { console.error(err) }
  loading.value = false
}

const loadCategories = async () => {
  try {
    const { data } = await newsApi.getCategories()
    if (data.success && data.data) categories.value = data.data as NewsCategoryDto[]
  } catch (err) { console.error(err) }
}

const filterByCategory = (slug: string) => {
  currentCategory.value = slug
  currentPage.value = 1
  loadNews()
}

watch(currentPage, loadNews)
onMounted(() => { loadCategories(); loadNews() })
</script>

<template>
  <div>
    <Breadcrumb :items="[{ label: 'News' }]" />
    <div class="bg-dark text-center py-10">
      <p class="text-gold-light font-serif text-sm tracking-[0.3em] uppercase mb-2">News</p>
      <h2 class="text-white text-2xl md:text-3xl font-bold">訊息公告</h2>
    </div>

    <div class="max-w-[1200px] mx-auto px-4 py-10">
      <!-- 分類篩選 -->
      <div class="flex flex-wrap gap-2 mb-8">
        <button
          :class="['px-4 py-2 text-sm border transition', !currentCategory ? 'bg-gold text-white border-gold' : 'border-gold/30 text-gray-text hover:border-gold']"
          @click="filterByCategory('')"
        >全部</button>
        <button
          v-for="cat in categories"
          :key="cat.slug"
          :class="['px-4 py-2 text-sm border transition', currentCategory === cat.slug ? 'bg-gold text-white border-gold' : 'border-gold/30 text-gray-text hover:border-gold']"
          @click="filterByCategory(cat.slug)"
        >{{ cat.name }}</button>
      </div>

      <div v-if="loading" class="py-20 text-center text-gray-text">載入中...</div>

      <!-- 新聞列表 -->
      <div v-else class="grid md:grid-cols-3 gap-8">
        <RouterLink
          v-for="item in news"
          :key="item.id"
          :to="`/news/${item.id}`"
          class="group"
        >
          <div class="aspect-[4/3] overflow-hidden mb-3 bg-gray-100">
            <img
              :src="item.coverImage ? `/uploads/${item.coverImage}` : '/_images/all/news_default.jpg'"
              :alt="item.title"
              class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
            />
          </div>
          <div class="flex items-center gap-2 mb-1">
            <span class="text-xs text-gold">{{ item.categoryName }}</span>
            <span class="text-xs text-gray-text">{{ new Date(item.publishDate).toLocaleDateString('zh-TW') }}</span>
          </div>
          <h3 class="font-bold group-hover:text-gold transition-colors">{{ item.title }}</h3>
          <p v-if="item.summary" class="text-sm text-gray-text mt-1 line-clamp-2">{{ item.summary }}</p>
        </RouterLink>
      </div>

      <!-- 分頁 -->
      <div v-if="totalPages > 1" class="flex justify-center gap-2 mt-10">
        <button
          v-for="p in totalPages"
          :key="p"
          :class="['w-10 h-10 border text-sm transition', currentPage === p ? 'bg-gold text-white border-gold' : 'border-gray-300 hover:border-gold']"
          @click="currentPage = p"
        >{{ p }}</button>
      </div>
    </div>
  </div>
</template>
