<template>
  <div>
    <Tabs default-value="working" class="w-full">
      <TabsList class="grid w-full" :class="haveDeletedFiles">
        <TabsTrigger value="working">Working Files</TabsTrigger>
        <TabsTrigger value="reference">Reference Files</TabsTrigger>
        <TabsTrigger value="deliverable">Deliverable Files</TabsTrigger>
        <TabsTrigger v-if="order && order.deleteddFileUrls && order.deleteddFileUrls.length > 0" value="deleted">Deleted Files</TabsTrigger>
      </TabsList>
      <TabsContent value="working">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div 
            v-if="
            !Array.isArray(order.translationFileUrls) ||
            !order.translationFileUrls.length
          " class="p-2 text-center">
            There are no working files, try refreshing the page
          </div>
          <Table v-else>
            <TableHeader>
              <TableRow>
                <TableHead>URL</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              <TableRow v-for="file in order.translationFileUrls" :key="file">
                <TableCell>{{ getFirebaseFileName(file) }}</TableCell>
                <TableCell>
                  <OrdersDetailsOptions :id="order.orderId" :url="file" :is-deleted="false" :is-delivered="false" :is-new-or-rejected="isNewOrRejected" />
                </TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </div>
      </TabsContent>
      <TabsContent value="reference">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div 
            v-if="
            !Array.isArray(order.referenceFileUrls) ||
            !order.referenceFileUrls.length
          " class="p-2 text-center">
            There are no reference files
          </div>
          <Table v-else>
            <TableHeader>
              <TableRow>
                <TableHead>URL</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              <TableRow v-for="file in order.referenceFileUrls" :key="file">
                <TableCell>{{ getFirebaseFileName(file) }}</TableCell>
                <TableCell>
                  <OrdersDetailsOptions :id="order.orderId" :url="file" :is-deleted="false" :is-delivered="false" :is-new-or-rejected="isNewOrRejected" />
                </TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </div>
      </TabsContent>
      <TabsContent value="deliverable">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div
            v-if="
            !Array.isArray(order.jobDeliverables) ||
            !order.jobDeliverables.length
          " class="p-2 text-center">
            There are no deliverable files
          </div>
          <Table v-else>
            <TableHeader>
              <TableRow>
                <TableHead>URL</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              <TableRow v-for="deliverable in getElementsWithHighestServiceOrder(order.jobDeliverables)" :key="deliverable.deliverableFileUrl">
                <TableCell>{{ getFirebaseFileName(deliverable.deliverableFileUrl || '') }}</TableCell>
                <TableCell>
                  <OrdersDetailsOptions :id="order.orderId" :url="deliverable.deliverableFileUrl || ''" :is-delivered="true" :is-new-or-rejected="isNewOrRejected"/>
                </TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </div>
      </TabsContent>
      <TabsContent value="deleted">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div 
            v-if="
            !Array.isArray(order.deleteddFileUrls) ||
            !order.deleteddFileUrls.length
          " class="p-2 text-center">
            There are no deleted files
          </div>
          <div v-else>
            <TableHeader>
              <TableRow>
                <TableHead>URL</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              <TableRow v-for="file in order.deleteddFileUrls" :key="file">
                <TableCell>{{ getFirebaseFileName(file) }}</TableCell>
                <TableCell>
                  <OrdersDetailsOptions :id="order.orderId" :url="file" :is-deleted="true" :is-new-or-rejected="isNewOrRejected"/>
                </TableCell>
              </TableRow>
            </TableBody>
          </div>
        </div>
      </TabsContent>
    </Tabs>
  </div>
</template>

<script lang="ts" setup>
import { getFirebaseFileName } from '@/utils/getFirebaseFileName'
import type { JobDeliverables } from '~/types/jobDeliverables'
import type { Order } from '~/types/order';
const props = defineProps({
  order: {
    type: Object as () => Order,
    default: () => ({
      translationFileUrls: [],
      referenceFileUrls: [],
      deliverableFileUrls: [],
      deleteddFileUrls: [],
      orderId: ''
    }) as Order
  }
})
const haveDeletedFiles = computed(() =>
  props.order.deleteddFileUrls && props.order.deleteddFileUrls.length > 0 ? 'grid-cols-4' : 'grid-cols-3'
)

const getElementsWithHighestServiceOrder = (
  jobDeliverables: JobDeliverables[]
): JobDeliverables[] => {
  let maxServiceOrder = -Infinity
  const result: JobDeliverables[] = []

  for (const item of jobDeliverables) {
    if (item.serviceOrder > maxServiceOrder) {
      maxServiceOrder = item.serviceOrder
      result.length = 0
      result.push(item)
    } else if (item.serviceOrder === maxServiceOrder) {
      result.push(item)
    }
  }

  return result
}

const isNewOrRejected = computed(() => props.order.orderStatus === 'NEW' || props.order.orderStatus === 'REJECTED')
</script>
