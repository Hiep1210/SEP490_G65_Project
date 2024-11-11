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
  order: Order,
  clientId: string,
  open: boolean
  status: string
}>()

const emit = defineEmits(['close', 'confirm']) // Emit update event
const isOpen = ref(props.open)
const price = props.order.orderPrice || '0'
const deposit = 1 / 2
const priceDeposit = Number(price) * deposit
const payRemaining = Number(price) - priceDeposit
const pricePay = ref()

if (props.status === 'IN_PROGRESS') {
  pricePay.value = priceDeposit
} else {
  pricePay.value = payRemaining
}

// console.log({payFinish})

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
    <DialogContent class="h-4/6 max-w-[425px]">
      <DialogHeader>
        <DialogTitle> Payment Detail </DialogTitle>
        <DialogDescription
          v-if="status === 'IN_PROGRESS'"
          class="text-justify italic"
          >To proceed with your order, a deposit is required. The remaining
          balance will be due once your order is completed. This two-step
          payment process ensures a smooth and secure
          transaction.</DialogDescription
        >
        <DialogDescription
          v-if="status !== 'IN_PROGRESS'"
          class="text-justify italic"
          >Your order is now complete! To finalize, please proceed with the
          remaining payment balance. Thank you for choosing us – we appreciate
          your trust and look forward to serving you again on your next
          order</DialogDescription
        >
        <Button
          variant="ghost"
          class="absolute top-2 right-2"
          @click="closeDialog"
        />
      </DialogHeader>
      <div class="h-full overflow-y-scroll">
        <div>
          <hr >
          <div class="flex">
            <div class="flex-auto">
              <p class="my-2">Order name:</p>
              <p class="my-2">Service:</p>
            </div>
            <div class="flex-auto">
              <p class="my-2">{{ order.orderName }}</p>
              <p>
                <span v-if="order.hasTranslateService" class="my-2"
                  >Translate
                </span>
                <span v-if="order.hasEditService" class="my-2"> Edit </span>
                <span v-if="order.hasEvaluateService" class="my-2">
                  Evaluate
                </span>
              </p>
            </div>
          </div>
          <hr >
          <div class="flex">
            <div class="flex-auto">
              <p class="my-3">Total prices:</p>
              <p 
              class="my-1" 
              :class=" status === 'IN_PROGRESS' ? ' my-3 font-semibold text-xl' : ''"
            >Deposit ({{ deposit*100 }}%):</p>
              <p 
              class="my-1"
              :class=" status !== 'IN_PROGRESS' ? ' my-3 font-semibold text-xl' : ''"
              >Remaining:</p>
            </div>
            <div class="flex-auto">
              <p class="my-1">${{ order.orderPrice }}</p>
              <p
                class="my-1"
                :class="
                  status === 'IN_PROGRESS' ? ' my-3 font-semibold text-2xl' : ''
                "
              >
                ${{ priceDeposit }}
              </p>
              <p
                class="my-1"
                :class="
                  status !== 'IN_PROGRESS' ? ' my-3 font-semibold text-2xl' : ''
                "
              >
                ${{ payRemaining }}
              </p>
            </div>
          </div>
          <div class="mb-10 text-center">
            <p
              v-if="status === 'IN_PROGRESS'"
              class="my-1 italic text-red-950 text-sm "
            >
              You are paying the deposit!
            </p>
            <p
              v-if="status !== 'IN_PROGRESS'"
              class="my-1 italic text-red-950 text-sm"
            >
              You are paying the remaining!
            </p>
          </div>
        </div>
        <PaymentPaypalButton
          :order-id="order.orderId"
          :client-id="clientId"
          :price="pricePay"
          :status="status"
          @payment-success="closeDialog"
        />
      </div>
      <DialogFooter>
        <Button class="bg-slate-500 hover:bg-slate-600" @click="$emit('close')"
          >Cancel Payment</Button
        >
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>
