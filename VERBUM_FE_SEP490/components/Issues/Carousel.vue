<script lang="ts" setup>
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardFooter
} from '@/components/ui/card'
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious
} from '@/components/ui/carousel'
import type { Issue } from '~/types/issues'


const props = defineProps<{
  issues: Issue[]
}>();

const acceptedIssues = (data: Issue[]) => {
  return data.filter( 
    item => item.status === "ACCEPTED"
  )
}
const items = ref(props.issues) 

const showIssuesDialog = ref(false)
const selectedData = ref();
const emit = defineEmits(['update'])

const openIssuesDialog = (data: Issue) => {
  selectedData.value = data;
  showIssuesDialog.value = true;
}
const closeIssuesDialog = () => {
  selectedData.value = '';
  showIssuesDialog.value = false;
}

const updateIssueInTable = (updatedIssue: Issue) => {
  emit('update', updatedIssue)
  closeIssuesDialog()
}
watch(
  () => props.issues,
  (newList) => {
    items.value = [...newList]
  },
  { deep: true }
)

</script>

<template>
  <Carousel class="w-full max-w-[80vw] px-5">
    <CarouselContent>
      <CarouselItem
        v-for="item in acceptedIssues(items)"
        :key="item.issueId"
        class="md:basis-1/2 lg:basis-1/3"
        @click="openIssuesDialog(item)"
      >
        <Card>
          <CardHeader>
            <CardTitle>{{ item.issueName }}</CardTitle>
            <CardDescription>{{ item.issueDescription }}</CardDescription>
          </CardHeader>
          <CardFooter>
            <Badge>{{ item.issueAttachments.length }} attachments</Badge>
          </CardFooter>
        </Card>
      </CarouselItem>
    </CarouselContent>
    <CarouselPrevious />
    <CarouselNext />
  </Carousel>
  <IssuesDialog
  v-if="showIssuesDialog"
  :row-data="selectedData"
  :open="showIssuesDialog"
  @close="closeIssuesDialog"
  @update="updateIssueInTable"
  />
</template>

<style></style>
