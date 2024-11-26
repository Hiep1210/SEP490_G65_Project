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
    <JobsSkeleton v-if="isLoading" />  
    <JobsDetails v-else :job="job" :role="role" />
  </div>
</template>

<style>

</style>