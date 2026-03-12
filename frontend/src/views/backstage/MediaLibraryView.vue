<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Button from 'primevue/button'
import FileUpload from 'primevue/fileupload'
import Dialog from 'primevue/dialog'
import { useConfirm } from 'primevue/useconfirm'
import { useToast } from 'primevue/usetoast'
import { uploadApi } from '@/api/upload'
import type { MediaFileDto } from '@/types/api'

const confirm = useConfirm()
const toast = useToast()

const files = ref<MediaFileDto[]>([])
const loading = ref(false)
const previewDialog = ref(false)
const previewUrl = ref('')

const loadFiles = async () => {
  loading.value = true
  try {
    const { data } = await uploadApi.getList()
    if (data.success && data.data) files.value = data.data as MediaFileDto[]
  } catch (err) { console.error(err); toast.add({ severity: 'error', summary: '錯誤', detail: '載入失敗', life: 3000 }) }
  loading.value = false
}

const onUpload = async (event: { files: File | File[] }) => {
  const uploadedFiles = Array.isArray(event.files) ? event.files : [event.files]
  for (const file of uploadedFiles) {
    try {
      await uploadApi.upload(file)
    } catch {
      toast.add({ severity: 'error', summary: '錯誤', detail: `${file.name} 上傳失敗`, life: 3000 })
    }
  }
  toast.add({ severity: 'success', summary: '成功', detail: '上傳完成', life: 3000 })
  await loadFiles()
}

const previewFile = (file: MediaFileDto) => {
  previewUrl.value = `/uploads/${file.filePath}`
  previewDialog.value = true
}

const deleteFile = (file: MediaFileDto) => {
  confirm.require({
    message: `確定要刪除「${file.originalName}」嗎？`,
    header: '確認刪除',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: '刪除',
    rejectLabel: '取消',
    accept: async () => {
      try {
        await uploadApi.delete(file.id)
        toast.add({ severity: 'success', summary: '成功', detail: '刪除成功', life: 3000 })
        await loadFiles()
      } catch {
        toast.add({ severity: 'error', summary: '錯誤', detail: '刪除失敗', life: 3000 })
      }
    },
  })
}

const formatSize = (bytes: number) => {
  if (bytes < 1024) return `${bytes} B`
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`
  return `${(bytes / 1024 / 1024).toFixed(1)} MB`
}

const isImage = (contentType: string) => contentType.startsWith('image/')

const copyPath = (file: MediaFileDto) => {
  navigator.clipboard.writeText(file.filePath)
  toast.add({ severity: 'info', summary: '已複製', detail: '檔案路徑已複製到剪貼簿', life: 2000 })
}

onMounted(loadFiles)
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold">媒體庫</h2>
    </div>

    <div class="mb-6">
      <FileUpload
        mode="basic"
        :auto="true"
        accept="image/*"
        :maxFileSize="10000000"
        chooseLabel="上傳檔案"
        customUpload
        @uploader="onUpload"
        :multiple="true"
      />
    </div>

    <div v-if="loading" class="text-center py-10 text-gray-400">載入中...</div>

    <div v-else class="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-6 gap-4">
      <div
        v-for="file in files"
        :key="file.id"
        class="border rounded-lg overflow-hidden group relative"
      >
        <div class="aspect-square bg-gray-100 flex items-center justify-center">
          <img
            v-if="isImage(file.contentType)"
            :src="`/uploads/${file.thumbnailPath || file.filePath}`"
            :alt="file.originalName"
            class="w-full h-full object-cover"
          />
          <i v-else class="pi pi-file text-4xl text-gray-300" />
        </div>

        <!-- 操作覆蓋層 -->
        <div class="absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center gap-2">
          <Button
            v-if="isImage(file.contentType)"
            icon="pi pi-eye"
            severity="info"
            size="small"
            rounded
            @click="previewFile(file)"
          />
          <Button icon="pi pi-copy" severity="secondary" size="small" rounded @click="copyPath(file)" v-tooltip="'複製路徑'" />
          <Button icon="pi pi-trash" severity="danger" size="small" rounded @click="deleteFile(file)" />
        </div>

        <div class="p-2">
          <p class="text-xs truncate" :title="file.originalName">{{ file.originalName }}</p>
          <p class="text-xs text-gray-400">{{ formatSize(file.fileSize) }}</p>
        </div>
      </div>
    </div>

    <div v-if="!loading && files.length === 0" class="text-center py-10 text-gray-400">
      尚無檔案，請點擊「上傳檔案」按鈕
    </div>

    <!-- 預覽 Dialog -->
    <Dialog v-model:visible="previewDialog" header="圖片預覽" :style="{ width: '80vw' }" modal>
      <div class="flex justify-center">
        <img :src="previewUrl" class="max-w-full max-h-[70vh] object-contain" />
      </div>
    </Dialog>
  </div>
</template>
