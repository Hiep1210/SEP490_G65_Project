import type { Order } from '~/types/order'
import { ref } from 'vue'
import { useToast } from '~/components/ui/toast'

const { toast } = useToast()

export const useOrders = () => {
  const isLoading = ref(false)
  const orders = ref<Order[]>([])
  const order = ref<Order | null>(null)

  const getOrders = async () => {
    isLoading.value = true
    try {
      const { data: ordersData } = await useAPI<Order[]>('/order/get-all', {
        method: 'GET',
        credentials: 'include'
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
        // Helper function to safely extract filename from a Firebase URL
        const getFileNameFromUrl = (
          url: string | undefined | null
        ): string | null => {
          if (
            typeof url === 'string' &&
            url.includes('uploads') &&
            url.includes('?alt=media')
          ) {
            const decodedUrl = decodeURIComponent(url)
            const match = decodedUrl.match(/uploads\/(.+?)\?alt=media/)
            return match ? match[1] : null
          }
          return null
        }

        // Helper function to trim off the date and time from a date string
        const trimDateTime = (date: string | undefined): string | undefined => {
          return date ? date.split(' ')[0] : undefined
        }

        const modifiedOrder = {
          ...orderData.value,
          translationFileUrls:
            orderData.value.translationFileUrls
              ?.map(getFileNameFromUrl)
              .filter((file): file is string => file !== null) || [],
          referenceFileUrls:
            orderData.value.referenceFileUrls
              ?.map(getFileNameFromUrl)
              .filter((file): file is string => file !== null) || [],
          deliverableFileUrls:
            orderData.value.deliverableFileUrls
              ?.map(getFileNameFromUrl)
              .filter((file): file is string => file !== null) || [],
          createdDate: trimDateTime(orderData.value.createdDate),
          dueDate: trimDateTime(orderData.value.dueDate)
        }

        order.value = modifiedOrder
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

  const cancelOrder = async (id: string) => {
    isLoading.value = true
    try {
      await useAPI('/order/cancel', {
        method: 'PUT',
        credentials: 'include',
        params: { orderId: id }
      })
    } catch (error) {
      console.error('Failed to cancel order:', error)
    } finally {
      isLoading.value = false
    }
  }

  const acceptorDeclineOrder = async (id: string, status: string) => {
    isLoading.value = true
    try {
      await useAPI('/order/acceptordecline', {
        method: 'PUT',
        credentials: 'include',
        params: { orderId: id, orderStatus: status }
      })

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
        description: `Order ${status === 'ACCEPTED' ? 'accepted' : 'rejected'} successfully`
      })
    } catch (error) {
      console.error(`Failed to ${status} order:`, error)
      toast({
        title: 'Error',
        description: `Failed to ${status} order. Please try again later.`
      })
    } finally {
      isLoading.value = false
    }
  }

  const setOrderPrice = async (orderId: string, orderPrice: string) => {
    try {
      await useAPI(`/order/price?orderId=${orderId}&price=${orderPrice}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' }
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
  return {
    isLoading,
    orders,
    order,
    getOrders,
    getOrder,
    cancelOrder,
    acceptorDeclineOrder,
    setOrderPrice
  }
}
