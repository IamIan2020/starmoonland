import apiClient from './client'
import type { ApiResponse, LoginRequest, LoginResponse, RefreshTokenRequest } from '@/types/api'

export const authApi = {
  login: (data: LoginRequest) =>
    apiClient.post<ApiResponse<LoginResponse>>('/admin/auth/login', data),

  refreshToken: (data: RefreshTokenRequest) =>
    apiClient.post<ApiResponse<LoginResponse>>('/admin/auth/refresh-token', data),
}
