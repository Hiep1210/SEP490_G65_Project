import { useToast } from '~/components/ui/toast'

export interface Receipt {
  receiptId: string
  payDate: string
  depositeOrPayment: boolean
  amount: number
  orderId: string
}

const { toast } = useToast()

export const useReceipt = () => {
  const receipts = ref<Receipt[]>([])
  const isLoading = ref(false)

  const getReceipts = async () => {
    isLoading.value = true
    try {
      const { data: receiptData } = await useAPI<Receipt[]>(
        '/receipt/get-all',
        {
          method: 'GET'
        }
      )
      if (!receiptData?.value || receiptData.value.length === 0) {
        toast({
          title: 'No receipts found!',
          description: 'There are no receipts available!!'
        })
        receipts.value = []
      } else {
        receipts.value = receiptData.value
      }
    } catch (error) {
      toast({
        title: 'Error fetching receipts!!',
        description: 'An error occurred while fetching receipts!!'
      })
      console.log('Error fetching receipts: ', error)
    } finally {
      isLoading.value = false
    }
  }

  const createReceipt = async (
    orderId: string,
    depositeOrPayment: boolean,
    amount: number
  ) => {
    try {
      const payload = {
        orderId: orderId,
        depositeOrPayment: depositeOrPayment,
        amount: amount
      }
      await useAPI('/receipt/add', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: payload
      })
      toast({
        title: 'Receipt created!!',
        description: `Receipt has been created`
      })
    } catch (error) {
      toast({
        title: 'Error adding receipt',
        description: 'An error occurred while adding the receipt.'
      })
      console.error('Error adding receipt:', error)
    }
  }

  return {
    receipts,
    isLoading,
    getReceipts,
    createReceipt
  }
}
