import type { Job } from '@/types/job'
export const useJobs = () => {
  const jobs = ref<Job[]>([])
  const job = ref<Job | null>(null)
  const user = useAuthStore().user
  const role = useAuthStore().user?.role
  
  const getJobs = async () => {
    try {
      if (role === 'LINGUIST') {
      const { data } = await useAPI(`/job/get-all?$expand=assigneeNames&$filter=assigneeNames/any(a: cast(a/id, Edm.String) eq '${user?.user_id}')`)
        jobs.value = data.value as Job[]
      }
      else {
        const { data } = await useAPI('/job/get-all')
        jobs.value = data.value as Job[]
      }
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

  return {
    getJobs,
    editJob,
    getJobsOfWork,
    jobs,
    job,
  }
}
