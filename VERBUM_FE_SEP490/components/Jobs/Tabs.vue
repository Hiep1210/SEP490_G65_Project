<template>
  <div>
    <Tabs default-value="document" class="w-full">
      <TabsList class="grid w-full" :class="grids" >
        <TabsTrigger value="document">Document</TabsTrigger>
        <TabsTrigger value="references">References</TabsTrigger>
        <TabsTrigger value="deliverable">Deliverable</TabsTrigger>
        <TabsTrigger v-if="props.job.previousJobDeliverables.length > 0" value="prevDeli">Previous Deliverables</TabsTrigger>
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
          <div v-else class="p-2"><a :href="job.documentUrl">{{ getFirebaseFileName(job.documentUrl) }}</a></div>
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
            <div v-for="file in job.referenceUrls" :key="file">
              {{ getFirebaseFileName(file) }}
            </div>
          </div>
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
          <div v-else class="p-2"><a :href="job.deliverableUrl">{{ getFirebaseFileName(job.deliverableUrl) }}</a></div>
        </div>
      </TabsContent>
      <TabsContent value="prevDeli">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div
            v-if="
              !(job.previousJobDeliverables.length > 0)
            "
            class="p-2 text-center"
          >
            There are no previous deliverables, try refreshing the page
          </div>
          <div v-else class="p-2">
            <!-- <a v-for="file in job.previousJobDeliverables" :key="file" :href="file">
              {{ getFirebaseFileName(file) }}
            </a> -->
            <div v-for="(label, file) in previousJobDeliverables" :key="file">
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
      previousJobDeliverables: '',
    })
  }
})
const grids = computed(() => {
  return props.job?.previousJobDeliverables?.length > 0 ? 'grid-cols-4' : 'grid-cols-3'
})
const previousJobDeliverables =  {
        "https://firebasestorage.googleapis.com/v0/b/verbum-sep490.appspot.com/o/uploads%2FSCP%20Reading%20Consent%20Form%202024-25.docx?alt=media&token=a0806aa4-2e13-4c3b-bccd-8c3405e43995": "Translate"
      }
</script>

<style>

</style>