import { mountSuspended } from '@nuxt/test-utils/runtime'
import { describe, it, expect } from 'vitest'
import Navbar from '@/components/Layout/Navbar.vue'
import { createRouter, createWebHistory } from 'vue-router'
import Projects from '@/pages/(tms)/projects.vue'
import { flushPromises } from '@vue/test-utils'

const routes = [
  {
    path: '/',
    component: {
      template: 'Welcome to the blogging app'
    }
  },
  {
    path: '/projects',
    component: Projects
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes: routes
})

router.push('/')
await router.isReady()

const wrapper = await mountSuspended(Navbar, {
  global: {
    plugins: [router]
  }
})

describe('Navbar', () => {
  it('renders correctly', () => {
    expect(wrapper.exists()).toBe(true)
  })

  it('displays the correct number of navigation items', () => {
    const navItems = wrapper.findAll('a')
    expect(navItems.length).toBe(5)
  })

  it('renders each navigation item with the correct label, icon, and link', () => {
    const navItems = wrapper.findAll('NuxtLink')

    navItems.forEach((item) => {
      const expectedLabel = 'Projects'
      const expectedIcon = 'Folder'
      const expectedLink = '/projects'

      expect(item.text()).toBe(expectedLabel)
      expect(item.find('component').exists()).toBe(true)
      expect(item.find('component').attributes().is).toBe(expectedIcon)
      expect(item.attributes().to).toBe(expectedLink)
    })
  })

  it('highlights the active navigation item', async () => {
    const navItems = wrapper.findAll('NuxtLink')
    navItems.forEach(async (item) => {
      const expectedLink = '/projects'
      expect(item.find('component').exists()).toBe(true)
      expect(item.attributes().to).toBe(expectedLink)
      await item.trigger('click')
      await flushPromises()
      expect(item.classes().includes('text-primary'))
    })
  })
})
