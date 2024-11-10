<script setup lang="ts">
const { jobs, getJobsOfWork } = useJobs()
const {assignList, getAssignList} = useUsers()
const role = useAuthStore().user?.role as string | undefined
const route = useRoute()
const workId = route.params.id as string
onMounted(() => {
    if (!jobs.value.length) {
        getJobsOfWork(workId)
    }
    if (role?.includes('MANAGER')) {
        getAssignList()
    }
})

provide('assignList', assignList)
</script>

<template>
    <div>
        <JobsStatusColumn :data="jobs"/>
    </div>
</template>

<style>

</style>