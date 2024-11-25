<script setup lang="ts">
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { formatDistanceToNow } from 'date-fns';
import { ref, computed, watch } from 'vue';
import type { Receipt } from '~/types/receipts';

import {
  Pagination,
  PaginationEllipsis,
  PaginationFirst,
  PaginationLast,
  PaginationList,
  PaginationListItem,
  PaginationNext,
  PaginationPrev,
} from '@/components/ui/pagination';
import { Button } from '@/components/ui/button';

const searchTerm = ref('');
const currentPage = ref(1);
const itemsPerPage = ref(10);



const props = defineProps<{
  receipts: Receipt[];
}>();

const receipts = ref(props.receipts);

const filteredReceipts = computed(() => {
  const search = searchTerm.value.toLowerCase();
  return receipts.value.filter((receipt) =>
  receipt.orderName.toLowerCase().includes(search)
  );
});

const paginatedReceipts = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage.value;
  const end = start + itemsPerPage.value;
  return filteredReceipts.value.slice(start, end);
});

const totalPages = computed(() =>
  Math.ceil(filteredReceipts.value.length / itemsPerPage.value)
);

const goToPage = (page: number) => {
  currentPage.value = page;
};

const getStatus = (depositOrPayment : boolean) => {
    return depositOrPayment ? 'Deposit' : "Payment"
}


watch(
  () => props.receipts,
  (newList) => {
    receipts.value = [...newList];
  },
  { deep: true }
);

</script>

<template>
  <div>
    <!-- Search Bar -->
    <div class="mb-4">
      <Input
        v-model="searchTerm"
        type="text"
        placeholder="Search by receipt name"
        class="input input-bordered w-full"
      />
    </div>

    <!-- Table -->
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Order Name</TableHead>
          <TableHead>Pay Date</TableHead>
          <TableHead>Status</TableHead>
          <TableHead>Prices</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        <TableRow
          v-for="receipt in paginatedReceipts"
          :key="receipt.receiptId"
        >
          <TableCell class="hyper-link">
            <NuxtLink :to="`/orders/details/${receipt.orderId}`">
              {{ receipt.orderName }}
            </NuxtLink>
          </TableCell>
          <TableCell>
            {{
              formatDistanceToNow(new Date(receipt.payDate), { addSuffix: true })
            }}
          </TableCell>
          <TableCell>
            <Badge :class="getReceiptBadgeClass(getStatus(receipt.depositeOrPayment))">
              {{ getStatus(receipt.depositeOrPayment) }}
            </Badge>
          </TableCell>
          <TableCell>
              {{ receipt.amount }}
          </TableCell>
        </TableRow>
      </TableBody>
    </Table>

    <div class="flex justify-end">
      <Pagination
      v-model="currentPage"
      :total="totalPages"
      :sibling-count="1"
      :items-per-page="itemsPerPage"
      show-edges
    >
      <PaginationList v-slot="{ items }" class="flex items-center gap-1 mt-4">
        <PaginationFirst />
        <PaginationPrev />

        <template v-for="(item, index) in items">
          <PaginationListItem
            v-if="item.type === 'page'"
            :key="index"
            :value="item.value"
            as-child
          >
            <Button
              class="w-10 h-10 p-0"
              :variant="item.value === currentPage ? 'default' : 'outline'"
              @click="goToPage(item.value)"
            >
              {{ item.value }}
            </Button>
          </PaginationListItem>
          <PaginationEllipsis v-else :key="item.type" :index="index" />
        </template>

        <PaginationNext />
        <PaginationLast />
      </PaginationList>
    </Pagination>
    </div>
  </div>

</template>
