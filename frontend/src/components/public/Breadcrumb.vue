<script setup lang="ts">
defineProps<{
  items: { label: string; to?: string }[]
}>()
</script>

<template>
  <nav class="bg-gray-50 py-3">
    <div class="max-w-[1200px] mx-auto px-4">
      <ol class="flex items-center gap-2 text-sm text-gray-text">
        <li>
          <RouterLink to="/" class="hover:text-gold transition">HOME</RouterLink>
        </li>
        <li v-for="(item, idx) in items" :key="idx" class="flex items-center gap-2">
          <span class="text-gray-300">/</span>
          <RouterLink v-if="item.to" :to="item.to" class="hover:text-gold transition uppercase">
            {{ item.label }}
          </RouterLink>
          <span v-else class="text-dark-light uppercase">{{ item.label }}</span>
        </li>
      </ol>
    </div>
  </nav>
</template>
