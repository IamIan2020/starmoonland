<script setup lang="ts">
import { ref, onMounted } from 'vue'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import Checkbox from 'primevue/checkbox'
import Select from 'primevue/select'
import Tabs from 'primevue/tabs'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import TabPanels from 'primevue/tabpanels'
import TabPanel from 'primevue/tabpanel'
import { useConfirm } from 'primevue/useconfirm'
import { useToast } from 'primevue/usetoast'
import WangEditor from '@/components/WangEditor.vue'
import { pagesApi } from '@/api/pages'
import type { PageCategoryDto, PageDto, PageSlideDto, PageTabDto } from '@/types/api'

const confirm = useConfirm()
const toast = useToast()

const categories = ref<PageCategoryDto[]>([])
const selectedCategoryId = ref<number | null>(null)
const pages = ref<PageDto[]>([])
const loading = ref(false)

// 頁面編輯 Dialog
const pageDialog = ref(false)
const pageForm = ref({
  id: 0,
  categoryId: 0,
  slug: '',
  title: '',
  subtitle: '',
  content: '',
  sortOrder: 0,
  isVisible: true,
  metaTitle: '',
  metaDescription: '',
})
const saving = ref(false)

// 輪播管理 Dialog
const slidesDialog = ref(false)
const currentPageId = ref(0)
const slides = ref<Omit<PageSlideDto, 'id'>[]>([])

// Tab 管理 Dialog
const tabsDialog = ref(false)
const tabs = ref<Omit<PageTabDto, 'id'>[]>([])

const loadCategories = async () => {
  try {
    const { data } = await pagesApi.adminGetCategories()
    if (data.success && data.data) categories.value = data.data as PageCategoryDto[]
  } catch { /* 靜默 */ }
}

const loadPages = async () => {
  if (!selectedCategoryId.value) return
  loading.value = true
  try {
    const { data } = await pagesApi.adminGetPages(selectedCategoryId.value)
    if (data.success && data.data) pages.value = data.data as PageDto[]
  } catch { /* 靜默 */ }
  loading.value = false
}

const openCreate = () => {
  if (!selectedCategoryId.value) {
    toast.add({ severity: 'warn', summary: '提示', detail: '請先選擇分類', life: 3000 })
    return
  }
  pageForm.value = {
    id: 0,
    categoryId: selectedCategoryId.value,
    slug: '',
    title: '',
    subtitle: '',
    content: '',
    sortOrder: 0,
    isVisible: true,
    metaTitle: '',
    metaDescription: '',
  }
  pageDialog.value = true
}

const openEdit = (page: PageDto) => {
  pageForm.value = {
    id: page.id,
    categoryId: page.categoryId,
    slug: page.slug,
    title: page.title,
    subtitle: page.subtitle || '',
    content: page.content || '',
    sortOrder: page.sortOrder,
    isVisible: page.isVisible,
    metaTitle: page.metaTitle || '',
    metaDescription: page.metaDescription || '',
  }
  pageDialog.value = true
}

const savePage = async () => {
  if (!pageForm.value.title || !pageForm.value.slug) {
    toast.add({ severity: 'warn', summary: '提示', detail: '請填寫標題和 Slug', life: 3000 })
    return
  }
  saving.value = true
  try {
    if (pageForm.value.id) {
      await pagesApi.adminUpdatePage(pageForm.value.id, pageForm.value)
    } else {
      await pagesApi.adminCreatePage(pageForm.value)
    }
    toast.add({ severity: 'success', summary: '成功', detail: '儲存成功', life: 3000 })
    pageDialog.value = false
    await loadPages()
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '儲存失敗', life: 3000 })
  }
  saving.value = false
}

const deletePage = (page: PageDto) => {
  confirm.require({
    message: `確定要刪除「${page.title}」嗎？`,
    header: '確認刪除',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: '刪除',
    rejectLabel: '取消',
    accept: async () => {
      try {
        await pagesApi.adminDeletePage(page.id)
        toast.add({ severity: 'success', summary: '成功', detail: '刪除成功', life: 3000 })
        await loadPages()
      } catch {
        toast.add({ severity: 'error', summary: '錯誤', detail: '刪除失敗', life: 3000 })
      }
    },
  })
}

