<script setup lang="ts">
import type { Order } from '~/types/order';

const { order, getOrder } = useOrders();
const route = useRoute();
const orderId = route.params.id;

const isEditing = ref(false);
const editedOrder = ref<Partial<Order> | null>(null);

onMounted(() => {
    getOrder(orderId);
});

// Enter edit mode
function enableEdit() {
    isEditing.value = true;
    editedOrder.value = { ...order.value }; // Clone current order data for editing
}

// Cancel edit mode
function cancelEdit() {
    isEditing.value = false;
    editedOrder.value = null; // Clear edited data
}

// Save edited order details
async function saveEdit() {
    try {
        await useAPI(`/order`, {
            method: 'PATCH',
            body: editedOrder.value,
        });
        if (order.value) {
            Object.assign(order.value, editedOrder.value as Order); // Cast to Order to ensure typing compatibility
        }
        isEditing.value = false;
    } catch (error) {
        console.error('Failed to save order:', error);
    }
}
const issues = [{
    id: 1,
    title: 'Issue 1',
}, {
    id: 2,
    title: 'Issue 2',
}, {
    id: 3,
    title: 'Issue 3',
}, {
    id: 4,
    title: 'Issue 4',
}, {
    id: 5,
    title: 'Issue 5',
}]
</script>

<template>
    <div>
        <div v-if="!order">
            <NuxtLoadingIndicator />
        </div>
        <div v-else class="flex flex-1 pb-5">
            <div class="pr-5 space-y-2">
                <div class="container mx-auto p-2 space-y-2 orderDetails">
                    <p class="text-[2rem] font-semibold">
                        <span v-if="!isEditing">{{ order?.orderName }}</span>
                        <input v-else v-model="editedOrder.orderName" class="text-2xl font-semibold border-1">
                    </p>
                    <div class="flex flex-col justify-items-end">
                        <span>#{{ order?.orderId }}</span>
                        <span v-if="!isEditing" class="text-gray-500">Status: {{ order?.orderStatus }}</span>
                        <input v-else v-model="editedOrder.orderStatus" placeholder="Status">

                        <span class="flex space-x-1">
                            <Badge variant="default">{{ order?.sourceLanguageId }}</Badge>
                            <LucideArrowBigRight />
                            <Badge variant="secondary">{{ order?.targetLanguageId }}</Badge>
                        </span>
                    </div>
                </div>

                <!-- Buttons for editing controls -->
                <div v-if="isEditing">
                    <Button @click="saveEdit">Save</Button>
                    <Button variant="outline" @click="cancelEdit">Cancel</Button>
                </div>
                <Button v-else @click="enableEdit">Edit Order</Button>

                <!-- Tabs and other order details remain unchanged -->
            </div>

            <div class="issuesList w-full space-y-2">
                <div class="head flex flex-1">
                    <div class="flex flex-1 text-center">
                        <span class="text-lg font-semibold text-center">Issues</span>
                    </div>
                    <Button variant="outline" size="sm">Add Issue</Button>
                </div>
                <div class="border rounded-md h-[25.3rem] overflow-auto">
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead>ID</TableHead>
                                <TableHead>Title</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            <TableRow v-for="issue in issues" :key="issue.id">
                                <TableCell>{{ issue.id }}</TableCell>
                                <TableCell>{{ issue.title }}</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </div>
            </div>
        </div>
    </div>
</template>
