<script lang="ts" setup>
import { getJobBadgeClass } from '@/utils/getBadgeClass'
import type { Job } from '~/types/job'

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
</script>

<template>
  <Dialog>
    <DialogTrigger as-child>
      <slot />
    </DialogTrigger>
    <DialogContent>
      <DialogHeader>
        <DialogTitle>{{ props.job?.name }}</DialogTitle>
        <div class="flex justify-between">
          <Badge :class="getJobBadgeClass(props.job?.status ?? '')">{{ props.job?.status }}</Badge>
        </div>
      </DialogHeader>
      <DialogDescription class="text-black">
        <div class="flex flex-col gap-2">
          <h1 class="text-2xl font-semibold">Details</h1>
        <div>
          <p v-if="props.job?.assigneeNames && props.job.assigneeNames.length > 0" class="text-sm">Assigned to: {{ props.job?.assigneeNames }}</p>
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
          <JobsAssignDialog />
          <Button variant="outline">Set Deadline</Button>
        </template>
        <template v-else>
          <Button variant="outline">Upload File</Button>
        </template>
      </DialogFooter>
    </DialogContent>

  </Dialog>
</template>

<style>

</style>