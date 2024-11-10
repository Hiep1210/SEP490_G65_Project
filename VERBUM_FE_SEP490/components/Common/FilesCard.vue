<script lang="ts" setup>

const props = defineProps<{
  fileName: String
}>()
</script>

<template>
  <div class="border rounded-xl flex flex-col gap-3 w-[150px]">
    <NuxtImg src="/img/file_icon.png" class="object-cover h-16 w-80"/>
  </div>
</template>