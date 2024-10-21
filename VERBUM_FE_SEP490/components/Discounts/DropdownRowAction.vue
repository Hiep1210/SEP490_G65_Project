<script setup>
import Button from '@/components/ui/button/Button.vue'; // Adjust to your Button component path
import DropdownMenu from '@/components/ui/dropdown-menu/DropdownMenu.vue'; // Adjust paths accordingly
import DropdownMenuContent from '@/components/ui/dropdown-menu/DropdownMenuContent.vue';
import DropdownMenuItem from '@/components/ui/dropdown-menu/DropdownMenuItem.vue';
import DropdownMenuTrigger from '@/components/ui/dropdown-menu/DropdownMenuTrigger.vue';

import UpdateDialog from './UpdateDialog.vue'


</script>

<template>
  <DropdownMenu>
    
    <DropdownMenuTrigger as-child>
      <Button
        variant="ghost"
        class="flex h-8 w-8 p-0 data-[state=open]:bg-muted"
      > ...
        <DotsHorizontalIcon class="h-4 w-4" />
        <span class="sr-only">Open menu</span>
      </Button>
    </DropdownMenuTrigger>
    <DropdownMenuContent align="end" class="w-[160px]">
      <DropdownMenuItem ><UpdateDialog/></DropdownMenuItem>
      <DropdownMenuItem>
        Delete
      </DropdownMenuItem>
    </DropdownMenuContent>
  </DropdownMenu>
</template>
