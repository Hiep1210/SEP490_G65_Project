<script setup lang="ts">
import { ref, watch } from 'vue'
import { Select, SelectContent, SelectGroup, SelectItem, SelectTrigger } from '@/components/ui/select'

// Props for the label and the sort key
const props = defineProps({
  label: {
    type: String,
    required: true
  },
  sortKey: {
    type: String,
    required: true
  },
  currentSortKey: {
    type: String,
    required: true
  },
  currentSortDirection: {
    type: String,
    required: true
  }
})

// Emit an event when sorting is changed
const emit = defineEmits(['update:sort'])

// Local state for tracking the selected sort direction
const selectedSortDirection = ref(props.currentSortDirection)

watch(selectedSortDirection, (newDirection) => {
  emit('update:sort', { key: props.sortKey, direction: newDirection })
})
</script>

<template>
  <div class="flex items-center gap-2">
    <span>{{ label }}</span>
    <Select v-model="selectedSortDirection">
      <SelectTrigger class="w-12 rounded-2xl p-2 border-none">
        <!-- <SelectValue placeholder="<>" /> -->
      </SelectTrigger>
      <SelectContent>
        <SelectGroup>
          <SelectItem value="asc">Ascending</SelectItem>
          <SelectItem value="desc">Descending</SelectItem>
          <SelectItem value="none">None</SelectItem>
        </SelectGroup>
      </SelectContent>
    </Select>
    <!-- Show the current sort direction if this header is sorted -->
  </div>
</template>
