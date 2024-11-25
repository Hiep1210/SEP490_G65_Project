<script lang="ts" setup>

import { useReceipt } from '~/composables/useReceipt'

const {receipts, getReceipts} = useReceipt();
useSeoMeta({
  title: 'Receipts'
})

definePageMeta({
  layout: 'default'
})

onMounted(async () => {
  if(!receipts.value.length){
    getReceipts();
  }
})

</script>

<template>
  <div class="">
     <ReceiptsTable :receipts="receipts"/>
  </div>
</template>
