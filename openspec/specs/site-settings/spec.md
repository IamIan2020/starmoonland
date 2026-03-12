## Requirements

### Requirement: 網站設定管理

系統 SHALL 提供 Key-Value 格式的網站設定管理。

#### Scenario: 預設設定項目

- **WHEN** 系統初始化
- **THEN** 建立以下設定項目：
  | Key | 描述 | 預設值 |
  |-----|------|--------|
  | site_name | 網站名稱 | 星月大地景觀休閒園區 |
  | phone | 聯絡電話 | 04-25831839 |
  | address | 地址 | 台中市后里區月眉北路482號 |
  | email | 聯絡信箱 | — |
  | facebook_url | Facebook 連結 | — |
  | line_url | LINE 連結 | — |
  | instagram_url | Instagram 連結 | — |
  | google_maps_embed | Google Maps 嵌入 URL | — |
  | admin_notification_email | 管理者通知信箱 | — |
  | footer_text | 頁腳文字 | — |

#### Scenario: 設定更新

- **WHEN** 呼叫 `PUT /api/admin/site-settings`，包含多組 Key-Value
- **THEN** 批次更新所有設定值

### Requirement: 公開設定 API

#### Scenario: 取得公開設定

- **WHEN** 呼叫 `GET /api/site-settings`
- **THEN** 回傳所有公開設定（排除敏感設定如 admin_notification_email）

### Requirement: 首頁輪播管理

系統 SHALL 提供首頁輪播圖片（HomepageSlide）的管理功能。

#### Scenario: 輪播圖欄位

- **WHEN** 建立或編輯首頁輪播
- **THEN** 包含：ImageUrl（圖片路徑）、AltText（替代文字）、LinkUrl（點擊連結，選填）、SortOrder（排序）、IsActive（是否啟用）

#### Scenario: 首頁 API

- **WHEN** 呼叫 `GET /api/homepage`
- **THEN** 回傳啟用中的輪播圖片（按 SortOrder 排序）、精選新聞（最新 3 則）、精選相簿

### Requirement: 交通資訊管理

系統 SHALL 提供交通資訊的管理功能。

#### Scenario: 交通資訊欄位

- **WHEN** 建立或編輯交通資訊
- **THEN** 包含：TabName（頁籤名稱，如「市區公車」「自行開車」）、Content（HTML 富文本）、SortOrder（排序）

#### Scenario: 公開 API

- **WHEN** 呼叫 `GET /api/traffic`
- **THEN** 回傳所有交通資訊（按 SortOrder 排序）

#### Scenario: 前台顯示

- **WHEN** 前台瀏覽交通資訊頁面
- **THEN** 以 Tab 頁籤形式呈現不同交通方式 + Google Maps 嵌入地圖
