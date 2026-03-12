/**
 * 解析圖片 URL
 * - 以 / 或 http 開頭 → 直接使用
 * - 其他 → 加上 /uploads/ 前綴（後端上傳的檔案）
 */
export function resolveImageUrl(url: string): string {
  if (!url) return ''
  if (url.startsWith('http://') || url.startsWith('https://') || url.startsWith('/')) return url
  return `/uploads/${url}`
}
