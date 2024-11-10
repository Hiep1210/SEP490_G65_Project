<script setup lang="ts">
import type { Order } from '~/types/order'
import type { Issue } from '~/types/issues'
import ConfirmDialog from '~/components/Issues/ConfirmDialog.vue'
import SetPricesDialog from '~/components/Payment/SetPricesDialog.vue'
import { useToast } from '~/components/ui/toast'
import { format } from 'date-fns'

const { toast } = useToast()
const { issues, getIssues, updateIssue, getIssuesByOrders } = useIssues()
const {supportedLanguages, getSupportedLanguages} = useLanguages()
const { order, getOrder, changeOrderStatus, setOrderPrice } = useOrders()
const route = useRoute()
const orderId = route.params.id
const { user } = useAuthStore()
const role = user?.role
const isEditing = ref(false)
const editedOrder = ref<Partial<Order> | null>(null)

const openSetPricesDialog = ref(false)
const openPaymentDialog = ref(false)
const openConfirmDialog = ref(false)
const openRatingDialog = ref(false)
const tempPrice = ref<string>('0')

onMounted(() => {
  getOrder(orderId)
  if (!issues.value.length) {
    getIssuesByOrders(orderId as string)
  }
  getSupportedLanguages()
})

// Enter edit mode
const enableEdit = () => {
  isEditing.value = true
  if (order.value) {
    const {
      translationFileUrls,
      referenceFileUrls,
      deliverableFileUrls,
      createdDate,
      discountId,
      paymentStatus,
      orderStatus,
      ...rest
    } = order.value as Order
    editedOrder.value = {
      ...rest,
      targetLanguageId: [...(order.value.targetLanguageId || [])],
      sourceLanguageId: order.value.sourceLanguageId
    }
  }
}

// Cancel edit mode
const cancelEdit = () => {
  isEditing.value = false
  editedOrder.value = null
}

// Save edited order details
const saveEdit = async () => {
  try {
    if (editedOrder.value) {
      const payload = {
        ...editedOrder.value,
        dueDate: editedOrder.value.dueDate
          ? format(new Date(editedOrder.value.dueDate), 'yyyy-MM-dd')
          : null,
        targetLanguageIdList: editedOrder.value?.targetLanguageId,
        translateService: editedOrder.value?.hasTranslateService,
        editService: editedOrder.value?.hasEditService,
        evaluateService: editedOrder.value?.hasEvaluateService,
        discountId: editedOrder.value?.discountId
      }
      const { status } = await useAPI(`/order/update`, {
        method: 'PUT',
        body: JSON.stringify(payload),
        headers: {
          'Content-Type': 'application/json'
        }
      })
      if (status.value === 'success') {
        toast({
          title: 'Success',
          description: `Order updated successfully`
        })
        window.location.reload()
      }
      } else {
        throw new Error('No edited order found')
    }

    if (order.value) {
      Object.assign(order.value, editedOrder.value as Order)
    }
    isEditing.value = false
  } catch (error) {
    console.error('Failed to save order:', error)
  }
}
// Add discount
const openAddDiscount = ref(false)
const addDiscount = async () => {
  try {
    if (editedOrder.value) {
      const payload = {
        ...editedOrder.value,
        discountId: editedOrder.value?.discountId
      }
      await useAPI('/order/update', {
        method: 'PUT',
        body: JSON.stringify(payload),
        headers: {
          'Content-Type': 'application/json'
        }
      })
    }
  } catch (error) {
    console.error('Failed to add discount:', error)
  }
}

const changeSourceLanguage = (languageId: string) => {
  if (editedOrder.value) {
    editedOrder.value.sourceLanguageId = languageId
  }
}

const changeTargetLanguage = (languageIds: string[]) => {
  if (editedOrder.value) {
    editedOrder.value.targetLanguageId = languageIds
  }
}

interface Language {
  languageId: string
  languageName: string
  support: boolean
}
const languageList = ref<Language[]>([])

const showIssuesDialog = ref(false)
const selectedData = ref()

const handleUpdate = async (updateIssues: Issue) => {
  await updateIssue(updateIssues)
  await getIssues()
}

const orderRepo = repo(useNuxtApp().$api)

