<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import Breadcrumb from '@/components/public/Breadcrumb.vue'
import { Fancybox } from '@fancyapps/ui'
import '@fancyapps/ui/dist/fancybox/fancybox.css'
import { albumsApi } from '@/api/albums'
import type { AlbumDto } from '@/types/api'
import { resolveImageUrl } from '@/utils/image'

const route = useRoute()
const album = ref<AlbumDto | null>(null)

onMounted(async () => {
  Fancybox.bind('[data-fancybox="gallery"]')
  const id = Number(route.params.id)
  try {
    const { data } = await albumsApi.getDetail(id)
    if (data.success && data.data) album.value = data.data as AlbumDto
  } catch (err) { console.error(err) }
})
</script>

<template>
  <div>
    <Breadcrumb :items="[{ label: 'Album', to: '/album' }, ...(album ? [{ label: album.title }] : [])]" />

    <div v-if="album" class="max-w-[1200px] mx-auto px-4 py-10">
      <h1 class="text-2xl font-bold mb-2">{{ album.title }}</h1>
      <p v-if="album.description" class="text-gray-text mb-8">{{ album.description }}</p>

      <div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-3">
        <a
          v-for="photo in album.photos"
          :key="photo.id"
          :href="resolveImageUrl(photo.imageUrl)"
          data-fancybox="gallery"
          :data-caption="photo.caption"
          class="aspect-square overflow-hidden bg-gray-100 cursor-pointer"
        >
          <img
            :src="resolveImageUrl(photo.thumbnailUrl || photo.imageUrl)"
            :alt="photo.caption || ''"
            class="w-full h-full object-cover hover:scale-105 transition-transform duration-300"
          />
        </a>
      </div>
    </div>

    <div v-else class="py-20 text-center text-gray-text">載入中...</div>
  </div>
</template>
