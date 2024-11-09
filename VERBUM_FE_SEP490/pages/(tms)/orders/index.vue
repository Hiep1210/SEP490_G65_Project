<script setup lang="ts">
useSeoMeta({
  title: 'Orders'
})
definePageMeta({
  layout: 'default'
})

const { orders, getOrders } = useOrders()

onMounted(() => {
  if (!orders.value.length) {
    getOrders(1)
  }
})

</script>

<template>
  <ClientOnly>
    <LazyOrdersTable :orders="orders" @update:orders="orders = $event" />
  </ClientOnly>
</template>
