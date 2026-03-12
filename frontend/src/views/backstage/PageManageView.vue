<script setup lang="ts">
import { ref, onMounted } from 'vue'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import Textarea from 'primevue/textarea'
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
const editActiveTab = ref('basic')
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

// 輪播圖（整合到編輯 Dialog 內）
const slides = ref<Omit<PageSlideDto, 'id'>[]>([])

// Tab 內容（整合到編輯 Dialog 內）
const pageTabs = ref<Omit<PageTabDto, 'id'>[]>([])
const activeContentTab = ref(0)

const loadCategories = async () => {
  try {
    const { data } = await pagesApi.adminGetCategories()
    if (data.success && data.data) categories.value = data.data as PageCategoryDto[]
  } catch (err) { console.error(err); toast.add({ severity: 'error', summary: '錯誤', detail: '載入失敗', life: 3000 }) }
}

const loadPages = async () => {
  if (!selectedCategoryId.value) return
  loading.value = true
  try {
    const { data } = await pagesApi.adminGetPages(selectedCategoryId.value)
    if (data.success && data.data) pages.value = data.data as PageDto[]
  } catch (err) { console.error(err); toast.add({ severity: 'error', summary: '錯誤', detail: '載入失敗', life: 3000 }) }
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
  slides.value = []
  pageTabs.value = []
  editActiveTab.value = 'basic'
  activeContentTab.value = 0
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
  slides.value = (page.slides || []).map(s => ({
    imageUrl: s.imageUrl,
    title: s.title || '',
    description: s.description || '',
    sortOrder: s.sortOrder,
  }))
  pageTabs.value = (page.tabs || []).map(t => ({
    title: t.title,
    content: t.content || '',
    sortOrder: t.sortOrder,
  }))
  editActiveTab.value = 'basic'
  activeContentTab.value = 0
  pageDialog.value = true
}

