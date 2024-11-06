<script setup lang="ts">
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table'
import type { Order } from '~/types/order'
import { ref } from 'vue'

const props = defineProps<{
  orders: Order[]
}>()

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString()
}

const selectedOrders = ref<string[]>([])

const toggleOrderSelection = (orderId: string) => {
  if (selectedOrders.value.includes(orderId)) {
    selectedOrders.value = selectedOrders.value.filter((id) => id !== orderId)
  } else {
    selectedOrders.value.push(orderId)
  }
}

const toggleAllOrders = (checked: boolean) => {
  if (checked) {
    selectedOrders.value = props.orders.map((order) => order.orderId)
  } else {
    selectedOrders.value = []
  }
}

const toDetails = (orderId: string) => {
  useRouter().push('/orders/details/' + orderId)
}
const toCreate = () => {
  useRouter().push('/orders/create')
}
</script>

<template>
  <div>
    <div class="flex justify-between space-x-4 pb-4">
      <Input placeholder="Search orders" />
      <Button variant="outline" @click="toCreate">Create an Order</Button>
    </div>

    <div class="border rounded-lg overflow-hidden">
      <div v-if="orders.length === 0" class="text-center">
        <span>
          <p class="text-lg font-semibold">
            There are no orders to display. What about creating one?
          </p>
        </span>
      </div>
      <Table v-else>
        <TableHeader>
          <TableRow>
            <TableHead class="w-[100px]">#</TableHead>
            <TableHead>Name</TableHead>
            <TableHead>Status</TableHead>
            <TableHead class="text-center">Created At</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow v-for="(order, index) in props.orders" :key="order.orderId" @click="toDetails(order.orderId)">
            <TableCell class="font-medium">{{ index + 1 }}</TableCell>
            <TableCell>{{ order.orderName }}</TableCell>
            <TableCell>{{ order.orderStatus }}</TableCell>
            <TableCell class="text-center">
              {{ formatDate(order.createdDate) }}
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>
  </div>
</template>
