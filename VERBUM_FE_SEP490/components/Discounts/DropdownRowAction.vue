<script lang="ts" setup>
import { ref, defineEmits } from 'vue';
import Button from '@/components/ui/button/Button.vue';
import DropdownMenu from '@/components/ui/dropdown-menu/DropdownMenu.vue';
import DropdownMenuItem from '@/components/ui/dropdown-menu/DropdownMenuItem.vue';
import DropdownMenuTrigger from '@/components/ui/dropdown-menu/DropdownMenuTrigger.vue';
import UpdateDialog from './UpdateDialog.vue';
import type { Discount } from '~/types/discount';

const props = defineProps<{
  rowData: Discount;  // Receive row data as a prop
}>();

const showDialog = ref(false);  // Track dialog visibility
const emit = defineEmits(['delete', 'update']); // Emit events for actions

const openDialog = () => {
  showDialog.value = true;
};

const closeDialog = () => {
  showDialog.value = false;
};

const deleteRow = () => {
  emit('delete', props.rowData);  // Emit delete event with row data
};
</script>


<template>
  <div>
    <DropdownMenu>
      <DropdownMenuTrigger as-child>
        <Button variant="ghost" class="flex h-8 w-8 p-0 data-[state=open]:bg-muted">
          ...
          <span class="sr-only">Open menu</span>
        </Button>
      </DropdownMenuTrigger>

      <DropdownMenuContent align="end" class="w-[160px]">
        <DropdownMenuItem @click="openDialog">
          Edit
        </DropdownMenuItem>
        <DropdownMenuItem @click="deleteRow">
          Delete
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>

    <UpdateDialog
      v-if="showDialog"
      :open="showDialog"
      :row-data="props.rowData"
      @close="closeDialog"
    />
  </div>
</template>

