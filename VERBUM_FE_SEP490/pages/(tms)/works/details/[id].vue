<script setup lang="ts">
import { getWorkBadgeClass } from '~/utils/getBadgeClass';

const { isLoading, work, getWorkById } = useWorks()
const { jobs, getJobsOfWork } = useJobs()
const { assignList, getAssignList } = useUsers()
const role = useAuthStore().user?.role as string | undefined
const route = useRoute()
const workId = route.params.id as string
onMounted(async () => {
    await getJobsOfWork(workId)
    await getWorkById(workId)
    if (role?.includes('MANAGER')) {
        await getAssignList()
    }
})

provide('assignList', assignList)
</script>

<template>
    <LoadingSpinner v-if="isLoading"/>
    <div v-else>
        <div v-if="work" class="mb-4">
            <h1 class="text-4xl font-bold mb-4">Works Details</h1>
            <div class="flex space-x-4">
                <div class="space-y-2">
                    <p class="text-3xl text-primary font-semibold">{{ work?.workName }}</p>
                    <Badge :class="getWorkBadgeClass(work?.orderStatus)">{{ work?.orderStatus }}</Badge>
                    <span class="text-3xl font-semibold flex space-x-1"><p>Due date:</p><p class="font-normal">{{ work?.dueDate }}</p>
                    </span>
                </div>
            </div>
        </div>
        <WorksStatusColumn :data="jobs" />
    </div>
</template>