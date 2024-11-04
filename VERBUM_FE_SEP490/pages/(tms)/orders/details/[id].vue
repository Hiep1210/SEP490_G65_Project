<script setup lang="ts">
import type { Order } from '~/types/order';

const { order, getOrder, cancelOrder, acceptorDeclineOrder } = useOrders();
const route = useRoute();
const orderId = route.params.id;
const { user } = useAuthStore()
const role = user?.role
const isEditing = ref(false);
const editedOrder = ref<Partial<Order> | null>(null);


onMounted(() => {
    getOrder(orderId);
});

// Enter edit mode
const enableEdit = () => {
    isEditing.value = true;
    if (order.value) {
        const { translationFileUrls, referenceFileUrls, deliverableFileUrls, createdDate, discountId, paymentStatus, orderStatus, ...rest } = order.value as Order;
        editedOrder.value = {
            ...rest,
            targetLanguageId: [...(order.value.targetLanguageId || [])],
            sourceLanguageId: order.value.sourceLanguageId
        };
    }
};

// Cancel edit mode
const cancelEdit = () => {
    isEditing.value = false;
    editedOrder.value = null;
};

// Save edited order details
const saveEdit = async () => {
    try {
        if (editedOrder.value) {
            const payload = {
                ...editedOrder.value,
                dueDate: editedOrder.value.dueDate
                    ? new Date(editedOrder.value.dueDate).toLocaleDateString('sv')
                    : null,
                targetLanguageIdList: editedOrder.value?.targetLanguageId,
                translateService: editedOrder.value?.hasTranslateService,
                editService: editedOrder.value?.hasEditService,
                evaluateService: editedOrder.value?.hasEvaluateService,
                discountId: editedOrder.value?.discountId
            };
            await useAPI(`/order/update`, {
                method: 'PUT',
                body: JSON.stringify(payload),
                headers: {
                    'Content-Type': 'application/json',
                },
            });
        }
        else {
            throw new Error('No edited order found');
        }

        if (order.value) {
            Object.assign(order.value, editedOrder.value as Order);
        }
        isEditing.value = false;
    } catch (error) {
        console.error('Failed to save order:', error);
    }
};
// Add discount
const openAddDiscount = ref(false);
const addDiscount = async () => {
    try {
        if (editedOrder.value) {
            const payload = {
                ...editedOrder.value,
                discountId: editedOrder.value?.discountId
            }
            await useAPI('/order/update', {
                method: 'PUT',
                body: JSON.stringify(payload),
                headers: {
                    'Content-Type': 'application/json',
                },
            })
        }
    }
    catch (error) {
        console.error('Failed to add discount:', error);
    }
}

const changeSourceLanguage = (languageId: string) => {
    if (editedOrder.value) {
        editedOrder.value.sourceLanguageId = languageId;
    }
};

const changeTargetLanguage = (languageIds: string[]) => {
    if (editedOrder.value) {
        editedOrder.value.targetLanguageId = languageIds;
    }
};

const issues = [
    { id: 1, title: 'Issue 1' },
    { id: 2, title: 'Issue 2' },
    { id: 3, title: 'Issue 3' },
    { id: 4, title: 'Issue 4' },
    { id: 5, title: 'Issue 5' },
];
interface Language {
    languageId: string;
    languageName: string;
    support: boolean;
}
const languageList = ref<Language[]>([]);

onMounted(async () => {
    try {
        const { data } = await useAPI('/lang');
        languageList.value = data.value as Language[];
    } catch (error) {
        console.error('Failed to fetch language list:', error);
    }
});
</script>

