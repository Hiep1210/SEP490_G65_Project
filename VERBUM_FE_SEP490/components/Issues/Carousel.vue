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

const inprogressIssues = computed(() => 
  props.issues.filter( 
    item => item.status === "OPEN"
  )
)
const items = ref(inprogressIssues.value) 

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
        v-for="item in items"
        :key="item.issueId"
        class="md:basis-1/2 lg:basis-1/3"
      >
        <Card>
          <CardHeader>
            <CardTitle>{{ item.issueName }}</CardTitle>
            <CardDescription>Status: {{ item.status }}</CardDescription>
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
</template>

<style></style>
