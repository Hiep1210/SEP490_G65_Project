import { useToast } from '~/components/ui/toast'

export interface Rating {
  ratingId?: string
  orderId: string
  inTime: number
  expectation: number
  issueResolved: number
  moreThought: string
}

const { toast } = useToast()

export const useRating = () => {
  const ratings = ref<Rating[]>([])
  const isLoading = ref(false)

  const createRating = async (rating: Rating) => {
    try {
      await useAPI('/rating/add', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(rating)
      })
      toast({
        title: 'Order Rated Successfully!!!',
        description: `Thank you for rating your order! Your feedback helps us improve our service.`
      })
    } catch (error) {
      toast({
        title: 'Error creating rating',
        description: 'An error occurred while creating the rating.'
      })
      console.error('Error creating rating:', error)
    }
  }

  return {
    ratings,
    isLoading,
    createRating
  }
}
