<script lang="ts" setup>
import type { Discount } from '~/types/discount';
import { columns } from '~/components/Discounts/columns';

useSeoMeta({
  title: 'Discounts',
})

definePageMeta({
    layout: 'default',
})


const { data: discountData, error: discountError } = await useAsyncData(
  'discountData',
  () => $fetch('http://localhost:8000/api/discount')
)

if (discountError.value) {
  console.error(
    'Failed to fetch data:',
    discountError.value
  )
}


const discounts: Discount[] = [
  {
    discountId: 1,
    discountName: "DISCOUNT001",
    discountPercent: 12
  },
  {
    discountId: 2,
    discountName: "DISCOUNT002",
    discountPercent: 22
  },
  {
    discountId: 3,
    discountName: "DISCOUNT003",
    discountPercent: 32
  },
  {
    discountId: 4,
    discountName: "DISCOUNT004",
    discountPercent: 42
  },
]

let data = [];

if(discountData){
  data = discountData.value;
}else{
  data = discounts;
}

</script>

<template>
  <DiscountsTable :columns="columns" :data="data"/>
</template>

<style></style>
