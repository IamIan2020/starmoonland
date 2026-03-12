## Requirements

### Requirement: 後端專案結構

系統 SHALL 使用 .NET 10 三層架構建立後端專案。

#### Scenario: Solution 結構

- **WHEN** 專案初始化完成
- **THEN** 存在以下結構：
  - `server/StarMoonLand.sln`
  - `server/src/StarMoonLand.Api/` — Controllers, DTOs, Middleware, Program.cs
  - `server/src/StarMoonLand.Core/` — Entities, Interfaces, Services
  - `server/src/StarMoonLand.Infrastructure/` — EF Core DbContext, Migrations, Repositories

#### Scenario: NuGet 套件

- **WHEN** 後端專案建立完成
- **THEN** 已安裝以下套件：
  - Npgsql.EntityFrameworkCore.PostgreSQL
  - Microsoft.AspNetCore.Authentication.JwtBearer
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore
  - Swashbuckle.AspNetCore
  - FluentValidation.DependencyInjectionExtensions
  - SixLabors.ImageSharp
  - System.IdentityModel.Tokens.Jwt
  - MailKit

### Requirement: 統一 API 回應格式

所有 API 端點 SHALL 使用統一的回應格式。

#### Scenario: 成功回應

- **WHEN** API 操作成功
- **THEN** 回傳 `ApiResponse<T>` 格式：
  ```json
  { "success": true, "data": T, "message": null, "errors": [] }
  ```

#### Scenario: 失敗回應

- **WHEN** API 操作失敗（驗證錯誤、業務邏輯錯誤）
- **THEN** 回傳 `ApiResponse<T>` 格式：
  ```json
  { "success": false, "data": null, "message": "錯誤描述", "errors": ["細項錯誤"] }
  ```

#### Scenario: 未預期例外

- **WHEN** 發生未預期的例外
- **THEN** `ExceptionHandlingMiddleware` 捕獲例外，回傳 HTTP 500 + 統一格式錯誤訊息

### Requirement: 前端專案結構

系統 SHALL 使用 Vue 3 + TypeScript + Vite 建立前端專案。

#### Scenario: 前端初始化

- **WHEN** 前端專案初始化完成
- **THEN** 存在 `frontend/` 目錄，包含：
  - Vite + Vue 3 + TypeScript 配置
  - Tailwind CSS 配置（品牌色彩主題）
  - PrimeVue 4 配置
  - Axios client + JWT interceptors
  - Vue Router（前台 + 後台路由）

#### Scenario: Tailwind 設計 Token

- **WHEN** Tailwind 配置完成
- **THEN** 主題包含品牌色彩：
  - `gold`: `#cda871`（主色）
  - `gold-light`: `#edc282`
  - `gold-dark`: `#c9a063`
  - `dark`: `#1a1a1a`（深色背景）
  - 中文字體：Noto Sans TC
  - 英文字體：EB Garamond

### Requirement: 資料庫連線

系統 SHALL 連線至本機 PostgreSQL 資料庫。

#### Scenario: 資料庫建立

- **WHEN** 應用程式啟動
- **THEN** 自動執行 EF Core Migration，確保資料庫結構為最新

#### Scenario: 連線設定

- **WHEN** 開發環境
- **THEN** 使用 `appsettings.json` 中的 `ConnectionStrings:DefaultConnection` 連線至 `starmoonland_dev` 資料庫
