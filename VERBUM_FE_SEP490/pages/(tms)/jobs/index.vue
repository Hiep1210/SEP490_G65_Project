<script setup lang="ts">
import { getColumns } from '~/components/Jobs/column';
useSeoMeta({
  title: 'Jobs'
})

const { jobs, getJobs } = useJobs()
const { assignList, getAssignList } = useUsers()
const role = useAuthStore().user?.role

const columns = getColumns(role)

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
        <JobsTable :columns="columns" :data="jobs" />
    </div>
</template>

<style scoped></style>
