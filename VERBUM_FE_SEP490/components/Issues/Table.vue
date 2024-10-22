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
const issues = mockIssues
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
        <TableCell>{{ issue.createdAt }}</TableCell>
        <TableCell>{{ issue.updatedAt }}</TableCell>
        <TableCell>{{ issue.status }}</TableCell>
        <TableCell>
          <DropdownMenu>
            <DropdownMenuTrigger>
              <Button variant="default">View</Button>
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
