<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Card from 'primevue/card'
import { contactsApi } from '@/api/contacts'
import { newsApi } from '@/api/news'
import { albumsApi } from '@/api/albums'
import type { ContactSubmissionDto, NewsDto, AlbumDto } from '@/types/api'

const unreadCount = ref(0)
const recentContacts = ref<ContactSubmissionDto[]>([])
const recentNews = ref<NewsDto[]>([])
const albumCount = ref(0)

onMounted(async () => {
  try {
    const [contactsRes, newsRes, albumsRes] = await Promise.all([
      contactsApi.adminGetList({ page: 1, pageSize: 5, isRead: false }),
      newsApi.adminGetList({ page: 1, pageSize: 5 }),
      albumsApi.adminGetList(),
    ])
    if (contactsRes.data) {
      unreadCount.value = contactsRes.data.totalCount
      recentContacts.value = contactsRes.data.data
    }
    if (newsRes.data) recentNews.value = newsRes.data.data
    if (albumsRes.data?.success && albumsRes.data.data) albumCount.value = (albumsRes.data.data as AlbumDto[]).length
  } catch (err) { console.error(err) }
})
</script>

<template>
  <div>
    <h2 class="text-2xl font-bold mb-6">儀表板</h2>

    <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-8">
      <Card>
        <template #title>
          <div class="flex items-center gap-2">
            <i class="pi pi-envelope text-orange-500" />
            <span>未讀留言</span>
          </div>
        </template>
        <template #content>
          <p class="text-3xl font-bold text-orange-500">{{ unreadCount }}</p>
        </template>
      </Card>
      <Card>
        <template #title>
          <div class="flex items-center gap-2">
            <i class="pi pi-file text-blue-500" />
            <span>新聞文章</span>
          </div>
        </template>
        <template #content>
          <p class="text-3xl font-bold text-blue-500">{{ recentNews.length }}+</p>
        </template>
      </Card>
      <Card>
        <template #title>
          <div class="flex items-center gap-2">
            <i class="pi pi-images text-green-500" />
            <span>相簿數量</span>
          </div>
        </template>
        <template #content>
          <p class="text-3xl font-bold text-green-500">{{ albumCount }}</p>
        </template>
      </Card>
    </div>

    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <Card>
        <template #title>最新未讀留言</template>
        <template #content>
          <div v-if="recentContacts.length === 0" class="text-gray-400">暫無未讀留言</div>
          <div v-else class="space-y-3">
            <div v-for="c in recentContacts" :key="c.id" class="border-b pb-3 last:border-b-0">
              <div class="flex justify-between items-start">
                <div>
                  <span class="font-medium">{{ c.name }}</span>
                  <span class="text-sm text-gray-400 ml-2">{{ c.email }}</span>
                </div>
                <span class="text-xs text-gray-400">{{ new Date(c.createdAt).toLocaleDateString('zh-TW') }}</span>
              </div>
              <p class="text-sm text-gray-500 mt-1 line-clamp-2">{{ c.message }}</p>
            </div>
          </div>
        </template>
      </Card>

      <Card>
        <template #title>最新新聞</template>
        <template #content>
          <div v-if="recentNews.length === 0" class="text-gray-400">暫無新聞</div>
          <div v-else class="space-y-3">
            <div v-for="n in recentNews" :key="n.id" class="border-b pb-3 last:border-b-0">
              <div class="flex justify-between items-start">
                <span class="font-medium">{{ n.title }}</span>
                <span class="text-xs text-gray-400">{{ new Date(n.publishDate).toLocaleDateString('zh-TW') }}</span>
              </div>
              <div class="flex gap-2 mt-1">
                <span v-if="n.categoryName" class="text-xs bg-blue-100 text-blue-700 px-2 py-0.5 rounded">{{ n.categoryName }}</span>
                <span :class="n.isPublished ? 'text-green-600' : 'text-gray-400'" class="text-xs">
                  {{ n.isPublished ? '已發佈' : '草稿' }}
                </span>
              </div>
            </div>
          </div>
        </template>
      </Card>
    </div>
  </div>
</template>
