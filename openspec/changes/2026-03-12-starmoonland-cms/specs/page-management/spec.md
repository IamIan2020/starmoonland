## Requirements

### Requirement: 頁面分類管理

系統 SHALL 提供頁面分類（PageCategory）的 CRUD 功能，對應現有網站的主選單區塊。

#### Scenario: 分類結構

- **WHEN** 系統初始化
- **THEN** 預設建立以下分類：
  | Slug | 中文名稱 | 英文名稱 |
  |------|---------|---------|
  | about | 星月故事 | About |
  | service | 星月服務 | Service |
  | dining | 餐飲宴會 | Dining |
  | wedding | 幸福婚宴 | Wedding |

#### Scenario: 分類欄位

- **WHEN** 建立或編輯分類
- **THEN** 包含以下欄位：Slug（URL 友善名稱）、TitleZh（中文名稱）、TitleEn（英文名稱）、BannerImage（橫幅圖片）、SortOrder（排序）、IsVisible（是否顯示）

### Requirement: 子頁面管理

系統 SHALL 提供頁面（Page）的 CRUD 功能，每個頁面隸屬於一個分類。

#### Scenario: 子頁面結構

- **WHEN** 查看 「星月服務」分類
- **THEN** 顯示子頁面列表：大地營區、大地烤肉、大地風呂、星月文旅

#### Scenario: 子頁面欄位

- **WHEN** 建立或編輯子頁面
- **THEN** 包含以下欄位：CategoryId、Slug、Title（標題）、Subtitle（副標題）、Content（HTML 富文本）、SortOrder、MetaTitle、MetaDescription、IsVisible

### Requirement: 頁面輪播圖

每個頁面 SHALL 支援多張輪播圖片（PageSlide）。

#### Scenario: 輪播圖管理

- **WHEN** 編輯頁面的輪播圖
- **THEN** 可以新增、刪除、排序輪播圖片，每張圖片包含：ImageUrl、Title（選填）、Description（選填）、SortOrder

#### Scenario: 批次更新

- **WHEN** 呼叫 `PUT /api/admin/pages/{id}/slides`
- **THEN** 以批次方式更新該頁面的所有輪播圖（刪除舊的，建立新的）

### Requirement: 頁面 Tab 內容

每個頁面 SHALL 支援多個 Tab 頁籤內容（PageTab）。

#### Scenario: Tab 管理

- **WHEN** 編輯頁面的 Tab
- **THEN** 可以新增、刪除、排序 Tab，每個 Tab 包含：Title（頁籤名稱）、Content（HTML 富文本）、SortOrder

#### Scenario: 前台顯示

- **WHEN** 前台瀏覽子頁面
- **THEN** Tab 以頁籤形式呈現，點擊切換顯示不同內容

### Requirement: 公開頁面 API

#### Scenario: 取得分類頁面列表

- **WHEN** 呼叫 `GET /api/pages/{categorySlug}`
- **THEN** 回傳該分類下所有可見子頁面的基本資訊（不含 Content）

#### Scenario: 取得頁面詳情

- **WHEN** 呼叫 `GET /api/pages/{categorySlug}/{pageSlug}`
- **THEN** 回傳頁面完整資訊，包含 Content、Slides、Tabs

#### Scenario: 分類不存在

- **WHEN** 呼叫 `GET /api/pages/{不存在的slug}`
- **THEN** 回傳 HTTP 404
