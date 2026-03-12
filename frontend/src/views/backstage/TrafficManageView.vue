<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import Tabs from 'primevue/tabs'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import TabPanels from 'primevue/tabpanels'
import TabPanel from 'primevue/tabpanel'
import { useToast } from 'primevue/usetoast'
import WangEditor from '@/components/WangEditor.vue'
import { settingsApi } from '@/api/settings'
import type { TrafficInfoDto } from '@/types/api'

const toast = useToast()

const trafficInfos = ref<TrafficInfoDto[]>([])
const loading = ref(false)
const saving = ref(false)

const loadTraffic = async () => {
  loading.value = true
  try {
    const { data } = await settingsApi.adminGetTraffic()
    if (data.success && data.data) trafficInfos.value = data.data as TrafficInfoDto[]
  } catch (err) { console.error(err); toast.add({ severity: 'error', summary: '錯誤', detail: '載入失敗', life: 3000 }) }
  loading.value = false
}

const addTab = () => {
  trafficInfos.value.push({
    id: 0,
    tabName: '新交通方式',
    content: '',
    sortOrder: trafficInfos.value.length,
  })
}

const removeTab = (idx: number) => {
  trafficInfos.value.splice(idx, 1)
}

const saveTraffic = async () => {
  saving.value = true
  try {
    await settingsApi.adminUpdateTraffic(trafficInfos.value)
    toast.add({ severity: 'success', summary: '成功', detail: '交通資訊已更新', life: 3000 })
    await loadTraffic()
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '更新失敗', life: 3000 })
  }
  saving.value = false
}

onMounted(loadTraffic)
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold">交通資訊管理</h2>
      <div class="flex gap-2">
        <Button label="新增 Tab" icon="pi pi-plus" severity="secondary" @click="addTab" />
        <Button label="儲存全部" icon="pi pi-check" :loading="saving" @click="saveTraffic" />
      </div>
    </div>

    <div v-if="loading" class="text-center py-10 text-gray-400">載入中...</div>

    <Tabs v-else :value="0">
      <TabList>
        <Tab v-for="(info, idx) in trafficInfos" :key="idx" :value="idx">
          {{ info.tabName || `Tab ${idx + 1}` }}
        </Tab>
      </TabList>
      <TabPanels>
        <TabPanel v-for="(info, idx) in trafficInfos" :key="idx" :value="idx">
          <div class="space-y-4 pt-4">
            <div class="flex gap-4 items-end">
              <div class="flex-1">
                <label class="block text-sm font-medium mb-1">Tab 名稱</label>
                <InputText v-model="info.tabName" class="w-full" />
              </div>
              <div>
                <label class="block text-sm font-medium mb-1">排序</label>
                <InputNumber v-model="info.sortOrder" class="w-24" />
              </div>
              <Button icon="pi pi-trash" severity="danger" size="small" @click="removeTab(idx)" v-tooltip="'刪除此 Tab'" />
            </div>
            <div>
              <label class="block text-sm font-medium mb-1">內容</label>
              <WangEditor :modelValue="info.content ?? ''" @update:modelValue="info.content = $event" />
            </div>
          </div>
        </TabPanel>
      </TabPanels>
    </Tabs>
  </div>
</template>
