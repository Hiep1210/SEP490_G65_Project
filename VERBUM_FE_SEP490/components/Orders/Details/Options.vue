<template>
  <div>
    <DropdownMenu>
      <DropdownMenuTrigger as-child><Button variant="outline">Options</Button></DropdownMenuTrigger>
      <DropdownMenuContent>
        <DropdownMenuItem @click="onClick('Download')">Download</DropdownMenuItem>
        <DropdownMenuItem v-if="!isDeleted" @click="onClick('Delete')">Delete</DropdownMenuItem>
        <DropdownMenuItem v-else @click="onClick('Recover')">Recover</DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  </div>
</template>

<script lang="ts" setup>
import { useToast } from '~/components/ui/toast';

const props = defineProps<{
  id: string,
  url: string,
  isDeleted?: boolean
}>()

const { toast } = useToast()

const onClick = async (options: string) => {
  switch (options) {
    case "Download":
      window.open(props.url, '_blank');
      break;

    case "Delete":
      {
        const { status } = await useAPI("/order/file", {
          method: 'DELETE',
          query: {
            orderId: props.id,
            fileURl: props.url
          }
        });

        if (status.value === "error") {
          toast({
            title: "Can't delete file",
            description: "Try again",
            variant: 'destructive'
          });
        } else if (status.value === "success") {
          toast({
            title: "Deleted successfully",
            description: "File is deleted",
          });
          window.location.reload();
        }
      }
      break;
    case "Recover":
      {
        const { status } = await useAPI("/order/file-recover", {
          method: 'PUT',
          query: {
            orderId: props.id,
            fileURl: props.url
          }
        });

        if (status.value === "error") {
          toast({
            title: "Can't recover file",
            description: "Try again",
            variant: 'destructive'
          });
        } else if (status.value === "success") {
          toast({
            title: "Recovered successfully",
            description: "File is recovered",
          });
          window.location.reload();
        }
      }
      break;

      default:
      console.warn("Unknown option:", options);
      break;
  }
};

</script>

<style></style>