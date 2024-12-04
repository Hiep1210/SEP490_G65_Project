<script lang="ts" setup>
defineProps<{
  orderId?: string | undefined
}>()

const { sendRejectOrder } = useOrders()
const reason = ref('')
</script>
<template>
  <Dialog>
    <DialogTrigger as-child>
      <Button variant="outline">Reject Order</Button>
    </DialogTrigger>
    <DialogContent>
      <DialogHeader>
        <DialogTitle>Reject Order</DialogTitle>
        <DialogDescription>
          Enter the response why we reject this order
        </DialogDescription>
      </DialogHeader>
      <div class="flex flex-col gap-2">
        <Label>Reason</Label>
        <Input v-model="reason" />
      </div>
      <DialogFooter>
        <Button @click="orderId && sendRejectOrder(orderId, reason)">Reject</Button>
      </DialogFooter>
    </DialogContent>
  </Dialog>
</template>