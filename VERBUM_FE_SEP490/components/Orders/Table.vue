<script setup lang="ts">
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
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
    selectedOrders.value = selectedOrders.value.filter(id => id !== orderId)
  } else {
    selectedOrders.value.push(orderId)
  }
}

const cancelSelectedOrders = () => {
  if (selectedOrders.value.length === 0) {
    alert("Please select at least one order to cancel.")
    return
  }

  alert(`Cancelling orders: ${selectedOrders.value.join(', ')}`)

  selectedOrders.value = []
}

const toggleAllOrders = (checked: boolean) => {
  if (checked) {
    selectedOrders.value = props.orders.map(order => order.id)
  } else {
    selectedOrders.value = []
  }
}

const toDetails = (orderId: string) => {
  useRouter().push("/orders/details/" + orderId)
  console.log(orderId)
}
</script>

<template>
  <div>
    <div class="flex justify-between space-x-4 pb-4">
      <Input placeholder="Search orders" />
      <Button variant="outline" @click="cancelSelectedOrders">Cancel Orders</Button>
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
            <TableHead class="w-[50px] text-center">
              <Checkbox 
                :checked="selectedOrders.length === props.orders.length"
                @update:checked="toggleAllOrders($event)" />
            </TableHead>
            <TableHead class="w-[100px]">ID</TableHead>
            <TableHead>Name</TableHead>
            <TableHead>Status</TableHead>
            <TableHead class="text-center">Created At</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow v-for="order in props.orders" :key="order.id" @click="toDetails(order.id)">
            <TableCell class="text-center">
              <Checkbox 
                :value="order.id" :checked="selectedOrders.includes(order.id)"
                @update:checked="toggleOrderSelection(order.id)" />
            </TableCell>
            <TableCell class="font-medium">{{ order.id }}</TableCell>
            <TableCell>{{ order.name }}</TableCell>
            <TableCell>{{ order.status }}</TableCell>
            <TableCell class="text-center">
              {{ formatDate(order.createAt) }}
            </TableCell>
            <TableCell>
              <Button variant="ghost" size="sm">
                ...
              </Button>
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>
  </div>
</template>
