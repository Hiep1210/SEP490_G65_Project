<script setup lang="ts">
useSeoMeta({
  title: 'Orders'
})
definePageMeta({
  layout: 'default'
})

const { orders, getOrders } = useOrders()

onMounted(async () => {
  if (!orders.value.length) {
    await getOrders()
  }
})


</script>

<template>
  <ClientOnly>
    <LazyOrdersTable :orders="orders" @update:orders="orders = $event" />
  </ClientOnly>
</template>