onMounted(async () => {
  try {
    const data = await orderRepo.getLanguages()
    languageList.value = data
  } catch (error) {
    console.error('Failed to fetch language list:', error)
  }
})
const payStatus = ref('')
const handlePay = (status: string) => {
  payStatus.value = status
  openPaymentDialog.value = true
}

const handleSetPrices = () => {
  openSetPricesDialog.value = true
  tempPrice.value = order.value?.orderPrice || '0'
}

const handlePaymentClose = () => {
  openPaymentDialog.value = false
  if(order.value?.orderStatus === 'ACCEPTED'){
    openRatingDialog.value = true
  }
}

const handleRatingClose = () => {
  openRatingDialog.value = false
  refreshOrder()
}

const handleRatingSubmit = () => {
  openRatingDialog.value = false
  refreshOrder()
}

const refreshOrder = async () => {
  if (orderId) {
    await getOrder(orderId)
  }
}

const confirmSetPrices = async () => {
  try {
    if (tempPrice.value !== null && order.value?.orderId) {
      // Make an API call to update the price
      console.log(tempPrice.value)
      await setOrderPrice(order.value?.orderId, tempPrice.value)
      // Update the local order price after successful API call
      order.value!.orderPrice = tempPrice.value
      openConfirmDialog.value = false // Close the confirmation dialog
      openSetPricesDialog.value = false // Close the Set Prices dialog
    }
  } catch (error) {
    console.error('Failed to set price:', error) // Log error if API call fails
  }
}
</script>

