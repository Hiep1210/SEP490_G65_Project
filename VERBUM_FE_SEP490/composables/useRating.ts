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
  const filteredRating = ref<Rating | null>(null)
  const isLoading = ref(false)

  const getRatings = async () => {
    isLoading.value = true
    try {
      const { data: ratingData } = await useAPI<Rating[]>('/rating/get-all', {
        method: 'GET'
      })

      if (!ratingData?.value || ratingData.value.length === 0) {
        toast({
          title: 'No ratings found!',
          description: 'There are no ratings available!!'
        })
        ratings.value = []
      } else {
        ratings.value = ratingData.value
      }
    } catch (error) {
      toast({
        title: 'Error fetching ratings!!',
        description: 'An error occurred while fetching ratings!!'
      })
      console.log('Error fetching ratings: ', error)
    } finally {
      isLoading.value = false
    }
  }

  const getRatingByOrderId = async (orderId: string) => {
    await getRatings()
    filteredRating.value = ratings.value.find(rating => rating.orderId === orderId) || null

    if (!filteredRating.value) {
      toast({
        title: 'No ratings found!',
        description: `No ratings available for order ID: ${orderId}.`
      })
    }
  }

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
    filteredRating,
    getRatings,
    getRatingByOrderId,
    createRating
  }
}
