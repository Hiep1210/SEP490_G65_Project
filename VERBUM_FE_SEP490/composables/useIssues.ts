import { useToast } from '~/components/ui/toast'
import type { Issue } from '~/types/issues'
import type { CreateIssuePayload } from '~/types/payload/createIssue'
import type { ResolveIssuePayload } from '~/types/payload/resolveIssue'

const { toast } = useToast()
export const useIssues = () => {
  const isLoading = ref(false)
  const issues = ref<Issue[]>([])
  const role = useAuthStore().user?.role
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
      if (role === 'CLIENT') {
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
        return issuesData.value
        }
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
      const {data, error} = await useAPI('/issue', {
        method: 'PUT',
        body: JSON.stringify(payload),
        headers: { 'Content-Type': 'application/json' }
      })
      if(!error.value) {
        toast({
          title: 'Issue updated !!',
          description: `Issue has been updated!!`
        })
      }
      return data
    } catch (error) {
      toast({
        title: 'Error updating issue',
        description: 'An error occurred while updating the issue!!'
      })
      console.error('Error updating issue:', error)
    }
  }

  const createIssue = async (payload: CreateIssuePayload) => {
    try {
      const {error, refresh} = await useAPI('/issue', {
        method: 'POST',
        body: JSON.stringify(payload),
        headers: { 'Content-Type': 'application/json' }
      })
      if (error.value) {
        toast({
          title: 'Error creating issue',
          description: `Failed to create issue: ${response.statusText}`,
        })
        console.error('Error creating issue:', response)
        return
      }
      toast({
        title: 'Issue created !!',
        description: `Issue has been created!!`
      })
      await refresh()
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

  const sendCancelResponse = async (issueId: string, responseContent: string) => {
    try {
      const payload = {
        id: issueId,
        responseContent: responseContent
      }
      await useAPI(
        `/issue/send-cancel-response`,
        {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload),
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

  const resolveIssue = async (issue: ResolveIssuePayload) => {
    try {
      await useAPI(
        `/issue`,
        {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(issue),
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
    updateIssueStatus,
    sendCancelResponse,
    resolveIssue
  }
}
