import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const publicRoutes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('@/components/layout/PublicLayout.vue'),
    children: [
      { path: '', name: 'home', component: () => import('@/views/public/HomeView.vue') },
      { path: 'about/:slug?', name: 'about', component: () => import('@/views/public/AboutView.vue') },
      { path: 'service/:slug?', name: 'service', component: () => import('@/views/public/ServiceView.vue') },
      { path: 'dining/:slug?', name: 'dining', component: () => import('@/views/public/DiningView.vue') },
      { path: 'wedding/:slug?', name: 'wedding', component: () => import('@/views/public/WeddingView.vue') },
      { path: 'news', name: 'newsList', component: () => import('@/views/public/NewsListView.vue') },
      { path: 'news/:id', name: 'newsDetail', component: () => import('@/views/public/NewsDetailView.vue') },
      { path: 'album', name: 'albumList', component: () => import('@/views/public/AlbumListView.vue') },
      { path: 'album/:id', name: 'albumDetail', component: () => import('@/views/public/AlbumDetailView.vue') },
      { path: 'traffic', name: 'traffic', component: () => import('@/views/public/TrafficView.vue') },
      { path: 'contact', name: 'contact', component: () => import('@/views/public/ContactView.vue') },
    ],
  },
]

const backstageRoutes: RouteRecordRaw[] = [
  {
    path: '/backstage/login',
    name: 'backstageLogin',
    component: () => import('@/views/backstage/LoginView.vue'),
  },
  {
    path: '/backstage',
    component: () => import('@/components/layout/AdminLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      { path: '', name: 'dashboard', component: () => import('@/views/backstage/DashboardView.vue') },
      { path: 'pages', name: 'pageManage', component: () => import('@/views/backstage/PageManageView.vue') },
      { path: 'news', name: 'newsManage', component: () => import('@/views/backstage/NewsManageView.vue') },
      { path: 'albums', name: 'albumManage', component: () => import('@/views/backstage/AlbumManageView.vue') },
      { path: 'contacts', name: 'contactList', component: () => import('@/views/backstage/ContactListView.vue') },
      { path: 'homepage', name: 'homepageSlide', component: () => import('@/views/backstage/HomepageSlideView.vue') },
      { path: 'traffic', name: 'trafficManage', component: () => import('@/views/backstage/TrafficManageView.vue') },
      { path: 'settings', name: 'settings', component: () => import('@/views/backstage/SettingsView.vue') },
      { path: 'media', name: 'mediaLibrary', component: () => import('@/views/backstage/MediaLibraryView.vue') },
    ],
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes: [...publicRoutes, ...backstageRoutes],
  scrollBehavior(_to, _from, savedPosition) {
    return savedPosition || { top: 0 }
  },
})

// 導航守衛
router.beforeEach((to, _from, next) => {
  if (to.matched.some((r) => r.meta.requiresAuth)) {
    const authStore = useAuthStore()
    if (!authStore.isAuthenticated) {
      next({ name: 'backstageLogin', query: { redirect: to.fullPath } })
      return
    }
  }
  next()
})

export default router
