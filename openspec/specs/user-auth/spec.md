## Requirements

### Requirement: 管理員登入

系統 SHALL 提供 `POST /api/admin/auth/login` 端點，允許管理員以 Email + 密碼登入。

#### Scenario: 登入成功

- **WHEN** 管理員提供正確的 Email 和密碼
- **THEN** 系統回傳 Access Token（15 分鐘效期）和 Refresh Token（7 天效期），
  並更新使用者的 `LastLoginAt` 欄位

#### Scenario: 帳號不存在

- **WHEN** 提供未註冊的 Email
- **THEN** 系統回傳 HTTP 401 + 錯誤訊息「帳號或密碼錯誤」

#### Scenario: 密碼錯誤

- **WHEN** 提供已註冊的 Email 但密碼錯誤
- **THEN** 系統回傳 HTTP 401 + 錯誤訊息「帳號或密碼錯誤」（不透露帳號是否存在）

#### Scenario: 帳號已停用

- **WHEN** 管理員帳號的 `IsActive` 為 false
- **THEN** 系統回傳 HTTP 401 + 錯誤訊息「帳號已停用」

### Requirement: Token 重新整理

系統 SHALL 提供 `POST /api/admin/auth/refresh-token` 端點。

#### Scenario: 重新整理成功

- **WHEN** 提供有效且未過期的 Refresh Token
- **THEN** 系統回傳新的 Access Token 和 Refresh Token

#### Scenario: Refresh Token 無效或過期

- **WHEN** 提供無效或已過期的 Refresh Token
- **THEN** 系統回傳 HTTP 401，前端應導向登入頁面

### Requirement: JWT 認證機制

系統 SHALL 使用 JWT Bearer Token 驗證管理 API 的存取權限。

#### Scenario: 未帶 Token 存取管理 API

- **WHEN** 請求管理 API（/api/admin/*）但未帶 Authorization header
- **THEN** 系統回傳 HTTP 401 Unauthorized

#### Scenario: Token 過期

- **WHEN** Access Token 已過期
- **THEN** 系統回傳 HTTP 401，前端 interceptor 自動使用 Refresh Token 換發新 Token

### Requirement: 前端認證流程

前端 SHALL 管理 Token 的儲存和自動重新整理。

#### Scenario: 登入後 Token 儲存

- **WHEN** 登入成功
- **THEN** Access Token 和 Refresh Token 存入 localStorage，Pinia auth store 更新為已認證狀態

#### Scenario: 自動重新整理

- **WHEN** API 請求收到 HTTP 401
- **THEN** Axios interceptor 自動呼叫 refresh-token API，更新 Token 後重試原始請求

#### Scenario: 閒置登出

- **WHEN** 使用者閒置超過 30 分鐘（無滑鼠/鍵盤活動）
- **THEN** 自動清除 Token 並導向登入頁面

#### Scenario: 路由守衛

- **WHEN** 未認證使用者存取 /backstage/* 路由（除 /backstage/login）
- **THEN** 自動導向 /backstage/login
