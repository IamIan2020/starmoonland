<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Breadcrumb from '@/components/public/Breadcrumb.vue'
import { settingsApi } from '@/api/settings'
import { useSettingsStore } from '@/stores/settings'
import type { TrafficInfoDto } from '@/types/api'
import { sanitizeHtml } from '@/utils/sanitize'

const settings = useSettingsStore()
const trafficInfos = ref<TrafficInfoDto[]>([])
const activeTab = ref(0)

onMounted(async () => {
  try {
    const { data } = await settingsApi.getTraffic()
    if (data.success && data.data) trafficInfos.value = data.data as TrafficInfoDto[]
  } catch (err) { console.error(err) }
})
</script>

<template>
  <div>
    <Breadcrumb :items="[{ label: 'Traffic' }]" />
    <div class="bg-dark text-center py-10">
      <p class="text-gold-light font-serif text-sm tracking-[0.3em] uppercase mb-2">Traffic</p>
      <h2 class="text-white text-2xl md:text-3xl font-bold">交通資訊</h2>
    </div>

    <div class="max-w-[1000px] mx-auto px-4 py-10">
      <!-- Tab -->
      <div class="flex gap-1 border-b border-gray-200 mb-6">
        <button
          v-for="(info, idx) in trafficInfos"
          :key="info.id"
          :class="[
            'px-5 py-3 text-sm font-medium transition-colors border-b-2 -mb-px',
            activeTab === idx ? 'border-gold text-gold' : 'border-transparent text-gray-text hover:text-gold'
          ]"
          @click="activeTab = idx"
        >
          {{ info.tabName }}
        </button>
      </div>

      <div v-if="trafficInfos[activeTab]" class="prose max-w-none mb-10" v-html="sanitizeHtml(trafficInfos[activeTab]!.content ?? '')" />

      <!-- Google Maps -->
      <div v-if="settings.get('google_maps_embed')" class="aspect-video">
        <iframe
          :src="settings.get('google_maps_embed')"
          width="100%"
          height="100%"
          style="border: 0"
          allowfullscreen
          loading="lazy"
        />
      </div>
    </div>
  </div>
</template>
