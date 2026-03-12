import apiClient from './client'
import type { ApiResponse, AlbumDto, AlbumCategoryDto, AlbumPhotoDto, CreateAlbumRequest, UpdateAlbumRequest } from '@/types/api'

export const albumsApi = {
  // 公開 API
  getList: (params?: { category?: string }) =>
    apiClient.get<ApiResponse<AlbumDto[]>>('/albums', { params }),

  getDetail: (id: number) =>
    apiClient.get<ApiResponse<AlbumDto>>(`/albums/${id}`),

  // 管理 API
  adminGetCategories: () =>
    apiClient.get<ApiResponse<AlbumCategoryDto[]>>('/admin/album-categories'),

  adminGetList: (params?: { categoryId?: number }) =>
    apiClient.get<ApiResponse<AlbumDto[]>>('/admin/albums', { params }),

  adminCreate: (data: CreateAlbumRequest) =>
    apiClient.post<ApiResponse<AlbumDto>>('/admin/albums', data),

  adminUpdate: (id: number, data: UpdateAlbumRequest) =>
    apiClient.put<ApiResponse<AlbumDto>>(`/admin/albums/${id}`, data),

  adminDelete: (id: number) =>
    apiClient.delete<ApiResponse<void>>(`/admin/albums/${id}`),

  adminUpdatePhotos: (albumId: number, photos: Omit<AlbumPhotoDto, 'id'>[]) =>
    apiClient.put<ApiResponse<AlbumPhotoDto[]>>(`/admin/albums/${albumId}/photos`, photos),
}
