<script setup lang="ts">
// middleware: false

import { useIssues } from '~/composables/useIssues'

const { issues, getIssues } = useIssues()

onMounted(() => {
  if (!issues.value.length) {
    getIssues()
  }
  console.log({ issues })
})
</script>

<template>
  <div class="flex flex-col space-y-4">
    <h1 class="text-2xl font-semibold">Active Issues</h1>
    <IssuesCarousel :issues="issues" />
    <h1 class="text-2xl font-semibold">All issues</h1>
    <IssuesTable :issues="issues"/>
  </div>
</template>
