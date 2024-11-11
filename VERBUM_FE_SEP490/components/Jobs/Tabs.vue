<template>
  <div>
    <Tabs default-value="document" class="w-full">
      <TabsList class="grid w-full grid-cols-2">
        <TabsTrigger value="document">Document</TabsTrigger>
        <TabsTrigger value="deliverable">Deliverable</TabsTrigger>
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
    </Tabs>
  </div>
</template>

<script lang="ts" setup>
import type { Job } from '~/types/job';

defineProps({
  job: {
    type: Object as PropType<Job>,
    default: () => ({})
  }
})
</script>

<style>

</style>