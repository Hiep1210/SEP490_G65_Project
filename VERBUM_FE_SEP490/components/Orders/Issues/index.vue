<script setup>
import { onMounted } from 'vue'

const { issues, getIssuesByOrders, updateIssue } = useIssues()

const props = defineProps({
  jobDeliverables: {
    type: Array,
    default: () => {}
  },
  orderId: {
    type: String,
    default: ''
  },
  role: {
    type: String,
    default: ''
  },
  user: {
    type: Object,
    default: () => ({})
  }
})

const fetchIssues = async () => {
  issues.value = await getIssuesByOrders(props.orderId)
}

const handleUpdate = async (updateIssues) => {
  await updateIssue(updateIssues)
  await fetchIssues()
}

onMounted(() => {
  fetchIssues()
})
</script>

<template>
  <div class="h-full">
    <div class="flex justify-between items-center p-3 border-b">
      <span class="text-lg font-semibold">Issues</span>
      <IssuesCreate v-if="props.role === 'CLIENT'" :order-id="props.orderId" :job-deliverables="jobDeliverables" />
    </div>
    <div v-if="issues" class="h-[15rem] overflow-auto p-2">
      <IssuesTable :issues="issues" :role="props.role" @update="handleUpdate" />
    </div>
    <div v-else class="w-full h-full flex justify-center items-center">
      <p class="font-bold">
        Have issues with the order?
        <span class="text-primary">Let us know.</span>
      </p>
    </div>
  </div>
</template>

<style scoped>
/* Your styles */
</style>
