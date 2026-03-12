<script setup lang="ts">
import { ref, onMounted } from 'vue'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import Textarea from 'primevue/textarea'
import Select from 'primevue/select'
import Checkbox from 'primevue/checkbox'
import { useConfirm } from 'primevue/useconfirm'
import { useToast } from 'primevue/usetoast'
import WangEditor from '@/components/WangEditor.vue'
import ImageUploader from '@/components/ImageUploader.vue'
import { newsApi } from '@/api/news'
import type { NewsDto, NewsCategoryDto } from '@/types/api'

const confirm = useConfirm()
const toast = useToast()

const newsList = ref<NewsDto[]>([])
const categories = ref<NewsCategoryDto[]>([])
const loading = ref(false)
const totalRecords = ref(0)
const currentPage = ref(1)
const pageSize = ref(10)
const filterCategoryId = ref<number | undefined>(undefined)

const newsDialog = ref(false)
const saving = ref(false)
const form = ref({
  id: 0,
  categoryId: 0,
  title: '',
  summary: '',
  content: '',
  coverImage: '',
  publishDate: new Date().toISOString(),
  isPublished: false,
  isPinned: false,
})

const loadCategories = async () => {
  try {
    const { data } = await newsApi.adminGetCategories()
    if (data.success && data.data) categories.value = data.data as NewsCategoryDto[]
  } catch (err) { console.error(err); toast.add({ severity: 'error', summary: '錯誤', detail: '載入失敗', life: 3000 }) }
}

const loadNews = async () => {
  loading.value = true
  try {
    const { data } = await newsApi.adminGetList({
      page: currentPage.value,
      pageSize: pageSize.value,
      categoryId: filterCategoryId.value,
    })
    newsList.value = data.data
    totalRecords.value = data.totalCount
  } catch (err) { console.error(err); toast.add({ severity: 'error', summary: '錯誤', detail: '載入失敗', life: 3000 }) }
  loading.value = false
}

const onPage = (event: { page: number; rows: number }) => {
  currentPage.value = event.page + 1
  pageSize.value = event.rows
  loadNews()
}

const openCreate = () => {
  form.value = {
    id: 0,
    categoryId: categories.value[0]?.id || 0,
    title: '',
    summary: '',
    content: '',
    coverImage: '',
    publishDate: new Date().toISOString(),
    isPublished: false,
    isPinned: false,
  }
  newsDialog.value = true
}

const openEdit = (news: NewsDto) => {
  form.value = {
    id: news.id,
    categoryId: news.categoryId,
    title: news.title,
    summary: news.summary || '',
    content: news.content || '',
    coverImage: news.coverImage || '',
    publishDate: news.publishDate,
    isPublished: news.isPublished,
    isPinned: news.isPinned,
  }
  newsDialog.value = true
}

const saveNews = async () => {
  if (!form.value.title || !form.value.categoryId) {
    toast.add({ severity: 'warn', summary: '提示', detail: '請填寫標題並選擇分類', life: 3000 })
    return
  }
  saving.value = true
  try {
    if (form.value.id) {
      await newsApi.adminUpdate(form.value.id, form.value)
    } else {
      await newsApi.adminCreate(form.value)
    }
    toast.add({ severity: 'success', summary: '成功', detail: '儲存成功', life: 3000 })
    newsDialog.value = false
    await loadNews()
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '儲存失敗', life: 3000 })
  }
  saving.value = false
}

const deleteNews = (news: NewsDto) => {
  confirm.require({
    message: `確定要刪除「${news.title}」嗎？`,
    header: '確認刪除',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: '刪除',
    rejectLabel: '取消',
    accept: async () => {
      try {
        await newsApi.adminDelete(news.id)
        toast.add({ severity: 'success', summary: '成功', detail: '刪除成功', life: 3000 })
        await loadNews()
      } catch {
        toast.add({ severity: 'error', summary: '錯誤', detail: '刪除失敗', life: 3000 })
      }
    },
  })
}

