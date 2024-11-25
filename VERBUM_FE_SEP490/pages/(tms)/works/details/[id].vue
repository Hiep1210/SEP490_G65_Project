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
        <h1 class="text-2xl font-bold">Works Details</h1>
        <h2 class="text-xl font-semibold">Work's Name</h2>
        <WorksStatusColumn :data="jobs"/>
    </div>
</template>

<style>

</style>