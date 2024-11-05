<script setup lang="ts">
const { jobs, getJobs } = useJobs()

onMounted(() => {
    if (!jobs.value.length) {
        getJobs()
    }
})
</script>

<template>
    <div>
        <JobsStatusColumn :data="jobs"/>
    </div>
</template>

<style scoped></style>
