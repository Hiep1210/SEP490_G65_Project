<script setup>
import DualListBox from '~/components/Languages/DualListBox.vue';
import { useToast } from '~/components/ui/toast';

useSeoMeta({
  title: 'Languages',
})

definePageMeta({
    layout: 'default',
})

const { data: availableData, error: availableError } = await useAsyncData(
  'availableData',
  () => $fetch('http://localhost:8000/api/lang')
)

const { data: selectedData, error: selectedError } = await useAsyncData(
  'selectedData',
  () => $fetch('http://localhost:8000/api/lang/support')
)

if (availableError.value || selectedError.value) {
  console.error(
    'Failed to fetch data:',
    availableError.value || selectedError.value
  )
}
const selects = [];
const availableItemsList = availableData.value || []
const selectedItemsList = selectedData.value || []

const handleSave = async () => {
  const {toast} = useToast();
  if (!selects || selects.length === 0) {
    return
  }

  const updatedItems = selects.map((item) => ({
    languageId: item.languageId,
    support: item.support
  }))


  try {
    await $fetch('http://localhost:8000/api/lang/support', {
      method: 'PUT',
      body: updatedItems,
      headers: {
        'Content-Type': 'application/json'
      }
    })
    toast({
      title: "Update Successfully!! ",
    })
  } catch (error) {
    console.error('Error while updating data:', error)
  }
  selects.length = 0
}
</script>

<template>
  <div>
    <DualListBox
      title-available-items="Unsupported Languages"
      title-selected-items="Supported Languages"
      :selects="selects"
      :available-items-list="availableItemsList"
      :selected-items-list="selectedItemsList"
      @save="handleSave"
    />
    <Toaster />
  </div>
</template>
