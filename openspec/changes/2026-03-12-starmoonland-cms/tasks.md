# 星月大地 CMS 開發任務清單

## 1. 專案建立與基礎架構

- [x] 1.1 建立 `server/StarMoonLand.sln` 與三個後端專案（Api / Core / Infrastructure）
- [x] 1.2 安裝後端 NuGet 套件（Npgsql、Identity、JWT、FluentValidation、Swagger、MailKit、ImageSharp）
- [x] 1.3 建立 `appsettings.json` 設定檔（PostgreSQL 連線、JWT、CORS、SMTP）
- [x] 1.4 建立統一回應格式 `ApiResponse<T>` 與 `ExceptionHandlingMiddleware`
- [x] 1.5 設定 `Program.cs`（Identity、JWT、CORS、Swagger、DI 註冊、自動 Migration）
- [x] 1.6 建立前端專案 `frontend/`（Vite + Vue 3 + TypeScript）
- [x] 1.7 安裝前端 npm 套件（Tailwind、PrimeVue、WangEditor、Swiper、Fancybox 等）
- [x] 1.8 設定 Tailwind CSS（從原站 style.css/template.css 提取設計 token 為主題配置）
- [x] 1.9 設定 PrimeVue 4 + Vite proxy 指向後端 API
- [x] 1.10 建立 Axios client + JWT interceptors（參考宮廟系統 client.ts）
- [x] 1.11 建立前端基本 Layout 元件（PublicLayout、AdminLayout）
- [x] 1.12 設定 Vue Router 基本路由結構（前台 + 後台）

## 2. 資料庫與 Entity 模型

- [x] 2.1 建立 `ApplicationUser` 實體（繼承 IdentityUser，擴充 DisplayName、IsActive、LastLoginAt）
- [x] 2.2 建立頁面相關 Entity（PageCategory、Page、PageSlide、PageTab）
- [x] 2.3 建立新聞相關 Entity（NewsCategory、News）
- [x] 2.4 建立相簿相關 Entity（AlbumCategory、Album、AlbumPhoto）
- [x] 2.5 建立 ContactSubmission Entity
- [x] 2.6 建立 TrafficInfo、HomepageSlide、SiteSetting Entity
- [x] 2.7 建立 MediaFile Entity
- [x] 2.8 建立 `AppDbContext`（繼承 IdentityDbContext）+ Fluent API 設定
- [x] 2.9 建立初始 Migration + 執行（驗證資料庫建立成功）
- [x] 2.10 撰寫 Seed Data（預設角色、管理員帳號、從 PHP 頁面提取的初始內容）

## 3. 認證系統

- [x] 3.1 建立 Auth DTOs（LoginRequest、LoginResponse、RefreshTokenRequest、TokenResponse）
- [ ] 3.2 建立 Auth FluentValidation Validators
- [x] 3.3 實作 IAuthService + AuthService（登入、JWT 產生/驗證、Refresh Token）
- [x] 3.4 實作 AuthController（POST /api/admin/auth/login、POST /api/admin/auth/refresh-token）
- [x] 3.5 註冊認證相關服務至 DI Container + 驗證編譯成功

## 4. 公開 API

- [x] 4.1 建立公開 API DTOs（PageCategoryDto、PageDto、NewsDto、AlbumDto 等）
- [x] 4.2 實作頁面 Service + PagesController（GET /api/pages/{categorySlug}、GET /api/pages/{categorySlug}/{pageSlug}）
- [x] 4.3 實作新聞 Service + NewsController（GET /api/news、GET /api/news/{id}）
- [x] 4.4 實作相簿 Service + AlbumsController（GET /api/albums、GET /api/albums/{id}）
- [x] 4.5 實作首頁 Service + HomepageController（GET /api/homepage）
- [x] 4.6 實作交通資訊 Service + TrafficController（GET /api/traffic）
- [x] 4.7 實作網站設定 Service + SiteSettingsController（GET /api/site-settings）
- [x] 4.8 建立 IEmailService + EmailService（MailKit SMTP）
- [x] 4.9 實作聯絡表單 Service + ContactController（POST /api/contact + Email 通知）
- [x] 4.10 驗證所有公開 API 編譯成功 + Swagger 測試

## 5. 管理 API

- [x] 5.1 建立管理 API DTOs（CreatePageRequest、UpdateNewsRequest 等所有 CRUD DTOs）
- [ ] 5.2 建立管理 API FluentValidation Validators
- [x] 5.3 實作管理頁面 Service + AdminPagesController（CRUD /api/admin/page-categories、/api/admin/pages）
- [x] 5.4 實作 AdminPagesController 輪播/Tab 批次更新（PUT /api/admin/pages/{id}/slides、PUT /api/admin/pages/{id}/tabs）
- [x] 5.5 實作管理新聞 Service + AdminNewsController（CRUD /api/admin/news）
- [x] 5.6 實作管理相簿 Service + AdminAlbumsController（CRUD /api/admin/albums + PUT /api/admin/albums/{id}/photos）
- [x] 5.7 實作管理聯絡表單 AdminContactsController（GET 列表、PUT 標記已讀、PUT 回覆 + Email 通知訪客）
- [x] 5.8 實作管理首頁輪播 + 交通資訊 + 網站設定 Controllers
- [x] 5.9 實作 FileUploadService（上傳 + ImageSharp 自動縮圖 + GUID 重命名）
- [x] 5.10 實作 AdminMediaController（POST 上傳、GET 列表、DELETE 刪除）
- [x] 5.11 驗證所有管理 API 編譯成功 + Swagger 測試

