## Requirements

### Requirement: 聯絡表單送出

系統 SHALL 提供 `POST /api/contact` 端點，讓訪客送出聯絡表單。

#### Scenario: 送出成功

- **WHEN** 訪客提供姓名、電話、Email、詢問事項
- **THEN** 系統儲存表單資料，並發送 Email 通知管理者有新留言

#### Scenario: 驗證失敗

- **WHEN** 缺少必填欄位或 Email 格式不正確
- **THEN** 回傳 HTTP 400 + FluentValidation 錯誤訊息

#### Scenario: Email 通知管理者

- **WHEN** 表單儲存成功
- **THEN** 透過 MailKit 發送 Email 至管理者信箱，內容包含訪客姓名、聯絡方式、詢問內容

### Requirement: 聯絡表單欄位

#### Scenario: 表單欄位定義

- **WHEN** 建立 ContactSubmission Entity
- **THEN** 包含以下欄位：Name（姓名）、Phone（電話）、Email（電子信箱）、Message（詢問事項）、IsRead（是否已讀）、CreatedAt（建立時間）、RepliedAt（回覆時間）、ReplyNote（內部備註）、ReplyContent（回覆內容）

### Requirement: 後台聯絡表單管理

#### Scenario: 表單列表

- **WHEN** 管理員查看聯絡表單列表
- **THEN** 顯示所有表單，未讀排在最前，可看到已讀/未讀狀態

#### Scenario: 標記已讀

- **WHEN** 呼叫 `PUT /api/admin/contacts/{id}/read`
- **THEN** 將該表單標記為已讀

#### Scenario: 回覆訪客

- **WHEN** 呼叫 `PUT /api/admin/contacts/{id}/reply`，包含 ReplyContent
- **THEN** 儲存回覆內容和時間，並發送 Email 通知訪客

#### Scenario: 回覆 Email 通知

- **WHEN** 回覆儲存成功
- **THEN** 透過 MailKit 發送 Email 至訪客信箱，內容包含原始詢問和管理者的回覆

### Requirement: 前台聯絡表單

#### Scenario: 表單 UI

- **WHEN** 前台瀏覽聯絡我們頁面
- **THEN** 顯示表單（姓名、電話、Email、詢問事項）+ 前端即時驗證

#### Scenario: 送出回饋

- **WHEN** 表單送出成功
- **THEN** 顯示「感謝您的留言，我們會盡快回覆」成功訊息
