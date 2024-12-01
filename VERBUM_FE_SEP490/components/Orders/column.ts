import type { ColumnDef } from '@tanstack/vue-table'
import type { Order } from '@/types/order'
import { formatToVietnamTimezone } from '~/utils/date'
import Badge from '../ui/badge/Badge.vue'

export const columns: ColumnDef<Order>[] = [
  {
    header: '#',
    cell: ({ row }) => h('div', {}, row.index + 1)
  },
  {
    accessorKey: 'orderName',
    header: 'Name',
    cell: ({ row }) =>
      h('div', { class: 'capitalize hyper-link' }, row.getValue('orderName'))
  },
  {
    accessorKey: 'createdDate',
    header: 'Created At',
    cell: ({ row }) => {
      const date = row.getValue('createdDate') as string
      const formattedDate = formatToVietnamTimezone(date)
      return h('div', {}, formattedDate)
    }
  },
  {
    accessorKey: 'orderStatus',
    header: 'Status',
    cell: ({ row }) => {
      const orderStatus = row.getValue('orderStatus') as string
      return h(
        Badge,
        { class: getOrderBadgeClass(orderStatus), variant: 'default' },
        { default: () => orderStatus }
      )
    }
  }
]
