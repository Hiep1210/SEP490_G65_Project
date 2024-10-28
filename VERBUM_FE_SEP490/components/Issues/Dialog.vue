<script lang="ts" setup>
import { ref, watch, onMounted, defineEmits } from 'vue'
import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle
} from '@/components/ui/dialog'
import { Table, TableHeader, TableRow, TableCell, TableBody } from '@/components/ui/table'
import type { Issue } from '~/types/issues'
import { formatDate } from '~/utils/date'
import { useUsers } from '~/composables/useUsers'
const { assignList, getAssignList } = useUsers()

const props = defineProps<{
  open: boolean
  rowData: Issue
}>()

const emit = defineEmits(['close', 'update'])
const isOpen = ref(props.open)
const isEditing = ref(false)

onMounted(() => {
  if (!assignList.value.length) {
    getAssignList()
  }
  console.log({ assignList })
})

watch(
  () => props.open,
  (newVal) => {
    isOpen.value = newVal
    if (!newVal) isEditing.value = false
  }
)

const issue = ref(props.rowData)
const statuses = ['CANCEL', 'OPEN', 'RESOLVED', 'ACCEPTED']

const emitUpdate = () => {
  emit('update', issue.value)
  isEditing.value = false
}

const enableEditing = () => {
  isEditing.value = true
}

const closeDialog = () => {
  emit('close')
  isEditing.value = false
}
</script>

<template>
  <Dialog :open="isOpen" @click-outside="closeDialog" @close="closeDialog">
    <DialogContent class="sm:max-w-[425px]">
      <DialogHeader>
        <DialogTitle class="font-semibold text-3xl text-cyan-700">
          {{ issue.issueName }}
        </DialogTitle>
        <Button
          variant="ghost"
          class="absolute top-2 right-2"
          @click="closeDialog"
        />
      </DialogHeader>
      <div class="p-3 rounded-xl border-2 border-stone-300">
        <Table>
          <TableHeader>
            <TableRow>
              <TableCell class="font-semibold">Issue name:</TableCell>
              <TableCell>
                <template v-if="isEditing">
                  <input v-model="issue.issueName" class="border border-cyan-700 rounded p-1 w-full" >
                </template>
                <template v-else>{{ issue.issueName }}</template>
              </TableCell>
            </TableRow>
          </TableHeader>
          <TableBody>
            <TableRow>
              <TableCell class="font-semibold">Client name:</TableCell>
              <TableCell>
                <template v-if="isEditing">
                  <input v-model="issue.clientName" class="border border-cyan-700 rounded p-1 w-full" >
                </template>
                <template v-else>{{ issue.clientName }}</template>
              </TableCell>
            </TableRow>
            <TableRow>
              <TableCell class="font-semibold">Created date:</TableCell>
              <TableCell>{{ formatDate(issue.createdAt) }}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell class="font-semibold">Updated date:</TableCell>
              <TableCell>{{ formatDate(issue.updatedAt) }}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell class="font-semibold">Status:</TableCell>
              <TableCell>
                <template v-if="isEditing">
                  <ul class="flex flex-col gap-1">
                    <li v-for="status in statuses" :key="status">
                      <label class="inline-flex items-center">
                        <input v-model="issue.status" type="radio"  :value="status"  class="mr-2" >
                        {{ status }}
                      </label>
                    </li>
                  </ul>
                </template>
                <template v-else>{{ issue.status }}</template>
              </TableCell>
            </TableRow>
            <TableRow>
              <TableCell class="font-semibold">Assign:</TableCell>
              <TableCell>
                <template v-if="isEditing">
                  <select v-model="issue.assigneeId" class="border border-cyan-700 rounded p-1 w-full">
                    <option
                      v-for="user in assignList"
                      :key="user.id"
                      :value="user.id"
                    >
                      {{ user.name }}
                    </option>
                  </select>
                </template>
                <template v-else>{{ issue.assigneeName }}</template>
              </TableCell>
            </TableRow>
          </TableBody>
        </Table>
      </div>

      <div class="p-3 rounded-xl border-2 border-stone-300">
        <div class="font-semibold">Description:</div>
        <p>
          <template v-if="isEditing">
            <textarea
              v-model="issue.issueDescription"
              class="border border-cyan-700 rounded p-1 w-full"
            />
          </template>
          <template v-else>{{ issue.issueDescription }}</template>
        </p>
      </div>
      <div class="p-3 rounded-xl border-2 border-stone-300">
        <div class="font-semibold">Files:</div>
        <p>{{ issue.issueAttachments }}</p>
      </div>
      <DialogFooter>
        <Button class="bg-slate-500" @click="closeDialog">Cancel</Button>
        <Button v-if="!isEditing" @click="enableEditing">Edit</Button>
        <Button v-if="isEditing" @click="emitUpdate">Update</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>
