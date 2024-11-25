<script setup lang="ts">

useSeoMeta({
  title: 'Jobs'
})

const { jobs, getJobs } = useJobs()
const { assignList, getAssignList } = useUsers()
const role = useAuthStore().user?.role
onMounted(() => {
    if (!jobs.value.length) {
        getJobs()
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

<style scoped></style>
