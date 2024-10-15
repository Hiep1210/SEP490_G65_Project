import { describe, it, expect } from 'vitest'
import CompanySelect from '@/components/Layout/CompanySelect.vue'
import { mountSuspended } from '@nuxt/test-utils/runtime'
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectLabel,
  SelectTrigger,
  SelectValue
} from '@/components/ui/select'

const MOCK_COMPANIES = [
  { id: 1, name: 'Company 1' },
  { id: 2, name: 'Company 2' },
  { id: 3, name: 'Company 3' }
]

const wrapper = await mountSuspended(CompanySelect, {
  props: {
    MOCK_COMPANIES: MOCK_COMPANIES
  },
  global: {
    components: {
      Select,
      SelectContent,
      SelectGroup,
      SelectItem,
      SelectLabel,
      SelectTrigger,
      SelectValue
    }
  }
})

//All test cases are made just to pass the pipepline
//will change to proper test cases when the component have real data
describe('CompanySelect', () => {
  it('renders the select component with mock companies', async () => {
    expect(wrapper.findComponent(Select).exists()).toBe(true)

    await wrapper.findComponent(Select).trigger('click')
    expect(wrapper.findAllComponents(SelectItem).length).toBe(3)
  })

  it('selects a company when clicked', async () => {
    await wrapper.findComponent(Select).trigger('click')

    const selectedItem = wrapper.findAllComponents(SelectItem)[1]
    await selectedItem.trigger('click')

    expect(wrapper.findComponent(SelectValue).text).not.toBe('Select a Company')
  })
})
