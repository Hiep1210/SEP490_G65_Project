<script setup lang="ts">
useSeoMeta({
  title: 'Orders'
})
definePageMeta({
  layout: 'default'
})

const { orders, getOrders } = useOrders()
const totalOrders = ref(0)

// onMounted(async () => {
//   if (!orders.value.length) {
//     await getOrders()
//   }
//   totalOrders.value = orders.value.length
// })

// watch(orders, (newOrders) => {
//   totalOrders.value = newOrders.length
// }, { immediate: true })


</script>

<template>
  <ClientOnly>
    <LazyOrdersTable :orders="orders" @update:orders="orders = $event" />
  </ClientOnly>
</template>
