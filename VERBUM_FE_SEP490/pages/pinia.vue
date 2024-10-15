<script setup>
import { Skeleton, SVGSkeleton } from '../components/ui/skeleton'
const taskStore = useTaskStore()

const filter = ref('all')
taskStore.getTasks()
</script>

<template>
  <div>
    <header
      class="text-center bg-gray-200 p-5 flex flex-col justify-center items-center"
    >
      <div class="flex justify-center items-end">
        <NuxtImg
          src="https://pinia.vuejs.org/logo.svg"
          alt="pinia logo"
          class="max-w-[60px] transform -rotate-10"
        />
        <h1
          class="m-0 text-2xl font-bold pt-6 ml-4 text-gray-500 transform rotate-2"
        >
          {{ taskStore.name }}
        </h1>
      </div>
      <div class="container w-72 mt-2">
        <PiniaTaskForm />
      </div>
    </header>

    <div class="flex justify-center mt-2">
      <div class="flex gap-2">
        <Button class="px-4 py-2 rounded" @click="filter = 'all'">
          All Tasks
        </Button>
        <Button class="px-4 py-2 rounded" @click="filter = 'favs'">
          Favs
        </Button>
        <Button class="px-4 py-2 rounded" @click="taskStore.$reset()">
          Reset
        </Button>
      </div>
    </div>

    <div v-if="taskStore.isLoading" class="container flex-col w-full">
      <div class="flex justify-center">
        <Skeleton class="w-[232px] max-w-full" />
      </div>
      <div class="flex flex-col items-center">
        <div class="h-14 w-72 border shadow-md p-3 m-2 flex">
          <h3>
            <Skeleton class="w-[120px] max-w-full" />
          </h3>
          <div class="flex ml-auto gap-2">
            <SVGSkeleton class="w-[24px] h-[24px]" />
            <SVGSkeleton class="w-[24px] h-[24px]" />
          </div>
        </div>
        <div class="h-14 w-72 border shadow-md p-3 m-2 flex">
          <h3>
            <Skeleton class="w-[120px] max-w-full" />
          </h3>
          <div class="flex ml-auto gap-2">
            <SVGSkeleton class="w-[24px] h-[24px]" />
            <SVGSkeleton class="w-[24px] h-[24px]" />
          </div>
        </div>
        <div class="h-14 w-72 border shadow-md p-3 m-2 flex">
          <h3>
            <Skeleton class="w-[120px] max-w-full" />
          </h3>
          <div class="flex ml-auto gap-2">
            <SVGSkeleton class="w-[24px] h-[24px]" />
            <SVGSkeleton class="w-[24px] h-[24px]" />
          </div>
        </div>
      </div>
    </div>

    <div v-if="!taskStore.isLoading && filter === 'all'" class="container">
      <p class="font-bold text-xl text-center">
        You have {{ taskStore.totalCount }} tasks left to do
      </p>
      <div
        v-for="task in taskStore.tasks"
        :key="task.id"
        class="flex justify-center"
      >
        <PiniaTaskDetails :task="task" />
      </div>
    </div>

    <div v-if="!taskStore.isLoading && filter === 'favs'" class="container">
      <p class="font-bold text-xl text-center">
        You have {{ taskStore.favsCount }} favs left to do
      </p>
      <div
        v-for="task in taskStore.favs"
        :key="task.id"
        class="flex justify-center"
      >
        <PiniaTaskDetails :task="task" />
      </div>
    </div>
  </div>
</template>
