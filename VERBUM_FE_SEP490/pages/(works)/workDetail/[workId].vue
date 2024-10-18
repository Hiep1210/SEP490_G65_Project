<script lang="ts" setup>
import type { Category } from '~/components/Category/category';
import type { Work } from '~/components/Works/work'
import WorkDetail from '~/components/Works/WorkDetail.vue'
import WorkIssues from '~/components/Works/WorkIssues.vue';

const token = useCookie('access_token')
const route = useRoute()
const workId = route.params.workId

useSeoMeta({
  title: 'Work Details'
})
definePageMeta({
  name: 'workDetail-workId'
})

const { data, error } = await useAsyncData<Work[]>('availableData', () =>
  $fetch('http://localhost:8000/api/work/get-all', {
    headers: {
      Authorization: `Bearer ${token.value}`
    }
  })
)

const {data: categoryData, error: categoryError} = await useAsyncData<Category[]>('categoryData', () =>
  $fetch('http://localhost:8000/api/category/get-all', {
    headers: {
      Authorization: `Bearer ${token.value}`
    }
  })
)

if (error || categoryError) {
  console.error('Failed to fetch data:', error)
}
const allWorks: Work[] = data.value || []
const allCategory: Category[] = categoryData.value || []
const selectedWork = allWorks.find((work) => work.workId === workId)
</script>

<template>
  <div class="">
    <div class="flex gap-2">
      <div class="flex-auto m-3">
        <WorkDetail 
        v-if="selectedWork && allCategory" 
        :work="selectedWork" 
        :categories="allCategory"
        />
      </div>
      <div class="flex-none w-1/3 bg-slate-400"><WorkIssues/></div>
    </div>
    <div class="">This is description of job!!!</div>
  </div>
</template>

<style></style>