<template>
  <div>
    <div v-if="!order">
      <NuxtLoadingIndicator />
    </div>
    <div v-else class="grid grid-cols-1 md:grid-cols-2 gap-5 pb-5">
      <!-- Order Information and File URLs Section -->
      <div class="space-y-4">
        <div class="container p-4 space-y-2 orderDetails border rounded-md">
          <div class="flex">
            <p class="text-[2rem] font-semibold flex-auto">
              {{ order?.orderName }}
            </p>
            <div v-if="order.orderStatus === 'ACCEPTED' && role === 'CLIENT' && order.orderPrice">
              <Button @click="handlePay('IN_PROGRESS')">Deposit </Button>
            </div>
            <div v-if="order.orderStatus === 'COMPLETED' && role === 'CLIENT' && order.orderPrice">
              <Button @click="handlePay('DELIVERED')">Paying Remaining </Button>
            </div>
            <div v-if="order.orderStatus === 'ACCEPTED' && role === 'DIRECTOR'">
              <Button @click="handleSetPrices">Set prices </Button>
            </div>
          </div>

          <!-- Order Details -->
          <div class="flex flex-col space-y-1">
            <div class="grid grid-cols-2 gap-x-2 text-sm">
              <span class="text-gray-500"
                >Status: {{ order?.orderStatus }}</span
              >
              <span v-if="order.orderPrice">Price: {{ order.orderPrice }}</span>
              <span v-if="order.discountId"
                >Discount: {{ order.discountId }}</span
              >
              <span>Created: {{ order.createdDate?.split('T')[0] }}</span>
              <span v-if="order.reference"
                >Reference: {{ order.reference }}</span
              >
            </div>

            <!-- Services Section -->
            <div class="flex items-center space-x-3">
              <span>Service:</span>
              <template v-if="!isEditing">
                <span
                  v-for="service in ['TRN', 'EDIT', 'EVL']"
                  v-show="
                    (order?.hasTranslateService && service === 'TRN') ||
                    (order?.hasEditService && service === 'EDIT') ||
                    (order?.hasEvaluateService && service === 'EVL')
                  "
                  :key="service"
                  class="font-bold"
                  >{{ service }}</span
                >
              </template>
              <template v-else-if="editedOrder">
                <span
                  v-for="service in ['Translate', 'Edit', 'Evaluate']"
                  :key="service"
                  class="font-bold"
                >
                  {{ service.substring(0, 2).toUpperCase() }}
                  <Checkbox
                    :id="`has${service}Service`"
                    v-model:checked="editedOrder[`has${service}Service`]"
                  />
                </span>
              </template>
            </div>

            <!-- Due Date -->
            <div class="flex items-center space-x-2">
              <span>Due date:</span>
              <span v-if="!isEditing">{{ order.dueDate?.split(' ')[0] }}</span>
              <Input
                v-else-if="editedOrder"
                v-model="editedOrder.dueDate"
                type="date"
                class="border rounded p-1 w-fit"
              />
            </div>

            <!-- Language Selection -->
            <div class="flex items-center space-x-1">
              <template v-if="!isEditing">
                <Badge variant="default">{{ order?.sourceLanguageId }}</Badge>
                <LucideArrowBigRight />
                <div class="flex gap-1">
                  <Badge
                    v-for="lang in order?.targetLanguageId"
                    :key="lang"
                    variant="secondary"
                    >{{ lang }}</Badge
                  >
                </div>
              </template>
              <template v-else>
                <OrdersDetailsLanguageSelector
                  :language-list="languageList"
                  :selected-languages="editedOrder?.sourceLanguageId || ''"
                  :original-languages="order?.sourceLanguageId || ''"
                  :is-source-language="true"
                  @update:selected-languages="changeSourceLanguage"
                />
                <LucideArrowBigRight />
                <OrdersDetailsLanguageSelector
                  :language-list="languageList"
                  :selected-languages="editedOrder?.targetLanguageId || []"
                  :original-languages="order?.targetLanguageId || []"
                  :is-source-language="false"
                  @update:selected-languages="changeTargetLanguage"
                />
              </template>
            </div>
          </div>
        </div>

        <!-- Action Buttons -->
        <div class="flex space-x-2">
          <template v-if="isEditing && role === 'CLIENT'">
            <Button @click="saveEdit">Save</Button>
            <Button variant="outline" @click="cancelEdit">Cancel</Button>
          </template>
          <Button v-else-if="role === 'CLIENT' && (order.orderStatus === 'NEW' || order.orderStatus === 'REJECTED')" @click="enableEdit"
            >Edit Order</Button
          >
          <Button
            v-if="role === 'CLIENT' && order.orderStatus === 'NEW'"
            variant="outline"
            @click="changeOrderStatus(order.orderId, 'CANCELLED')"
            >Cancel Order</Button
          >
          <template v-if="role === 'STAFF'">
            <Button v-if="order.orderStatus === 'NEW'" @click="changeOrderStatus(order.orderId, 'ACCEPTED')"
              >Accept Order</Button
            >
            <OrdersDetailsDialog v-if="order.orderStatus === 'NEW'" :order-id="order.orderId" />
          </template>
          <Button
            v-if="role === 'CLIENT'"
            variant="outline"
            @click="openAddDiscount = true"
            >Add Discount</Button
          >
        </div>

        <OrdersDetailsTabs :order="order" />
      </div>

      <!-- Smaller Issues List Section -->
      <div class="space-y-4 border rounded-md">
        <div class="flex justify-between items-center p-3 border-b">
          <span class="text-lg font-semibold">Issues</span>
          <IssuesCreate v-if="role === 'CLIENT'" :order-id="orderId" />
        </div>
        <div v-if="issues.length !== 0" class="h-[15rem] overflow-auto p-2">
          <IssuesTable
            :issues="issues"
            :role="user?.role as string"
            @update="handleUpdate"
          />
        </div>
        <div v-else class="w-full h-full flex justify-center items-center">
          <p class="font-bold">
            Have issues with the order?
            <span class="text-primary">Let us know.</span>
          </p>
        </div>
      </div>

      <!-- Set Prices Dialog -->
      <SetPricesDialog
        :order="order"
        :price="tempPrice"
        :open="openSetPricesDialog"
        :supported-language="supportedLanguages"
        @close="openSetPricesDialog = false"
        @confirm="
          (newPrice) => {
            openConfirmDialog = true
            tempPrice = newPrice
          }
        "
      />

      <!-- Confirm Dialog -->
      <ConfirmDialog
        :title="'Confirm Price Update'"
        :description="'Are you sure you want to update the price ?'"
        :open="openConfirmDialog"
        @close="openConfirmDialog = false"
        @confirm="confirmSetPrices"
      />

      <PaymentDialog
        :order="order"
        :status="payStatus"
        :open="openPaymentDialog"
        @close="handlePaymentClose"
      />

      <RatingDialog
          :open="openRatingDialog"
          :order-id="order.orderId"
          @close="handleRatingClose"
          @submit="handleRatingSubmit"
      />
    </div>
  </div>
</template>
