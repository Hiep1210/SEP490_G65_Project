<script lang="ts" setup>
import { ref, watch, onMounted, defineEmits } from 'vue'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
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
import { getIssueBadgeClass } from '@/utils/getBadgeClass'
const { assignList, getAssignList } = useUsers()
const { updateIssueStatus, sendCancelResponse } = useIssues()

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
  'If you change status to CANCEL, you CAN NOT reopen it. Are you sure you want to change status?'
const isConfirmDialogOpen = ref(false)
const isCancelDialogOpen = ref(false)
const reasonForCancellation = ref('')

onMounted(() => {
  if (!assignList.value.length) {
    getAssignList()
  }
  console.log({ assignList })
})

const issue = ref(props.rowData)
const issueStatuses = ['CANCEL', 'OPEN', 'RESOLVED', 'ACCEPTED']
const selectedStatus = ref(issue.value.status)
const emitUpdate = () => {
  emit('update', issue.value)
  isEditing.value = false
}

const enableEditing = () => {
  isEditing.value = true
}

const handleConfirmStatus = async () => {
  await sendCancelResponse(issue.value.issueId, reasonForCancellation.value)
  isCancelDialogOpen.value = false
}

const handleCancelStatus = () => {
  selectedStatus.value = previousStatus.value
  isCancelDialogOpen.value = false
  reasonForCancellation.value = ''
}

const handleStatusChange = async (issuesId: string, oldStatus: string, newStatus: string) => {
  if (newStatus === 'CANCEL') {
    isCancelDialogOpen.value = true
    previousStatus.value = oldStatus
  } else {
    await updateIssueStatus(issuesId, newStatus)
    issue.value.status = newStatus
  }
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
    <DialogContent class="max-w-[1000px]">
      <DialogHeader>
        <DialogTitle class="font-semibold text-4xl text-cyan-700">
          {{ issue.issueName }}
        </DialogTitle>
      </DialogHeader>
      <Select v-model="selectedStatus"
        @update:modelValue="(newStatus) => handleStatusChange(issue.issueId, issue.status, newStatus)">
        <SelectTrigger class="max-w-fit border-none focus:ring-0 focus:ring-offset-0 [&_svg]:hidden">
          <SelectValue>
            <Badge :class="getIssueBadgeClass(selectedStatus)">{{ selectedStatus }}</Badge>
          </SelectValue>
        </SelectTrigger>
        <SelectContent>
          <SelectGroup>
            <SelectItem v-for="issueStatus in issueStatuses" :key="issueStatus" :value="issueStatus">
              <Badge :class="getIssueBadgeClass(issueStatus)">{{ issueStatus }}</Badge>
            </SelectItem>
          </SelectGroup>
        </SelectContent>
      </Select>
      <div class="p-3 rounded-xl border-2 border-stone-300">
        <Table>
          <TableHeader>
            <TableRow>
              <TableCell class="font-semibold">Issue name:</TableCell>
              <TableCell>
                <template v-if="isEditing">
                  <Input v-model="issue.issueName" class="rounded p-1 w-full" />
                </template>
                <template v-else>{{ issue.issueName }}</template>
              </TableCell>
            </TableRow>
          </TableHeader>
          <TableBody>
            <TableRow>
              <TableCell class="font-semibold">Created by:</TableCell>
              <TableCell>
                <template v-if="isEditing">
                  <Input v-model="issue.clientName" class="border border-cyan-700 rounded p-1 w-full" />
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
                  <select v-model="issue.assigneeId" class="border border-cyan-700 rounded p-1 w-full">
                    <option v-for="user in assignList" :key="user.id" :value="user.id">
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
            <textarea v-model="issue.issueDescription" class="border border-cyan-700 rounded p-1 w-full" />
          </template>
          <template v-else>{{ issue.issueDescription }}</template>
        </p>
      </div>
      <div class="p-3 rounded-xl border-2 border-stone-300">
        <div class="font-semibold">Files:</div>
        <p>{{ issue.issueAttachments }}</p>
      </div>
      <DialogFooter>
        <Button v-if="isEditing" class="bg-slate-500" @click="closeDialog">Cancel</Button>
        <Button v-if="!isEditing" class="bg-slate-500" @click="closeDialog">Close</Button>
        <Button v-if="!isEditing" @click="enableEditing">Edit</Button>
        <Button v-if="isEditing" @click="emitUpdate">Update</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
  <IssuesConfirmDialog :title="titleStatusConfirm" :description="descriptionStatusConfirm" :open="isConfirmDialogOpen"
    @close="handleCancelStatus" @confirm="handleConfirmStatus" />
  <Dialog :open="isCancelDialogOpen" @close="handleCancelStatus">
    <DialogContent class="max-w-md">
      <DialogHeader>
        <DialogTitle>Provide Cancellation Reason</DialogTitle>
      </DialogHeader>
      <Input v-model="reasonForCancellation" placeholder="Enter reason for cancellation" class="w-full" />
      <DialogDescription class="text-red-500 font-semibold">
        {{ descriptionStatusConfirm }}
      </DialogDescription>
      <DialogFooter>
        <Button class="bg-gray-500" @click="handleCancelStatus">Cancel</Button>
        <Button class="bg-red-500" @click="handleConfirmStatus">Confirm</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>