// 輪播管理
const openSlides = (page: PageDto) => {
  currentPageId.value = page.id
  slides.value = (page.slides || []).map(s => ({
    imageUrl: s.imageUrl,
    title: s.title || '',
    description: s.description || '',
    sortOrder: s.sortOrder,
  }))
  slidesDialog.value = true
}

const addSlide = () => {
  slides.value.push({ imageUrl: '', title: '', description: '', sortOrder: slides.value.length })
}

const removeSlide = (idx: number) => {
  slides.value.splice(idx, 1)
}

const saveSlides = async () => {
  saving.value = true
  try {
    await pagesApi.adminUpdateSlides(currentPageId.value, slides.value)
    toast.add({ severity: 'success', summary: '成功', detail: '輪播圖已更新', life: 3000 })
    slidesDialog.value = false
    await loadPages()
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '更新失敗', life: 3000 })
  }
  saving.value = false
}

// Tab 管理
const openTabs = (page: PageDto) => {
  currentPageId.value = page.id
  tabs.value = (page.tabs || []).map(t => ({
    title: t.title,
    content: t.content || '',
    sortOrder: t.sortOrder,
  }))
  tabsDialog.value = true
}

const addTab = () => {
  tabs.value.push({ title: '', content: '', sortOrder: tabs.value.length })
}

const removeTab = (idx: number) => {
  tabs.value.splice(idx, 1)
}

const saveTabs = async () => {
  saving.value = true
  try {
    await pagesApi.adminUpdateTabs(currentPageId.value, tabs.value)
    toast.add({ severity: 'success', summary: '成功', detail: 'Tab 已更新', life: 3000 })
    tabsDialog.value = false
    await loadPages()
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '更新失敗', life: 3000 })
  }
  saving.value = false
}

const onCategoryChange = () => {
  loadPages()
}

