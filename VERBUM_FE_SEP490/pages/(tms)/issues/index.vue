<script setup lang="ts">
middleware: false
</script>

<template>
  <div class="flex flex-col space-y-4">
    <h1 class="text-2xl font-semibold">Active Issues</h1>
    <IssuesCarousel />
    <h1 class="text-2xl font-semibold">All issues</h1>
    <IssuesTable />
  </div>
</template>
