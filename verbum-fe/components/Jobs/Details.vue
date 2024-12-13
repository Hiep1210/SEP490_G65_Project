<script lang="ts" setup>
import { getJobBadgeClass } from '@/utils/getBadgeClass'
import type { Job } from '~/types/job'
import { useToast } from '@/components/ui/toast/use-toast'
import { formatToVietnamTimezone } from '#imports';

const props = defineProps<{
  job?: Job | undefined
  role?: string
}>()

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
  const isStatusValid = !['SUBMITTED', 'APPROVED'].includes(props.job.status)

  return hasRequiredFields && isPermitted && isStatusValid
})

const { toast } = useToast()
const assignLinguists = async (payload: { assigneesId: string[]; dueDate: string }) => {
  const assignPayload = {
    id: props.job?.id,
    name: props.job?.name,
    status: "IN_PROGRESS",
    assigneesId: payload.assigneesId,
    dueDate: payload.dueDate,
  }
  try {
    const res = await repo(useNuxtApp().$api).assignLinguists(assignPayload)
    if (!res) {
      toast({
        title: 'Failed to assign linguists',
        description: 'Please try again later',
        variant: 'destructive',
      })
    } 
    toast({
      title: 'Linguists assigned',
      description: 'The job has been assigned to the linguists',
    })
    window.location.reload()
  } catch (error) {
    console.error('Failed to assign linguists:', error)
  }
}
const { approve, reject } = useJobs()
</script>

<template>
  <div class="container mx-auto p-4">
    <header class="mb-6 flex justify-between">
      <div class="space-y-2">
        <h1 class="text-3xl font-semibold text-primary">
          {{getJobName(props.job?.name ?? '')}}
        </h1>
        <Badge :class="getJobBadgeClass(props.job?.status ?? '')">{{ props.job?.status }}</Badge>
      </div>
      <JobsEditDialog v-if="canEdit" :work-due-date="props.job?.workDueDate ?? '' " :assigned-linguists="props.job?.assigneeNames ?? []" :old-due-date="props.job?.dueDate ?? ''"  @edit="assignLinguists">
        <Button variant="outline">Edit</Button>
      </JobsEditDialog>
    </header>

    <section class="mb-6 border rounded p-4">
      <div class="flex flex-col gap-2">
        <div class="space-y-2">
          <h1 v-if="props.job && props.job?.rejectReason?.length > 0" class="font-semibold text-red-500">
            Reject reason: <span class="font-normal">{{ props.job?.rejectReason }}</span>
          </h1>
          <h1 v-if="props.job && props.job?.assigneeNames?.length > 0" class="font-semibold">
            Assigned to: <span class="font-normal">{{ props.job?.assigneeNames.map((assignee) => assignee.name).join(', ') }}</span>
          </h1>
          <h1 class="font-semibold">Target Language: <span class="font-normal">{{ props.job?.targetLanguageId }}</span></h1>
          <h1 v-if="props.job?.workDueDate" class="font-semibold">Work's Due Date: <span class="font-normal">{{ formatToVietnamTimezone(props.job?.workDueDate) }}</span></h1>
          <h1 v-if="props.job?.dueDate" class="font-semibold">Due Date: <span class="font-normal">{{ formatToVietnamTimezone(props.job?.dueDate) }}</span></h1>
          <h1 v-if="props.job?.createdAt" class="font-semibold">Created At: <span class="font-normal">{{ formatToVietnamTimezone(props.job?.createdAt) }}</span></h1>
          <h1 v-if="props.job?.updatedAt" class="font-semibold">Updated At: <span class="font-normal">{{ formatToVietnamTimezone(props.job?.updatedAt) }}</span></h1>
        </div>
      </div>
    </section>

    <section class="flex justify-end gap-4 mb-4">
      <template v-if="props.role?.includes('MANAGER')">
        <JobsAssignDialog
          v-if="props.job?.assigneeNames?.length === 0 || (props.job?.status &&  ['NEW'].includes(props.job?.status))"
          :work-due-date="props.job?.workDueDate || ''"
          @assign="assignLinguists" />
        <Button 
          v-if="props.job?.status === 'SUBMITTED' || props.job?.status === 'APPROVED'"
          variant="outline" 
          :disabled="(props.job?.status !== 'SUBMITTED' )|| !props.job">
          Approve
        </Button>
        <JobsRejectDialog
          v-if="props.job?.status === 'SUBMITTED' || props.job?.status === 'APPROVED'"
          @reject="reject(job?.id, $event)" >
          <Button variant="outline" :disabled="(props.job?.status !== 'SUBMITTED' )">Reject</Button> 
        </JobsRejectDialog>
      </template>
      <template v-else>
        <JobsUploadFileDialog :job="props.job">
          <Button variant="outline" :disabled="!canUploadFile">Upload File</Button>
        </JobsUploadFileDialog>
      </template>
    </section>
    <section v-if="props.job" class="mb-6">
      <JobsTabs :job="props.job" :target-lang-id="job?.targetLanguageId" />
    </section>
  </div>
</template>
