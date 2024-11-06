<script setup lang="ts">
// middleware: false

import { useIssues } from '~/composables/useIssues'
import type { Issue } from '~/types/issues'

const { issues, getIssues, updateIssue } = useIssues()
const { user } = useAuthStore()
const currentUserRole = user?.role

onMounted(() => {
  if (!issues.value.length) {
    getIssues()
  }
})

const handleUpdate = async (updateIssues: Issue) => {
  await updateIssue(updateIssues)
  await getIssues()
}
</script>

<template>
  <div class="flex flex-col space-y-4">
    <h1 class="text-2xl font-semibold">Active Issues</h1>
    <IssuesCarousel :issues="issues" :role="currentUserRole"/>
    <h1 class="text-2xl font-semibold">All issues</h1>
    <IssuesTable
      :issues="issues"
      :role="currentUserRole"
      @update="handleUpdate"
    />
  </div>
</template>
