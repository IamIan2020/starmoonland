## Requirements

### Requirement: 相簿分類

系統 SHALL 支援相簿分類管理。

#### Scenario: 分類欄位

- **WHEN** 建立或編輯相簿分類
- **THEN** 包含：Name（名稱）、Slug（URL 友善名稱）、SortOrder（排序）

### Requirement: 相簿管理

系統 SHALL 提供相簿的 CRUD 功能。

#### Scenario: 相簿欄位

- **WHEN** 建立或編輯相簿
- **THEN** 包含以下欄位：CategoryId（分類）、Title（標題）、Description（描述）、CoverImage（封面圖片）、IsPublished（是否發佈）、SortOrder（排序）

### Requirement: 相簿照片管理

每個相簿 SHALL 支援多張照片（AlbumPhoto）。

#### Scenario: 照片欄位

- **WHEN** 上傳照片至相簿
- **THEN** 每張照片包含：ImageUrl（原圖）、ThumbnailUrl（縮圖，自動產生）、Caption（說明文字，選填）、SortOrder（排序）

#### Scenario: 拖拽排序

- **WHEN** 在後台管理照片
- **THEN** 可使用拖拽方式重新排序照片，排序結果透過 API 批次儲存

#### Scenario: 批次更新

- **WHEN** 呼叫 `PUT /api/admin/albums/{id}/photos`
- **THEN** 以批次方式更新該相簿的所有照片（排序、說明文字）

### Requirement: 公開相簿 API

#### Scenario: 相簿列表

- **WHEN** 呼叫 `GET /api/albums?category=`
- **THEN** 回傳已發佈相簿列表（含封面圖片、標題、照片數量）

#### Scenario: 分類篩選

- **WHEN** 呼叫 `GET /api/albums?category=xxx`
- **THEN** 僅回傳該分類的已發佈相簿

#### Scenario: 相簿詳情

- **WHEN** 呼叫 `GET /api/albums/{id}`
- **THEN** 回傳相簿資訊及所有照片（按 SortOrder 排序）

### Requirement: 前台相簿展示

#### Scenario: 無限捲動

- **WHEN** 前台瀏覽相簿列表
- **THEN** 使用無限捲動載入更多相簿（取代傳統分頁）

#### Scenario: 燈箱效果

- **WHEN** 前台點擊相簿中的照片
- **THEN** 使用 Fancybox 開啟燈箱，可左右切換瀏覽所有照片
