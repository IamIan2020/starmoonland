<script setup lang="ts">
import { ref, computed } from 'vue'
import Button from 'primevue/button'
import { useToast } from 'primevue/usetoast'
import { uploadApi } from '@/api/upload'
import { resolveImageUrl } from '@/utils/image'

const props = withDefaults(defineProps<{
  modelValue: string
  height?: string
  accept?: string
}>(), {
  height: '160px',
  accept: 'image/jpeg,image/png,image/gif,image/webp',
})

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

const toast = useToast()
const uploading = ref(false)
const fileInput = ref<HTMLInputElement | null>(null)

const previewUrl = computed(() => {
  if (!props.modelValue) return ''
  return resolveImageUrl(props.modelValue)
})

const triggerUpload = () => {
  fileInput.value?.click()
}

const onFileSelected = async (event: Event) => {
  const input = event.target as HTMLInputElement
  const file = input.files?.[0]
  if (!file) return

  // 檢查檔案大小（10MB）
  if (file.size > 10 * 1024 * 1024) {
    toast.add({ severity: 'error', summary: '錯誤', detail: '檔案大小不可超過 10MB', life: 3000 })
    return
  }

  uploading.value = true
  try {
    const { data } = await uploadApi.upload(file)
    if (data.success && data.data) {
      emit('update:modelValue', `/uploads/${data.data.filePath}`)
      toast.add({ severity: 'success', summary: '成功', detail: '上傳成功', life: 2000 })
    } else {
      toast.add({ severity: 'error', summary: '錯誤', detail: data.message || '上傳失敗', life: 3000 })
    }
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '上傳失敗', life: 3000 })
  }
  uploading.value = false

  // 清除 input 以便重複上傳同一檔案
  input.value = ''
}

const clearImage = () => {
  emit('update:modelValue', '')
}
</script>

<template>
  <div class="image-uploader">
    <input
      ref="fileInput"
      type="file"
      :accept="accept"
      class="hidden"
      @change="onFileSelected"
    />

    <!-- 有圖片：顯示預覽 + 操作按鈕 -->
    <div v-if="modelValue" class="relative group border rounded overflow-hidden" :style="{ height }">
      <img :src="previewUrl" class="w-full h-full object-cover" />
      <div class="absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center gap-2">
        <Button
          icon="pi pi-refresh"
          severity="secondary"
          size="small"
          rounded
          v-tooltip="'更換圖片'"
          :loading="uploading"
          @click="triggerUpload"
        />
        <Button
          icon="pi pi-trash"
          severity="danger"
          size="small"
          rounded
          v-tooltip="'移除圖片'"
          @click="clearImage"
        />
      </div>
      <div v-if="uploading" class="absolute inset-0 bg-black/60 flex items-center justify-center">
        <i class="pi pi-spin pi-spinner text-white text-2xl" />
      </div>
    </div>

    <!-- 無圖片：顯示上傳區域 -->
    <div
      v-else
      class="border-2 border-dashed rounded cursor-pointer hover:border-blue-400 hover:bg-blue-50/50 transition-colors flex flex-col items-center justify-center gap-2"
      :style="{ height }"
      @click="triggerUpload"
    >
      <div v-if="uploading" class="flex flex-col items-center gap-2">
        <i class="pi pi-spin pi-spinner text-2xl text-blue-400" />
        <span class="text-sm text-gray-500">上傳中...</span>
      </div>
      <template v-else>
        <i class="pi pi-cloud-upload text-3xl text-gray-300" />
        <span class="text-sm text-gray-400">點擊上傳圖片</span>
      </template>
    </div>
  </div>
</template>
