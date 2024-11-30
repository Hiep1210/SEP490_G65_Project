<script setup lang="ts">
import { computed } from 'vue'
import { Check, Languages, PackageCheck, PackagePlus, Truck } from 'lucide-vue-next'

const props = defineProps<{ orderStatus: string | undefined }>()

const steps = [{
  step: 1,
  title: 'New',
  value: 'NEW',
  description: 'Wait for the center to accept your order',
  icon: PackagePlus,
}, {
  step: 2,
  title: 'Accepted',
  value: 'ACCEPTED',
  description: 'Wait for the center to check your package',
  icon: PackageCheck,
}, {
  step: 3,
  title: 'In Progress',
  value: 'IN_PROGRESS',
  description: 'The center is working on your package',
  icon: Languages,
}, {
  step: 4,
  title: 'Completed',
  value: 'COMPLETED',
  description: 'Your package is ready for delivery',
  icon: Check,
}, {
  step: 5,
  title: 'Delivered',
  value: 'DELIVERED',
  description: 'Your package has been delivered',
  icon: Truck,
}]

const activeStep = computed(() => {
  return steps.find(step => step.value === props.orderStatus)?.step || 1
})

const isStepAchieved = (step: number) => {
  return step <= activeStep.value
}

console.log(activeStep.value)
</script>

<template>
  <div class="flex w-full items-start gap-2">
    <div 
      v-for="step in steps"
      :key="step.step"
      class="relative flex w-full flex-col items-center justify-center"
    >
      <Separator 
        v-if="step.step !== steps[steps.length - 1].step"
        class="absolute left-[calc(50%+20px)] right-[calc(-50%+10px)] top-5 block h-0.5 shrink-0 rounded-full bg-muted"
        :class="isStepAchieved(step.step + 1) ? 'bg-primary' : 'bg-muted'"/>
      <Button
        size="icon"
        class="z-10 rounded-full shrink-0 pointer-events-none"
        :class="isStepAchieved(step.step) ? ['ring-2 ring-ring ring-offset-2 ring-offset-background'] : ''"
        :variant="isStepAchieved(step.step) ? 'default' : 'outline'"
      >
        <component :is="step.icon" class="w-4 h-4 size-5" />
      </Button>
      <div class="mt-1 flex flex-col items-center text-center">
        <div
          class="text-sm font-semibold transition lg:text-base"
        >
          {{ step.title }}
        </div>
        <div
          class="sr-only text-xs text-muted-foreground transition md:not-sr-only lg:text-sm"
        >
          {{ step.description }}
        </div>
      </div>
    </div>
  </div>
</template>
