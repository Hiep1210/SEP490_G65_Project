<template>
  <div>
    <DropdownMenu>
      <DropdownMenuTrigger as-child><Button variant="outline">Options</Button></DropdownMenuTrigger>
      <DropdownMenuContent>
        <DropdownMenuItem @click="onClick('Download')">Download</DropdownMenuItem>
        <DropdownMenuItem @click="onClick('Delete')">Delete</DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  </div>
</template>

<script lang="ts" setup>
import { useToast } from '~/components/ui/toast';

const props = defineProps<{
  id: string,
  url: string
}>()

const { toast } = useToast()

const onClick = async (options: string) => {
  if (options === "Download") {
    window.open(props.url, '_blank')
  }
  else if (options === "Delete") {
    const { status } = await useAPI("/order/file", {
      method: 'DELETE',
      query: {
        orderId: props.id,
        fileURI: props.url
      }
    })
    if (status.value === "error") {
      toast({
        title: "Can't delete file",
        description: "Try again",
        variant: 'destructive'
      })
    }
    else if (status.value === "success") {
      toast({
        title: "Deleted successfully",
        description: "File is deleted",
      })
    }
  }
}
</script>

<style></style>