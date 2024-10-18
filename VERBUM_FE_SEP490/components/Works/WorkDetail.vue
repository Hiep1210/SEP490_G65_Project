<script lang="ts" setup>
import { ref } from 'vue'
import type { Work } from './work'
import type { Category } from '../Category/category';

const props = defineProps<{
  work: Work;
  categories: Category[];
}>()

const statusOptions = [
  { value: 'INPROGRESS', label: 'IN PROGRESS' },
  { value: 'CANCELED', label: 'CANCELED' },
  { value: 'COMPLETED', label: 'COMPLETED' },
  { value: 'NEW', label: 'NEW' }
]

// Set up ref variables to hold the editable values and the edit mode
const editMode = ref(false)

const editableWork = ref({
  orderName: props.work.orderName,
  workId: props.work.workId,
  createdDate: props.work.createdDate,
  orderStatus: props.work.orderStatus,
  sourceLanguageId: props.work.sourceLanguageId,
  dueDate : props.work.dueDate,
  newCategory: props.work.newCategory

})

// Toggle edit mode
const toggleEditMode = () => {
  editMode.value = !editMode.value
}

// Save changes
const saveChanges = () => {
  // Example: Call an API to save the changes to a backend here.
  editMode.value = false
}

// Cancel editing
const cancelEdit = () => {
  // Reset editableWork to the original work data when canceled
  editableWork.value = { ...props.work }
  editMode.value = false
}

const categories = props.categories

const getCategories = (newCategory: Category[] | Category) => {
  if (Array.isArray(newCategory) && newCategory.length > 0) {
    // If newCategory is an array, find all matching category names
    return newCategory
      .map(cat => {
        const category = categories.find(category => category.id === cat.id);
        return category ? category.name : null; // Return the category name or null if not found
      })
      .filter(Boolean) // Filter out null values
      .join(', '); // Join names into a single string
  } else if (newCategory) {
    // If newCategory is a single category, find and return its name
    const category = categories.find(category => category.id === newCategory.id);
    return category ? category.name : 'No Category Selected'; // Return the category name or a message if not found
  } else {
    return 'No Category Selected'; // Return a message if newCategory is empty
  }
};
</script>

<template>
  <div>
    <div class="mb-2">
      <!-- Show Edit button when not in edit mode -->
      <button 
      v-if="!editMode" 
      class="bg-cyan-600 text-white px-4 py-2 float-right mr-2 mt-2 rounded-2xl"
      @click="toggleEditMode" 
      >
        Edit
      </button>

      <!-- Show Save and Cancel buttons when in edit mode -->
      <div v-if="editMode" class="float-right">
        <button 
        class="bg-cyan-600 text-white px-4 py-2 mr-2 mt-2 rounded-2xl"
        @click="saveChanges" 
        >
          Save
        </button>
        <button 
        class="bg-gray-600 text-white px-4 py-2 mt-2 rounded-2xl"
        @click="cancelEdit" 
        >
          Cancel
        </button>
      </div>

      <h1 class="text-3xl font-bold text-cyan-600">
        Work name: {{ editableWork.orderName }}
        <!-- <input v-model="editableWork.orderName" class="border rounded px-2" > -->
      </h1>
    </div>

    <div class="overflow-x-auto">
      <table class="min-w-full table-auto border-none border-gray-300">
        <tbody>
          <tr v-if="editMode">
            <td class="px-3 pb-1 text-right font-medium">Work name:</td>
            <td class="px-3 pb-1">
              <!-- Toggle between plain text and input field -->
              <!-- <span v-if="!editMode">{{ editableWork.orderName }}</span> -->
              <input v-model="editableWork.orderName" class="border rounded px-2" >
            </td>
          </tr>
          <tr>
            <td class="px-3 pb-1 text-right font-medium">Work ID:</td>
            <td class="px-3 pb-1">{{ editableWork.workId }}
            </td>
          </tr>
          <tr>
            <td class="px-3 pb-1 text-right font-medium">Created at:</td>
            <td class="px-3 pb-1">
              <span v-if="!editMode">{{ editableWork.createdDate || '   ' }}</span>
              <input v-else v-model="editableWork.createdDate" class="border rounded px-2" >
            </td>
          </tr>
          <tr>
            <td class="px-3 pb-1 text-right font-medium">Due date:</td>
            <td class="px-3 pb-1">
                <span v-if="!editMode">{{ editableWork.dueDate || '' }}</span>
                <input v-else v-model="editableWork.dueDate" class="border rounded px-2" >
              </td>
          </tr>
          <tr>
            <td class="px-3 pb-1 text-right font-medium">Status:</td>
            <td class="px-3 pb-1">
              <span v-if="!editMode">{{ editableWork.orderStatus }}</span>
              <select v-else v-model="editableWork.orderStatus" class="border rounded px-2">
                <option v-for="option in statusOptions" :key="option.value" :value="option.value">
                  {{ option.label }}
                </option>
              </select>
            </td>
          </tr>
          <tr>
            <td class="px-3 pb-1 text-right font-medium">Source language:</td>
            <td class="px-3 pb-1">
              <span v-if="!editMode">{{ editableWork.sourceLanguageId }}</span>
              <input v-else v-model="editableWork.sourceLanguageId" class="border rounded px-2" >
            </td>
          </tr>
          <tr>
            <td class="px-3 pb-1 text-right font-medium">Category:</td>
            <td class="px-3 pb-1">
              <select v-if="editMode" v-model="editableWork.newCategory" class="border rounded px-2">
                <option v-for="category in categories" :key="category.id" :value="category.id">
                  {{ category.name }}
                </option>
              </select>
              <span v-else>
                {{ Array.isArray(getCategories(editableWork.newCategory)) 
                  ? (getCategories(editableWork.newCategory).length > 3 
                      ? getCategories(editableWork.newCategory).slice(0, 3).concat(', ') + ', ...' 
                      : getCategories(editableWork.newCategory).concat(', '))
                  : getCategories(editableWork.newCategory) }}
              
              </span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<style scoped>
/* Style the input fields */
input {
  width: 100%;
  padding: 4px;
  margin-bottom: 4px;
}
</style>
