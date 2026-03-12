import apiClient from './client'
import type { ApiResponse, PageCategoryDto, PageDto, CreatePageCategoryRequest, CreatePageRequest, UpdatePageRequest, PageSlideDto, PageTabDto } from '@/types/api'

export const pagesApi = {
  // 公開 API
  getCategory: (categorySlug: string) =>
    apiClient.get<ApiResponse<PageCategoryDto>>(`/pages/${categorySlug}`),

  getPage: (categorySlug: string, pageSlug: string) =>
    apiClient.get<ApiResponse<PageDto>>(`/pages/${categorySlug}/${pageSlug}`),

  // 管理 API
  adminGetCategories: () =>
    apiClient.get<ApiResponse<PageCategoryDto[]>>('/admin/page-categories'),

  adminCreateCategory: (data: CreatePageCategoryRequest) =>
    apiClient.post<ApiResponse<PageCategoryDto>>('/admin/page-categories', data),

  adminUpdateCategory: (id: number, data: CreatePageCategoryRequest) =>
    apiClient.put<ApiResponse<PageCategoryDto>>(`/admin/page-categories/${id}`, data),

  adminDeleteCategory: (id: number) =>
    apiClient.delete<ApiResponse<void>>(`/admin/page-categories/${id}`),

  adminGetPages: (categoryId?: number) =>
    apiClient.get<ApiResponse<PageDto[]>>('/admin/pages', { params: { categoryId } }),

  adminCreatePage: (data: CreatePageRequest) =>
    apiClient.post<ApiResponse<PageDto>>('/admin/pages', data),

  adminUpdatePage: (id: number, data: UpdatePageRequest) =>
    apiClient.put<ApiResponse<PageDto>>(`/admin/pages/${id}`, data),

  adminDeletePage: (id: number) =>
    apiClient.delete<ApiResponse<void>>(`/admin/pages/${id}`),

  adminUpdateSlides: (pageId: number, slides: Omit<PageSlideDto, 'id'>[]) =>
    apiClient.put<ApiResponse<PageSlideDto[]>>(`/admin/pages/${pageId}/slides`, slides),

  adminUpdateTabs: (pageId: number, tabs: Omit<PageTabDto, 'id'>[]) =>
    apiClient.put<ApiResponse<PageTabDto[]>>(`/admin/pages/${pageId}/tabs`, tabs),
}
