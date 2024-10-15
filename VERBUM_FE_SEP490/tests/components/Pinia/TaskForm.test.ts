import { mountSuspended } from '@nuxt/test-utils/runtime'
import { describe, it, expect, vi, beforeEach } from 'vitest'
import TaskForm from '@/components/Pinia/TaskForm.vue'
import { createTestingPinia } from '@pinia/testing'
import { useTaskStore } from '@/stores/TaskStore.ts'
import { mockedStore } from '@/tests/stores/mockedStore.ts'

describe('TaskForm', () => {
  let wrapper: ReturnType<typeof mountSuspended>
  let taskStore: ReturnType<typeof mockedStore>

  beforeEach(async () => {
    wrapper = await mountSuspended(TaskForm, {
      global: {
        plugins: [
          createTestingPinia({
            createSpy: vi.fn
          })
        ]
      }
    })

    taskStore = mockedStore(useTaskStore)
  })

  it('renders the form', () => {
    // Check if the input is rendered
    expect(wrapper.find('input[type="text"]').exists()).toBe(true)

    // Check if the button is rendered
    expect(wrapper.find('button').text()).toBe('Add')
  })

  it('adds a task when the form is submitted', async () => {
    const addTaskSpy = vi.spyOn(taskStore, 'addTask')

    const input = wrapper.find('input[type="text"]')
    await input.setValue('New Task')
    await wrapper.find('form').trigger('submit.prevent')

    expect(addTaskSpy).toHaveBeenCalledOnce()
    expect(addTaskSpy).toHaveBeenCalledWith({
      title: 'New Task',
      isFav: false,
      id: expect.any(String)
    })

    expect(input.element.value).toBe('')
  })

  it('does not add a task if the input is empty', async () => {
    const addTaskSpy = vi.spyOn(taskStore, 'addTask')

    const input = wrapper.find('input[type="text"]')

    await input.setValue('')
    await wrapper.find('form').trigger('submit.prevent')

    expect(addTaskSpy).not.toHaveBeenCalled()

    expect(input.element.value).toBe('')
  })
})
