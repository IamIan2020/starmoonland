## Why

星月大地目前使用純 PHP（無框架）+ jQuery + Bootstrap 3 建置，所有內容硬編碼在 HTML/PHP 中：
1. **無後台管理**：每次內容變更都需要工程師直接修改 PHP 檔案
2. **無資料庫**：所有資料（文字、圖片路徑）直接寫在 HTML 中，無法動態管理
3. **無 API**：前後端完全耦合，無法提供結構化資料介面
4. **技術老舊**：Bootstrap 3、jQuery、Slick Carousel 等套件已停止維護
5. **無響應式優化**：現有響應式設計不完善，手機體驗差
6. **未來需延伸購物功能**：現有架構無法支撐電商需求

需要將整個網站遷移至現代化架構，建立完整的 CMS 後台管理系統，使非技術人員也能自行管理網站內容。

## What Changes

- 建立 .NET 10 後端 API 伺服器（三層架構：Api / Core / Infrastructure）
- 建立 PostgreSQL 資料庫，包含所有內容管理所需的資料表
- 建立 Vue 3 前端應用（前台公開網站 + 後台管理面板）
- 實作 JWT 認證系統（管理員登入）
- 實作完整的內容管理 API（頁面、新聞、相簿、聯絡表單、媒體庫等）
- 從現有 PHP 頁面提取內容，遷移至資料庫
- 前台使用 Tailwind CSS 重現品牌形象設計
- 後台使用 PrimeVue 4 建立管理介面
- 實作聯絡表單 Email 通知功能（MailKit）

## Capabilities

### New Capabilities

- `project-foundation`：專案基礎架構（.NET + Vue 3 + PostgreSQL）
- `user-auth`：管理員認證系統（JWT Access + Refresh Token）
- `page-management`：頁面內容管理（分類、子頁面、輪播、Tab）
- `news-management`：新聞公告管理（分類、發佈控制、置頂）
- `album-management`：相簿管理（分類、照片、拖拽排序）
- `contact-management`：聯絡表單管理（已讀/回覆 + Email 通知）
- `site-settings`：網站設定管理（電話、地址、社群連結等）
- `media-library`：媒體庫（檔案上傳、縮圖產生、瀏覽刪除）
- `public-frontend`：前台公開網站（Tailwind CSS 品牌設計）
- `admin-backend`：後台管理面板（PrimeVue 4）

### Modified Capabilities

（無，這是全新建置）

## Impact

- **後端**：全新建立 .NET 10 三層架構 API 伺服器
- **前端**：全新建立 Vue 3 + TypeScript 應用（前台 + 後台）
- **資料庫**：建立 PostgreSQL 資料庫，包含 17+ 資料表
- **API**：建立 10+ 公開端點、20+ 管理端點
- **部署**：參考宮廟系統的 IIS + PowerShell 腳本模式
- **舊網站**：PHP 原始碼保留在 git 歷史中，遷移完成後可移除
