<script setup lang="ts">
import { ref } from 'vue'
import type { Work } from '~/composables/useWorks'

const props = defineProps<{
  data: Work[]
}>()

const allStatus = ref([
  {
    status: 'NEW',
    class: 'bg-cyan-200 text-black'
  },
  {
    status: 'IN-PROGRESS',
    class: 'bg-teal-200 text-black'
  },
  {
    status: 'COMPLETED',
    class: 'bg-emerald-200 text-black'
  },
  {
    status: 'CANCEL',
    class: 'bg-gray-200 text-black'
  }
])
</script>

<template>
  <div class="flex flex-row gap-2 mt-2">
    <div
      v-for="item in allStatus"
      :key="item.status"
      class="flex flex-col w-1/4"
    >
      <div :class="item.class" class="p-2 rounded-xl h-full">
        <p class="font-bold  text-cyan-950 text-center mb-3">
          {{ item.status }}
        </p>
        <div v-for="work in props.data" :key="work.workId">
          <div
            v-if="
              work.orderStatus === item.status ||
              (item.status === 'NEW' && work.orderStatus === null)
            "
          >
            <WorksCard :data="work"/>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
