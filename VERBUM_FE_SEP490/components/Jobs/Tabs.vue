<template>
  <div>
    <Tabs default-value="document" class="w-full">
      <TabsList class="grid w-full" :class="grids" >
        <TabsTrigger value="document">Document</TabsTrigger>
        <TabsTrigger value="references">References</TabsTrigger>
        <TabsTrigger value="deliverable">Deliverable</TabsTrigger>
        <TabsTrigger v-if="Object.keys(job.previousJobDeliverables).length > 0" value="relatedJob">Related Jobs</TabsTrigger>
      </TabsList>
      <TabsContent value="document">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div
            v-if="
            !job.documentUrl
            "
            class="p-2 text-center"
          >
            There are no working files
          </div>
          <div v-else class="p-2"><a class="hyper-link" :href="job.documentUrl">{{ getFirebaseFileName(job.documentUrl) }}</a></div>
        </div>
      </TabsContent>
      <TabsContent value="references">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div
            v-if="
            !job.referenceUrls || job.referenceUrls.length === 0
            "
            class="p-2 text-center"
          >
            There are no reference files
          </div>
          <div v-else class="p-2">
            <a v-for="file in job.referenceUrls" :key="file" class="hyper-link">
              {{ getFirebaseFileName(file) }}
            </a>
          </div>
        </div>
      </TabsContent>
      <TabsContent value="relatedJob">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div
            v-if="
              !job.deliverableUrl
            "
            class="p-2 text-center"
          >
            There are no deliverables
          </div>
          <div v-else class="p-2"><a class="hyper-link" :href="job.deliverableUrl">{{ getFirebaseFileName(job.deliverableUrl) }}</a></div>
        </div>
      </TabsContent>
      <TabsContent value="prevDeli">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div
            v-if="
              !(Object.keys(job.previousJobDeliverables).length > 0)
            "
            class="p-2 text-center"
          >
            Previous service's deliverable has not been completed
          </div>
          <div v-else class="p-2">
            <div v-for="(label, file) in job.previousJobDeliverables" :key="file">
              <div class="space-x-6">
                <Badge>{{ label }}</Badge>
                <a :href="file" class="hyper-link">{{ getFirebaseFileName(file) }}</a>
              </div>
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
      previousJobDeliverables: {},
    })
  }
})
const grids = computed(() => {
  return Object.keys(props.job.previousJobDeliverables).length > 0 ? 'grid-cols-4' : 'grid-cols-3'
})
</script>

<style>

</style>