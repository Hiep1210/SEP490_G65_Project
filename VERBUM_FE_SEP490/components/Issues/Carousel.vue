<script lang="ts" setup>
import { Card } from '@/components/ui/card'
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious
} from '@/components/ui/carousel'
const items = Array.from({ length: 5 }, (_, i) => i + 1)
</script>

<template>
  <Carousel class="w-full max-w-[80vw] px-5">
    <CarouselContent>
      <CarouselItem
        v-for="item in items"
        :key="item"
        class="md:basis-1/2 lg:basis-1/3"
      >
        <Card>
          <CardHeader>
            <CardTitle>ORDER00{{ item }}</CardTitle>
            <CardDescription
              >Lorem ipsum dolor sit amet consectetur, adipisicing elit. Dolorum
              at nam sit alias eveniet adipisci qui nesciunt earum illo
              perferendis pariatur fugit voluptatem eaque, possimus voluptatibus
              excepturi? Non, doloremque obcaecati.</CardDescription
            >
          </CardHeader>
          <CardFooter>
            <Badge>Status</Badge>
          </CardFooter>
        </Card>
      </CarouselItem>
    </CarouselContent>
    <CarouselPrevious />
    <CarouselNext />
  </Carousel>
</template>

<style></style>
