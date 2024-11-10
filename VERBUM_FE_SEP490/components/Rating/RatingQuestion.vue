<script lang="ts" setup>
defineProps<{
  question: string
}>()
const rating = ref(0)
</script>
<template>
  <div>
    <div>
      <p>{{ question }}</p>
    </div>
    <div class="flex justify-center space-x-2 mb-6">
      <button
        v-for="star in 5"
        :key="star"
        :class="[
          'text-3xl focus:outline-none',
          rating >= star ? 'text-yellow-400' : 'text-gray-300'
        ]"
        @click="rating = star"
      >
        ★
      </button>
    </div>
  </div>
</template>
