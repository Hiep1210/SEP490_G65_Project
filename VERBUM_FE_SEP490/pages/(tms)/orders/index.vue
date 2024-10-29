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
        getOrders()
    }
})

watch(orders, newOrders => {
    console.log('newOrders', newOrders)
})
provide('orders', orders)
</script>

<template>
    <ClientOnly>
        <LazyOrdersTable :orders="orders" />
    </ClientOnly>
</template>
