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
// import { usePayment } from '~/composables/usePayment'
const props = defineProps<{
  order: Order
  open: boolean
  status: string
}>()
// const {payWithPayPal} = usePayment();
const emit = defineEmits(['close', 'confirm']) // Emit update event
const isOpen = ref(props.open)
const price = props.order.orderPrice || '0'
const deposit = 1 / 2
const priceDeposit = Number(price) * deposit
const payRemaining = Number(price) - priceDeposit
const pricePay = ref()
const isDeposit = ref()

if (props.status === 'IN_PROGRESS') {
  pricePay.value = priceDeposit
  isDeposit.value = true
} else if(props.status === 'DELIVERED') {
  pricePay.value = payRemaining
  isDeposit.value = false
}
const { successPayment } = useOrders()
// console.log({payFinish})

watch(
  () => props.open,
  (newVal) => {
    isOpen.value = newVal
  }
)

const usePaypal = async () => {
  await successPayment(props.status, props.order.orderId)
  window.open(
    `http://localhost:8000/api/order/payment?orderId=${props.order.orderId}&isDeposit=${isDeposit.value}`
  )
}

// const handlePayWithPaypal = async() => {
//   await payWithPayPal(props.order.orderId, isDeposit )

// }

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
          <hr />
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
          <hr />
          <div class="flex">
            <div class="flex-auto">
              <p class="my-3">Total prices:</p>
              <p
                class="my-1"
                :class="
                  status === 'IN_PROGRESS' ? ' my-3 font-semibold text-xl' : ''
                "
              >
                Deposit ({{ deposit * 100 }}%):
              </p>
              <p
                class="my-1"
                :class="
                  status !== 'IN_PROGRESS' ? ' my-3 font-semibold text-xl' : ''
                "
              >
                Remaining:
              </p>
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
              class="my-1 italic text-red-950 text-sm"
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
        <!-- <PaymentPaypalButton
          :order-id="order.orderId"
          :price="pricePay"
          :status="status"
          @payment-success="closeDialog"
        /> -->
        <div class="text-center">
          <button
            type="button"
            class="text-gray-900 bg-[#F7BE38] hover:bg-[#F7BE38]/90 focus:ring-4 focus:outline-none focus:ring-[#F7BE38]/50 font-medium rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center dark:focus:ring-[#F7BE38]/50 me-2 mb-2"
            @click="usePaypal"
          >
            <svg
              class="w-4 h-4 me-2 -ms-1"
              aria-hidden="true"
              focusable="false"
              data-prefix="fab"
              data-icon="paypal"
              role="img"
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 384 512"
            >
              <path
                fill="currentColor"
                d="M111.4 295.9c-3.5 19.2-17.4 108.7-21.5 134-.3 1.8-1 2.5-3 2.5H12.3c-7.6 0-13.1-6.6-12.1-13.9L58.8 46.6c1.5-9.6 10.1-16.9 20-16.9 152.3 0 165.1-3.7 204 11.4 60.1 23.3 65.6 79.5 44 140.3-21.5 62.6-72.5 89.5-140.1 90.3-43.4 .7-69.5-7-75.3 24.2zM357.1 152c-1.8-1.3-2.5-1.8-3 1.3-2 11.4-5.1 22.5-8.8 33.6-39.9 113.8-150.5 103.9-204.5 103.9-6.1 0-10.1 3.3-10.9 9.4-22.6 140.4-27.1 169.7-27.1 169.7-1 7.1 3.5 12.9 10.6 12.9h63.5c8.6 0 15.7-6.3 17.4-14.9 .7-5.4-1.1 6.1 14.4-91.3 4.6-22 14.3-19.7 29.3-19.7 71 0 126.4-28.8 142.9-112.3 6.5-34.8 4.6-71.4-23.8-92.6z"
              ></path>
            </svg>
            Check out with PayPal
          </button>
        </div>
      </div>
      <DialogFooter>
        <Button class="bg-slate-500 hover:bg-slate-600" @click="$emit('close')">
          Cancel Payment
        </Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>
