## Requirements

### Requirement: 新聞分類

系統 SHALL 支援新聞分類管理。

#### Scenario: 預設分類

- **WHEN** 系統初始化
- **THEN** 建立以下分類：活動情報、媒體報導、其他公告

#### Scenario: 分類欄位

- **WHEN** 建立或編輯新聞分類
- **THEN** 包含：Name（名稱）、Slug（URL 友善名稱）、SortOrder（排序）

### Requirement: 新聞文章管理

系統 SHALL 提供新聞文章的 CRUD 功能。

#### Scenario: 新聞欄位

- **WHEN** 建立或編輯新聞
- **THEN** 包含以下欄位：CategoryId（分類）、Title（標題）、Summary（摘要）、Content（HTML 富文本）、CoverImage（封面圖片）、PublishDate（發佈日期）、IsPublished（是否發佈）、IsPinned（是否置頂）、ViewCount（瀏覽次數）

#### Scenario: 發佈控制

- **WHEN** 新聞的 `IsPublished` 為 false
- **THEN** 前台不顯示該新聞，後台仍可看到並編輯

#### Scenario: 置頂功能

- **WHEN** 新聞設為置頂（IsPinned = true）
- **THEN** 在列表中排在最前面，不受日期排序影響

### Requirement: 公開新聞 API

#### Scenario: 新聞列表

- **WHEN** 呼叫 `GET /api/news?page=1&pageSize=10&category=`
- **THEN** 回傳已發佈新聞的分頁列表，置頂新聞排最前，其餘按 PublishDate 降序排列
- **THEN** 回傳分頁資訊（totalCount、totalPages、currentPage）

#### Scenario: 分類篩選

- **WHEN** 呼叫 `GET /api/news?category=活動情報`
- **THEN** 僅回傳該分類的已發佈新聞

#### Scenario: 新聞詳情

- **WHEN** 呼叫 `GET /api/news/{id}`
- **THEN** 回傳新聞完整內容，並遞增 ViewCount

#### Scenario: 新聞不存在或未發佈

- **WHEN** 呼叫 `GET /api/news/{不存在或未發佈的id}`
- **THEN** 回傳 HTTP 404
