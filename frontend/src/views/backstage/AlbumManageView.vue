<script setup lang="ts">
import { ref, onMounted } from 'vue'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import Textarea from 'primevue/textarea'
import Select from 'primevue/select'
import Checkbox from 'primevue/checkbox'
import { useConfirm } from 'primevue/useconfirm'
import { useToast } from 'primevue/usetoast'
import { albumsApi } from '@/api/albums'
import type { AlbumDto, AlbumCategoryDto, AlbumPhotoDto } from '@/types/api'

const confirm = useConfirm()
const toast = useToast()

const albums = ref<AlbumDto[]>([])
const categories = ref<AlbumCategoryDto[]>([])
const loading = ref(false)
const filterCategoryId = ref<number | undefined>(undefined)

const albumDialog = ref(false)
const saving = ref(false)
const form = ref({
  id: 0,
  categoryId: 0,
  title: '',
  description: '',
  coverImage: '',
  isPublished: true,
  sortOrder: 0,
})

// 照片管理
const photosDialog = ref(false)
const currentAlbumId = ref(0)
const photos = ref<Omit<AlbumPhotoDto, 'id'>[]>([])

const loadCategories = async () => {
  try {
    const { data } = await albumsApi.adminGetCategories()
    if (data.success && data.data) categories.value = data.data as AlbumCategoryDto[]
  } catch { /* 靜默 */ }
}

const loadAlbums = async () => {
  loading.value = true
  try {
    const { data } = await albumsApi.adminGetList({ categoryId: filterCategoryId.value })
    if (data.success && data.data) albums.value = data.data as AlbumDto[]
  } catch { /* 靜默 */ }
  loading.value = false
}

const openCreate = () => {
  form.value = {
    id: 0,
    categoryId: categories.value[0]?.id || 0,
    title: '',
    description: '',
    coverImage: '',
    isPublished: true,
    sortOrder: 0,
  }
  albumDialog.value = true
}

const openEdit = (album: AlbumDto) => {
  form.value = {
    id: album.id,
    categoryId: album.categoryId,
    title: album.title,
    description: album.description || '',
    coverImage: album.coverImage || '',
    isPublished: album.isPublished,
    sortOrder: album.sortOrder,
  }
  albumDialog.value = true
}

const saveAlbum = async () => {
  if (!form.value.title) {
    toast.add({ severity: 'warn', summary: '提示', detail: '請填寫標題', life: 3000 })
    return
  }
  saving.value = true
  try {
    if (form.value.id) {
      await albumsApi.adminUpdate(form.value.id, form.value)
    } else {
      await albumsApi.adminCreate(form.value)
    }
    toast.add({ severity: 'success', summary: '成功', detail: '儲存成功', life: 3000 })
    albumDialog.value = false
    await loadAlbums()
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '儲存失敗', life: 3000 })
  }
  saving.value = false
}

const deleteAlbum = (album: AlbumDto) => {
  confirm.require({
    message: `確定要刪除「${album.title}」嗎？`,
    header: '確認刪除',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: '刪除',
    rejectLabel: '取消',
    accept: async () => {
      try {
        await albumsApi.adminDelete(album.id)
        toast.add({ severity: 'success', summary: '成功', detail: '刪除成功', life: 3000 })
        await loadAlbums()
      } catch {
        toast.add({ severity: 'error', summary: '錯誤', detail: '刪除失敗', life: 3000 })
      }
    },
  })
}

// 照片管理
const openPhotos = (album: AlbumDto) => {
  currentAlbumId.value = album.id
  photos.value = (album.photos || []).map(p => ({
    imageUrl: p.imageUrl,
    thumbnailUrl: p.thumbnailUrl || '',
    caption: p.caption || '',
    sortOrder: p.sortOrder,
  }))
  photosDialog.value = true
}

const addPhoto = () => {
  photos.value.push({ imageUrl: '', thumbnailUrl: '', caption: '', sortOrder: photos.value.length })
}

const removePhoto = (idx: number) => {
  photos.value.splice(idx, 1)
}

const savePhotos = async () => {
  saving.value = true
  try {
    await albumsApi.adminUpdatePhotos(currentAlbumId.value, photos.value)
    toast.add({ severity: 'success', summary: '成功', detail: '照片已更新', life: 3000 })
    photosDialog.value = false
    await loadAlbums()
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '更新失敗', life: 3000 })
  }
  saving.value = false
}

const onFilterChange = () => {
  loadAlbums()
}

