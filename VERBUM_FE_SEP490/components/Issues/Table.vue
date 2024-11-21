<script setup lang="ts">
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table'
import { formatDistanceToNow } from 'date-fns'
import type { Issue } from '~/types/issues';

const showIssuesDialog = ref(false)
const selectedData = ref();
const emit = defineEmits(['update', 'update-status'])

const openIssuesDialog = (data: Issue) => {
  selectedData.value = data;
  showIssuesDialog.value = true;
}

const closeIssuesDialog = () => {
  selectedData.value = '';
  showIssuesDialog.value = false;
}

const props = defineProps<{
  issues: Issue[]
  role: string
}>();

const issues = ref(props.issues);

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
        <TableHead>Order</TableHead>
        <TableHead>Created</TableHead>
        <TableHead>Updated</TableHead>
        <TableHead>Status</TableHead>
      </TableRow>
    </TableHeader>
    <TableBody>
      <TableRow v-for="issue in issues" :key="issue.issueId" @click="openIssuesDialog(issue)">
        <TableCell class="font-medium">
          {{ issue.issueName }}
        </TableCell>
        <TableCell class="font-medium underline">
          <NuxtLink :to="`/orders/details/${issue.orderId}`">{{ issue.orderName }}</NuxtLink>
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
        <TableCell>
          <Badge :class="getIssueBadgeClass(issue.status)">{{ issue.status }}</Badge>
        </TableCell>
      </TableRow>
    </TableBody>
  </Table>
  <IssuesDialog
  v-if="showIssuesDialog"
  :row-data="selectedData"
  :open="showIssuesDialog"
  :role="props.role"
  @close="closeIssuesDialog"
  @update="updateIssueInTable"
  @update-status="updateIssueStatus"
  />
</template>