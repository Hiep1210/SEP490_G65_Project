<script lang="ts" setup>
import { columns } from '~/components/Works/columns'
import { useWorks } from '~/composables/useWorks'
useSeoMeta({
  title: 'Works'
})

definePageMeta({
  layout: 'default'
})

const { works, getWorks } = useWorks()


const currentPage = ref(1)
const pageSize = ref(10) // Number of items per page
const searchQuery = ref('')
const statusFilter = ref('') // Filter by status

// Fetch works on mount
onMounted(() => {
  getWorks();
});

// Computed properties for filtered and paginated works
const filteredWorks = computed(() => {
  return works.value.filter((work) => {
    const matchesSearch = work.workName
      .toLowerCase()
      .includes(searchQuery.value.toLowerCase())
    const matchesStatus =
      !statusFilter.value || work.orderStatus === statusFilter.value
    return matchesSearch && matchesStatus
  })
})

const paginatedWorks = computed(() => {
  const start = (currentPage.value - 1) * pageSize.value
  const end = start + pageSize.value
  return filteredWorks.value.slice(start, end)
})

const totalPages = computed(() =>
  Math.ceil(filteredWorks.value.length / pageSize.value)
)

// Watch for search, filter, or page changes
const handlePageChange = (page: number) => {
  currentPage.value = page
}
</script>

<template>
  <div>
    <!-- Search and Filter UI -->
    <div class="flex gap-4 mb-4">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Search by work name"
         class="border w-1/3 px-4 py-2 rounded-xl  text-gray-900 dark:bg-gray-800 dark:text-gray-100 dark:border-gray-700"
      >

      <select v-model="statusFilter" class="border px-4 py-2 rounded-xl bg-gray-100 text-gray-900 dark:bg-gray-800 dark:text-gray-100 dark:border-gray-700">
        <option value="">All Statuses</option>
        <option value="ACCEPTED">ACCEPTED</option>
        <option value="IN_PROGRESS">IN PROGRESS</option>
        <option value="NEW">NEW</option>
      </select>
    </div>

    <WorksTable :columns="columns" :data="paginatedWorks" />

    <!-- <WorksStatusColumn :data="works"/> -->

    <!-- Pagination Controls -->
    <div class="flex justify-between items-center mt-4">
      <button :disabled="currentPage === 1" class="border px-4 py-2 rounded-xl" @click="handlePageChange(currentPage - 1)">
         Previous
      </button>

      <span>Page {{ currentPage }} of {{ totalPages }} </span>

      <button
        :disabled="currentPage === totalPages"
        class="border px-4 py-2 rounded-xl"
        @click="handlePageChange(currentPage + 1)"
      >
        Next
      </button>
    </div>
  </div>
</template>

<style></style>
