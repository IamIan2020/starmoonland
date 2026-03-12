<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink } from 'vue-router'

const mobileMenuOpen = ref(false)

const menuItems = [
  {
    label: '星月故事', to: '/about',
    children: [
      { label: '星月緣起', to: '/about/origin' },
      { label: '星月願景', to: '/about/vision' },
      { label: '吉祥物介紹', to: '/about/mascot' },
    ],
  },
  { label: '訊息公告', to: '/news' },
  {
    label: '星月服務', to: '/service',
    children: [
      { label: '大地營區', to: '/service/camp' },
      { label: '大地烤肉', to: '/service/bbq' },
      { label: '大地風呂', to: '/service/hotspring' },
      { label: '星月文旅', to: '/service/stay' },
    ],
  },
  {
    label: '餐飲宴會', to: '/dining',
    children: [
      { label: '景觀宴會館', to: '/dining/banquet' },
      { label: '景觀咖啡廳', to: '/dining/cafe' },
    ],
  },
  {
    label: '幸福婚宴', to: '/wedding',
    children: [
      { label: '求婚策畫', to: '/wedding/proposal' },
      { label: '婚禮席宴', to: '/wedding/ceremony' },
    ],
  },
  { label: '大地景觀', to: '/album' },
  { label: '交通資訊', to: '/traffic' },
  { label: '聯絡我們', to: '/contact' },
]
</script>

<template>
  <header class="bg-dark text-white sticky top-0 z-50">
    <div class="max-w-[1400px] mx-auto flex items-center justify-between px-4 py-3">
      <!-- Logo -->
      <RouterLink to="/" class="text-gold font-serif text-2xl font-bold tracking-wider">
        星月大地
      </RouterLink>

      <!-- Desktop Nav -->
      <nav class="hidden lg:flex items-center gap-1">
        <div v-for="item in menuItems" :key="item.label" class="relative group">
          <RouterLink
            :to="item.to"
            class="px-4 py-2 text-sm tracking-wide hover:text-gold transition-colors"
          >
            {{ item.label }}
          </RouterLink>
          <!-- Dropdown -->
          <div
            v-if="item.children"
            class="absolute top-full left-0 bg-dark/95 min-w-[160px] py-2 opacity-0 invisible group-hover:opacity-100 group-hover:visible transition-all"
          >
            <RouterLink
              v-for="child in item.children"
              :key="child.label"
              :to="child.to"
              class="block px-4 py-2 text-sm hover:text-gold hover:bg-white/5 transition-colors"
            >
              {{ child.label }}
            </RouterLink>
          </div>
        </div>
      </nav>

      <!-- Mobile Menu Button -->
      <button class="lg:hidden text-gold text-2xl" @click="mobileMenuOpen = !mobileMenuOpen">
        <i :class="mobileMenuOpen ? 'pi pi-times' : 'pi pi-bars'" />
      </button>
    </div>

    <!-- Mobile Menu -->
    <nav v-if="mobileMenuOpen" class="lg:hidden bg-dark border-t border-gold/20 px-4 pb-4">
      <template v-for="item in menuItems" :key="item.label">
        <RouterLink
          :to="item.to"
          class="block py-2 text-sm hover:text-gold"
          @click="mobileMenuOpen = false"
        >
          {{ item.label }}
        </RouterLink>
        <template v-if="item.children">
          <RouterLink
            v-for="child in item.children"
            :key="child.label"
            :to="child.to"
            class="block py-1.5 pl-4 text-xs text-gray-text hover:text-gold"
            @click="mobileMenuOpen = false"
          >
            {{ child.label }}
          </RouterLink>
        </template>
      </template>
    </nav>
  </header>
</template>
