<template>
  <Dialog>
    <DialogTrigger as-child>
      <slot />
    </DialogTrigger>
    <DialogContent>
      <DialogHeader>
        <DialogTitle>Reject Reason</DialogTitle>
        <Input v-model="reason" placeholder="Enter reject reason" />
      </DialogHeader>
    <DialogFooter>
      <DialogClose>
        <Button variant="outline" @click="reason = ''">Cancel</Button>
      </DialogClose>
      <Button :disabled="!reason" @click="reject">Reject</Button>
    </DialogFooter>
  </DialogContent>
  </Dialog>
</template>

<script lang="ts" setup>
const reason = ref('')

const emit = defineEmits(['reject'])
const reject = () => {
  emit('reject', reason.value)
}
</script>

<style></style>