## Requirements

### Requirement: 檔案上傳

系統 SHALL 提供 `POST /api/admin/media/upload` 端點，允許管理員上傳檔案。

#### Scenario: 圖片上傳

- **WHEN** 上傳圖片檔案（jpg、png、gif、webp）
- **THEN** 系統儲存原圖，並使用 ImageSharp 自動產生縮圖（最大寬度 300px）
- **THEN** 檔案以 GUID 重命名，避免檔名衝突
- **THEN** 建立 MediaFile 記錄

#### Scenario: 非圖片檔案上傳

- **WHEN** 上傳非圖片檔案（pdf、doc 等）
- **THEN** 系統儲存檔案，不產生縮圖

#### Scenario: 檔案大小限制

- **WHEN** 上傳檔案超過 10MB
- **THEN** 回傳 HTTP 400 + 錯誤訊息「檔案大小不得超過 10MB」

#### Scenario: 不支援的格式

- **WHEN** 上傳不支援的檔案格式
- **THEN** 回傳 HTTP 400 + 錯誤訊息「不支援的檔案格式」

### Requirement: MediaFile Entity

#### Scenario: Entity 欄位

- **WHEN** 建立 MediaFile
- **THEN** 包含以下欄位：Filename（GUID 檔名）、OriginalName（原始檔名）、ContentType（MIME 類型）、FileSize（檔案大小 bytes）、FilePath（儲存路徑）、ThumbnailPath（縮圖路徑，可為 null）、CreatedAt（上傳時間）

### Requirement: 媒體庫瀏覽

#### Scenario: 媒體列表

- **WHEN** 呼叫 `GET /api/admin/media`
- **THEN** 回傳所有媒體檔案列表，按上傳時間降序排列

#### Scenario: 刪除檔案

- **WHEN** 呼叫 `DELETE /api/admin/media/{id}`
- **THEN** 刪除資料庫記錄和實體檔案（原圖 + 縮圖）

### Requirement: 媒體庫 UI

#### Scenario: 後台媒體庫頁面

- **WHEN** 管理員進入媒體庫頁面
- **THEN** 以圖片網格方式顯示所有媒體，支援上傳新檔案和刪除

#### Scenario: 內容編輯器整合

- **WHEN** 在 WangEditor 中插入圖片
- **THEN** 可從媒體庫選擇已上傳的圖片，或直接上傳新圖片
