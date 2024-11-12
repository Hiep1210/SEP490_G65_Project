<script lang="ts" setup>
import { ref, watch, onMounted, defineEmits, computed } from 'vue'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogDescription
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
import { getFirebaseFileName } from '~/utils/getFirebaseFileName'
import type { ResolveIssuePayload } from '~/types/payload/resolveIssue'
import { useFileDialog } from '@vueuse/core'
import { cn } from '@/lib/utils'
import {
  ref as storageRef,
  getDownloadURL,
  uploadBytesResumable
} from 'firebase/storage'
import { ServiceManagersRole } from '~/constants/userRole'

const { assignList, getAssignList } = useUsers()
const { updateIssueStatus, sendCancelResponse, updateIssue, resolveIssue } =
  useIssues()

const props = defineProps<{
  open: boolean
  rowData: Issue
  role: string
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
const isResolveDialogOpen = ref(false)
const reasonForCancellation = ref('')

const storage = useFirebaseStorage()
const downloadUrls = ref<string[]>([])
const uploadProgress = ref<number[]>([])

const downloadUrlsString = computed(() => downloadUrls.value.join(','))

const { files, open: openFileSelect } = useFileDialog()

async function uploadFiles() {
  if (files.value?.length) {
    const promises = Array.from(files.value).map(
      (file, index) =>
        new Promise<string>((resolve, reject) => {
          const fileRef = storageRef(storage, `uploads/${file.name}`)
          const uploadTask = uploadBytesResumable(fileRef, file)

          uploadTask.on(
            'state_changed',
            (snapshot) => {
              const progress =
                (snapshot.bytesTransferred / snapshot.totalBytes) * 100
              uploadProgress.value[index] = Math.round(progress)
            },
            (error) => {
              reject(error)
            },
            async () => {
              const url = await getDownloadURL(fileRef)
              resolve(url)
            }
          )
        })
    )

    const urls = await Promise.all(promises)
    downloadUrls.value = [...downloadUrls.value, ...urls]
  }
}

watch(files, () => {
  if (files.value?.length) {
    uploadProgress.value = Array(files.value.length).fill(0)
    uploadFiles()
  }
})

onMounted(() => {
  if (!assignList.value.length) {
    getAssignList()
  }
  console.log({ assignList })
})

const issue = ref(props.rowData)
const issueStatuses = ['OPEN', 'IN_PROGRESS', 'CANCEL', 'SUBMITTED', 'RESOLVED']
const selectedStatus = ref(issue.value.status)

const updateIssueDetail = async () => {
  const payload = {
    issueId: issue.value.issueId,
    issueName: issue.value.issueName,
    issueDescription: issue.value.issueDescription,
    assigneeId: issue.value.assigneeId,
    issueAttachments: issue.value.issueAttachments
  }
  await updateIssue(payload)
  if (issue.value.status === 'OPEN') {
    await updateIssueStatus(issue.value.issueId, 'IN_PROGRESS')
  }
  isEditing.value = false
}

const enableEditing = () => {
  isEditing.value = true
}

const getUserIdByName = (users: User[], name: string): string | undefined => {
  const user = users.find((user) => user.name === name)
  return user?.id
}

const handleResolveIssue = async () => {
  const solutionAttachment: IssueAttachments = {
    issueId: issue.value.issueId,
    attachmentUrl: downloadUrlsString,
    tag: 'SOLUTION',
    isDeleted: false
  };

  const updatedIssueAttachments = [...issue.value.issueAttachments, solutionAttachment];

  const payload: ResolveIssuePayload = {
    issueId: issue.value.issueId,
    issueName: issue.value.issueName,
    issueDescription: issue.value.issueDescription,
    assigneeId: getUserIdByName(assignList.value, issue.value.assigneeName),
    issueAttachments: updatedIssueAttachments
  };

  console.log(payload);

  await resolveIssue(payload);
  await updateIssueStatus(issue.value.issueId, 'SUBMITTED');
  isResolveDialogOpen.value = false;
}

const handleConfirmStatus = async () => {
  await sendCancelResponse(issue.value.issueId, reasonForCancellation.value)
  await updateIssueStatus(issue.value.issueId, 'CANCEL')
  isCancelDialogOpen.value = false
}

const handleCancelStatus = () => {
  selectedStatus.value = previousStatus.value
  isCancelDialogOpen.value = false
  reasonForCancellation.value = ''
}

const handleStatusChange = async (
  issuesId: string,
  oldStatus: string,
  newStatus: string
) => {
  if (newStatus === 'CANCEL') {
    isCancelDialogOpen.value = true
    previousStatus.value = oldStatus
  } else if (newStatus === 'SUBMITTED') {
    isResolveDialogOpen.value = true
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



// Compute allowed statuses based on the role
const filteredIssueStatuses = computed(() => {
  switch (props.role) {
    case 'CLIENT':
      return ['CANCEL']
    case 'EDIT_MANAGER':
    case 'EVALUATE_MANAGER':
    case 'TRANSLATE_MANAGER':
      return ['CANCEL', 'RESOLVED']
    case 'LINGUIST':
      return ['SUBMITTED']
    default:
      return issueStatuses
  }
})

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
    <DialogContent class="max-w-[1000px] max-h-[750px] overflow-y-scroll">
      <DialogHeader>
        <DialogTitle class="font-semibold text-4xl text-cyan-700">
          {{ issue.issueName }}
        </DialogTitle>
      </DialogHeader>

      <div
        v-if="issue.cancelResponse"
        class="p-3 rounded-xl border-2 border-stone-300"
      >
        <div class="font-semibold text-red-600">Cancellation Reason:</div>
        <p>{{ issue.cancelResponse }}</p>
      </div>

      <div class="p-3 rounded-xl border-2 border-stone-300">
        <Table>
          <TableHeader>
            <TableRow>
              <TableCell class="font-semibold">Issue name:</TableCell>
              <TableCell>
                <template v-if="isEditing && role === 'CLIENT'">
                  <Input v-model="issue.issueName" class="rounded p-1 w-full" />
                </template>
                <template v-else>{{ issue.issueName }}</template>
              </TableCell>
            </TableRow>
          </TableHeader>
          <TableBody>
            <TableRow v-if="role !== 'CLIENT'">
              <TableCell class="font-semibold">Created by:</TableCell>
              <TableCell>
                {{ issue.clientName }}
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
            <TableRow v-if="role !== 'CLIENT'">
              <TableCell class="font-semibold">Assign:</TableCell>
              <TableCell>
                <template
                  v-if="
                    isEditing &&
                    ServiceManagersRole.includes(role) &&
                    issue.status === 'OPEN'
                  "
                >
                  <Select
                    v-model="issue.assigneeId"
                    class="border border-cyan-700 rounded w-full"
                  >
                    <SelectTrigger class="w-[180px]">
                      <SelectValue :placeholder="issue.assigneeName" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectGroup>
                        <SelectItem
                          v-for="user in assignList"
                          :key="user.id"
                          :value="user.id"
                        >
                          {{ user.name }}
                        </SelectItem>
                      </SelectGroup>
                    </SelectContent>
                  </Select>
                </template>
                <template v-else>{{ issue.assigneeName }}</template>
              </TableCell>
            </TableRow>
            <TableRow>
              <TableCell class="font-semibold">Status:</TableCell>
              <TableCell
                ><Select
                  v-model="selectedStatus"
                  @update:modelValue="
                    (newStatus) =>
                      handleStatusChange(issue.issueId, issue.status, newStatus)
                  "
                >
                  <SelectTrigger
                    class="max-w-fit p-0 border-none focus:ring-0 focus:ring-offset-0 [&_svg]:hidden"
                  >
                    <SelectValue>
                      <Badge :class="getIssueBadgeClass(selectedStatus)"
                        >{{ selectedStatus }}
                      </Badge>
                    </SelectValue>
                  </SelectTrigger>
                  <SelectContent>
                    <SelectGroup>
                      <SelectItem
                        v-for="issueStatus in filteredIssueStatuses"
                        :key="issueStatus"
                        :value="issueStatus"
                      >
                        <Badge :class="getIssueBadgeClass(issueStatus)"
                          >{{ issueStatus }}
                        </Badge>
                      </SelectItem>
                    </SelectGroup>
                  </SelectContent>
                </Select></TableCell
              >
            </TableRow>
          </TableBody>
        </Table>
      </div>

      <div class="p-3 rounded-xl border-2 border-stone-300">
        <div class="font-semibold">Description:</div>
        <p>
          <template v-if="isEditing && role === 'CLIENT'">
            <Textarea
              v-model="issue.issueDescription"
              class="border border-cyan-700 rounded p-1 w-full"
            />
          </template>
          <template v-else>{{ issue.issueDescription }}</template>
        </p>
      </div>

      <div class="p-3 rounded-xl border-2 border-stone-300">
        <div class="font-semibold">Files:</div>
        <div v-if="issue.issueAttachments.length !== 0">
          <div
            v-for="attachment in issue.issueAttachments"
            :key="attachment.attachmentUrl"
          >
            <a
              :href="attachment.attachmentUrl"
              target="_blank"
              rel="noopener noreferrer"
              class="border rounded-xl flex flex-col gap-3 w-[150px] justify-center items-center p-2 hover:bg-stone-200"
              :title="getFirebaseFileName(attachment.attachmentUrl)"
            >
              <img
                src="~/assets/img/file_icon.png"
                loading="eager"
                format="avif"
                width="100"
                height="50"
                alt="file icon"
              />
              <h1
                class="whitespace-nowrap overflow-hidden text-ellipsis w-full text-center px-2"
              >
                {{ getFirebaseFileName(attachment.attachmentUrl) }}
              </h1>
            </a>
          </div>
        </div>
        <div v-else>
          <p class="text-primary font-semibold">No attachments found</p>
        </div>
      </div>

      <DialogFooter>
        <Button v-if="isEditing" class="bg-slate-500" @click="closeDialog"
          >Cancel
        </Button>
        <Button v-if="!isEditing" class="bg-slate-500" @click="closeDialog"
          >Close
        </Button>
        <Button
          v-if="!isEditing && issue.status !== 'CANCEL'"
          @click="enableEditing"
          >Edit
        </Button>
        <Button v-if="isEditing" @click="updateIssueDetail">Update</Button>
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

  <Dialog :open="isCancelDialogOpen" @close="handleCancelStatus">
    <DialogContent class="max-w-md">
      <DialogHeader>
        <DialogTitle>Provide Cancellation Reason</DialogTitle>
      </DialogHeader>
      <Input
        v-model="reasonForCancellation"
        placeholder="Enter reason for cancellation"
        class="w-full"
      />
      <DialogDescription class="text-red-500 font-semibold">
        {{ descriptionStatusConfirm }}
      </DialogDescription>
      <DialogFooter>
        <Button class="bg-gray-500" @click="handleCancelStatus">Cancel</Button>
        <Button class="bg-red-500" @click="handleConfirmStatus">Confirm</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>

  <Dialog :open="isResolveDialogOpen">
    <DialogContent class="max-w-md">
      <DialogHeader>
        <DialogTitle>Upload Issues Solution</DialogTitle>
      </DialogHeader>
      <Button
        class="block"
        type="button"
        @click="openFileSelect({ accept: '*', multiple: true })"
      >
        Upload Files
      </Button>
      <Card v-if="files?.length" :class="cn($attrs.class ?? '')">
        <CardHeader>
          <CardDescription>Uploaded files</CardDescription>
        </CardHeader>
        <CardContent class="grid gap-3">
          <div
            v-for="(file, index) in files"
            :key="file.name"
            class="mb-4 grid grid-cols-[25px_minmax(0,1fr)] items-start pb-4 last:mb-0 last:pb-0"
          >
            <span class="flex h-2 w-2 translate-y-1 rounded-full bg-sky-500" />
            <div class="flex flex-col gap-1">
              <p class="text-sm font-medium leading-none">
                {{ file.name }}
              </p>
              <div class="flex gap-5 max-w-sm">
                <Progress v-model="uploadProgress[index]" />
                <p class="text-sm font-medium leading-none">
                  {{ uploadProgress[index] || 0 }}%
                </p>
              </div>
            </div>
          </div>
        </CardContent>
      </Card>
      <DialogFooter>
        <DialogClose as-child>
          <Button class="bg-gray-500">Cancel</Button>
        </DialogClose>
        <Button class="bg-red-500" @click="handleResolveIssue">Submit</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>
