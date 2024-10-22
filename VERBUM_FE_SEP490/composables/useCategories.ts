import { useToast } from '~/components/ui/toast'

interface Category {
  id: number
  name: string
}

const { toast } = useToast()
export const useCategories = () => {
  const categories = ref<Category[]>([])
  const isLoading = ref(false)

  const getCategories = async () => {
    isLoading.value = true
    try {
      const { data: categoriesData } = await useAPI<Category[]>(
        '/category/get-all',
        {
          method: 'GET'
        }
      )
      if (!categoriesData?.value || categoriesData.value.length === 0) {
        toast({
          title: 'No categories found',
          description: 'There are no categories available'
        })
        categories.value = []
      } else {
        categories.value = categoriesData.value
      }
    } catch (error) {
      console.error('Error fetching categories:', error)
    } finally {
      isLoading.value = false
    }
  }

  return { categories, isLoading, getCategories }
}
