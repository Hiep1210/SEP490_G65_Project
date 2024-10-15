import { mountSuspended } from '@nuxt/test-utils/runtime'
import { describe, it, expect } from 'vitest'
import DefaultLayout from '@/layouts/default.vue'
import LayoutBreadcrumbs from '@/components/Layout/Breadcrumbs.vue'
import LayoutCompanySelect from '@/components/Layout/CompanySelect.vue'
import LayoutDarkmodeToggle from '@/components/Layout/DarkmodeToggle.vue'
import LayoutNavbar from '@/components/Layout/Navbar.vue'
import LayoutUserDropdown from '@/components/Layout/UserDropdown.vue'

const wrapper = await mountSuspended(DefaultLayout, {
  global: {
    components: {
      LayoutBreadcrumbs,
      LayoutCompanySelect,
      LayoutDarkmodeToggle,
      LayoutNavbar,
      LayoutUserDropdown
    }
  }
})

describe('Layout', () => {
  it('renders correctly', () => {
    expect(wrapper.findComponent(LayoutCompanySelect).exists()).toBe(true)
    expect(wrapper.findComponent(LayoutNavbar).exists()).toBe(true)
    expect(wrapper.findComponent(LayoutBreadcrumbs).exists()).toBe(true)
    expect(wrapper.findComponent(LayoutDarkmodeToggle).exists()).toBe(true)
    expect(wrapper.findComponent(LayoutUserDropdown).exists()).toBe(true)
  })

  it('renders the logo correctly', () => {
    const logo = wrapper.find('img')
    expect(logo.exists()).toBe(true)
  })
})