## 6. 前端類型定義與 API 層

- [x] 6.1 建立 TypeScript 類型定義 `types/api.ts`（對應所有後端 DTO）
- [x] 6.2 建立 API 模組（auth.ts、pages.ts、news.ts、albums.ts、contacts.ts、settings.ts、upload.ts）
- [x] 6.3 建立 Pinia stores（auth.ts、settings.ts）
- [x] 6.4 設定 Vue Router 完整路由 + 導航守衛（認證檢查）
- [x] 6.5 建立 useIdleTimeout composable
- [x] 6.6 驗證前端編譯成功（npm run build）

## 7. 前台公開網站（Tailwind CSS）

- [x] 7.1 建立 PublicHeader（響應式導覽列：8 主選單 + 下拉子選單 + 手機漢堡選單）
- [x] 7.2 建立 PublicFooter（聯絡資訊 + 社群連結）
- [x] 7.3 建立 HomeView（HeroSlider + 新聞預覽 + 服務四宮格 + 餐飲 + 婚宴 + 相簿預覽）
- [x] 7.4 建立 HeroSlider 元件（Swiper 取代 Owl Carousel）
- [x] 7.5 建立 PageCategory 共用元件（About/Service/Dining/Wedding 使用 — 分類頁籤 + Swiper thumbs 輪播 + 內容 Tab）
- [x] 7.6 建立 AboutView、ServiceView、DiningView、WeddingView（使用 PageCategory 元件）
- [x] 7.7 建立 NewsListView + NewsDetailView（分頁、分類篩選）
- [x] 7.8 建立 AlbumListView（無限捲動）+ AlbumDetailView（Fancybox 燈箱）
- [x] 7.9 建立 TrafficView（Tab 切換 + Google Maps embed）
- [x] 7.10 建立 ContactView（聯絡表單 + 前端驗證）
- [x] 7.11 建立 Breadcrumb 元件
- [x] 7.12 驗證所有前台頁面顯示正確 + 響應式測試（npm run build 成功）

## 8. 後台管理面板（PrimeVue 4）

- [x] 8.1 建立 AdminLayout（側邊欄導航 + 頂部列 + 使用者資訊）
- [x] 8.2 建立 LoginView（管理員登入頁面）
- [ ] 8.3 建立 DashboardView（未讀表單數、最近更新統計）
- [ ] 8.4 建立 PageManageView（頁面分類 + 子頁面 CRUD + WangEditor + 輪播/Tab 編輯）
- [ ] 8.5 建立 NewsManageView（新聞 CRUD + 發佈控制 + 置頂 + WangEditor）
- [ ] 8.6 建立 AlbumManageView（相簿 CRUD + 照片上傳 + SortableJS 拖拽排序）
- [ ] 8.7 建立 ContactListView（已讀/未讀標記 + 回覆功能）
- [ ] 8.8 建立 HomepageSlideView（首頁輪播管理 + 排序）
- [ ] 8.9 建立 TrafficManageView（交通資訊 Tab 編輯 + WangEditor）
- [ ] 8.10 建立 SettingsView（網站設定 key-value 編輯）
- [ ] 8.11 建立 MediaLibraryView（瀏覽、上傳、刪除 + 圖片預覽）
- [ ] 8.12 建立 WangEditor 共用元件
- [ ] 8.13 驗證所有後台頁面功能正確（npm run build 成功）

## 9. 整合測試與內容遷移

- [ ] 9.1 前後端聯調測試（登入 → 後台 CRUD → 前台顯示）
- [ ] 9.2 從 PHP 頁面提取文字內容 → 建立 Seed Data
- [ ] 9.3 複製 `_images/` 圖片至上傳目錄
- [ ] 9.4 聯絡流程測試（前台送出 → Email 通知 → 後台查看 → 回覆 → Email 通知訪客）
- [ ] 9.5 響應式測試（手機/平板/桌面）
- [ ] 9.6 最終編譯驗證（dotnet build + npm run build 無錯誤）

---

## 進度追蹤

| 分項 | 完成度 | 狀態 |
|------|--------|------|
| 1. 專案建立與基礎架構 | 12/12 | ✅ 完成 |
| 2. 資料庫與 Entity 模型 | 10/10 | ✅ 完成 |
| 3. 認證系統 | 4/5 | 進行中 |
| 4. 公開 API | 10/10 | ✅ 完成 |
| 5. 管理 API | 10/11 | 進行中 |
| 6. 前端類型定義與 API 層 | 6/6 | ✅ 完成 |
| 7. 前台公開網站 | 12/12 | ✅ 完成 |
| 8. 後台管理面板 | 2/13 | 進行中 |
| 9. 整合測試與內容遷移 | 0/6 | 未開始 |
| **合計** | **66/85** | **進行中** |
