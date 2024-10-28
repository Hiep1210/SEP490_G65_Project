<script lang="ts" setup>
import { ref, watch, defineEmits } from 'vue'
import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle
} from '@/components/ui/dialog'
import type { Issue } from '~/types/issues'
import { formatDate } from '~/utils/date'

const props = defineProps<{
  open: boolean
  rowData: Issue // Receive row data as a prop
}>()

const emit = defineEmits(['close', 'update']) // Emit update event
const isOpen = ref(props.open)

watch(
  () => props.open,
  (newVal) => {
    isOpen.value = newVal
  }
)


const issue = props.rowData

console.log('over here: ', props.rowData)

const closeDialog = () => {
  emit('close') // Emit close event
}
</script>

<template>
  <Dialog :open="isOpen" @click-outside="closeDialog" @close="closeDialog">
    <DialogContent class="sm:max-w-[425px]">
      <DialogHeader>
        <DialogTitle class="font-semibold text-3xl text-cyan-700">{{issue.issueName}}</DialogTitle>
        <Button
          variant="ghost"
          class="absolute top-2 right-2"
          @click="closeDialog"
        />
      </DialogHeader>
      <div class=" p-3 rounded-xl border-2 border-stone-300">
        <DialogDescription> Client name: {{ issue.clientName }} </DialogDescription>
        <DialogDescription> Created date: {{ formatDate(issue.createdAt) }} </DialogDescription>
        <DialogDescription> Updated date: {{ formatDate(issue.updatedAt) }} </DialogDescription>
        <DialogDescription> Status: {{ issue.status }} </DialogDescription>
        <DialogDescription> Assign: {{ issue.assigneeName }} </DialogDescription>
      </div>
      <div class=" p-3 rounded-xl border-2 border-stone-300">
        <div>Description: </div>
        <DialogDescription> {{ issue.issueDescription }} </DialogDescription>
      </div>
      <div class=" p-3 rounded-xl border-2 border-stone-300">
        <div>Files: </div>
        <DialogDescription> {{ issue.issueDescription }} </DialogDescription>
      </div>
      <DialogFooter>
        <Button class="bg-slate-500" @click="closeDialog">Cancel</Button>
        <Button >Edit</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>
