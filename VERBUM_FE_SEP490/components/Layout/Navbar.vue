<script setup lang="ts">
import { House, FolderOpen, FileWarning, Book, User } from 'lucide-vue-next'
import { useRoute } from 'vue-router'
const route = useRoute()


const navbarItems = [
  { navName: 'Home', navLink: '/', navIcon: House },
  { navName: 'Orders', navLink: '/orders', navIcon: FolderOpen },
  {
    navName: 'Issues',
    navLink: '/issues',
    navIcon: FileWarning
  },
  { navName: 'Term bases', navLink: '/term-bases', navIcon: Book },
  { navName: 'Users', navLink: '/users', navIcon: User }
]
</script>

<template>
  <nav class="grid items-start px-2 text-sm font-medium lg:px-4">
    <NuxtLink
      v-for="item in navbarItems"
      :key="item.navName"
      :to="item.navLink"
      :class="[
        'flex items-center gap-3 rounded-lg px-3 py-2 transition-all text-md',
        route.path === item.navLink
          ? 'text-primary'
          : 'text-muted-foreground hover:text-primary'
      ]"
    >
      <component :is="item.navIcon" class="h-7 w-7" />
      {{ item.navName }}
    </NuxtLink>
  </nav>
</template>
