import { setActivePinia, createPinia } from 'pinia'
import { useTaskStore } from '@/stores/TaskStore.js'
import { vi, describe, it, expect, beforeEach } from 'vitest'

type Task = {
  id: string
  title: string
  isFav: boolean
}

// Mock fetch API globally
globalThis.fetch = vi.fn()

describe('Task Store', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks() // Clear mocks before each test
  })

  it('initializes with default state', () => {
    const store = useTaskStore()

    expect(store.tasks).toEqual([])
    expect(store.isLoading).toBe(false)
    expect(store.name).toBe('Pinia demo')
  })

  it('fetches tasks and updates the state', async () => {
    const store = useTaskStore()

    const mockTasks: Task[] = [
      { id: '1', title: 'Task 1', isFav: false },
      { id: '2', title: 'Task 2', isFav: true }
    ]
    vi.mocked(fetch).mockResolvedValueOnce({
      ok: true,
      json: async () => mockTasks
    } as Response)

    await store.getTasks()

    expect(store.tasks).toEqual(mockTasks)
    expect(store.isLoading).toBe(false)
  })

  it('handles errors in getTasks action', async () => {
    const store = useTaskStore()

    vi.mocked(fetch).mockResolvedValueOnce({
      ok: false,
      statusText: 'Server Error'
    } as Response)

    await store.getTasks()

    expect(store.tasks).toEqual([])
    expect(store.isLoading).toBe(false)
  })

  it('adds a new task', async () => {
    const store = useTaskStore()

    const newTask: Task = { id: '3', title: 'Task 3', isFav: false }
    vi.mocked(fetch).mockResolvedValueOnce({
      ok: true,
      json: async () => newTask
    } as Response)

    await store.addTask(newTask)

    expect(store.tasks).toContainEqual(newTask)
    expect(store.isLoading).toBe(false)
  })

  it('handles errors in addTask action', async () => {
    const store = useTaskStore()

    const newTask: Task = { id: '3', title: 'Task 3', isFav: false }
    vi.mocked(fetch).mockResolvedValueOnce({
      ok: false,
      statusText: 'Server Error'
    } as Response)

    await store.addTask(newTask)

    expect(store.tasks).not.toContainEqual(newTask)
    expect(store.isLoading).toBe(false)
  })

  it('deletes a task', async () => {
    const store = useTaskStore()
    store.tasks = [
      { id: '1', title: 'Task 1', isFav: false },
      { id: '2', title: 'Task 2', isFav: true }
    ]

    vi.mocked(fetch).mockResolvedValueOnce({ ok: true } as Response)

    await store.deleteTask('1')

    expect(store.tasks).toEqual([{ id: '2', title: 'Task 2', isFav: true }])
    expect(store.isLoading).toBe(false)
  })

  it('handles errors in deleteTask action', async () => {
    const store = useTaskStore()
    store.tasks = [
      { id: '2', title: 'Task 2', isFav: true },
      { id: '1', title: 'Task 1', isFav: false }
    ]

    vi.mocked(fetch).mockResolvedValueOnce({
      ok: false,
      statusText: 'Server Error'
    } as Response)

    await store.deleteTask('1')

    expect(store.tasks).toEqual([
      { id: '2', title: 'Task 2', isFav: true },
      { id: '1', title: 'Task 1', isFav: false }
    ])
    expect(store.isLoading).toBe(false)
  })

  it("toggles a task's favorite status", async () => {
    const store = useTaskStore()
    store.tasks = [{ id: '1', title: 'Task 1', isFav: false }]

    vi.mocked(fetch).mockResolvedValueOnce({ ok: true } as Response)

    await store.toggleFav('1')

    expect(store.tasks[0].isFav).toBe(true)
    expect(store.isLoading).toBe(false)
  })

  it('handles errors in toggleFav action', async () => {
    const store = useTaskStore()
    store.tasks = [{ id: '1', title: 'Task 1', isFav: false }]

    vi.mocked(fetch).mockResolvedValueOnce({
      ok: false,
      statusText: 'Server Error'
    } as Response)

    await store.toggleFav('1')

    expect(store.tasks[0].isFav).toBe(true)
    expect(store.isLoading).toBe(false)
  })

  it('calculates the correct number of favorites', () => {
    const store = useTaskStore()
    store.tasks = [
      { id: '1', title: 'Task 1', isFav: true },
      { id: '2', title: 'Task 2', isFav: false }
    ]

    expect(store.favsCount).toBe(1)
  })

  it('returns the correct number of total tasks', () => {
    const store = useTaskStore()
    store.tasks = [
      { id: '1', title: 'Task 1', isFav: true },
      { id: '2', title: 'Task 2', isFav: false }
    ]

    expect(store.totalCount).toBe(2)
  })
})
