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

  const addCategory = async (category: Category) => {
    try {
      await useAPI('/category/add', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(category)
      })
    } catch (error) {
      console.error('Error adding category:', error)
    }
  }
  const deleteCategory = async (id: number) => {
    try {
      await useAPI(`/category/delete/`, {
        method: 'DELETE',
        body: JSON.stringify({ id }),
        headers: { 'Content-Type': 'application/json' }
      })
    } catch (error) {
      console.error('Error deleting category:', error)
    }
  }
  
  const updateCategory = async (category: Category) => {
    try {
      await useAPI(`/category/update/`, {
        method: 'PUT',
        body: JSON.stringify(category),
        headers: { 'Content-Type': 'application/json' }
      })
      console.log(category)
    } catch (error) {
      console.error('Error updating category:', error)
    }
  }


  return {
    categories,
    isLoading,
    getCategories,
    addCategory,
    deleteCategory,
    updateCategory
  }
}
