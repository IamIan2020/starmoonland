## Context

星月大地是位於台中后里的休閒渡假村，現有網站使用純 PHP + jQuery + Bootstrap 3 建置。
所有內容硬編碼在 HTML/PHP 中，無資料庫、無後台、無 API。

現有網站結構：8 個主選單（星月故事、訊息公告、星月服務、餐飲宴會、幸福婚宴、大地景觀、交通資訊、聯絡我們），
其中 Service/Dining/Wedding 共用相似的頁面模式（分類頁籤 + 輪播 + 內容 Tab）。

設計 Token 來源：`_css/style.css` + `_css/template.css`
- 品牌主色：金色系 `#cda871` / `#c9a063` / `#edc282`
- 背景深色：`#1a1a1a`
- 字體：Noto Sans TC（中文）、EB Garamond（英文襯線）

架構參考：宮廟系統（E:\Web\temples），盡量統一架構模式。

## Goals / Non-Goals

**Goals:**
- 建立完整的 CMS 後台管理系統，讓非技術人員可自行管理所有網站內容
- 前台完整重現現有網站的視覺設計和互動效果
- 統一架構模式與宮廟系統一致，降低維護成本
- 預留未來延伸購物功能的空間（使用 PrimeVue 4 後台）
- 聯絡表單具備 Email 通知功能

**Non-Goals:**
- 不做會員系統（只有管理員帳號）
- 不做多語系（僅繁體中文，但保留英文標題欄位）
- 不做 SEO 優化的 SSR/SSG（第一版使用 SPA，未來可升級）
- 不做購物車/金流（未來擴充）
- 不做 OAuth 社群登入
- 不做 Docker 容器化
- 不做即時通知（WebSocket）

## Decisions

### 1. 三層架構 vs Clean Architecture

**選擇：三層架構（Api / Core / Infrastructure）**
- 替代方案：Clean Architecture（4-5 層）
- 理由：與宮廟系統一致，CMS 功能單純不需要額外抽象層。三層架構已足夠支撐所有 CRUD 操作。

### 2. 前台 Tailwind CSS vs PrimeVue

**選擇：前台 Tailwind CSS，後台 PrimeVue 4**
- 替代方案：全部用 PrimeVue 或全部用 Tailwind
- 理由：前台需要高度自訂的品牌形象設計（金色主題、特殊版面），PrimeVue 的預設樣式會限制設計自由度。後台則需要大量表格、表單、對話框等管理元件，PrimeVue 提供完整的元件庫。

### 3. 頁面結構：PageCategory + Page 模式

**選擇：通用的 PageCategory → Page → PageSlide/PageTab 三層結構**
- 替代方案：為每種頁面（About、Service、Dining、Wedding）建立獨立 Entity
- 理由：現有網站的 About/Service/Dining/Wedding 共用相同的頁面模式（分類頁籤 + 輪播 + 內容 Tab），使用通用結構可以用一套 CRUD API 管理所有這些頁面。

### 4. 前端單一專案 vs 多專案

**選擇：單一前端專案（views/public/ + views/backstage/）**
- 替代方案：前台和後台分為兩個獨立的 Vite 專案
- 理由：共用 API client、types、stores，減少重複程式碼。透過路由區分前台/後台，Tailwind 和 PrimeVue 可以在同一專案共存。

### 5. 輪播套件

**選擇：Swiper（取代 Slick + Owl Carousel）**
- 替代方案：繼續使用 Slick 的 Vue 封裝版
- 理由：Swiper 原生支援 Vue 3、有 thumbs 功能可重現雙向同步輪播、社群活躍度高。

### 6. 圖片燈箱

**選擇：@fancyapps/ui（Fancybox 5）**
- 替代方案：lightgallery、vue-easy-lightbox
- 理由：原站已使用 Fancybox，v5 支援 Vue 3，API 一致性高。

### 7. 認證模式

**選擇：JWT（Access Token 15min + Refresh Token 7days）**
- 替代方案：Cookie-based session
- 理由：與宮廟系統一致，前後端分離架構下 JWT 更適合。只有管理員需要登入，不需要複雜的 session 管理。

### 8. Email 發送

**選擇：MailKit（SMTP）**
- 替代方案：SendGrid、Mailgun 等第三方服務
- 理由：與宮廟系統一致，使用 Gmail SMTP 即可滿足 CMS 的通知需求（聯絡表單通知、回覆通知）。

## Risks / Trade-offs

- **[SPA 不利 SEO]** → 第一版先用 SPA，未來可用 `@unhead/vue` 管理 meta tag，或升級至 Nuxt SSR
- **[Tailwind + PrimeVue 共存可能樣式衝突]** → 使用 PrimeVue 的 unstyled mode + Tailwind 整合，或用 CSS scope 隔離
- **[JWT 存 localStorage 有 XSS 風險]** → 短效期 Access Token + Refresh Token 降低影響
- **[Gmail SMTP 每日 500 封上限]** → CMS 通知量很低，完全足夠
- **[圖片遷移需手動處理]** → 寫 Seed Data 腳本，將現有圖片路徑對應到新的媒體庫
