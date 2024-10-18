<script lang="ts" setup>
import type { Work } from '~/components/Works/work';
import WorksTable from '~/components/Works/WorksTable.vue';
const token = useCookie('access_token');

useSeoMeta({
  title: 'Works',
})

const { data: availableData, error: availableError } = await useAsyncData(
  'availableData',
  () => $fetch('http://localhost:8000/api/lang')
)

const { data, error } = await useAsyncData<Work[]>(
  'availableData',
  () => $fetch('http://localhost:8000/api/work/get-all', {
    headers: {
      Authorization: `Bearer ${token.value}`
    }
  })
);

if (error || availableError) {
  console.error(
    'Failed to fetch data:',
    error
  )
}
console.log("here",availableData)
const allWorks: Work[] = data.value || [];


</script>

<template>
  <WorksTable 
  :all-works="allWorks"
  />
</template>

<style></style>
