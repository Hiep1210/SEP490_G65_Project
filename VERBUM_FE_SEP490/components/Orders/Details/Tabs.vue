<template>
  <div>
    <Tabs default-value="working" class="w-full">
      <TabsList class="grid w-full grid-cols-3">
        <TabsTrigger value="working">Working Files</TabsTrigger>
        <TabsTrigger value="reference">Reference Files</TabsTrigger>
        <TabsTrigger value="deliverable">Deliverable Files</TabsTrigger>
      </TabsList>
      <TabsContent value="working">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div v-if="!Array.isArray(order.translationFileUrls) || !order.translationFileUrls.length" class="p-2 text-center">
              There are no working files, try refreshing the page
          </div>
          <Table v-else>
            <TableHeader>
              <TableRow>
                <TableHead>URL</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              <TableRow v-for="file in order.translationFileUrls" :key="file">
                <TableCell>{{ file }}</TableCell>
                <TableCell>
                  <Button variant="outline" size="sm">
                    Options
                  </Button>
                </TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </div>
      </TabsContent>
      <TabsContent value="reference">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div v-if="!Array.isArray(order.referenceFileUrls) || !order.referenceFileUrls.length" class="p-2 text-center">
              There are no reference files
          </div>
          <Table v-else>
            <TableHeader>
              <TableRow>
                <TableHead>URL</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              <TableRow v-for="file in order.referenceFileUrls" :key="file">
                <TableCell>{{ file }}</TableCell>
                <TableCell>
                  <Button variant="outline" size="sm">
                    Options
                  </Button>
                </TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </div>
      </TabsContent>
      <TabsContent value="deliverable">
        <div class="border rounded-md h-max-[18rem] overflow-auto">
          <div v-if="!Array.isArray(order.deliverableFileUrls) || !order.deliverableFileUrls.length" class="p-2 text-center">
              There are no deliverable files
          </div>
          <Table v-else>
            <TableHeader>
              <TableRow>
                <TableHead>URL</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              <TableRow v-for="file in order.deliverableFileUrls" :key="file">
                <TableCell>{{ file }}</TableCell>
                <TableCell>
                  <Button variant="outline" size="sm">
                    Options
                  </Button>
                </TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </div>
      </TabsContent>
    </Tabs>
  </div>
</template>

<script lang="ts" setup>
defineProps({
  order: {
    type: Object,
    default: () => ({ translationFileUrls: [], referenceFileUrls: [], deliverableFileUrls: [] })
  }
})

</script>