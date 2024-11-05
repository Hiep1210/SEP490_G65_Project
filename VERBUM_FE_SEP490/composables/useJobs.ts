import type { Job } from '@/types/job'
export const useJobs = () => {
  const jobs = ref<Job[]>([])
  const job = ref<Job | null>(null)

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

  return {
    getJobs,
    editJob,
    jobs,
    job,
  }
}
