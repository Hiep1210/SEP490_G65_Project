<script setup lang="ts">
import { columns } from '~/components/Orders/column';

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
    <LazyOrdersTable :columns="columns" :data="orders" />
  </ClientOnly>
</template>
