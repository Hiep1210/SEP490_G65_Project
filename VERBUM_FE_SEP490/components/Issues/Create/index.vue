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
import { h, ref } from 'vue'
import * as z from 'zod'

const formSchema = toTypedSchema(
  z.object({
    issueName: z.string().min(2).max(50),
    issueDescription: z.string().min(10).max(255),
    issueAttachments: z.string().min(1)
  })
)

function onSubmit(values: any) {
  console.log('Form submitted!', values)
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
      <DialogContent class="max-w-[1000px]">
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
