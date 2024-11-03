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
import {
  Table,
  TableHeader,
  TableRow,
  TableCell,
  TableBody
} from '@/components/ui/table'
import type { Issue } from '~/types/issues'
import { formatDate } from '~/utils/date'
import { useUsers } from '~/composables/useUsers'
import {getIssueBadgeClass} from '@/utils/getBadgeClass'
const { assignList, getAssignList } = useUsers()

const props = defineProps<{
  open: boolean
  rowData: Issue
}>()

const emit = defineEmits(['close', 'update', 'update-status'])
const isOpen = ref(props.open)
const isEditing = ref(false)
const isStatusEditing = ref(false)
const previousStatus = ref('')
const titleStatusConfirm = 'Change status'
const descriptionStatusConfirm =
  'If you change status to CANCEL the issue you can not reopen it. If you not skip this note. '
const isConfirmDialogOpen = ref(false)
onMounted(() => {
  if (!assignList.value.length) {
    getAssignList()
  }
  console.log({ assignList })
})

const issue = ref(props.rowData)
const statuses = ['CANCEL', 'OPEN', 'RESOLVED', 'ACCEPTED']

const emitUpdate = () => {
  emit('update', issue.value)
  isEditing.value = false
}

const emitUpdateStatus = () => {
  emit('update-status', issue.value.issueId, issue.value.status)
  isStatusEditing.value = false
}

const enableEditing = () => {
  isEditing.value = true
}

const startStatusEditing = () => {
  previousStatus.value = issue.value.status // Save the current status
  isStatusEditing.value = true
}

const cancelStatusUpdate = () => {
  issue.value.status = previousStatus.value // Revert to the previous status
  isStatusEditing.value = false
}

const openConfirmDialog = () => {
  isConfirmDialogOpen.value = true
}

const handleConfirmStatus = () => {
  emitUpdateStatus()
  isConfirmDialogOpen.value = false // Close the confirmation dialog
}

const handleCancelStatus = () => {
  // Revert the status change when Cancel is clicked
  issue.value.status = previousStatus.value
  isConfirmDialogOpen.value = false // Close the confirmation dialog
}

const closeDialog = () => {
  emit('close')
  isEditing.value = false
}

watch(
  () => props.open,
  (newVal) => {
    isOpen.value = newVal
    if (!newVal) {
      isEditing.value = false
      isStatusEditing.value = false
    }
  }
)
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
          <TableRow class="border-none">
            <TableCell class="font-semibold">Status:</TableCell>
            <TableCell>
              <template v-if="isStatusEditing">
                <ul class="flex flex-col gap-1 mb-2">
                  <li v-for="status in statuses" :key="status">
                    <label class="inline-flex items-center">
                      <input
                        v-model="issue.status"
                        type="radio"
                        :value="status"
                        class="mr-2"
                      />
                      {{ status }}
                    </label>
                  </li>
                </ul>
                <div class="flex justify-end gap-2">
                  <Button
                    class="bg-gray-500 text-white"
                    @click="cancelStatusUpdate"
                    >Cancel</Button
                  >
                  <Button @click="openConfirmDialog">Save</Button>
                </div>
              </template>
              <template v-else>
                <Badge :class="getIssueBadgeClass(issue.status)">{{
                  issue.status
                }}</Badge>
                <p
                  class="cursor-pointer text-cyan-700 italic"
                  @click="startStatusEditing"
                >
                  Update status
                </p>
              </template>
            </TableCell>
          </TableRow>
        </Table>
      </div>
      <div class="p-3 rounded-xl border-2 border-stone-300">
        <Table>
          <TableHeader>
            <TableRow>
              <TableCell class="font-semibold">Issue name:</TableCell>
              <TableCell>
                <template v-if="isEditing">
                  <input
                    v-model="issue.issueName"
                    class="border border-cyan-700 rounded p-1 w-full"
                  />
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
                  <input
                    v-model="issue.clientName"
                    class="border border-cyan-700 rounded p-1 w-full"
                  />
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
              <TableCell class="font-semibold">Assign:</TableCell>
              <TableCell>
                <template v-if="isEditing">
                  <select
                    v-model="issue.assigneeId"
                    class="border border-cyan-700 rounded p-1 w-full"
                  >
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
        <Button v-if="isEditing" class="bg-slate-500" @click="closeDialog"
          >Cancel</Button
        >
        <Button v-if="!isEditing" class="bg-slate-500" @click="closeDialog"
          >Close</Button
        >
        <Button v-if="!isEditing" @click="enableEditing">Edit</Button>
        <Button v-if="isEditing" @click="emitUpdate">Update</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
  <IssuesConfirmDialog
    :title="titleStatusConfirm"
    :description="descriptionStatusConfirm"
    :open="isConfirmDialogOpen"
    @close="handleCancelStatus"
    @confirm="handleConfirmStatus"
  />
</template>
