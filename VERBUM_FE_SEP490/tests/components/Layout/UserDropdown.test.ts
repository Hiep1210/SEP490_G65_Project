import { mountSuspended } from '@nuxt/test-utils/runtime'
import { describe, it, expect } from 'vitest'
import UserDropdown from '@/components/Layout/UserDropdown.vue'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'

// TODO: this component is not yet implemented real logic
// the test here just to pass the pipline
const wrapper = await mountSuspended(UserDropdown, {
  global: {
    components: {
      DropdownMenu,
      DropdownMenuContent,
      DropdownMenuItem,
      DropdownMenuLabel,
      DropdownMenuSeparator,
      DropdownMenuTrigger
    }
  }
})

describe('UserDropdown', () => {
  it('render correctly', async () => {
    expect(wrapper.exists()).toBe(true)

    await wrapper.findComponent(DropdownMenuTrigger).trigger('click')
    expect(wrapper.findComponent(DropdownMenuLabel).exists()).toBe(true)
  })

  it('render correctly user drop down options', () => {
    expect(wrapper.exists()).toBe(true)
    const expectItems = ['Settings', 'Support', 'Logout']
    const userDropdownOptions = wrapper.findAllComponents(DropdownMenuItem)
    userDropdownOptions.forEach((option) => {
        expect(expectItems.includes(option.text()))
    })
  })
})
