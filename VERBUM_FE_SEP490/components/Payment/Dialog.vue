<script lang="ts" setup>
import { ref, watch, defineEmits } from 'vue'
import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle
} from '@/components/ui/dialog'
import type { Order } from '~/types/order'

const props = defineProps<{
  order: Order
  open: boolean
}>()

const emit = defineEmits(['close', 'confirm']) // Emit update event
const isOpen = ref(props.open)
const price = props.order.orderPrice || "0"

watch(
  () => props.open,
  (newVal) => {
    isOpen.value = newVal
  }
)

const closeDialog = () => {
  emit('close') // Emit close event
}
</script>

<template>
  <Dialog :open="isOpen" @click-outside="closeDialog" @close="closeDialog">
    <DialogContent class="h-screen overflow-y-scroll max-w-[425px]">
      <DialogHeader>
        <DialogTitle> Payment Detail </DialogTitle>
        <DialogDescription
          >You are paying for order: {{ order.orderName }}</DialogDescription
        >
        <Button
          variant="ghost"
          class="absolute top-2 right-2"
          @click="closeDialog"
        />
      </DialogHeader>

      <div>
        <hr >
        <div class="flex">
          <div class="flex-auto">
            <p class="my-3">Order name:</p>
            <p class="my-3">Service:</p>
          </div>
          <div class="flex-auto">
            <p class="my-3">{{ order.orderName }}</p>
            <p v-if="order.hasTranslateService" class="my-3">Translate</p>
            <p v-if="order.hasEditService" class="my-3">Edit</p>
            <p v-if="order.hasEvaluateService" class="my-3">Evaluate</p>
          </div>
        </div>
        <hr >
        <div class="flex">
          <div class="flex-auto">
            <p class="my-3 pt-1">Total prices:</p>
          </div>
          <div class="flex-auto">
            <p class="my-3 font-semibold text-2xl">
              {{ order.orderPrice }} USD
            </p>
          </div>
        </div>
      </div>
      <PaymentPaypalButton :order-id="order.orderId" :price="price" @payment-success="closeDialog"/>
      <DialogFooter>
        <Button class="bg-slate-500 hover:bg-slate-600" @click="$emit('close')"
          >Cancel Payment</Button
        >
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>
