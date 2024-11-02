<script lang="ts" setup>
// import { columns } from '~/components/Works/columns';
import { useWorks } from '~/composables/useWorks';
useSeoMeta({
  title: 'Works',
})

definePageMeta({
    layout: 'default',
})

const {works, getWorks} = useWorks();

onMounted(() => {
  if (!works.value.length) {
    getWorks()
  }
})


</script>

<template>
  <div>
    <!-- <WorksTable :columns="columns" :data="works"/> -->
  
    <WorksStatusColumn :data="works"/>
  </div>
</template>

<style></style>

