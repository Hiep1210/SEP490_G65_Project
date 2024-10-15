import { mountSuspended } from '@nuxt/test-utils/runtime'
import { describe, it, expect, vi } from 'vitest'
import TaskDetail from '@/components/Pinia/TaskDetails.vue'
import { Trash2, Heart } from 'lucide-vue-next'
import { createTestingPinia } from '@pinia/testing'
import { useTaskStore } from '@/stores/TaskStore'
import { mockedStore } from '@/tests/stores/mockedStore.ts'

const task = { id: 1, title: 'Test Task', isFav: false }

const wrapper = await mountSuspended(TaskDetail, {
  props: {
    task: task
  },
  global: {
    plugins: [
      createTestingPinia({
        createSpy: vi.fn
      })
    ],
    components: { Trash2, Heart }
  }
})

const taskStore = mockedStore(useTaskStore)

describe('TaskDetail.vue', () => {
  it('can mount the componenet', async () => {
    expect(wrapper.html()).toContain('Test Task')
  })

  it('calls deleteTask when Trash2 icon is clicked', async () => {
    const deleteTaskSpy = vi.spyOn(taskStore, 'deleteTask')

    await wrapper.findComponent(Trash2).trigger('click')

    expect(deleteTaskSpy).toHaveBeenCalledOnce()
    expect(deleteTaskSpy).toHaveBeenCalledWith(task.id)
  })

  it('calls toggleFav when Heart icon is clicked', async () => {
    const toggleFavSpy = vi.spyOn(taskStore, 'toggleFav')

    await wrapper.findComponent(Heart).trigger('click')

    expect(toggleFavSpy).toHaveBeenCalledOnce()
    expect(toggleFavSpy).toHaveBeenCalledWith(task.id)
  })
})
