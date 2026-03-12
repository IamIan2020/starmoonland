// 通用 API 回應
export interface ApiResponse<T> {
  success: boolean
  data?: T
  message?: string
  errors: string[]
}

export interface PagedResponse<T> {
  success: boolean
  data: T[]
  totalCount: number
  totalPages: number
  currentPage: number
  pageSize: number
}

// 認證
export interface LoginRequest {
  email: string
  password: string
}

export interface LoginResponse {
  accessToken: string
  refreshToken: string
  user: UserInfo
}

export interface UserInfo {
  id: string
  email: string
  displayName: string
}

export interface RefreshTokenRequest {
  refreshToken: string
}

// 頁面分類
export interface PageCategoryDto {
  id: number
  slug: string
  titleZh: string
  titleEn: string
  bannerImage?: string
  sortOrder: number
  isVisible: boolean
  pages: PageSummaryDto[]
}

export interface PageSummaryDto {
  id: number
  slug: string
  title: string
  subtitle?: string
  sortOrder: number
}

// 頁面
export interface PageDto {
  id: number
  categoryId: number
  slug: string
  title: string
  subtitle?: string
  content?: string
  sortOrder: number
  isVisible: boolean
  metaTitle?: string
  metaDescription?: string
  slides: PageSlideDto[]
  tabs: PageTabDto[]
}

export interface PageSlideDto {
  id: number
  imageUrl: string
  title?: string
  description?: string
  sortOrder: number
}

export interface PageTabDto {
  id: number
  title: string
  content?: string
  sortOrder: number
}

// 新聞分類
export interface NewsCategoryDto {
  id: number
  name: string
  slug: string
  sortOrder: number
}

// 新聞
export interface NewsDto {
  id: number
  categoryId: number
  categoryName?: string
  title: string
  summary?: string
  content?: string
  coverImage?: string
  publishDate: string
  isPublished: boolean
  isPinned: boolean
  viewCount: number
  createdAt: string
}

// 相簿分類
export interface AlbumCategoryDto {
  id: number
  name: string
  slug: string
  sortOrder: number
}

// 相簿
export interface AlbumDto {
  id: number
  categoryId: number
  categoryName?: string
  title: string
  description?: string
  coverImage?: string
  isPublished: boolean
  sortOrder: number
  photoCount: number
  photos: AlbumPhotoDto[]
}

export interface AlbumPhotoDto {
  id: number
  imageUrl: string
  thumbnailUrl?: string
  caption?: string
  sortOrder: number
}

// 聯絡表單
export interface ContactSubmitRequest {
  name: string
  phone: string
  email: string
  message: string
}

export interface ContactSubmissionDto {
  id: number
  name: string
  phone: string
  email: string
  message: string
  isRead: boolean
  createdAt: string
  repliedAt?: string
  replyNote?: string
  replyContent?: string
}

// 交通資訊
export interface TrafficInfoDto {
  id: number
  tabName: string
  content?: string
  sortOrder: number
}

// 首頁輪播
export interface HomepageSlideDto {
  id: number
  imageUrl: string
  altText?: string
  linkUrl?: string
  sortOrder: number
  isActive: boolean
}

// 網站設定
export interface SiteSettingDto {
  id: number
  key: string
  value?: string
  description?: string
}

// 媒體檔案
export interface MediaFileDto {
  id: number
  filename: string
  originalName: string
  contentType: string
  fileSize: number
  filePath: string
  thumbnailPath?: string
  createdAt: string
}

// 首頁資料
export interface HomepageData {
  slides: HomepageSlideDto[]
  latestNews: NewsDto[]
  featuredAlbums: AlbumDto[]
}

// 管理 API DTOs
export interface CreatePageCategoryRequest {
  slug: string
  titleZh: string
  titleEn: string
  bannerImage?: string
  sortOrder: number
  isVisible: boolean
}

export interface CreatePageRequest {
  categoryId: number
  slug: string
  title: string
  subtitle?: string
  content?: string
  sortOrder: number
  isVisible: boolean
  metaTitle?: string
  metaDescription?: string
}

export interface UpdatePageRequest extends CreatePageRequest {
  id: number
}

export interface CreateNewsRequest {
  categoryId: number
  title: string
  summary?: string
  content?: string
  coverImage?: string
  publishDate: string
  isPublished: boolean
  isPinned: boolean
}

export interface UpdateNewsRequest extends CreateNewsRequest {
  id: number
}

export interface CreateAlbumRequest {
  categoryId: number
  title: string
  description?: string
  coverImage?: string
  isPublished: boolean
  sortOrder: number
}

export interface UpdateAlbumRequest extends CreateAlbumRequest {
  id: number
}

export interface ReplyContactRequest {
  replyContent: string
  replyNote?: string
}

export interface UpdateSiteSettingsRequest {
  settings: { key: string; value: string }[]
}
