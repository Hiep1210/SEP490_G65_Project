<script lang="ts" setup>
import { ref } from 'vue'
import type { Discount } from '~/types/discount'
import { columns } from '~/components/Discounts/columns'
import { Button } from '@/components/ui/button'
const token = useCookie('access_token')

useSeoMeta({
  title: 'Discounts'
})

definePageMeta({
  layout: 'default'
})

// Fetch discounts from API
const { data: discountData, error: discountError } = await useAsyncData<
  Discount[]
>('discountData', () =>
  $fetch('http://localhost:8000/api/discount', {
    headers: {
      Authorization: `Bearer ${token.value}`
    }
  })
)

if (discountError.value) {
  console.error('Failed to fetch data:', discountError.value)
}

console.log(discountData)

// Initialize data with fetched discounts or fallback data
const data = ref<Discount[]>(discountData.value || [])

// Track dialog state
const isDialogOpen = ref(false)

// Function to refresh discount data
const refreshDiscounts = async () => {
  try {
    const response: Discount[] = await $fetch(
      'http://localhost:8000/api/discount',
      {
        headers: {
          Authorization: `Bearer ${token.value}`
        }
      }
    )
    data.value = response // Update the data with the latest from the API
  } catch (error) {
    console.error('Failed to refresh discounts:', error)
  }
}

// Handle creating a new discount
const handleCreateDiscount = async (newDiscount: Discount) => {
  try {
    await $fetch('http://localhost:8000/api/discount', {
      headers: {
        Authorization: `Bearer ${token.value}`
      },
      method: 'POST',
      body: {
        ...newDiscount,
        isUpdate: true // Add the required `isUpdate` flag
      }
    })

    // Refresh the discounts after successful creation
    await refreshDiscounts()

    isDialogOpen.value = false // Close the dialog
  } catch (error) {
    console.error('Failed to create discount:', error)
    alert('Failed to create discount. Please try again.')
  }
}

// Handle dialog close
const closeDialog = () => {
  isDialogOpen.value = false
}
</script>

<template>
  <div>
    <Button 
    class="mr-16 my-3 float-right"
    @click="isDialogOpen = true"
    >Create Discount
  </Button>

    <DiscountsTable :columns="columns" :data="data" />

    <!-- Create Discount Dialog -->
    <DiscountsCreateDialog
      :open="isDialogOpen"
      @close="closeDialog"
      @create="handleCreateDiscount"
    />
  </div>
</template>
