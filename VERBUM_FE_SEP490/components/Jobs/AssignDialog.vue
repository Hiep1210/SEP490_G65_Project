<template>
  <div>
    <Dialog>
      <DialogTrigger as-child><Button variant="outline">Assign</Button></DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Assign Job</DialogTitle>
          <DialogDescription class="text-black space-y-2">
            Choose a linguist
          </DialogDescription>
        </DialogHeader>
        <Select>
          <SelectTrigger>
            <SelectValue placeholder="Select a linguist" model-value="selectedUser?.name" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem v-for="user in assignList" :key="user.user_id" :value="user.user_id" @click="selectedUser = user">{{ user.name }}
            </SelectItem>
          </SelectContent>
        </Select>
        <DialogFooter>
          <Button variant="outline" @click="assignJob">Assign</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>

  </div>
</template>

<script lang="ts" setup>
import type { User } from '@/types/user'
const assignList = inject<Ref<User[]>>('assignList')
const selectedUser = ref<User | null>(null)

const assignJob = () => {
  console.log(selectedUser.value)
}
</script>

<style></style>