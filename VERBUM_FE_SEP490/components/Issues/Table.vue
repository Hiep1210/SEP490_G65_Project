<script setup lang="ts">
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table'
import mockIssues from '~/mock/issues'
import { formatDistanceToNow } from 'date-fns'

const issues = mockIssues

const getBadgeClass = (status: string) => {
  switch (status) {
    case 'Open':
      return 'bg-green-500 text-white'
    case 'In Progress':
      return 'bg-yellow-500 text-black'
    case 'Closed':
      return 'bg-gray-500 text-white'
    default:
      return 'bg-gray-300 text-black'
  }
}
</script>

<template>
  <Table>
    <TableHeader>
      <TableRow>
        <TableHead> Issue Name </TableHead>
        <TableHead>Created</TableHead>
        <TableHead>Updated</TableHead>
        <TableHead>Status</TableHead>
        <TableHead>Reference Files</TableHead>
      </TableRow>
    </TableHeader>
    <TableBody>
      <TableRow v-for="issue in issues" :key="issue.issueId">
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
        <TableCell>
          <Badge :class="getBadgeClass(issue.status)">{{ issue.status }}</Badge>
        </TableCell>
        <TableCell>
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
</template>
