import { defineStore } from 'pinia'
import { ref } from 'vue'
import { settingsApi } from '@/api/settings'
import type { SiteSettingDto } from '@/types/api'

export const useSettingsStore = defineStore('settings', () => {
  const settings = ref<Record<string, string>>({})
  const loaded = ref(false)

  async function fetchSettings() {
    if (loaded.value) return
    try {
      const { data } = await settingsApi.getSiteSettings()
      if (data.success && data.data) {
        const map: Record<string, string> = {}
        data.data.forEach((s: SiteSettingDto) => {
          if (s.value) map[s.key] = s.value
        })
        settings.value = map
        loaded.value = true
      }
    } catch {
      // 靜默失敗
    }
  }

  function get(key: string, fallback = ''): string {
    return settings.value[key] || fallback
  }

  return { settings, loaded, fetchSettings, get }
})
