import { useToast } from '~/components/ui/toast'
import type { Issue, IssueAttachment } from '~/types/issues'

const { toast } = useToast()
export const useIssues = () => {
  const isLoading = ref(false)
  const issues = ref<Issue[]>([])

  const getIssues = async () => {
    try {
      const { data: issuesData } = await useAPI<Issue[]>('/issue', {
        method: 'GET'
      })
      if (!issuesData?.value || issuesData.value.length === 0) {
        toast({
          title: 'No issues found!',
          description: 'There are no issues available!!'
        })
        issues.value = []
      } else {
        issues.value = issuesData.value
      }
    } catch (error) {
      toast({
        title: 'Error fetching issues!!',
        description: 'An error occurred while fetching issues!!'
      })
      console.log('Error fetching issues: ', error)
    } finally {
      isLoading.value = false
    }
  }

  const getIssuesByOrders = async (orderId: string) => {
    try {
      const { data: issuesData } = await useAPI<Issue[]>(
        `/issue?$filter=orderId eq ${orderId}`,
        {
          method: 'GET'
        }
      )
      if (!issuesData?.value || issuesData.value.length === 0) {
        toast({
          title: 'No issues found!',
          description: 'There are no issues available!!'
        })
        issues.value = []
      } else {
        issues.value = issuesData.value
      }
    } catch (error) {
      toast({
        title: 'Error fetching issues!!',
        description: 'An error occurred while fetching issues!!'
      })
      console.log('Error fetching issues: ', error)
    } finally {
      isLoading.value = false
    }
  }

  const updateIssue = async (issue: Issue) => {
    try {
      const payload = issue
      await useAPI('/issue', {
        method: 'PUT',
        body: JSON.stringify(payload),
        headers: { 'Content-Type': 'application/json' }
      })
      console.log({ payload })
      toast({
        title: 'Issue updated !!',
        description: `Issue has been updated!!`
      })
    } catch (error) {
      toast({
        title: 'Error updating issue',
        description: 'An error occurred while updating the issue!!'
      })
      console.error('Error updating issue:', error)
    }
  }

  const createIssue = async (
    payload: createIssuePayload
  ) => {
    try {
        await useAPI('/issue', {
          method: 'POST',
          body: JSON.stringify(payload),
          headers: { 'Content-Type': 'application/json' }
        })
        toast({
          title: 'Issue created !!',
          description: `Issue has been created!!`
        })
    } catch (error) {
      toast({
        title: 'Error creating issue',
        description: 'An error occurred while creating the issue!!'
      })
      console.error('Error creating issue:', error)
    }
  }

  const updateIssueStatus = async (issuesId: string, status: string) => {
    try {
      await useAPI(
        `/issue/change-status?issueId=${issuesId}&status=${status}`,
        {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' }
        }
      )
      toast({
        title: 'Issue status updated !!',
        description: `Issue status has been updated!!`
      })
    } catch (error) {
      toast({
        title: 'Error updating issue status',
        description: 'An error occurred while updating the issue status!!'
      })
      console.error('Error updating issue status:', error)
    }
  }

  return {
    isLoading,
    issues,
    getIssues,
    getIssuesByOrders,
    updateIssue,
    createIssue,
    updateIssueStatus
  }
}
