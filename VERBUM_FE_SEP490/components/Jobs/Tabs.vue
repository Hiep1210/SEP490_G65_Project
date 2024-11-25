<template>
  <div>
    <Tabs default-value="document" class="w-full">
      <TabsList class="grid w-full" :class="grids" >
        <TabsTrigger value="document">Document</TabsTrigger>
        <TabsTrigger value="deliverable">Deliverable</TabsTrigger>
        <TabsTrigger value="prevDeli">Previous Deliverables</TabsTrigger>
      </TabsList>
      <TabsContent value="document">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div
            v-if="
            !job.documentUrl
            "
            class="p-2 text-center"
          >
            There are no working files, try refreshing the page
          </div>
          <div v-else class="p-2">{{ getFirebaseFileName(job.documentUrl) }}</div>
        </div>
      </TabsContent>
      <TabsContent value="deliverable">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div
            v-if="
              !job.deliverableUrl
            "
            class="p-2 text-center"
          >
            There are no deliverables, try refreshing the page
          </div>
          <div v-else class="p-2">{{ getFirebaseFileName(job.deliverableUrl) }}</div>
        </div>
      </TabsContent>
      <TabsContent value="prevDeli">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div
            v-if="
              !job.previousJobDeliverables
            "
            class="p-2 text-center"
          >
            There are no previous deliverables, try refreshing the page
          </div>
          <div v-else class="p-2">
            <div v-for="file in job.previousJobDeliverables" :key="file">
              {{ getFirebaseFileName(file) }}
            </div>
          </div>
        </div>
      </TabsContent>
    </Tabs>
  </div>
</template>

<script lang="ts" setup>
import type { Job } from '~/types/job';

const props = defineProps({
  job: {
    type: Object as PropType<Job> | undefined,
    default: () => ({
      documentUrl: '',
      deliverableUrl: '',
      previousJobDeliverables: '',
    })
  }
})
const grids = computed(() => {
  return props.job?.previousJobDeliverables?.length ? 'grid-cols-3' : 'grid-cols-2'
})
</script>

<style>

</style>