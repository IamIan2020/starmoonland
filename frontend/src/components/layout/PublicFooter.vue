<script setup lang="ts">
import { useSettingsStore } from '@/stores/settings'

const settings = useSettingsStore()
</script>

<template>
  <footer class="bg-dark text-white py-10">
    <div class="max-w-[1200px] mx-auto px-4 text-center">
      <h3 class="font-serif text-gold text-xl mb-4">星月大地</h3>
      <div class="flex flex-col sm:flex-row items-center justify-center gap-4 text-sm text-gray-text mb-4">
        <span v-if="settings.get('phone')">
          <i class="pi pi-phone mr-1" /> {{ settings.get('phone') }}
        </span>
        <span v-if="settings.get('address')">
          <i class="pi pi-map-marker mr-1" /> {{ settings.get('address') }}
        </span>
      </div>
      <div class="flex items-center justify-center gap-4 mb-6">
        <a v-if="settings.get('facebook_url')" :href="settings.get('facebook_url')" target="_blank" class="text-gold hover:text-gold-light transition">
          <i class="pi pi-facebook text-xl" />
        </a>
        <a v-if="settings.get('instagram_url')" :href="settings.get('instagram_url')" target="_blank" class="text-gold hover:text-gold-light transition">
          <i class="pi pi-instagram text-xl" />
        </a>
      </div>
      <p class="text-xs text-gray-text">{{ settings.get('footer_text', '© 星月大地景觀休閒園區 All Rights Reserved.') }}</p>
    </div>
  </footer>
</template>
