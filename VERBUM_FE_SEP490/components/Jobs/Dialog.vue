<script lang="ts" setup>
import { getJobBadgeClass } from '@/utils/getBadgeClass'
import type { Job } from '~/types/job'
import { useToast } from '@/components/ui/toast/use-toast'
const props = defineProps<{
  job?: Job
  role?: string
}>()
const jobStatuses = ['OPEN', 'IN_PROGRESS', 'SUBMITTED', 'APPROVED']

const canUploadFile = computed(() => {
  if (!props.job) return false

  const isStatusValid = !['SUBMITTED', 'APPROVED'].includes(props.job.status)
  const hasRequiredFields = props.job.dueDate && props.job.assigneeNames?.length > 0

  return isStatusValid && hasRequiredFields
})

const canEdit = computed(() => {
  if (!props.job) return false

  const hasRequiredFields = props.job.dueDate && props.job.assigneeNames?.length > 0
  const isPermitted = props.role?.includes('MANAGER')

  return hasRequiredFields && isPermitted
})

const { toast } = useToast()
const assignLinguists = async (payload: {
  assigneesId: string[]
  dueDate: string
}) => {
  const assignPayload = {
    id: props.job?.id,
    name: props.job?.name,
    status: "IN_PROGRESS",
    assigneesId: payload.assigneesId,
    dueDate: payload.dueDate
  }
  try {
    const res = await repo(useNuxtApp().$api).assignLinguists(assignPayload)
    console.log(res)
    if (!res) {
      toast({
        title: 'Linguists assigned successfully',
        description: 'Linguists have been assigned to the job',
      })
      window.location.reload()
    }
    else {
      toast({
        title: 'Failed to assign linguists',
        description: 'Please try again later',
        variant: 'destructive'
      })
    }
  } catch (error) {
    console.error('Failed to assign linguists:', error)
  }
}
const { approve, reject } = useJobs()
</script>

<template>
  <Dialog>
    <DialogTrigger as-child>
      <slot />
    </DialogTrigger>
    <DialogContent>
      <DialogHeader>
        <DialogTitle>{{ props.job?.assigneeNames?.map((assignee: any) => assignee.name).join(', ') }}</DialogTitle>
        <div class="flex justify-between">
          <Badge :class="getJobBadgeClass(props.job?.status ?? '')">{{ props.job?.status }}</Badge>
        </div>
      </DialogHeader>
      <DialogDescription class="text-black">
        <div class="flex flex-col gap-2">
          <div class="flex justify-between">
            <h1 class="text-2xl font-semibold">Details</h1>
            <JobsEditDialog v-if="canEdit">
              <Button variant="outline">Edit</Button>
            </JobsEditDialog>
          </div>
          <div>
            <p v-if="props.job?.assigneeNames && props.job.assigneeNames.length > 0" class="text-sm">Assigned to: {{
              props.job?.assigneeNames.map(assignee => assignee.name).join(', ') }}</p>
            <p class="text-sm">Target Language: {{ props.job?.targetLanguageId }} </p>
            <p class="text-sm">Word Count: {{ props.job?.wordCount }} </p>
            <p v-if="props.job?.dueDate" class="text-sm">Due Date: {{ props.job?.dueDate }} </p>
            <p v-if="props.job?.createdAt" class="text-sm">Created At: {{ props.job?.createdAt }} </p>
            <p v-if="props.job?.updatedAt" class="text-sm">Updated At: {{ props.job?.updatedAt }} </p>
          </div>
          <JobsTabs :job="props.job" />
        </div>
      </DialogDescription>
      <DialogFooter>
        <template v-if="props.role?.includes('MANAGER')">
          <JobsAssignDialog v-if="props.job?.assigneeNames?.length === 0" :order-due-date="props.job?.dueDate" @assign="assignLinguists" />
          <Button variant="outline" :disabled="props.job?.status !== 'SUBMITTED' || !props.job" @click="approve(props.job)">Approve</Button>
          <Button variant="outline" :disabled="props.job?.status !== 'SUBMITTED' || !props.job" @click="reject(props.job)">Reject</Button>
        </template>
        <template v-else>
          <JobsUploadFileDialog :job="props.job">
            <Button variant="outline" :disabled="!canUploadFile">Upload File</Button>
          </JobsUploadFileDialog>
        </template>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>