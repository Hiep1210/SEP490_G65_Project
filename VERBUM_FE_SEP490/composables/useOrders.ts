import type { Order } from '~/types/order'
import { ref } from 'vue'
import { useToast } from '~/components/ui/toast'

const { toast } = useToast()

export const useOrders = () => {
  const router = useRouter()
  const isLoading = ref(false)
  const orders = ref<Order[]>([])
  const order = ref<Order | null>(null)

  const getOrders = async () => {
    isLoading.value = true
    try {
      const { data: ordersData } = await useAPI<Order[]>('/order/get-all?$orderby=orderName desc', {
        method: 'GET',
        credentials: 'include',
      })

      if (!ordersData?.value || ordersData.value.length === 0) {
        toast({
          title: 'No orders found',
          description: 'There are no orders available'
        })
        orders.value = []
      } else {
        orders.value = ordersData.value
      }
    } catch (error) {
      console.error('Failed to fetch orders:', error)
      toast({
        title: 'Error',
        description: 'Failed to fetch orders. Please try again later.'
      })
    } finally {
      isLoading.value = false
    }
  }

  const getOrder = async (id: string | string[]) => {
    isLoading.value = true
    try {
      const { data: orderData } = await useAPI<Order>(`/order/get-details`, {
        params: { id },
        method: 'GET',
        credentials: 'include'
      })

      if (!orderData?.value) {
        toast({
          title: 'No order found',
          description: 'This order does not exist'
        })
        navigateTo('/orders')
        order.value = null
      } else {
        order.value = orderData.value
      }
    } catch (error) {
      console.error('Failed to fetch order details:', error)
      toast({
        title: 'Error',
        description: 'Failed to fetch order details. Please try again later.'
      })
    } finally {
      isLoading.value = false
    }
  }


  const changeOrderStatus = async (id: string, status: string) => {
    isLoading.value = true
    try {
      await useAPI('/order/change-status', { method: 'PUT', credentials: 'include', params: { orderId: id, orderStatus: status } })

      if (status === 'ACCEPTED' && order.value) {
        const payload = {
          orderId: order.value.orderId,
          orderName: order.value.orderName,
          dueDate: order.value.dueDate
            ? new Date(order.value.dueDate).toISOString().replace('Z', '')
            : null,
          hasTranslateService: order.value.hasTranslateService,
          hasEditService: order.value.hasEditService,
          hasEvaluateService: order.value.hasEvaluateService
        }
        const { data: guidResponse } = await useAPI<string[]>('work/generate', {
          method: 'POST',
          credentials: 'include',
          body: JSON.stringify(payload),
          headers: { 'Content-Type': 'application/json' }
        })

        if (guidResponse?.value?.length) {
          const payload2 = {
            workIds: guidResponse.value,
            documentURLs: order.value.translationFileUrls,
            targetLanguageIds: order.value.targetLanguageId
          }
          await useAPI('job/add', {
            method: 'POST',
            credentials: 'include',
            body: JSON.stringify(payload2),
            headers: { 'Content-Type': 'application/json' }
          })
        }
      }
      toast({
        title: 'Success',
        description: `Status changed successfully`
      })
      router.push('/orders')
    } catch (error) {
      console.error(`Failed to change status:`, error)
      toast({
        title: 'Error',
        description: `Failed to change status. Please try again later.`
      })
    } finally {
      isLoading.value = false
    }
  }

  const sendRejectOrder = async (id: string, reason: string) => {
    isLoading.value = true
    try {
      const payload = {
        id,
        responseContent: reason
      }
      await useAPI('/order/change-status', { method: 'PUT', credentials: 'include', params: { orderId: id, orderStatus: 'REJECTED' } })
      await useAPI('/order/send-reject-response', { method: 'PUT', credentials: 'include', body: JSON.stringify(payload), headers: { 'Content-Type': 'application/json' } })
      toast({
        title: 'Success',
        description: `Order rejected successfully`
      })
      router.push('/orders')
    } catch (error) {
      console.error('Failed to send reject order:', error)
      toast({
        title: 'Error',
        description: `Failed to reject order. Please try again later.`
      })
    } finally {
      isLoading.value = false
    }
  }

  const setOrderPrice = async (orderId: string, orderPrice: string) => {
    try {
      await useAPI(`/order/price`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        params: { orderId: orderId, price: orderPrice }
      })

      toast({
        title: 'Order price updated !!',
        description: `Order price has been updated!!`
      })
    } catch (error) {
      toast({
        title: 'Error updating Order price',
        description: 'An error occurred while updating the Order price!!'
      })
      console.error('Error updating Order price:', error)
    }
  }

  const updateSuccessPayment = async (orderId: string, status: string) => {
    try {
      await useAPI(`/order/change-status`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        params: { orderId: orderId, orderStatus: status }
      })

      toast({
        title: 'Your order is paid successfully!!',
        description: `We are going to do your order. Thank you for choosing our service.`
      })
    } catch (error) {
      toast({
        title: 'Error updating order status ',
        description: 'An error occurred while updating the order status!!'
      })
      console.error('Error updating Order status:', error)
    }
  }

  return {
    isLoading,
    orders,
    order,
    getOrders,
    getOrder,
    changeOrderStatus,
    sendRejectOrder,
    setOrderPrice,
    updateSuccessPayment
  }
}
