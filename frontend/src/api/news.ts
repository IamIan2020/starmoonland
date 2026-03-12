import apiClient from './client'
import type { ApiResponse, PagedResponse, NewsDto, NewsCategoryDto, CreateNewsRequest, UpdateNewsRequest } from '@/types/api'

export const newsApi = {
  // 公開 API
  getList: (params: { page?: number; pageSize?: number; category?: string }) =>
    apiClient.get<PagedResponse<NewsDto>>('/news', { params }),

  getDetail: (id: number) =>
    apiClient.get<ApiResponse<NewsDto>>(`/news/${id}`),

  // 管理 API
  adminGetCategories: () =>
    apiClient.get<ApiResponse<NewsCategoryDto[]>>('/admin/news-categories'),

  adminGetList: (params: { page?: number; pageSize?: number; categoryId?: number; keyword?: string }) =>
    apiClient.get<PagedResponse<NewsDto>>('/admin/news', { params }),

  adminCreate: (data: CreateNewsRequest) =>
    apiClient.post<ApiResponse<NewsDto>>('/admin/news', data),

  adminUpdate: (id: number, data: UpdateNewsRequest) =>
    apiClient.put<ApiResponse<NewsDto>>(`/admin/news/${id}`, data),

  adminDelete: (id: number) =>
    apiClient.delete<ApiResponse<void>>(`/admin/news/${id}`),
}
