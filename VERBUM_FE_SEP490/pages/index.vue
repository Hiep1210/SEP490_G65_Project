<script setup lang="ts">
definePageMeta({
  title: 'Home',
  layout: false
})

onMounted(async () => {})
</script>
<template>
  <div><Button @click.prevent="useAuth().logout()">Click me</Button></div>
</template>
