import apiClient from './client'
import type { ApiResponse, SiteSettingDto, HomepageData, HomepageSlideDto, TrafficInfoDto, UpdateSiteSettingsRequest } from '@/types/api'

export const settingsApi = {
  // 公開 API
  getSiteSettings: () =>
    apiClient.get<ApiResponse<SiteSettingDto[]>>('/site-settings'),

  getHomepage: () =>
    apiClient.get<ApiResponse<HomepageData>>('/homepage'),

  getTraffic: () =>
    apiClient.get<ApiResponse<TrafficInfoDto[]>>('/traffic'),

  // 管理 API
  adminUpdateSettings: (data: UpdateSiteSettingsRequest) =>
    apiClient.put<ApiResponse<void>>('/admin/site-settings', data),

  adminGetSlides: () =>
    apiClient.get<ApiResponse<HomepageSlideDto[]>>('/admin/homepage/slides'),

  adminUpdateSlides: (slides: HomepageSlideDto[]) =>
    apiClient.put<ApiResponse<void>>('/admin/homepage/slides', slides),

  adminGetTraffic: () =>
    apiClient.get<ApiResponse<TrafficInfoDto[]>>('/admin/traffic'),

  adminUpdateTraffic: (data: TrafficInfoDto[]) =>
    apiClient.put<ApiResponse<void>>('/admin/traffic', data),
}
