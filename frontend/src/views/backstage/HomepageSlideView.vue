<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import Checkbox from 'primevue/checkbox'
import { useToast } from 'primevue/usetoast'
import { settingsApi } from '@/api/settings'
import type { HomepageSlideDto } from '@/types/api'

const toast = useToast()

const slides = ref<HomepageSlideDto[]>([])
const loading = ref(false)
const saving = ref(false)

const loadSlides = async () => {
  loading.value = true
  try {
    const { data } = await settingsApi.adminGetSlides()
    if (data.success && data.data) slides.value = data.data as HomepageSlideDto[]
  } catch { /* 靜默 */ }
  loading.value = false
}

const addSlide = () => {
  slides.value.push({
    id: 0,
    imageUrl: '',
    altText: '',
    linkUrl: '',
    sortOrder: slides.value.length,
    isActive: true,
  })
}

const removeSlide = (idx: number) => {
  slides.value.splice(idx, 1)
}

const saveSlides = async () => {
  saving.value = true
  try {
    await settingsApi.adminUpdateSlides(slides.value)
    toast.add({ severity: 'success', summary: '成功', detail: '輪播圖已更新', life: 3000 })
    await loadSlides()
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '更新失敗', life: 3000 })
  }
  saving.value = false
}

onMounted(loadSlides)
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold">首頁輪播管理</h2>
      <div class="flex gap-2">
        <Button label="新增輪播" icon="pi pi-plus" severity="secondary" @click="addSlide" />
        <Button label="儲存全部" icon="pi pi-check" :loading="saving" @click="saveSlides" />
      </div>
    </div>

    <div v-if="loading" class="text-center py-10 text-gray-400">載入中...</div>

    <div v-else class="space-y-4">
      <div v-for="(slide, idx) in slides" :key="idx" class="border rounded-lg p-4">
        <div class="flex justify-between items-center mb-3">
          <h3 class="font-medium">輪播 {{ idx + 1 }}</h3>
          <Button icon="pi pi-trash" severity="danger" size="small" text @click="removeSlide(idx)" />
        </div>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          <div>
            <label class="block text-sm mb-1">圖片路徑 *</label>
            <InputText v-model="slide.imageUrl" class="w-full" placeholder="例：slides/hero1.jpg" />
          </div>
          <div>
            <label class="block text-sm mb-1">替代文字</label>
            <InputText v-model="slide.altText" class="w-full" />
          </div>
          <div>
            <label class="block text-sm mb-1">連結網址</label>
            <InputText v-model="slide.linkUrl" class="w-full" />
          </div>
          <div class="flex items-end gap-4">
            <div>
              <label class="block text-sm mb-1">排序</label>
              <InputNumber v-model="slide.sortOrder" class="w-20" />
            </div>
            <div class="pb-1">
              <Checkbox v-model="slide.isActive" :binary="true" :inputId="`slideActive${idx}`" />
              <label :for="`slideActive${idx}`" class="ml-1 text-sm">啟用</label>
            </div>
          </div>
        </div>
        <div v-if="slide.imageUrl" class="mt-3">
          <img :src="`/uploads/${slide.imageUrl}`" class="h-20 object-cover rounded" />
        </div>
      </div>

      <div v-if="slides.length === 0" class="text-center py-10 text-gray-400">
        尚無輪播圖，請點擊「新增輪播」按鈕
      </div>
    </div>
  </div>
</template>
