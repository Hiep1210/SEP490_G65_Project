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
  console.log('Fetched Issues:', issues.value)
}

const handleUpdate = async (updateIssues) => {
  await updateIssue(updateIssues)
  window.location.reload()
}

onMounted(() => {
  fetchIssues()
})
</script>

<template>
  <div class="h-full">
    <div class="flex justify-between items-center p-3 border-b">
      <span class="text-lg font-semibold text-primary">Issues</span>
      <IssuesCreate
        v-if="props.role === 'CLIENT'"
        :order-id="props.orderId"
        :job-deliverables="jobDeliverables"
      />
    </div>
    <div v-if="issues" class="h-[15rem] overflow-auto p-2">
      <IssuesTable :issues="issues" :role="props.role" @update="handleUpdate" />
    </div>
    <div v-else class="w-full h-full flex justify-center items-center">
      <p v-if="props.role === 'CLIENT'" class="font-bold">
        Have issues with the order?
        <span class="text-primary">Let us know.</span>
      </p>
      <p v-else class="font-bold">
        <span class="text-primary">No issue found</span>
      </p>
    </div>
  </div>
</template>
