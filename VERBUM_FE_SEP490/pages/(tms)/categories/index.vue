<script setup lang="ts">

import { useCategories } from '~/composables/useCategories';

useSeoMeta({
    title: 'Categories',
})
const { categories, getCategories } = useCategories()

onMounted(() => {
    if (!categories.value.length) {
        getCategories()
    }
})

watch(categories, newCategories => {
    console.log('newCategories', newCategories)
})
</script>

<template>
    <div>
        <h1>Categories</h1>
        <div class="container mx-auto">
            <Badge v-for="category in categories" :key="category.id" variant="outline"
                class="p-2 text-lg font-semibold">
                {{ category.name }}
            </Badge>
        </div>
    </div>
</template>