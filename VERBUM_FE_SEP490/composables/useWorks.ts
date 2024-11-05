import { useToast } from '~/components/ui/toast'

export interface Work {
  workId: string
  workName: string
  sourceLanguageId: string
  targetLanguageId: string[]
  translationFileUrls: string[]
  referenceFileUrls: string[]
  orderStatus: string
}
const { toast } = useToast()

export const useWorks = () => {
  const works = ref<Work[]>([])
  const isLoading = ref(false)


  const getWorks = async () => {
    isLoading.value = true
    try {
      const { data: worksData } = await useAPI<Work[]>('/work/get-all', {
        method: 'GET'
      })
      if (!worksData?.value || worksData.value.length === 0) {
        toast({
          title: 'No works found!',
          description: 'There are no works available!!'
        })
        works.value = []
      } else {
        works.value = worksData.value
      }
    } catch (error) {
      toast({
        title: 'Error fetching works!!',
        description: 'An error occurred while fetching works!!'
      })
      console.log('Error fetching works: ', error)
    } finally {
      isLoading.value = false
    }
  }

  return {
    works,
    isLoading,
    getWorks
  }
}
