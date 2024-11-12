<script lang="ts" setup>
import { getJobBadgeClass } from '@/utils/getBadgeClass'
import type { Job } from '~/types/job'
import { useToast } from '@/components/ui/toast/use-toast'
const props = defineProps<{
  job?: Job
  role?: string
}>()
const jobStatuses = ['OPEN', 'IN_PROGRESS', 'SUBMITTED', 'APPROVED']

const filteredJobStatuses = computed(() => {
  switch (props.role) {
    case 'CLIENT':
      return ['CANCEL']
    case 'EDIT_MANAGER':
    case 'EVALUATE_MANAGER':
    case 'TRANSLATE_MANAGER':
      return ['CANCEL', 'APPROVED']
    case 'LINGUIST':
      return ['SUBMITTED', 'IN_PROGRESS']
    default:
      return jobStatuses
  }
})

const canUploadFile = computed(() => {
  if (!props.job) return false

  const isStatusValid = !['SUBMITTED', 'APPROVED'].includes(props.job.status)
  const hasRequiredFields = props.job.dueDate && props.job.assigneeNames?.length > 0

  return isStatusValid && hasRequiredFields
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
const approve = async () => {
  try {
    const {status, error} = await useAPI('/job/approve', {
      method: 'PUT',
      query: {
        jobId: props.job?.id,
        orderId: props.job?.orderId
      }
    })
    if (status.value === "error") {
      toast({
        title: 'Failed to approve job',
        description: error.value?.message,
        variant: 'destructive'
      })
    }
    if (status.value === "success") {
      toast({
        title: 'Job approved successfully',
        description: 'The job has been approved successfully',
      })
      window.location.reload()
    }
  } catch (error) {
    console.error('Failed to approve job:', error)
  }
}
const reject = async () => {
  try {
    const {status, error} = await useAPI('/job/edit', {
      method: 'PUT',
      body: {
        ...props.job,
        status: "IN_PROGRESS"
      }
    })
    if (status.value === "error") {
      toast({
        title: 'Failed to reject job',
        description: error.value?.message,
        variant: 'destructive'
      })
    }
    if (status.value === "success") {
      toast({
        title: 'Job rejected successfully',
        description: 'The job has been rejected successfully',
      })
      window.location.reload()
    }
  } catch (error) {
    console.error('Failed to reject job:', error)
  }
}
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
            <Dialog>
              <DialogTrigger><Button variant="outline">Edit</Button></DialogTrigger>
              <DialogContent>
                <DialogHeader>
                  <DialogTitle>Are you sure absolutely sure?</DialogTitle>
                  <DialogDescription>
                    Edit the job details
                  </DialogDescription>
                </DialogHeader>
              </DialogContent>
            </Dialog>
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
          <JobsAssignDialog v-if="props.job?.assigneeNames?.length === 0" @assign="assignLinguists" />
          <Button variant="outline" :disabled="props.job?.status !== 'SUBMITTED'" @click="approve">Approve</Button>
          <Button variant="outline" :disabled="props.job?.status !== 'SUBMITTED'" @click="reject">Reject</Button>
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