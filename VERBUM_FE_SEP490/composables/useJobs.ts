import type { Job } from '@/types/job'
import { useToast } from '~/components/ui/toast'

const { toast } = useToast()
export const useJobs = () => {
  const jobs = ref<Job[]>([])
  const job = ref<Job | undefined>(undefined)
  const isLoading = ref(false)
  
  const getJobs = async () => {
    try {
        const { data } = await useAPI('/job/get-all')
        jobs.value = data.value as Job[]
    } catch (error) {
      console.error('Failed to fetch jobs:', error)
    }
  }
  const editJob = async (job: Partial<Job>) => {
    try {
      await useAPI('/job/edit', {
        method: 'PUT',
        body: job,
      })
    } catch (error) {
      console.error('Failed to edit job:', error)
    }
  }

  const getJobsOfWork = async (workId: string) => {
    try {
      const { data } = await useAPI(`/job/get-all?filter=WorkId eq ${workId}`)
      jobs.value = data.value as Job[]
    } catch (error) {
      console.error('Failed to fetch jobs of work:', error)
    }
  }
  const getJobsDetail = async (jobId: string) => {
    isLoading.value = true
    try {
      const { data, status } = await useAPI(`/job/get-detail`,{
        method: 'GET',
        query: {
          jobId: jobId
        }
      })
      
      if (status.value === "error") {
        toast({
          title: 'Failed to fetch job detail',
          description: 'Cannot fetch job detail',
          variant: 'destructive'
        })
        return
      }

      job.value = data.value as Job
      toast({
        title: 'Job detail fetched successfully',
        description: 'The job detail has been fetched successfully',
      })
    } catch (error) {
      console.error('Failed to fetch job detail:', error)
    }
    finally {
      isLoading.value = false
    }
  }
  const approve = async (job: Partial<Job> | undefined) => {
    try {
      const {status, error} = await useAPI('/job/approve', {
        method: 'PUT',
        query: {
          jobId: job?.id,
          orderId: job?.orderId
        }
      })
      if (status.value === "error") {
        toast({
          title: 'Failed to approve job',
          description: error.value?.message,
          variant: 'destructive'
        })
      }
      if (status.value === "success") {
        toast({
          title: 'Job approved successfully',
          description: 'The job has been approved successfully',
        })
        window.location.reload()
      }
    } catch (error) {
      console.error('Failed to approve job:', error)
    }
  }
  const reject = async (job: Partial<Job> | undefined) => {
    try {
      const {status, error} = await useAPI('/job/edit', {
        method: 'PUT',
        body: {
          ...job,
          status: "IN_PROGRESS"
        }
      })
      if (status.value === "error") {
        toast({
          title: 'Failed to reject job',
          description: error.value?.message,
          variant: 'destructive'
        })
      }
      if (status.value === "success") {
        toast({
          title: 'Job rejected successfully',
          description: 'The job has been rejected successfully',
        })
        window.location.reload()
      }
    } catch (error) {
      console.error('Failed to reject job:', error)
    }
  }
  return {
    getJobs,
    editJob,
    getJobsOfWork,
    getJobsDetail,
    approve,
    reject,
    jobs,
    job,
    isLoading
  }
}
