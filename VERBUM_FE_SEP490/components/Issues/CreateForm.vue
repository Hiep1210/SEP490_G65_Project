<script setup lang="ts">
import { useFileDialog } from '@vueuse/core'
import { ref, watch, computed } from 'vue'
import { cn } from '@/lib/utils'
import { ref as storageRef, getDownloadURL } from 'firebase/storage'
import type { IssueAttachment } from '~/types/issues';
import { useIssues } from '~/composables/useIssues'

const storage = useFirebaseStorage()
const downloadUrls = ref<string[]>([])
const isConfirmDialogOpen = ref(false)

const titleStatusConfirm = "Create issue?";
const descriptionStatusConfirm = "We apologize for any errors in our work that do not align with your requirements. This issue will be created as what you wrote, and we will check and notify you! "

const downloadUrlsString = computed(() => downloadUrls.value.join(','))
const {createIssue} = useIssues();

const validateForm = () => {
  return newIssue.value.issueName.trim() !== '' && newIssue.value.issueDescription.trim() !== '';
};

async function uploadFiles() {
  if (files.value?.length) {
    const promises = Array.from(files.value).map(async (file) => {
      const fileRef = storageRef(storage, `uploads/${file.name}`)
      const { upload } = useStorageFile(fileRef)

      await upload(file)

      const url = await getDownloadURL(fileRef)
      return url
    })

    const urls = await Promise.all(promises)
    downloadUrls.value = [...downloadUrls.value, ...urls]
  }
}

interface NewIssue {
  orderId: string, 
  issueName: string,
  issueDescription: string,
  issueAttachments: IssueAttachment[]
}

const props = defineProps<{
  orderId: string
}>()
const newIssue = ref<NewIssue>({
  orderId: props.orderId,
  issueName: '',
  issueDescription: '',
  issueAttachments: []
})

const handleCreateIssue = async (newIssue: NewIssue) => {
  await createIssue(newIssue.issueName, newIssue.orderId, newIssue.issueDescription, newIssue.issueAttachments);
}

const openConfirmDialog = () => {
  if (validateForm()) {
    isConfirmDialogOpen.value = true;
  } else {
    // Handle the error (e.g., show a message to the user)
    alert('Please fill in all required fields.');
  }
};

const handleConfirmCreate = () => {
  handleCreateIssue(newIssue.value)
  isConfirmDialogOpen.value = false // Close the confirmation dialog
}

const handleCancelConfirm = () => {
  isConfirmDialogOpen.value = false
}

const { files, open } = useFileDialog()

watch(files, () => {
  if (files.value?.length) {
    uploadFiles()
  }
})
</script>

<template>
  

  <FormField  v-slot="{ componentField }" name="issueName" >
    <FormItem>
      <FormLabel>Title</FormLabel>
      <FormControl>
        <Textarea
          v-model="newIssue.issueName"
          placeholder="The tile for this issue"
          class="resize-none"
          v-bind="componentField"
          required
        />
      </FormControl>
      <FormMessage />
    </FormItem>
  </FormField>

  <FormField v-slot="{ componentField }" name="issueDescription" >
    <FormItem class="mt-2">
      <FormLabel>Details</FormLabel>
      <FormControl>
        <Textarea
        v-model="newIssue.issueDescription"
          placeholder="Tell us whats the issues our translation having"
          class="resize-none"
          v-bind="componentField"
          required
        />
      </FormControl>
      <FormMessage />
    </FormItem>
  </FormField>

  <FormField
    v-slot="{ componentField }"
    name="referenceFileURLs"
    :model-value="downloadUrlsString"
  >
    <FormItem class="flex flex-col align-middle gap-2 mt-2" >
      <FormLabel class="my-auto ">Reference Files</FormLabel>
      <FormControl>
        <Button type="button" @click="open({ accept: '*', multiple: true })">
          Upload Files
        </Button>
        <Input
          type="hidden"
          v-bind="componentField"
          :value="downloadUrlsString"
        />
      </FormControl>
      <Card v-if="files?.length" :class="cn($attrs.class ?? '')">
        <CardHeader>
          <CardDescription>Uploaded files</CardDescription>
        </CardHeader>
        <CardContent class="grid gap-4">
          <div>
            <div
              v-for="file in files"
              :key="file.name"
              class="mb-4 grid grid-cols-[25px_minmax(0,1fr)] items-start pb-4 last:mb-0 last:pb-0"
            >
              <span
                class="flex h-2 w-2 translate-y-1 rounded-full bg-sky-500"
              />
              <div class="space-y-1">
                <p class="text-sm font-medium leading-none">
                  {{ file.name }}
                </p>
              </div>
            </div>
          </div>
        </CardContent>
      </Card>
      <FormMessage />
    </FormItem>
  </FormField>

  <div class="flex justify-end gap-2 mt-5"> 
    <Button class="bg-slate-500 hover:bg-slate-600"> Cancel</Button>
    <Button @click="openConfirmDialog"> Create issue</Button>
  </div>

  <IssuesConfirmDialog
  :title="titleStatusConfirm"
  :description="descriptionStatusConfirm"
  :open="isConfirmDialogOpen"
  @close="handleCancelConfirm"
  @confirm="handleConfirmCreate"
/>
</template>
