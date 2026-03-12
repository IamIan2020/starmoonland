<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import Breadcrumb from '@/components/public/Breadcrumb.vue'
import { albumsApi } from '@/api/albums'
import type { AlbumDto } from '@/types/api'
import { resolveImageUrl } from '@/utils/image'

const albums = ref<AlbumDto[]>([])
const loading = ref(true)

onMounted(async () => {
  try {
    const { data } = await albumsApi.getList()
    if (data.success && data.data) albums.value = data.data as AlbumDto[]
  } catch (err) { console.error(err) }
  loading.value = false
})
</script>

<template>
  <div>
    <Breadcrumb :items="[{ label: 'Album' }]" />
    <div class="bg-dark text-center py-10">
      <p class="text-gold-light font-serif text-sm tracking-[0.3em] uppercase mb-2">Album</p>
      <h2 class="text-white text-2xl md:text-3xl font-bold">大地景觀</h2>
    </div>

    <div class="max-w-[1200px] mx-auto px-4 py-10">
      <div v-if="loading" class="py-20 text-center text-gray-text">載入中...</div>
      <div v-else class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
        <RouterLink
          v-for="album in albums"
          :key="album.id"
          :to="`/album/${album.id}`"
          class="group"
        >
          <div class="aspect-square overflow-hidden mb-2 bg-gray-100">
            <img
              :src="album.coverImage ? resolveImageUrl(album.coverImage) : '/_images/all/album_default.jpg'"
              :alt="album.title"
              class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
            />
          </div>
          <h3 class="text-sm font-bold group-hover:text-gold transition-colors">{{ album.title }}</h3>
          <p class="text-xs text-gray-text">{{ album.photoCount }} 張照片</p>
        </RouterLink>
      </div>
    </div>
  </div>
</template>
