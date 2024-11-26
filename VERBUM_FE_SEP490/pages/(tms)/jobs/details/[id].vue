<script lang="ts" setup>
const route = useRoute()
const jobId = route.params.id as string

const { isLoading, job, getJobsDetail } = useJobs()
const { assignList, getAssignList } = useUsers()
const role = useAuthStore().user?.role as string | undefined

onMounted(() => {
  getJobsDetail(jobId)
  if (role?.includes('MANAGER')) {
    getAssignList()
  }
})
provide('assignList', assignList)
</script>
<template>
  <div>
      <div v-if="isLoading" class="space-y-2">
        <Skeleton class="h-[10rem] w-full" />
      </div>
    <JobsDetails v-else :job="job" :role="role" />
  </div>
</template>

<style>

</style>