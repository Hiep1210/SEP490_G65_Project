<script lang="ts" setup>
import { columns } from '~/components/Works/columns'
import { useWorks } from '~/composables/useWorks'
useSeoMeta({
  title: 'Works'
})

definePageMeta({
  layout: 'default'
})

const { works, getWorks } = useWorks()

// Fetch works on mount
onMounted(() => {
  getWorks()
})

</script>

<template>
  <div>
    <div class="h-[60vh]">
      <WorksTable :columns="columns" :data="works" />
    </div>
  </div>
</template>

<style></style>