onMounted(async () => {
  await loadCategories()
  await loadAlbums()
})
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold">相簿管理</h2>
      <Button label="新增相簿" icon="pi pi-plus" @click="openCreate" />
    </div>

    <div class="mb-4">
      <Select
        v-model="filterCategoryId"
        :options="[{ id: undefined, name: '全部分類' }, ...categories]"
        optionLabel="name"
        optionValue="id"
        placeholder="篩選分類"
        class="w-full md:w-64"
        @change="onFilterChange"
      />
    </div>

    <DataTable :value="albums" :loading="loading" stripedRows>
      <Column header="封面" style="width: 80px">
        <template #body="{ data }">
          <img v-if="data.coverImage" :src="`/uploads/${data.coverImage}`" class="w-12 h-12 object-cover rounded" />
          <div v-else class="w-12 h-12 bg-gray-200 rounded flex items-center justify-center">
            <i class="pi pi-image text-gray-400" />
          </div>
        </template>
      </Column>
      <Column field="title" header="標題" />
      <Column field="categoryName" header="分類" style="width: 120px" />
      <Column field="photoCount" header="照片數" style="width: 80px" />
      <Column field="sortOrder" header="排序" style="width: 80px" />
      <Column header="狀態" style="width: 80px">
        <template #body="{ data }">
          <i :class="data.isPublished ? 'pi pi-check text-green-500' : 'pi pi-times text-red-500'" />
        </template>
      </Column>
      <Column header="操作" style="width: 160px">
        <template #body="{ data }">
          <Button icon="pi pi-pencil" severity="info" size="small" text @click="openEdit(data)" />
          <Button icon="pi pi-images" severity="secondary" size="small" text @click="openPhotos(data)" v-tooltip="'照片管理'" />
          <Button icon="pi pi-trash" severity="danger" size="small" text @click="deleteAlbum(data)" />
        </template>
      </Column>
    </DataTable>

    <!-- 相簿編輯 Dialog -->
    <Dialog v-model:visible="albumDialog" :header="form.id ? '編輯相簿' : '新增相簿'" :style="{ width: '600px' }" modal>
      <div class="space-y-4">
        <div>
          <label class="block text-sm font-medium mb-1">標題 *</label>
          <InputText v-model="form.title" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">分類</label>
          <Select v-model="form.categoryId" :options="categories" optionLabel="name" optionValue="id" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">描述</label>
          <Textarea v-model="form.description" rows="3" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">封面圖片路徑</label>
          <InputText v-model="form.coverImage" class="w-full" placeholder="例：albums/cover1.jpg" />
        </div>
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium mb-1">排序</label>
            <InputNumber v-model="form.sortOrder" class="w-full" />
          </div>
          <div class="flex items-end pb-1">
            <Checkbox v-model="form.isPublished" :binary="true" inputId="albumPublished" />
            <label for="albumPublished" class="ml-2 text-sm">發佈</label>
          </div>
        </div>
      </div>
      <template #footer>
        <Button label="取消" severity="secondary" @click="albumDialog = false" />
        <Button label="儲存" icon="pi pi-check" :loading="saving" @click="saveAlbum" />
      </template>
    </Dialog>

    <!-- 照片管理 Dialog -->
    <Dialog v-model:visible="photosDialog" header="照片管理" :style="{ width: '70vw' }" modal>
      <div v-for="(photo, idx) in photos" :key="idx" class="border rounded p-4 mb-3">
        <div class="flex justify-between items-center mb-2">
          <span class="font-medium">照片 {{ idx + 1 }}</span>
          <Button icon="pi pi-trash" severity="danger" size="small" text @click="removePhoto(idx)" />
        </div>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-3">
          <div>
            <label class="block text-sm mb-1">圖片路徑 *</label>
            <InputText v-model="photo.imageUrl" class="w-full" />
          </div>
          <div>
            <label class="block text-sm mb-1">縮圖路徑</label>
            <InputText v-model="photo.thumbnailUrl" class="w-full" />
          </div>
          <div>
            <label class="block text-sm mb-1">說明文字</label>
            <InputText v-model="photo.caption" class="w-full" />
          </div>
        </div>
      </div>
      <Button label="新增照片" icon="pi pi-plus" severity="secondary" @click="addPhoto" class="mb-4" />
      <template #footer>
        <Button label="取消" severity="secondary" @click="photosDialog = false" />
        <Button label="儲存" icon="pi pi-check" :loading="saving" @click="savePhotos" />
      </template>
    </Dialog>
  </div>
</template>
