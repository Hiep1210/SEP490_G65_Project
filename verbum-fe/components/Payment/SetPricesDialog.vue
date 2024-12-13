<script lang="ts" setup>
import { ref, watch, defineEmits } from 'vue'
import { Button } from '@/components/ui/button'
import type { Order } from '~/types/order'
import type { Language } from '~/types/language'

const props = defineProps<{
  order: Order
  open: boolean
  price: string
  supportedLanguage: Language[]
}>()
const emit = defineEmits(['close', 'confirm'])
const isOpen = ref(props.open)
const prices = ref(props.price)

watch(
  () => props.open,
  (newVal) => {
    isOpen.value = newVal
    prices.value = props.price

  }
)

const closeDialog = () => {
  emit('close')
}

const confirmPrice = () => {
  if (prices.value != null) {
    emit('confirm', prices.value)
  }
}

const totalSupportedLanguages = () => {
  let count = 0
  for (const item of props.supportedLanguage) {
    if (props.order.targetLanguageId) {
      for (const lang of props.order.targetLanguageId) {
        if (lang === item.languageId) {
          count++
        }
      }
    }
  }
  return count;
}

const total = totalSupportedLanguages();

const isSupported = (language: string) => {
  for (const item of props.supportedLanguage) {
    if (item.languageId === language) {
      return 'text-green-700 font-semibold'
    }
  }
}


</script>

<template>
  <Dialog :open="isOpen" @click-outside="closeDialog">
    <DialogContent>
      <DialogTitle class="text-cyan-600 font-bold">Set Prices for {{ order.orderName }}</DialogTitle>
      <!-- <DialogDescription v-if="order.dueDate">Due date: {{order.dueDate}}</DialogDescription> -->
      <hr>
      <div class="flex w-full gap-3">
        <div class="flex-none w-1/3">
          <p class="font-semibold">Service:</p>
        </div>
        <div class="flex-auto">
          <ul>
            <li v-if="order.hasTranslateService">
              <span class="font-bold">TRN</span> - Translate
            </li>
            <li v-if="order.hasEditService">
              <span class="font-bold">EDIT</span> - Edit
            </li>
            <li v-if="order.hasEvaluateService">
              <span class="font-bold">EVL</span> - Evaluate
            </li>
          </ul>
        </div>
      </div>
      <hr>
      <div class="flex w-full gap-3">
        <div class="flex-none w-1/3">
          <p class="font-semibold">Source Language:</p>
          <p class="font-semibold">Target Language:</p>
        </div>
        <div class="flex-auto">
          
          <p
            :class="
              order.sourceLanguageId ? isSupported(order.sourceLanguageId) : ''
            "
          >
            {{ order.sourceLanguageId }}
          </p>
          
          <ul v-for="item in order.targetLanguageId" :key="item">
            <li :class="isSupported(item)">{{ item }}</li>
          </ul>
          <p class="text-xs italic text-gray-600">
            Order have <span class="text-green-700 font-bold">{{ total }}</span> supported languages for target languages.
          </p>
        </div>
      </div>
      <p class="text-sm italic font-semibold">
        Supported languages will be in
        <span class="text-green-700">green color</span>.
      </p>
      <hr>
      <p class="font-bold text-cyan-600">Pricing Matrix</p>
      <div class="flex w-full gap-3">
        <div class="flex-none w-1/3">
          <p class="font-semibold">Price/hour:</p>
          <p class="font-semibold">Price/word/service:</p>

        </div>
        <div class="flex-none ">
          <p class="text-cyan-600 font-semibold" > 10 USD</p>
          <p class="text-cyan-600 font-semibold" > 0.05 USD</p>
        </div>
      </div>
      <hr>
      <div>
        
        <label class="font-semibold">Price: </label>
        <input
          v-model="prices"
          class="border px-4 py-2 rounded-xl bg-gray-100 text-gray-900 dark:bg-gray-800 dark:text-gray-100 dark:border-gray-700"
          type="number"
        > <span>USD</span>
      </div>
      <DialogFooter>
        <Button @click="closeDialog">Cancel</Button>
        <Button @click="confirmPrice">Update</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>
