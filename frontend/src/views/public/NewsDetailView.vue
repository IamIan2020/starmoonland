<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import Breadcrumb from '@/components/public/Breadcrumb.vue'
import { newsApi } from '@/api/news'
import type { NewsDto } from '@/types/api'
import { sanitizeHtml } from '@/utils/sanitize'

const route = useRoute()
const news = ref<NewsDto | null>(null)

onMounted(async () => {
  const id = Number(route.params.id)
  try {
    const { data } = await newsApi.getDetail(id)
    if (data.success && data.data) news.value = data.data as NewsDto
  } catch (err) { console.error(err) }
})
</script>

<template>
  <div>
    <Breadcrumb :items="[{ label: 'News', to: '/news' }, ...(news ? [{ label: news.title }] : [])]" />

    <div v-if="news" class="max-w-[800px] mx-auto px-4 py-10">
      <div class="mb-6">
        <span class="text-xs text-gold mr-2">{{ news.categoryName }}</span>
        <span class="text-xs text-gray-text">{{ new Date(news.publishDate).toLocaleDateString('zh-TW') }}</span>
      </div>
      <h1 class="text-2xl md:text-3xl font-bold mb-6">{{ news.title }}</h1>
      <img
        v-if="news.coverImage"
        :src="`/uploads/${news.coverImage}`"
        :alt="news.title"
        class="w-full mb-8"
      />
      <div class="prose max-w-none" v-html="sanitizeHtml(news.content ?? '')" />
    </div>

    <div v-else class="py-20 text-center text-gray-text">載入中...</div>
  </div>
</template>