const onFilterChange = () => {
  currentPage.value = 1
  loadNews()
}

onMounted(async () => {
  await loadCategories()
  await loadNews()
})
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold">新聞管理</h2>
      <Button label="新增新聞" icon="pi pi-plus" @click="openCreate" />
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

    <DataTable
      :value="newsList"
      :loading="loading"
      lazy
      paginator
      :rows="pageSize"
      :totalRecords="totalRecords"
      :first="(currentPage - 1) * pageSize"
      @page="onPage"
      stripedRows
    >
      <Column field="title" header="標題" />
      <Column field="categoryName" header="分類" style="width: 120px" />
      <Column header="發佈日期" style="width: 120px">
        <template #body="{ data }">
          {{ new Date(data.publishDate).toLocaleDateString('zh-TW') }}
        </template>
      </Column>
      <Column field="viewCount" header="瀏覽" style="width: 80px" />
      <Column header="狀態" style="width: 120px">
        <template #body="{ data }">
          <span v-if="data.isPinned" class="text-xs bg-orange-100 text-orange-700 px-2 py-0.5 rounded mr-1">置頂</span>
          <span :class="data.isPublished ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-500'" class="text-xs px-2 py-0.5 rounded">
            {{ data.isPublished ? '已發佈' : '草稿' }}
          </span>
        </template>
      </Column>
      <Column header="操作" style="width: 120px">
        <template #body="{ data }">
          <Button icon="pi pi-pencil" severity="info" size="small" text @click="openEdit(data)" />
          <Button icon="pi pi-trash" severity="danger" size="small" text @click="deleteNews(data)" />
        </template>
      </Column>
    </DataTable>

    <Dialog
      v-model:visible="newsDialog"
      :header="form.id ? '編輯新聞' : '新增新聞'"
      :style="{ width: '80vw', maxWidth: '1000px' }"
      :position="'top'"
      :draggable="false"
      modal
    >
      <div :style="{ maxHeight: 'calc(100vh - 200px)', overflowY: 'auto' }">
        <div class="grid grid-cols-1 md:grid-cols-[200px_1fr] gap-4 mb-4">
          <!-- 封面圖片 -->
          <div>
            <label class="block text-sm font-medium mb-1">封面圖片</label>
            <ImageUploader v-model="form.coverImage" height="160px" />
          </div>
          <!-- 基本欄位 -->
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium mb-1">標題 *</label>
              <InputText v-model="form.title" class="w-full" />
            </div>
            <div>
              <label class="block text-sm font-medium mb-1">分類 *</label>
              <Select v-model="form.categoryId" :options="categories" optionLabel="name" optionValue="id" class="w-full" />
            </div>
            <div>
              <label class="block text-sm font-medium mb-1">發佈日期</label>
              <InputText v-model="form.publishDate" class="w-full" placeholder="YYYY-MM-DD" />
            </div>
            <div class="flex items-end gap-4 pb-1">
              <div>
                <Checkbox v-model="form.isPublished" :binary="true" inputId="newsPublished" />
                <label for="newsPublished" class="ml-2 text-sm">發佈</label>
              </div>
              <div>
                <Checkbox v-model="form.isPinned" :binary="true" inputId="newsPinned" />
                <label for="newsPinned" class="ml-2 text-sm">置頂</label>
              </div>
            </div>
          </div>
        </div>
        <div class="mb-4">
          <label class="block text-sm font-medium mb-1">摘要</label>
          <Textarea v-model="form.summary" rows="3" class="w-full" />
        </div>
        <div class="mb-4">
          <label class="block text-sm font-medium mb-1">內容</label>
          <WangEditor v-model="form.content" />
        </div>
      </div>
      <template #footer>
        <Button label="取消" severity="secondary" @click="newsDialog = false" />
        <Button label="儲存" icon="pi pi-check" :loading="saving" @click="saveNews" />
      </template>
    </Dialog>
  </div>
</template>