onMounted(async () => {
  await loadCategories()
  if (categories.value.length > 0) {
    selectedCategoryId.value = categories.value[0]!.id
    await loadPages()
  }
})
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold">頁面管理</h2>
      <Button label="新增頁面" icon="pi pi-plus" @click="openCreate" />
    </div>

    <div class="mb-4">
      <label class="block text-sm font-medium mb-1">分類篩選</label>
      <Select
        v-model="selectedCategoryId"
        :options="categories"
        optionLabel="titleZh"
        optionValue="id"
        placeholder="選擇分類"
        class="w-full md:w-64"
        @change="onCategoryChange"
      />
    </div>

    <DataTable :value="pages" :loading="loading" stripedRows>
      <Column field="title" header="標題" />
      <Column field="slug" header="Slug" />
      <Column field="subtitle" header="副標題" />
      <Column field="sortOrder" header="排序" style="width: 80px" />
      <Column field="isVisible" header="顯示" style="width: 80px">
        <template #body="{ data }">
          <i :class="data.isVisible ? 'pi pi-check text-green-500' : 'pi pi-times text-red-500'" />
        </template>
      </Column>
      <Column header="操作" style="width: 280px">
        <template #body="{ data }">
          <div class="flex gap-1">
            <Button icon="pi pi-pencil" severity="info" size="small" text @click="openEdit(data)" />
            <Button icon="pi pi-images" severity="secondary" size="small" text @click="openSlides(data)" v-tooltip="'輪播圖'" />
            <Button icon="pi pi-list" severity="secondary" size="small" text @click="openTabs(data)" v-tooltip="'Tab內容'" />
            <Button icon="pi pi-trash" severity="danger" size="small" text @click="deletePage(data)" />
          </div>
        </template>
      </Column>
    </DataTable>

    <!-- 頁面編輯 Dialog -->
    <Dialog v-model:visible="pageDialog" :header="pageForm.id ? '編輯頁面' : '新增頁面'" :style="{ width: '80vw' }" modal>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
        <div>
          <label class="block text-sm font-medium mb-1">標題 *</label>
          <InputText v-model="pageForm.title" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">Slug *</label>
          <InputText v-model="pageForm.slug" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">副標題</label>
          <InputText v-model="pageForm.subtitle" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">排序</label>
          <InputNumber v-model="pageForm.sortOrder" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">Meta Title</label>
          <InputText v-model="pageForm.metaTitle" class="w-full" />
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">Meta Description</label>
          <InputText v-model="pageForm.metaDescription" class="w-full" />
        </div>
      </div>
      <div class="mb-4">
        <Checkbox v-model="pageForm.isVisible" :binary="true" inputId="pageVisible" />
        <label for="pageVisible" class="ml-2 text-sm">顯示此頁面</label>
      </div>
      <div class="mb-4">
        <label class="block text-sm font-medium mb-1">內容</label>
        <WangEditor v-model="pageForm.content" />
      </div>
      <template #footer>
        <Button label="取消" severity="secondary" @click="pageDialog = false" />
        <Button label="儲存" icon="pi pi-check" :loading="saving" @click="savePage" />
      </template>
    </Dialog>

    <!-- 輪播圖管理 Dialog -->
    <Dialog v-model:visible="slidesDialog" header="輪播圖管理" :style="{ width: '70vw' }" modal>
      <div v-for="(slide, idx) in slides" :key="idx" class="border rounded p-4 mb-3">
        <div class="flex justify-between items-center mb-2">
          <span class="font-medium">圖片 {{ idx + 1 }}</span>
          <Button icon="pi pi-trash" severity="danger" size="small" text @click="removeSlide(idx)" />
        </div>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-3">
          <div>
            <label class="block text-sm mb-1">圖片路徑</label>
            <InputText v-model="slide.imageUrl" class="w-full" placeholder="例：pages/slide1.jpg" />
          </div>
          <div>
            <label class="block text-sm mb-1">標題</label>
            <InputText v-model="slide.title" class="w-full" />
          </div>
          <div>
            <label class="block text-sm mb-1">描述</label>
            <InputText v-model="slide.description" class="w-full" />
          </div>
        </div>
      </div>
      <Button label="新增圖片" icon="pi pi-plus" severity="secondary" @click="addSlide" class="mb-4" />
      <template #footer>
        <Button label="取消" severity="secondary" @click="slidesDialog = false" />
        <Button label="儲存" icon="pi pi-check" :loading="saving" @click="saveSlides" />
      </template>
    </Dialog>

    <!-- Tab 管理 Dialog -->
    <Dialog v-model:visible="tabsDialog" header="Tab 內容管理" :style="{ width: '80vw' }" modal>
      <Tabs :value="0">
        <TabList>
          <Tab v-for="(tab, idx) in tabs" :key="idx" :value="idx">
            {{ tab.title || `Tab ${idx + 1}` }}
          </Tab>
        </TabList>
        <TabPanels>
          <TabPanel v-for="(tab, idx) in tabs" :key="idx" :value="idx">
            <div class="flex justify-between items-center mb-3">
              <div class="flex-1 mr-4">
                <label class="block text-sm mb-1">Tab 標題</label>
                <InputText v-model="tab.title" class="w-full" />
              </div>
              <Button icon="pi pi-trash" severity="danger" size="small" text @click="removeTab(idx)" />
            </div>
            <WangEditor :modelValue="tab.content ?? ''" @update:modelValue="tab.content = $event" />
          </TabPanel>
        </TabPanels>
      </Tabs>
      <Button label="新增 Tab" icon="pi pi-plus" severity="secondary" @click="addTab" class="mt-4" />
      <template #footer>
        <Button label="取消" severity="secondary" @click="tabsDialog = false" />
        <Button label="儲存" icon="pi pi-check" :loading="saving" @click="saveTabs" />
      </template>
    </Dialog>
  </div>
</template>
