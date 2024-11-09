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
import { repo } from '~/utils/repo'

const props = defineProps<{
  orders: Order[]
}>()

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString()
}

const toDetails = (orderId: string) => {
  useRouter().push('/orders/details/' + orderId)
}
const toCreate = () => {
  useRouter().push('/orders/create')
}

const searchValue = ref('')

const ordersRepo = repo(useNuxtApp().$api)

const searchOrders = async (value: string) => {
  const orders = await ordersRepo.searchOrders(value)
  emit('update:orders', orders)
}

const emit = defineEmits<{
  'update:orders': [orders: Order[]]
}>()

const currentPage = ref(1)
const pageSize = ref(10)
const totalOrders = ref(0)
const fetchOrders = async () => {
  const response = await ordersRepo.getOrders(currentPage.value, pageSize.value)
  emit('update:orders', response)
  totalOrders.value = response.length
}

watch([currentPage, pageSize], fetchOrders, { immediate: true })
</script>

<template>
  <div>
    <div class="flex justify-between space-x-4 pb-4">
      <Input v-model="searchValue" placeholder="Search orders" @keydown.enter="searchOrders(searchValue)" />
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
              {{ formatDate(order.createdDate ?? '') }}
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>

    <div class="pagination flex items-center justify-center space-x-4 mt-4">
      <Button variant="outline" :disabled="currentPage === 1" @click="currentPage--">Previous</Button>
      <span>Page {{ currentPage }} of {{ Math.ceil(totalOrders / pageSize) }}</span>
      <Button variant="outline" :disabled="currentPage * pageSize >= totalOrders" @click="currentPage++">Next</Button>
    </div>
  </div>
</template>
