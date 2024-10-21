<script setup lang="ts">
import { ref } from 'vue'
import { Ellipsis } from 'lucide-vue-next'
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table'

// Define the order with items
const order = ref({
    id: 'TMS-1',
    name: 'Order 1',
    status: 'Processing',
    customerName: 'John Doe',
    sourceLanguage: 'English',
    targetLanguage: 'Spanish',
    issues: [{
        id: 'ISSUE-1',
        title: 'Missing invoice',
    },
    {
        id: 'ISSUE-2',
        title: 'Missing reference files',
    }],
    sourceFiles: [{
        id: 'FILE-1',
        name: 'Example.docx',
        status: 'Processing',
        targetLanguage: 'Spanish',
    },
    {
        id: 'FILE-2',
        name: 'Example2.docx',
        status: 'Processing',
        targetLanguage: 'Spanish',
    }],
    referenceFiles: ['Example.pdf'],
    services: ['Translation', 'Evaluation'],
})
</script>

<template>
    <div>
        <div class="flex flex-1 pb-5">
            <div class="pr-5 space-y-2">
                <div class="container mx-auto p-2 space-y-2 orderDetails">
                    <p class="text-[2rem] font-semibold">{{ order.name }}</p>
                    <div class="flex flex-col justify-items-end">
                        <span>
                            #{{ order.id }}
                        </span>
                        <span class="text-gray-500">
                            Status: {{ order.status }}
                        </span>
                        <span>
                            Customer: {{ order.customerName }}
                        </span>
                        <span class="flex space-x-1">
                            <Badge variant="default">{{ order.sourceLanguage }}</Badge>
                            <LucideArrowBigRight />
                            <Badge variant="secondary">{{ order.targetLanguage }}</Badge>
                        </span>
                    </div>
                </div>

                <Tabs default-value="working" class="w-full">
                    <TabsList class="grid w-full grid-cols-2">
                        <TabsTrigger value="working">
                            Working Files
                        </TabsTrigger>
                        <TabsTrigger value="reference">
                            Refernce Files
                        </TabsTrigger>
                    </TabsList>
                    <TabsContent value="working">
                        <div class="border rounded-md w-[52rem] h-[14rem]">
                            <Table>
                                <TableHeader>
                                    <TableRow>
                                        <TableHead>ID</TableHead>
                                        <TableHead>Name</TableHead>
                                        <TableHead>Status</TableHead>
                                        <TableHead>Target Language</TableHead>
                                    </TableRow>
                                </TableHeader>
                                <TableBody>
                                    <TableRow v-for="file in order.sourceFiles" :key="file.id">
                                        <TableCell>{{ file.id }}</TableCell>
                                        <TableCell>{{ file.name }}</TableCell>
                                        <TableCell>{{ file.status }}</TableCell>
                                        <TableCell>{{ file.targetLanguage }}</TableCell>
                                        <TableCell>
                                            <Button variant="ghost" size="sm">
                                                <Ellipsis />
                                            </Button>
                                        </TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </div>
                    </TabsContent>
                    <TabsContent value="reference">
                        <div class="border rounded-md w-[52rem] h-[14rem]">
                            <Table>
                                <TableHeader>
                                    <TableRow>
                                        <TableHead>ID</TableHead>
                                        <TableHead>Name</TableHead>
                                        <TableHead>Status</TableHead>
                                        <TableHead>Target Language</TableHead>
                                    </TableRow>
                                </TableHeader>
                                <TableBody>
                                    <TableRow v-for="file in order.sourceFiles" :key="file.id">
                                        <TableCell>{{ file.id }}</TableCell>
                                        <TableCell>{{ file.name }}</TableCell>
                                        <TableCell>{{ file.status }}</TableCell>
                                        <TableCell>{{ file.targetLanguage }}</TableCell>
                                        <TableCell>
                                            <Button variant="ghost" size="sm">
                                                <Ellipsis />
                                            </Button>
                                        </TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </div>
                    </TabsContent>
                </Tabs>

            </div>
            <div class="issuesList w-full space-y-2">
                <div class="head flex flex-1">
                    <div class="flex flex-1 text-center">
                        <span class="text-lg font-semibold text-center">Issues</span>
                    </div>
                    <Button variant="outline" size="sm">Add Issue</Button>
                </div>
                <div class="border rounded-md h-[25.3rem]">
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead>ID</TableHead>
                                <TableHead>Title</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            <TableRow v-for="issue in order.issues" :key="issue.id">
                                <TableCell>{{ issue.id }}</TableCell>
                                <TableCell>{{ issue.title }}</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </div>
            </div>
        </div>
        <Separator />
        <div class="border rounded-md bg-slate-300 h-[12rem]" />
    </div>
</template>
