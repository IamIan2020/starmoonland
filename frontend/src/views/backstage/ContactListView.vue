<script setup lang="ts">
import { ref, onMounted } from 'vue'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import Textarea from 'primevue/textarea'
import InputText from 'primevue/inputtext'
import Tag from 'primevue/tag'
import Select from 'primevue/select'
import { useToast } from 'primevue/usetoast'
import { contactsApi } from '@/api/contacts'
import type { ContactSubmissionDto } from '@/types/api'

const toast = useToast()

const contacts = ref<ContactSubmissionDto[]>([])
const loading = ref(false)
const totalRecords = ref(0)
const currentPage = ref(1)
const pageSize = ref(10)
const filterRead = ref<boolean | undefined>(undefined)

// 詳情/回覆 Dialog
const detailDialog = ref(false)
const selectedContact = ref<ContactSubmissionDto | null>(null)
const replyContent = ref('')
const replyNote = ref('')
const replying = ref(false)

const readFilterOptions = [
  { label: '全部', value: undefined },
  { label: '未讀', value: false },
  { label: '已讀', value: true },
]

const loadContacts = async () => {
  loading.value = true
  try {
    const { data } = await contactsApi.adminGetList({
      page: currentPage.value,
      pageSize: pageSize.value,
      isRead: filterRead.value,
    })
    contacts.value = data.data
    totalRecords.value = data.totalCount
  } catch (err) { console.error(err); toast.add({ severity: 'error', summary: '錯誤', detail: '載入失敗', life: 3000 }) }
  loading.value = false
}

const onPage = (event: { page: number; rows: number }) => {
  currentPage.value = event.page + 1
  pageSize.value = event.rows
  loadContacts()
}

const viewDetail = async (contact: ContactSubmissionDto) => {
  selectedContact.value = contact
  replyContent.value = contact.replyContent || ''
  replyNote.value = contact.replyNote || ''
  detailDialog.value = true

  // 自動標記已讀
  if (!contact.isRead) {
    try {
      await contactsApi.adminMarkRead(contact.id)
      contact.isRead = true
    } catch (err) { console.error(err) }
  }
}

const sendReply = async () => {
  if (!selectedContact.value || !replyContent.value.trim()) {
    toast.add({ severity: 'warn', summary: '提示', detail: '請填寫回覆內容', life: 3000 })
    return
  }
  replying.value = true
  try {
    await contactsApi.adminReply(selectedContact.value.id, {
      replyContent: replyContent.value,
      replyNote: replyNote.value,
    })
    toast.add({ severity: 'success', summary: '成功', detail: '回覆已寄出', life: 3000 })
    detailDialog.value = false
    await loadContacts()
  } catch {
    toast.add({ severity: 'error', summary: '錯誤', detail: '回覆失敗', life: 3000 })
  }
  replying.value = false
}

const onFilterChange = () => {
  currentPage.value = 1
  loadContacts()
}

onMounted(loadContacts)
</script>

<template>
  <div>
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold">聯絡表單</h2>
    </div>

    <div class="mb-4">
      <Select
        v-model="filterRead"
        :options="readFilterOptions"
        optionLabel="label"
        optionValue="value"
        placeholder="篩選狀態"
        class="w-full md:w-48"
        @change="onFilterChange"
      />
    </div>

    <DataTable
      :value="contacts"
      :loading="loading"
      lazy
      paginator
      :rows="pageSize"
      :totalRecords="totalRecords"
      :first="(currentPage - 1) * pageSize"
      @page="onPage"
      stripedRows
      :rowClass="(data: ContactSubmissionDto) => !data.isRead ? 'font-bold' : ''"
    >
      <Column header="狀態" style="width: 80px">
        <template #body="{ data }">
          <Tag v-if="data.repliedAt" severity="success" value="已回覆" />
          <Tag v-else-if="data.isRead" severity="info" value="已讀" />
          <Tag v-else severity="warn" value="未讀" />
        </template>
      </Column>
      <Column field="name" header="姓名" style="width: 120px" />
      <Column field="email" header="Email" style="width: 200px" />
      <Column field="phone" header="電話" style="width: 140px" />
      <Column header="訊息" class="max-w-xs">
        <template #body="{ data }">
          <span class="line-clamp-2">{{ data.message }}</span>
        </template>
      </Column>
      <Column header="日期" style="width: 120px">
        <template #body="{ data }">
          {{ new Date(data.createdAt).toLocaleDateString('zh-TW') }}
        </template>
      </Column>
      <Column header="操作" style="width: 80px">
        <template #body="{ data }">
          <Button icon="pi pi-eye" severity="info" size="small" text @click="viewDetail(data)" />
        </template>
      </Column>
    </DataTable>

    <!-- 詳情/回覆 Dialog -->
    <Dialog v-model:visible="detailDialog" header="留言詳情" :style="{ width: '600px' }" modal>
      <div v-if="selectedContact" class="space-y-4">
        <div class="grid grid-cols-2 gap-4 bg-gray-50 p-4 rounded">
          <div>
            <label class="block text-xs text-gray-500">姓名</label>
            <p class="font-medium">{{ selectedContact.name }}</p>
          </div>
          <div>
            <label class="block text-xs text-gray-500">電話</label>
            <p>{{ selectedContact.phone }}</p>
          </div>
          <div>
            <label class="block text-xs text-gray-500">Email</label>
            <p>{{ selectedContact.email }}</p>
          </div>
          <div>
            <label class="block text-xs text-gray-500">日期</label>
            <p>{{ new Date(selectedContact.createdAt).toLocaleString('zh-TW') }}</p>
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium mb-1">留言內容</label>
          <div class="bg-gray-50 p-4 rounded whitespace-pre-wrap">{{ selectedContact.message }}</div>
        </div>

        <hr />

        <div v-if="selectedContact.repliedAt" class="bg-green-50 p-4 rounded">
          <p class="text-sm text-green-700 mb-2">已於 {{ new Date(selectedContact.repliedAt).toLocaleString('zh-TW') }} 回覆</p>
          <div class="whitespace-pre-wrap">{{ selectedContact.replyContent }}</div>
        </div>

        <template v-else>
          <div>
            <label class="block text-sm font-medium mb-1">回覆內容（將寄送至訪客信箱）</label>
            <Textarea v-model="replyContent" rows="5" class="w-full" />
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">內部備註（不會寄送）</label>
            <InputText v-model="replyNote" class="w-full" />
          </div>
        </template>
      </div>
      <template #footer>
        <Button label="關閉" severity="secondary" @click="detailDialog = false" />
        <Button
          v-if="selectedContact && !selectedContact.repliedAt"
          label="回覆並寄送"
          icon="pi pi-send"
          :loading="replying"
          @click="sendReply"
        />
      </template>
    </Dialog>
  </div>
</template>
