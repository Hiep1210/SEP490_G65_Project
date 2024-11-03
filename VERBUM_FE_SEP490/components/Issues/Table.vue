<script setup lang="ts">
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table'
// import mockIssues from '~/mock/issues'
import { formatDistanceToNow } from 'date-fns'
import {getIssueBadgeClass} from '@/utils/getBadgeClass'
import type { Issue } from '~/types/issues'

const showIssuesDialog = ref(false)
const selectedData = ref()
const emit = defineEmits(['update', 'update-status'])

const openIssuesDialog = (data: Issue) => {
  selectedData.value = data
  showIssuesDialog.value = true
}
const closeIssuesDialog = () => {
  selectedData.value = ''
  showIssuesDialog.value = false
}

const props = defineProps<{
  issues: Issue[]
}>()

const issues = ref(props.issues)

const updateIssueInTable = (updatedIssue: Issue) => {
  emit('update', updatedIssue)
  closeIssuesDialog()
}

const updateIssueStatus = (issuesId: string, status: string) => {
  emit('update-status', issuesId, status)
}

watch(
  () => props.issues,
  (newList) => {
    issues.value = [...newList]
  },
  { deep: true }
)
</script>

<template>
  <Table>
    <TableHeader>
      <TableRow>
        <TableHead>Issue Name</TableHead>
        <TableHead>Created</TableHead>
        <TableHead>Updated</TableHead>
        <TableHead>Order</TableHead>
        <TableHead>Status</TableHead>
        <TableHead>Reference Files</TableHead>
      </TableRow>
    </TableHeader>
    <TableBody>
      <TableRow
        v-for="issue in issues"
        :key="issue.issueId"
        @click="openIssuesDialog(issue)"
      >
        <TableCell class="font-medium">
          {{ issue.issueName }}
        </TableCell>
        <TableCell>
          {{
            formatDistanceToNow(new Date(issue.createdAt), { addSuffix: true })
          }}
        </TableCell>
        <TableCell>
          {{
            formatDistanceToNow(new Date(issue.updatedAt), { addSuffix: true })
          }}
        </TableCell>
        <TableCell class="font-medium">
          {{ issue.orderName }}
        </TableCell>
        <TableCell>
          <Badge :class="getIssueBadgeClass(issue.status)">{{ issue.status }}</Badge>
        </TableCell>
        <TableCell @click.stop>
          <DropdownMenu>
            <DropdownMenuTrigger>
              <Button variant="default">View All Files</Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent>
              <DropdownMenuItem
                v-for="attachmentUrl in issue.issueAttachments"
                :key="attachmentUrl.attachmentUrl"
              >
                <NuxtLink :to="attachmentUrl.attachmentUrl">
                  {{ attachmentUrl.attachmentUrl }}
                </NuxtLink>
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </TableCell>
      </TableRow>
    </TableBody>
  </Table>
  <IssuesDialog
    v-if="showIssuesDialog"
    :row-data="selectedData"
    :open="showIssuesDialog"
    @close="closeIssuesDialog"
    @update="updateIssueInTable"
    @update-status="updateIssueStatus"
  />
</template>
