<script setup lang="ts">
import { ref, reactive } from 'vue'
import Breadcrumb from '@/components/public/Breadcrumb.vue'
import { contactsApi } from '@/api/contacts'

const form = reactive({ name: '', phone: '', email: '', message: '' })
const errors = reactive<Record<string, string>>({})
const success = ref(false)
const loading = ref(false)

const validate = () => {
  const e: Record<string, string> = {}
  if (!form.name.trim()) e.name = '請輸入姓名'
  if (!form.phone.trim()) e.phone = '請輸入電話'
  if (!form.email.trim()) e.email = '請輸入電子信箱'
  else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email)) e.email = '電子信箱格式不正確'
  if (!form.message.trim()) e.message = '請輸入詢問事項'
  Object.assign(errors, e)
  return Object.keys(e).length === 0
}

const handleSubmit = async () => {
  if (!validate()) return
  loading.value = true
  try {
    const { data } = await contactsApi.submit(form)
    if (data.success) {
      success.value = true
      Object.assign(form, { name: '', phone: '', email: '', message: '' })
    }
  } catch {
    errors.submit = '送出失敗，請稍後再試'
  }
  loading.value = false
}
</script>

<template>
  <div>
    <Breadcrumb :items="[{ label: 'Contact' }]" />
    <div class="bg-dark text-center py-10">
      <p class="text-gold-light font-serif text-sm tracking-[0.3em] uppercase mb-2">Contact</p>
      <h2 class="text-white text-2xl md:text-3xl font-bold">聯絡我們</h2>
    </div>

    <div class="max-w-[600px] mx-auto px-4 py-10">
      <div v-if="success" class="text-center py-10">
        <div class="text-gold text-5xl mb-4"><i class="pi pi-check-circle" /></div>
        <h3 class="text-xl font-bold mb-2">感謝您的留言</h3>
        <p class="text-gray-text">我們會盡快回覆您</p>
        <button class="mt-6 border border-gold text-gold px-6 py-2 hover:bg-gold hover:text-white transition" @click="success = false">
          再次留言
        </button>
      </div>

      <form v-else @submit.prevent="handleSubmit" class="space-y-5">
        <div>
          <label class="block text-sm font-medium mb-1">姓名 <span class="text-red-accent">*</span></label>
          <input v-model="form.name" type="text" class="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-gold" />
          <p v-if="errors.name" class="text-red-accent text-xs mt-1">{{ errors.name }}</p>
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">電話 <span class="text-red-accent">*</span></label>
          <input v-model="form.phone" type="tel" class="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-gold" />
          <p v-if="errors.phone" class="text-red-accent text-xs mt-1">{{ errors.phone }}</p>
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">電子信箱 <span class="text-red-accent">*</span></label>
          <input v-model="form.email" type="email" class="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-gold" />
          <p v-if="errors.email" class="text-red-accent text-xs mt-1">{{ errors.email }}</p>
        </div>
        <div>
          <label class="block text-sm font-medium mb-1">詢問事項 <span class="text-red-accent">*</span></label>
          <textarea v-model="form.message" rows="5" class="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-gold" />
          <p v-if="errors.message" class="text-red-accent text-xs mt-1">{{ errors.message }}</p>
        </div>
        <p v-if="errors.submit" class="text-red-accent text-sm">{{ errors.submit }}</p>
        <button
          type="submit"
          :disabled="loading"
          class="w-full bg-gold text-white py-3 rounded hover:bg-gold-dark transition disabled:opacity-50"
        >
          {{ loading ? '送出中...' : '送出' }}
        </button>
      </form>
    </div>
  </div>
</template>
