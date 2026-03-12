## Requirements

### Requirement: 後台 Layout

後台 SHALL 使用 AdminLayout，包含側邊欄和頂部列。

#### Scenario: 側邊欄導航

- **WHEN** 管理員登入後
- **THEN** 左側顯示導航選單：儀表板、頁面管理、新聞管理、相簿管理、聯絡表單、首頁輪播、交通資訊、網站設定、媒體庫

#### Scenario: 頂部列

- **WHEN** 任何後台頁面
- **THEN** 頂部顯示管理員名稱和登出按鈕

#### Scenario: 響應式

- **WHEN** 小螢幕瀏覽後台
- **THEN** 側邊欄可收合，以漢堡選單切換

### Requirement: 登入頁面

#### Scenario: 登入 UI

- **WHEN** 存取 /backstage/login
- **THEN** 顯示 Email + 密碼表單 + 登入按鈕

#### Scenario: 登入成功

- **WHEN** 輸入正確帳密並點擊登入
- **THEN** 導向 /backstage（儀表板）

#### Scenario: 登入失敗

- **WHEN** 帳密錯誤
- **THEN** 顯示錯誤訊息，不導向

### Requirement: 儀表板

#### Scenario: 統計資訊

- **WHEN** 進入儀表板
- **THEN** 顯示：未讀聯絡表單數量、最近更新的新聞、最近更新的相簿

### Requirement: 頁面管理

#### Scenario: 分類列表

- **WHEN** 進入頁面管理
- **THEN** 左側顯示分類列表（可新增、編輯、排序），右側顯示選中分類的子頁面

#### Scenario: 子頁面編輯

- **WHEN** 編輯子頁面
- **THEN** 使用 WangEditor 編輯 HTML 內容，可管理輪播圖和 Tab

### Requirement: 新聞管理

#### Scenario: 新聞列表

- **WHEN** 進入新聞管理
- **THEN** 以 PrimeVue DataTable 顯示新聞列表，支援搜尋、分類篩選、分頁

#### Scenario: 新聞編輯

- **WHEN** 新增或編輯新聞
- **THEN** 表單包含標題、分類、摘要、封面圖片上傳、WangEditor 內容編輯、發佈控制、置頂設定

### Requirement: 相簿管理

#### Scenario: 相簿列表

- **WHEN** 進入相簿管理
- **THEN** 顯示相簿列表，支援分類篩選

#### Scenario: 照片管理

- **WHEN** 編輯相簿
- **THEN** 可批次上傳照片、使用 SortableJS 拖拽排序、編輯說明文字、刪除照片

### Requirement: 聯絡表單管理

#### Scenario: 表單列表

- **WHEN** 進入聯絡表單列表
- **THEN** 顯示所有表單，未讀高亮顯示，可標記已讀

#### Scenario: 回覆功能

- **WHEN** 點擊回覆按鈕
- **THEN** 開啟回覆對話框，輸入回覆內容後送出，自動寄信通知訪客

### Requirement: 其他管理功能

#### Scenario: 首頁輪播管理

- **WHEN** 進入首頁輪播管理
- **THEN** 可新增、刪除、排序首頁輪播圖片

#### Scenario: 交通資訊管理

- **WHEN** 進入交通資訊管理
- **THEN** 可使用 WangEditor 編輯各交通方式的 Tab 內容

#### Scenario: 網站設定

- **WHEN** 進入網站設定
- **THEN** 以表單形式編輯所有 Key-Value 設定

#### Scenario: 媒體庫

- **WHEN** 進入媒體庫
- **THEN** 以網格方式瀏覽所有已上傳檔案，支援上傳和刪除
