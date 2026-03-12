## Requirements

### Requirement: 前台 Layout

前台 SHALL 使用統一的 PublicLayout，包含 Header 和 Footer。

#### Scenario: 響應式導覽列

- **WHEN** 桌面版瀏覽
- **THEN** 顯示水平導覽列，8 個主選單（星月故事、訊息公告、星月服務、餐飲宴會、幸福婚宴、大地景觀、交通資訊、聯絡我們），部分選單有下拉子選單

#### Scenario: 手機導覽列

- **WHEN** 螢幕寬度 < 992px
- **THEN** 顯示漢堡選單按鈕，點擊展開側邊抽屜式選單

#### Scenario: Footer

- **WHEN** 任何頁面底部
- **THEN** 顯示聯絡電話、地址、社群連結（Facebook、LINE、Instagram），資料從 SiteSetting API 取得

### Requirement: 首頁

#### Scenario: Hero 輪播

- **WHEN** 進入首頁
- **THEN** 頂部顯示全螢幕輪播圖片（Swiper），底部覆蓋顯示電話和地址

#### Scenario: 新聞預覽

- **WHEN** 進入首頁
- **THEN** 顯示最新 3 則新聞（標題、日期、摘要、封面圖片），帶「更多」連結

#### Scenario: 服務四宮格

- **WHEN** 進入首頁
- **THEN** 顯示四大服務（大地營區、大地烤肉、大地風呂、星月文旅），交替左右佈局（圖片 + 文字描述）

#### Scenario: 餐飲區塊

- **WHEN** 進入首頁
- **THEN** 顯示餐飲宴會預覽（景觀宴會館、景觀咖啡廳），帶頁籤切換 + 輪播

#### Scenario: 婚宴區塊

- **WHEN** 進入首頁
- **THEN** 顯示幸福婚宴預覽（求婚策畫、婚禮席宴），帶頁籤切換 + 輪播

#### Scenario: 相簿預覽

- **WHEN** 進入首頁
- **THEN** 顯示精選相簿圖片輪播

### Requirement: 內容頁面（PageCategory 元件）

About/Service/Dining/Wedding SHALL 共用 PageCategory 元件。

#### Scenario: 分類頁籤

- **WHEN** 進入 /about、/service、/dining 或 /wedding
- **THEN** 顯示該分類下所有子頁面的頁籤（預設選中第一個或 URL 指定的）

#### Scenario: 輪播展示

- **WHEN** 選中某個子頁面
- **THEN** 顯示該頁面的輪播圖片（Swiper thumbs 雙向同步輪播：左側文字導航 + 右側大圖）

#### Scenario: 內容 Tab

- **WHEN** 子頁面有多個 Tab
- **THEN** 以頁籤形式顯示不同內容區塊（如營區資訊、營地須知、營區價目）

### Requirement: 新聞頁面

#### Scenario: 新聞列表

- **WHEN** 進入 /news
- **THEN** 顯示新聞列表（封面圖片、標題、日期、摘要），支援分類篩選和分頁

#### Scenario: 新聞詳情

- **WHEN** 點擊新聞進入 /news/:id
- **THEN** 顯示新聞完整內容（HTML 富文本）

### Requirement: 相簿頁面

#### Scenario: 相簿列表

- **WHEN** 進入 /album
- **THEN** 以圖片牆形式顯示所有相簿，支援無限捲動

#### Scenario: 相簿詳情

- **WHEN** 點擊相簿進入 /album/:id
- **THEN** 顯示該相簿所有照片，點擊照片開啟 Fancybox 燈箱

### Requirement: 交通資訊頁面

#### Scenario: Tab 切換

- **WHEN** 進入 /traffic
- **THEN** 以 Tab 頁籤顯示不同交通方式（市區公車、自行開車），下方嵌入 Google Maps

### Requirement: 聯絡我們頁面

#### Scenario: 聯絡表單

- **WHEN** 進入 /contact
- **THEN** 顯示聯絡表單（姓名、電話、Email、詢問事項）+ 即時前端驗證

#### Scenario: 送出成功

- **WHEN** 表單驗證通過並送出
- **THEN** 顯示成功訊息，表單清空

### Requirement: 品牌視覺設計

前台 SHALL 使用 Tailwind CSS 重現星月大地品牌形象。

#### Scenario: 色彩主題

- **WHEN** 任何前台頁面
- **THEN** 使用品牌色彩：金色主調（#cda871）、深色背景（#1a1a1a）、紅色重點（#ac0000）

#### Scenario: 字體

- **WHEN** 任何前台頁面
- **THEN** 中文使用 Noto Sans TC，英文標題使用 EB Garamond（襯線字）

#### Scenario: 響應式設計

- **WHEN** 在不同裝置瀏覽
- **THEN** 頁面自適應桌面（1400px+）、平板（768-1200px）、手機（<768px）
