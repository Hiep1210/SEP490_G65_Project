<script setup lang="ts">
import type { Order } from '~/types/order'
import { columns } from '~/components/Orders/columns'

useSeoMeta({
  title: 'Orders'
})
definePageMeta({
  layout: 'default'
})

const orders: Order[] = [
  {
    id: 1,
    name: 'Order 1',
    status: 'Pending',
    createAt: new Date().toISOString()
  },
  {
    id: 2,
    name: 'Order 2',
    status: 'Completed',
    createAt: new Date().toISOString()
  },
  {
    id: 3,
    name: 'Order 3',
    status: 'Pending',
    createAt: new Date().toISOString()
  }
]
</script>

<template>
  <OrdersDataTable :columns="columns" :data="orders" />
</template>
