<script setup lang="ts">
const { jobs, getJobsOfWork } = useJobs()

const route = useRoute()
const workId = route.params.id as string
onMounted(() => {
    if (!jobs.value.length) {
        getJobsOfWork(workId)
    }
})
</script>

<template>
    <div>
        <JobsStatusColumn :data="jobs"/>
    </div>
</template>

<style>

</style>