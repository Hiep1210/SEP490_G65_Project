<script setup lang="ts">
import { ref } from 'vue'
import type { Work } from '~/composables/useWorks'

// Define the type for the status object
interface Status {
  status: string
  class: string
  currentPage: number
  itemsPerPage: number
}

const props = defineProps<{
  data: Work[]
}>()

const allStatus = ref<Status[]>([
  {
    status: 'NEW',
    class: 'bg-cyan-200 text-black',
    currentPage: 1,
    itemsPerPage: 5 // Set the number of items to show per page for each status
  },
  {
    status: 'IN-PROGRESS',
    class: 'bg-teal-200 text-black',
    currentPage: 1,
    itemsPerPage: 5
  },
  {
    status: 'COMPLETED',
    class: 'bg-emerald-200 text-black',
    currentPage: 1,
    itemsPerPage: 5
  },
  {
    status: 'CANCEL',
    class: 'bg-gray-200 text-black',
    currentPage: 1,
    itemsPerPage: 5
  }
])

// Search query for filtering works by name
const searchQuery = ref('')

// Function to calculate total pages for a specific status
const totalPages = (status: Status) => {
  const filteredWorks = filteredWorksByStatus(status)
  return Math.ceil(filteredWorks.length / status.itemsPerPage)
}

// Function to filter works based on status and search query
const filteredWorksByStatus = (status: Status) => {
  return props.data.filter(work => {
    const matchesStatus =
      work.orderStatus === status.status ||
      (status.status === 'NEW' && work.orderStatus === null)
    const matchesQuery = work.workName.toLowerCase().includes(searchQuery.value.toLowerCase())
    return matchesStatus && matchesQuery
  })
}

// Function to get paginated works for a specific status
const paginatedWorks = (status: Status) => {
  const filteredWorks = filteredWorksByStatus(status)
  const start = (status.currentPage - 1) * status.itemsPerPage
  return filteredWorks.slice(start, start + status.itemsPerPage)
}

// Navigation functions for changing pages
const nextPage = (status: Status) => {
  if (status.currentPage < totalPages(status)) {
    status.currentPage++
  }
}

const previousPage = (status: Status) => {
  if (status.currentPage > 1) {
    status.currentPage--
  }
}
</script>

<template>
  <div class="relative w-2/6 mb-3">
    <input
      id="default-search"
      v-model="searchQuery"
      type="search"
      class="block w-full p-4 ps-10 text-sm text-gray-900 border border-gray-300 rounded-xl bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
      placeholder="Enter work name ..."
    >
    <button
      type="submit"
      class="text-white absolute end-2.5 bottom-2.5 bg-cyan-600 hover:bg-cyan-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
    >
      Search
    </button>
  </div>
  <div class="flex flex-row gap-3 mt-2">
    <div
      v-for="item in allStatus"
      :key="item.status"
      class="flex flex-col w-1/4 statusCol"
    >
      <div :class="item.class" class="p-2 rounded-xl h-5/6">
        <p class="font-bold text-cyan-950 text-center my-3">
          {{ item.status }}
        </p>
        <div v-for="work in paginatedWorks(item)" :key="work.workId">
          <WorksCard
            v-if="
              work.orderStatus === item.status ||
              (item.status === 'NEW' && work.orderStatus === null)
            "
            :data="work"
          />
        </div>
      </div>
      <div class="flex justify-between mt-4">
        <button :disabled="item.currentPage === 1" @click="previousPage(item)">
          Previous
        </button>
        <span>Page {{ item.currentPage }} of {{ totalPages(item) }}</span>
        <button
          :disabled="item.currentPage === totalPages(item)"
          @click="nextPage(item)"
        >
          Next
        </button>
      </div>
    </div>
  </div>
</template>

<style>
.statusCol {
  height: 80vh;
}
</style>