<template>
    <div>
        <div v-if="!order">
            <NuxtLoadingIndicator />
        </div>
        <div v-else class="grid grid-cols-1 md:grid-cols-2 gap-5 pb-5">
            <!-- Order Information and File URLs Section -->
            <div class="space-y-4">
                <div class="container p-4 space-y-2 orderDetails border rounded-md">
                    <p class="text-[2rem] font-semibold">{{ order?.orderName }}</p>

                    <!-- Order Details -->
                    <div class="flex flex-col space-y-1">
                        <div class="grid grid-cols-2 gap-x-2 text-sm">
                            <span>ID: {{ order?.orderId }}</span>
                            <span class="text-gray-500">Status: {{ order?.orderStatus }}</span>
                            <span v-if="order.orderPrice">Price: {{ order.orderPrice }}</span>
                            <span v-if="order.discountId">Discount: {{ order.discountId }}</span>
                            <span>Created: {{ order.createdDate?.split('T')[0] }}</span>
                            <span v-if="order.reference">Reference: {{ order.reference }}</span>
                        </div>

                        <!-- Services Section -->
                        <div class="flex items-center space-x-3">
                            <span>Service:</span>
                            <template v-if="!isEditing">
                                <span
v-for="service in ['TR', 'ED', 'EV']" v-show="order?.hasTranslateService && service === 'TR' ||
                                    order?.hasEditService && service === 'ED' ||
                                    order?.hasEvaluateService && service === 'EV'" :key="service" class="font-bold">{{
                                    service }}</span>
                            </template>
                            <template v-else-if="editedOrder">
                                <span
v-for="service in ['Translate', 'Edit', 'Evaluate']" :key="service"
                                    class="font-bold">
                                    {{ service.substring(0, 2).toUpperCase() }}
                                    <Checkbox
:id="`has${service}Service`"
                                        v-model:checked="editedOrder[`has${service}Service`]" />
                                </span>
                            </template>
                        </div>

                        <!-- Due Date -->
                        <div class="flex items-center space-x-2">
                            <span>Due date:</span>
                            <span v-if="!isEditing">{{ order.dueDate }}</span>
                            <input
v-else-if="editedOrder" v-model="editedOrder.dueDate" type="date"
                                class="border rounded p-1">
                        </div>

                        <!-- Language Selection -->
                        <div class="flex items-center space-x-1">
                            <template v-if="!isEditing">
                                <Badge variant="default">{{ order?.sourceLanguageId }}</Badge>
                                <LucideArrowBigRight />
                                <div class="flex gap-1">
                                    <Badge v-for="lang in order?.targetLanguageId" :key="lang" variant="secondary">{{
                                        lang }}</Badge>
                                </div>
                            </template>
                            <template v-else>
                                <OrdersDetailsLanguageSelector
:language-list="languageList"
                                    :selected-languages="editedOrder?.sourceLanguageId || ''"
                                    :original-languages="order?.sourceLanguageId || ''" :is-source-language="true"
                                    @update:selected-languages="changeSourceLanguage" />
                                <LucideArrowBigRight />
                                <OrdersDetailsLanguageSelector
:language-list="languageList"
                                    :selected-languages="editedOrder?.targetLanguageId || []"
                                    :original-languages="order?.targetLanguageId || []" :is-source-language="false"
                                    @update:selected-languages="changeTargetLanguage" />
                            </template>
                        </div>
                    </div>
                </div>

                <!-- Action Buttons -->
                <div class="flex space-x-2">
                    <template v-if="isEditing">
                        <Button @click="saveEdit">Save</Button>
                        <Button variant="outline" @click="cancelEdit">Cancel</Button>
                    </template>
                    <Button v-if="!isEditing && role === 'CLIENT'" @click="enableEdit">Edit Order</Button>
                    <Button v-if="role === 'CLIENT'" variant="outline" @click="cancelOrder">Cancel Order</Button>
                    <template v-if="role === 'STAFF'">
                        <Button @click="acceptorDeclineOrder(order.orderId, 'ACCEPTED')">Accept Order</Button>
                        <Button variant="outline" @click="acceptorDeclineOrder(order.orderId, 'REJECTED')">Reject
                            Order</Button>
                    </template>
                    <Button v-if="role === 'CLIENT'" variant="outline" @click="openAddDiscount = true">Add
                        Discount</Button>
                </div>

                <OrdersDetailsTabs :order="order" />
            </div>

            <!-- Smaller Issues List Section -->
            <div v-if="issues" class="space-y-4">
                <div class="flex justify-between items-center p-3 border-b">
                    <span class="text-lg font-semibold">Issues</span>
                    <Button variant="outline" size="sm">Add Issue</Button>
                </div>
                <div class="border rounded-md h-[15rem] overflow-auto p-2">
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
