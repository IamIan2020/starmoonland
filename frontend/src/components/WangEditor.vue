<script setup lang="ts">
import { ref, shallowRef, onBeforeUnmount, watch } from 'vue'
import { Editor, Toolbar } from '@wangeditor/editor-for-vue'
import type { IDomEditor, IEditorConfig, IToolbarConfig } from '@wangeditor/editor'
// @ts-ignore - WangEditor CSS import

const props = defineProps<{
  modelValue: string
}>()

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

const editorRef = shallowRef<IDomEditor>()
const valueHtml = ref(props.modelValue || '')

watch(() => props.modelValue, (val) => {
  if (val !== valueHtml.value) valueHtml.value = val || ''
})

const toolbarConfig: Partial<IToolbarConfig> = {}

const editorConfig: Partial<IEditorConfig> = {
  placeholder: '請輸入內容...',
  MENU_CONF: {
    uploadImage: {
      server: '/api/admin/media/upload',
      fieldName: 'file',
      maxFileSize: 10 * 1024 * 1024,
      headers: {
        Authorization: `Bearer ${localStorage.getItem('accessToken') || ''}`,
      },
      customInsert(res: any, insertFn: (url: string, alt?: string, href?: string) => void) {
        if (res.success && res.data) {
          insertFn(`/uploads/${res.data.filePath}`, res.data.originalName, '')
        }
      },
    },
  },
}

const handleCreated = (editor: IDomEditor) => {
  editorRef.value = editor
}

const handleChange = (editor: IDomEditor) => {
  const html = editor.getHtml()
  valueHtml.value = html
  emit('update:modelValue', html)
}

onBeforeUnmount(() => {
  editorRef.value?.destroy()
})
</script>

<template>
  <div class="wang-editor-container border rounded overflow-hidden">
    <Toolbar :editor="editorRef" :defaultConfig="toolbarConfig" class="border-b" />
    <Editor
      :defaultConfig="editorConfig"
      v-model="valueHtml"
      class="editor-content"
      @onCreated="handleCreated"
      @onChange="handleChange"
    />
  </div>
</template>

<style>
@import '@wangeditor/editor/dist/css/style.css';

.wang-editor-container .editor-content {
  height: 400px;
  overflow-y: auto;
}
</style>
