import apiClient from './client'
import type { ApiResponse, MediaFileDto } from '@/types/api'

export const uploadApi = {
  upload: (file: File) => {
    const formData = new FormData()
    formData.append('file', file)
    return apiClient.post<ApiResponse<MediaFileDto>>('/admin/media/upload', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    })
  },

  getList: () =>
    apiClient.get<ApiResponse<MediaFileDto[]>>('/admin/media'),

  delete: (id: number) =>
    apiClient.delete<ApiResponse<void>>(`/admin/media/${id}`),
}
