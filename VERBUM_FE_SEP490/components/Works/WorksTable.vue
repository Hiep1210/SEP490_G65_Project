<script setup lang="ts">
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from '@/components/ui/table';
import { Checkbox } from '@/components/ui/checkbox';
import { ref, computed } from 'vue';
import SortableHeader from './SortableHeader.vue';
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue
} from '@/components/ui/select';
import type { Work } from './work';

const props = defineProps<{
  allWorks: Work[];
}>();

const data = ref(props.allWorks);
const searchName = ref('');
const selectedStatus = ref('');
const sortKey = ref('');
const sortDirection = ref<'asc' | 'desc' | 'none'>('none');

// Sorting function
const sortedData = computed(() => {
  let sorted = [...data.value];
  if (sortKey.value && sortDirection.value !== 'none') {
    sorted = sorted.sort((a: Work, b: Work) => {
      const key = sortKey.value as keyof Work;
      const result =
        a[key] > b[key] ? 1 : a[key] < b[key] ? -1 : 0;
      return sortDirection.value === 'asc' ? result : -result;
    });
  }
  return sorted;
});

// Filtering function
const filteredWorksByName = computed(() => {
  return sortedData.value.filter((item) => {
    const matchesName = item.orderName
      ? item.orderName.toLowerCase().includes(searchName.value.toLowerCase())
      : false;
    const matchesStatus =
      selectedStatus.value && selectedStatus.value !== 'all'
        ? item.orderStatus === selectedStatus.value
        : true;
    return matchesName && matchesStatus;
  });
});

// Update sorting function
const updateSort = ({
  key,
  direction
}: {
  key: string;
  direction: 'asc' | 'desc' | 'none';
}) => {
  sortKey.value = key;
  sortDirection.value = direction;
};
</script>

<template>
  <div>
    <!-- Search and Status Select Section -->
    <div class="flex gap-2">
      <div class="flex flex-auto gap-3">
        <!-- Search Input -->
        <div class="flex-auto">
          <input
            v-model="searchName"
            type="text"
            placeholder="Search work..."
            class="border p-2 mb-2 w-full rounded-2xl"
          >
        </div>
        <!-- Status Select -->
        <div class="flex-auto">
          <!-- Custom Select Component -->
          <Select v-model="selectedStatus">
            <SelectTrigger class="w-[180px] rounded-2xl p-2">
              <SelectValue placeholder="Status" />
            </SelectTrigger>
            <SelectContent>
              <SelectGroup>
                <SelectItem value="all">All</SelectItem>
                <SelectItem value="NEW">New</SelectItem>
                <SelectItem value="IN-PROGRESS">In Progress</SelectItem>
                <SelectItem value="CANCELED">Canceled</SelectItem>
                <SelectItem value="COMPLETED">Completed</SelectItem>
              </SelectGroup>
            </SelectContent>
          </Select>
        </div>
      </div>
      <div class="flex-auto" />
    </div>

    <!-- Table Section -->
    <Table>
      <TableCaption>A list of your recent works.</TableCaption>

      <TableHeader>
        <TableRow>
          <TableHead class="w-[100px]">
            <Checkbox id="" />
          </TableHead>
          <TableHead class="w-[100px]">
            <SortableHeader
              label="#"
              sort-key="workId"
              :current-sort-key="sortKey"
              :current-sort-direction="sortDirection"
              @update:sort="updateSort"
            />
          </TableHead>
          <TableHead>
            <SortableHeader
              label="Work name"
              sort-key="workName"
              :current-sort-key="sortKey"
              :current-sort-direction="sortDirection"
              @update:sort="updateSort"
            />
          </TableHead>
          <TableHead>
            <SortableHeader
              label="Source language"
              sort-key="sourceLanguage"
              :current-sort-key="sortKey"
              :current-sort-direction="sortDirection"
              @update:sort="updateSort"
            />
          </TableHead>
          <TableHead>
            <SortableHeader
              label="Target language"
              sort-key="targetLanguage"
              :current-sort-key="sortKey"
              :current-sort-direction="sortDirection"
              @update:sort="updateSort"
            />
          </TableHead>
          <TableHead>Files</TableHead>
          <TableHead>References</TableHead>
          <TableHead>Status</TableHead>
          <TableHead>
            <SortableHeader
              label="Created date"
              sort-key="createdDate"
              :current-sort-key="sortKey"
              :current-sort-direction="sortDirection"
              @update:sort="updateSort"
            />
          </TableHead>
          <TableHead>
            <SortableHeader
              label="Due date"
              sort-key="dueDate"
              :current-sort-key="sortKey"
              :current-sort-direction="sortDirection"
              @update:sort="updateSort"
            />
          </TableHead>
        </TableRow>
      </TableHeader>

      <TableBody>
        <TableRow
          v-for="item in filteredWorksByName"
          :key="item.workId"
          class="cursor-pointer"
        >
          <TableCell><Checkbox id="" /></TableCell>
          <TableCell>{{ item.workId }}</TableCell>
          <TableCell>{{ item.orderName }}</TableCell>
          <TableCell>{{ item.sourceLanguageId }}</TableCell>
          <TableCell>
            {{ Array.isArray(item.targetLanguageId) 
                ? (item.targetLanguageId.length > 3 
                    ? item.targetLanguageId.slice(0, 3).join(', ') + ', ...' 
                    : item.targetLanguageId.join(', '))
                : item.targetLanguageId }}
          </TableCell>
          <TableCell>{{ item.files }}</TableCell>
          <TableCell>
            {{ Array.isArray(item.translationFileUrls) 
              ? (item.translationFileUrls.length > 3 
                  ? item.translationFileUrls.slice(0, 3).concat('\n ') + ', ...' 
                  : item.translationFileUrls.join(', '))
              : item.translationFileUrls }}
          </TableCell>
          <TableCell>{{ item.orderStatus }}</TableCell>
          <TableCell>{{ item.createdDate }}</TableCell>
          <TableCell>{{ item.dueDate }}</TableCell>
          <TableCell>...</TableCell>
        </TableRow>
      </TableBody>
    </Table>
  </div>
</template>
