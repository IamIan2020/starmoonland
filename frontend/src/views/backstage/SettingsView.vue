<script setup lang="ts">
import { ref, onMounted } from 'vue'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import { useToast } from 'primevue/usetoast'
import { settingsApi } from '@/api/settings'
import type { SiteSettingDto } from '@/types/api'

const toast = useToast()

const settings = ref<SiteSettingDto[]>([])
const loading = ref(false)
const saving = ref(false)

// 設定欄位的顯示名稱
const labelMap: Record<string, string> = {
  phone: '電話',
  address: '地址',
  email: '聯絡信箱',
  facebook_url: 'Facebook 網址',
  instagram_url: 'Instagram 網址',
  line_url: 'LINE 網址',
  google_maps_embed: 'Google Maps 嵌入網址',
  admin_notification_email: '管理員通知信箱',
  business_hours: '營業時間',
  site_title: '網站標題',
  site_description: '網站描述',
}

const loadSettings = async () => {
  loading.value = true
  try {
    const { data } = await settingsApi.getSiteSettings()
    if (data.success && data.data) settings.value = data.data as SiteSettingDto[]
  } catch { /* 靜默 */ }
  loading.value = false
}

const saveSettings = async () => {
  saving.value = true
  try {
    await settingsApi.adminUpdateSettings({
      settings: settings.value.map(s => ({ key: s.key, value: s.value || '' })),
    })
    toast.add({ severity: 'success', summary: '成功', detail: '設定已儲存', life: 3000 })
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '儲存失敗', life: 3000 })
  }
  saving.value = false
}

const getLabel = (key: string) => labelMap[key] || key

onMounted(loadSettings)
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold">網站設定</h2>
      <Button label="儲存" icon="pi pi-check" :loading="saving" @click="saveSettings" />
    </div>

    <div v-if="loading" class="text-center py-10 text-gray-400">載入中...</div>

    <div v-else class="max-w-[800px] space-y-4">
      <div v-for="setting in settings" :key="setting.key" class="grid grid-cols-1 md:grid-cols-3 gap-2 items-center border-b pb-4">
        <div>
          <label class="font-medium text-sm">{{ getLabel(setting.key) }}</label>
          <p v-if="setting.description" class="text-xs text-gray-400">{{ setting.description }}</p>
        </div>
        <div class="md:col-span-2">
          <InputText
            v-if="setting.key !== 'google_maps_embed'"
            v-model="setting.value"
            class="w-full"
          />
          <textarea
            v-else
            v-model="setting.value"
            rows="3"
            class="w-full border rounded px-3 py-2 text-sm"
            placeholder="Google Maps 嵌入網址"
          />
        </div>
      </div>
    </div>
  </div>
</template>
