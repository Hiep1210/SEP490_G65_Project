<script lang="ts">
const route = useRoute()
const orderId = route.params.orderId
</script>
<template>
  <IssuesCreateForm :orderId="orderId" />
</template>
