import { mountSuspended, mockNuxtImport } from '@nuxt/test-utils/runtime'
import { describe, it, expect, vi } from 'vitest'
import { Moon, Sun } from 'lucide-vue-next'
import DarkmodeToggle from '@/components/Layout/DarkmodeToggle.vue'
import Button from '@/components/ui/button/Button.vue'

const wrapper = await mountSuspended(DarkmodeToggle, {
  global: {
    components: { Sun, Moon }
  }
})

const { useColorModeMock } = vi.hoisted(() => {
  return {
    useColorModeMock: vi.fn().mockImplementation(() => {
      return { preference: 'light' }
    })
  }
})

mockNuxtImport('useColorMode', () => {
  return useColorModeMock
})

describe('ToggleThemeButton', () => {
  it('renders the correct icon based on the initial color mode', async () => {
    const sunIcon = wrapper.findComponent(Sun)
    expect(sunIcon.exists()).toBe(true)
  })

  it('toggles the color mode on button click', async () => {
    await wrapper.findComponent(Button).trigger('click')

    const moonIcon = wrapper.findComponent(Moon)
    expect(moonIcon.exists()).toBe(true)
  })
})
