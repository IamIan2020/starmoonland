import apiClient from './client'
import type { ApiResponse, PagedResponse, ContactSubmissionDto, ContactSubmitRequest, ReplyContactRequest } from '@/types/api'

export const contactsApi = {
  // 公開 API
  submit: (data: ContactSubmitRequest) =>
    apiClient.post<ApiResponse<void>>('/contact', data),

  // 管理 API
  adminGetList: (params?: { page?: number; pageSize?: number; isRead?: boolean }) =>
    apiClient.get<PagedResponse<ContactSubmissionDto>>('/admin/contacts', { params }),

  adminMarkRead: (id: number) =>
    apiClient.put<ApiResponse<void>>(`/admin/contacts/${id}/read`),

  adminReply: (id: number, data: ReplyContactRequest) =>
    apiClient.put<ApiResponse<void>>(`/admin/contacts/${id}/reply`, data),
}
