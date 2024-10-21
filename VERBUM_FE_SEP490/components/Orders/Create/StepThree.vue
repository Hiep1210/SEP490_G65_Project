<script setup lang="ts">
import { useFileDialog } from '@vueuse/core'
import { ref, watch, computed } from 'vue'
import { cn } from '@/lib/utils'
import { ref as storageRef, getDownloadURL } from 'firebase/storage'

const storage = useFirebaseStorage()
const downloadUrls = ref<string[]>([])

const downloadUrlsString = computed(() => downloadUrls.value.join(','))

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

const { files, open } = useFileDialog()

watch(files, () => {
  if (files.value?.length) {
    uploadFiles()
  }
})
</script>

<template>
  <FormField v-slot="{ componentField }" name="reference">
    <FormItem>
      <FormLabel>Reference</FormLabel>
      <FormControl>
        <Textarea
          placeholder="Tell us further details, notes, references, instructions or any special requests"
          class="resize-none"
          v-bind="componentField"
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
    <FormItem class="flex flex-col">
      <FormLabel>Reference Files</FormLabel>
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
</template>
