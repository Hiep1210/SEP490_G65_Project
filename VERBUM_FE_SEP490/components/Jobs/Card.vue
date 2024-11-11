<script setup lang="ts">
import type { Job } from '~/types/job';

const props = defineProps<{
  data: Job
}>();
const emit = defineEmits(['click:card'])
</script>

<template>
  <div class="bg-slate-50 m-1 mb-3 rounded-lg p-2 h-20 w-[17rem] hover:bg-slate-100 cursor-pointer" @click="emit('click:card')">
    <div class="font-semibold truncate" :title="props.data.name">
      {{ props.data.name }}
    </div>
    <div>
      Target Language: {{ props.data.targetLanguageId }}
    </div>
  </div>
</template>