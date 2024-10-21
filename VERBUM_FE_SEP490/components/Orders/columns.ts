/* eslint-disable @typescript-eslint/no-unused-vars */
import type { ColumnDef } from '@tanstack/vue-table'
import type { Order } from '~/types/order'
import Checkbox from '../ui/checkbox/Checkbox.vue'
import { Button } from '../ui/button'
export const columns: ColumnDef<Order>[] = [
  {
    id: 'select',
    header: ({ table }) => h(Checkbox, {}),
    cell: ({ row }) => h(Checkbox, {}),
    enableSorting: false,
    enableHiding: false
  },
  {
    accessorKey: 'id',
    header: 'ID',
    cell: ({ row }) => h('div', { class: 'uppercase' }, row.getValue('id'))
  },
  {
    accessorKey: 'name',
    header: 'Name',
    cell: ({ row }) => h('div', { class: 'capitalize' }, row.getValue('name'))
  },
  {
    accessorKey: 'status',
    header: 'Status',
    cell: ({ row }) => h('div', { class: 'uppercase' }, row.getValue('status'))
  },
  {
    accessorKey: 'createAt',
    header: ({ column }) => {
      return h(Button, {
        variant: 'ghost',
        onClick: () => column.toggleSorting(column.getIsSorted() === 'asc')
      })
    },
    cell: ({ row }) => h('div', { class: '' }, row.getValue('createAt'))
  }
]