const savePage = async () => {
  if (!pageForm.value.title || !pageForm.value.slug) {
    toast.add({ severity: 'warn', summary: '提示', detail: '請填寫標題和 Slug', life: 3000 })
    editActiveTab.value = 'basic'
    return
  }
  saving.value = true
  try {
    let pageId = pageForm.value.id
    if (pageId) {
      await pagesApi.adminUpdatePage(pageId, pageForm.value)
    } else {
      const { data } = await pagesApi.adminCreatePage(pageForm.value)
      if (data.success && data.data) pageId = (data.data as PageDto).id
    }

    // 儲存輪播圖
    if (pageId) {
      await pagesApi.adminUpdateSlides(pageId, slides.value)
      await pagesApi.adminUpdateTabs(pageId, pageTabs.value)
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

// 輪播圖操作
const addSlide = () => {
  slides.value.push({ imageUrl: '', title: '', description: '', sortOrder: slides.value.length })
}

const removeSlide = (idx: number) => {
  slides.value.splice(idx, 1)
}

// Tab 操作
const addContentTab = () => {
  pageTabs.value.push({ title: '', content: '', sortOrder: pageTabs.value.length })
  activeContentTab.value = pageTabs.value.length - 1
}

const removeContentTab = (idx: number) => {
  pageTabs.value.splice(idx, 1)
  if (activeContentTab.value >= pageTabs.value.length) {
    activeContentTab.value = Math.max(0, pageTabs.value.length - 1)
  }
}

// 頁面統計資訊
const getSlideCount = (page: PageDto) => page.slides?.length || 0
const getTabCount = (page: PageDto) => page.tabs?.length || 0

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
      <Column header="輪播/Tab" style="width: 120px">
        <template #body="{ data }">
          <div class="flex gap-2 text-xs">
            <span v-if="getSlideCount(data)" class="bg-blue-100 text-blue-700 px-2 py-0.5 rounded">
              {{ getSlideCount(data) }} 張輪播
            </span>
            <span v-if="getTabCount(data)" class="bg-green-100 text-green-700 px-2 py-0.5 rounded">
              {{ getTabCount(data) }} 個Tab
            </span>
          </div>
        </template>
      </Column>
      <Column field="sortOrder" header="排序" style="width: 80px" />
      <Column field="isVisible" header="顯示" style="width: 80px">
        <template #body="{ data }">
          <i :class="data.isVisible ? 'pi pi-check text-green-500' : 'pi pi-times text-red-500'" />
        </template>
      </Column>
      <Column header="操作" style="width: 120px">
        <template #body="{ data }">
          <div class="flex gap-1">
            <Button icon="pi pi-pencil" severity="info" size="small" text @click="openEdit(data)" v-tooltip="'編輯'" />
            <Button icon="pi pi-trash" severity="danger" size="small" text @click="deletePage(data)" v-tooltip="'刪除'" />
          </div>
        </template>
      </Column>
    </DataTable>

    <!-- 頁面編輯 Dialog（整合全部功能） -->
    <Dialog
      v-model:visible="pageDialog"
      :header="pageForm.id ? '編輯頁面' : '新增頁面'"
      :style="{ width: '85vw', maxWidth: '1200px' }"
      :position="'top'"
      :draggable="false"
      modal
      class="page-edit-dialog"
    >
      <div class="page-dialog-body">
        <Tabs v-model:value="editActiveTab">
          <TabList>
            <Tab value="basic">
              <i class="pi pi-file-edit mr-2" />基本資訊
            </Tab>
            <Tab value="content">
              <i class="pi pi-align-left mr-2" />頁面內容
            </Tab>
            <Tab value="slides">
              <i class="pi pi-images mr-2" />輪播圖
              <span v-if="slides.length" class="ml-2 bg-blue-100 text-blue-700 text-xs px-1.5 py-0.5 rounded-full">{{ slides.length }}</span>
            </Tab>
            <Tab value="tabs">
              <i class="pi pi-list mr-2" />Tab 內容
              <span v-if="pageTabs.length" class="ml-2 bg-green-100 text-green-700 text-xs px-1.5 py-0.5 rounded-full">{{ pageTabs.length }}</span>
            </Tab>
          </TabList>

          <TabPanels>
            <!-- 基本資訊 -->
            <TabPanel value="basic">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4 pt-4">
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
            </TabPanel>

            <!-- 頁面內容（WangEditor） -->
            <TabPanel value="content">
              <div class="pt-4">
                <p class="text-sm text-gray-500 mb-3">
                  使用編輯器設定頁面內容，可插入文字、圖片等。適用於「星月緣起」、「吉祥物介紹」等以內容為主的頁面。
                </p>
                <WangEditor v-model="pageForm.content" />
              </div>
            </TabPanel>

            <!-- 輪播圖管理 -->
            <TabPanel value="slides">
              <div class="pt-4">
                <p class="text-sm text-gray-500 mb-3">
                  設定輪播圖後，前台會顯示為「金色文字面板 + 圖片輪播」的風格。適用於「星月願景」、「大地營區」等頁面。
                </p>

                <div v-for="(slide, idx) in slides" :key="idx" class="border rounded p-4 mb-3">
                  <div class="flex justify-between items-center mb-2">
                    <span class="font-medium">圖片 {{ idx + 1 }}</span>
                    <Button icon="pi pi-trash" severity="danger" size="small" text @click="removeSlide(idx)" />
                  </div>
                  <div class="grid grid-cols-1 md:grid-cols-3 gap-3">
                    <div>
                      <label class="block text-sm mb-1">圖片路徑</label>
                      <InputText v-model="slide.imageUrl" class="w-full" placeholder="例：/_images/about/pic.png" />
                    </div>
                    <div>
                      <label class="block text-sm mb-1">標題（金色面板標題）</label>
                      <InputText v-model="slide.title" class="w-full" placeholder="例：壯麗山景" />
                    </div>
                    <div>
                      <label class="block text-sm mb-1">描述（金色面板文字）</label>
                      <Textarea v-model="slide.description" class="w-full" rows="2" placeholder="例：隨著四季..." />
                    </div>
                  </div>
                </div>

                <Button label="新增輪播圖" icon="pi pi-plus" severity="secondary" @click="addSlide" />

                <div v-if="slides.length === 0" class="text-center py-8 text-gray-400 border-2 border-dashed rounded mt-4">
                  <i class="pi pi-images text-4xl mb-2" />
                  <p>尚未設定輪播圖</p>
                  <p class="text-sm">點擊「新增輪播圖」開始設定</p>
                </div>
              </div>
            </TabPanel>

            <!-- Tab 內容管理 -->
            <TabPanel value="tabs">
              <div class="pt-4">
                <p class="text-sm text-gray-500 mb-3">
                  設定 Tab 內容後，前台會在輪播圖下方顯示可切換的 Tab 區塊。適用於「營區資訊/營地須知/營區價目」等多區塊內容。
                </p>

                <div v-if="pageTabs.length > 0">
                  <Tabs v-model:value="activeContentTab">
                    <TabList>
                      <Tab v-for="(tab, idx) in pageTabs" :key="idx" :value="idx">
                        {{ tab.title || `Tab ${idx + 1}` }}
                      </Tab>
                    </TabList>
                    <TabPanels>
                      <TabPanel v-for="(tab, idx) in pageTabs" :key="idx" :value="idx">
                        <div class="flex justify-between items-center mb-3 pt-3">
                          <div class="flex-1 mr-4">
                            <label class="block text-sm mb-1">Tab 標題</label>
                            <InputText v-model="tab.title" class="w-full" />
                          </div>
                          <Button icon="pi pi-trash" severity="danger" size="small" text @click="removeContentTab(idx)" v-tooltip="'刪除此 Tab'" />
                        </div>
                        <WangEditor :modelValue="tab.content ?? ''" @update:modelValue="tab.content = $event" />
                      </TabPanel>
                    </TabPanels>
                  </Tabs>
                </div>

                <Button label="新增 Tab" icon="pi pi-plus" severity="secondary" @click="addContentTab" class="mt-4" />

                <div v-if="pageTabs.length === 0" class="text-center py-8 text-gray-400 border-2 border-dashed rounded mt-4">
                  <i class="pi pi-list text-4xl mb-2" />
                  <p>尚未設定 Tab 內容</p>
                  <p class="text-sm">點擊「新增 Tab」開始設定</p>
                </div>
              </div>
            </TabPanel>
          </TabPanels>
        </Tabs>
      </div>

      <template #footer>
        <Button label="取消" severity="secondary" @click="pageDialog = false" />
        <Button label="全部儲存" icon="pi pi-check" :loading="saving" @click="savePage" />
      </template>
    </Dialog>
  </div>
</template>

<style scoped>
.page-edit-dialog :deep(.p-dialog-content) {
  padding: 0 1.5rem;
  overflow-y: auto;
  max-height: calc(100vh - 200px);
}

.page-dialog-body {
  min-height: 400px;
}
</style>
