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
        title: 'Linguists assigned successfully',
        description: 'Linguists have been assigned to the job',
      })
      window.location.reload()
    } else {
      toast({
        title: 'Failed to assign linguists',
        description: 'Please try again later',
        variant: 'destructive',
      })
    }
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
          {{props.job?.name }}
        </h1>
        <Badge :class="getJobBadgeClass(props.job?.status ?? '')">{{ props.job?.status }}</Badge>
      </div>
      <JobsEditDialog v-if="canEdit">
        <Button variant="outline">Edit</Button>
      </JobsEditDialog>
    </header>

    <section class="mb-6">
      <div class="mt-4 space-y-2">
        <div>
          <p v-if="props.job && props.job?.assigneeNames?.length > 0" class="">
            Assigned to: {{ props.job?.assigneeNames.map((assignee) => assignee.name).join(', ') }}
          </p>
          <p>Target Language: {{ props.job?.targetLanguageId }}</p>
          <p v-if="props.job?.workDueDate">Work's Due Date: {{ formatToVietnamTimezone(props.job?.workDueDate) }}</p>
          <p v-if="props.job?.dueDate" class="">Due Date: {{ formatToVietnamTimezone(props.job?.dueDate) }}</p>
          <p v-if="props.job?.createdAt" class="">Created At: {{ formatToVietnamTimezone(props.job?.createdAt) }}</p>
          <p v-if="props.job?.updatedAt" class="">Updated At: {{ formatToVietnamTimezone(props.job?.updatedAt) }}</p>
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
          variant="outline" 
          :disabled="(props.job?.status !== 'SUBMITTED' )|| !props.job"
          @click="approve(props.job)">
          Approve
        </Button>
        <Button 
        variant="outline" 
        :disabled="props.job?.status !== 'SUBMITTED' || !props.job"
          @click="reject(props.job)">
          Reject
        </Button>
      </template>
      <template v-else>
        <JobsUploadFileDialog :job="props.job">
          <Button variant="outline" :disabled="!canUploadFile">Upload File</Button>
        </JobsUploadFileDialog>
      </template>
    </section>
    <section v-if="props.job" class="mb-6">
      <JobsTabs :job="props.job" />
    </section>
  </div>
</template>
