<script setup lang="ts">
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table'

import { getIssueBadgeClass } from '@/utils/getBadgeClass'
import type { Issue } from '~/types/issues'

const props = defineProps<{
  issues: Issue[]
}>()

const issues = ref(props.issues)
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
      </TableRow>
    </TableHeader>
    <TableBody>
      <TableRow
        v-for="issue in issues"
        :key="issue.issueId"
      >
        <TableCell class="font-medium">
          {{ issue.issueName }}
        </TableCell>
        <TableCell>
          {{
            formatDistanceToNowUserTimezone(issue.createdAt)
          }}
        </TableCell>
        <TableCell>
          {{
            formatDistanceToNowUserTimezone(issue.updatedAt)
          }}
        </TableCell>
        <TableCell class="font-medium">
          {{ issue.orderName }}
        </TableCell>
        <TableCell>
          <Badge :class="getIssueBadgeClass(issue.status)">{{
            issue.status
          }}</Badge>
        </TableCell>
      </TableRow>
    </TableBody>
  </Table>
</template>
