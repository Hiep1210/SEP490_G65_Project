<script setup lang="ts">
// middleware: false

import { useIssues } from '~/composables/useIssues'
import type { Issue } from '~/types/issues'

const { issues, getIssues, updateIssue, updateIssueStatus } = useIssues()

onMounted(() => {
  if (!issues.value.length) {
    getIssues()
  }
})

const handleUpdate = async (updateIssues: Issue) => {
  await updateIssue(updateIssues)
  await getIssues()
}

const handleUpdateStatus = async (issuesId: string, status: string) => {
  await updateIssueStatus(issuesId, status)
}
</script>

<template>
  <div class="flex flex-col space-y-4">
    <h1 class="text-2xl font-semibold">Active Issues</h1>
    <IssuesCarousel :issues="issues" />
    <h1 class="text-2xl font-semibold">All issues</h1>
    <IssuesTable
      :issues="issues"
      @update="handleUpdate"
      @update-status="handleUpdateStatus"
    />
  </div>
</template>
