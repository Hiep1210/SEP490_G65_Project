<script lang="ts" setup>
import { ref, watch, defineEmits } from 'vue';
import { Button } from '@/components/ui/button';
import type { Order } from '~/types/order';

const props = defineProps<{ 
    order: Order; 
    open: boolean;
    price: string; 
}>();
const emit = defineEmits(['close', 'confirm']);
const isOpen = ref(props.open);
const price = ref(props.price);

watch(() => props.open, (newVal) => {
    isOpen.value = newVal;
});

const closeDialog = () => {
    emit('close');
};

const confirmPrice = () => {
    if (price.value != null) {
        emit('confirm', price.value);
    }
};
</script>

<template>
  <Dialog :open="isOpen" @click-outside="closeDialog">
    <DialogContent>
      <DialogTitle>Set Prices for {{ order.orderName }}</DialogTitle>
      <input v-model="price" class="border p-1 w-full" type="number" >
      <DialogFooter>
        <Button @click="closeDialog">Cancel</Button>
        <Button @click="confirmPrice">Update</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>
