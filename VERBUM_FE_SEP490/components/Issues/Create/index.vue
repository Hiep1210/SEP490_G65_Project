<script setup lang="ts">
import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import { toTypedSchema } from '@vee-validate/zod'
import * as z from 'zod'
import type { CreateIssuePayload } from '~/types/payload/createIssue'

const route = useRoute()
const orderId = route.params.id
const { createIssue } = useIssues()

const formSchema = toTypedSchema(
  z.object({
    issueName: z.string().min(2).max(50),
    issueDescription: z.string().min(10).max(255),
    issueAttachments: z.string().min(1)
  })
)

async function onSubmit(values: CreateIssuePayload) {
  console.log(values)
  const payload = {
    ...values,
    orderId: orderId,
    issueAttachments: values.issueAttachments
      .split(',')
      .map((url: string) => ({ attachmentUrl: url.trim() }))
  }
  await createIssue(payload)
}
</script>

<template>
  <Form
    id="dialogForm"
    v-slot="{ submitForm }"
    :validation-schema="formSchema"
    @submit="onSubmit"
  >
    <Dialog>
      <DialogTrigger as-child>
        <Button variant="outline"> Create Issue </Button>
      </DialogTrigger>
      <DialogContent class="max-w-[1000px] max-h-[750px] overflow-y-scroll">
        <DialogHeader>
          <DialogTitle>Create Issues</DialogTitle>
          <DialogDescription>
            Let us know what issues you are having with your order.
          </DialogDescription>
        </DialogHeader>

        <form @submit="submitForm">
          <IssuesCreateForm />
        </form>

        <DialogFooter>
          <Button type="submit" form="dialogForm"> Create </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  </Form>
</template>
