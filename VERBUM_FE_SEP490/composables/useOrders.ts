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

  return { isLoading, orders, order, getOrders, getOrder }
}
