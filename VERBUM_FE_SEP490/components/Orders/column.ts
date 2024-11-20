import type { ColumnDef } from "@tanstack/vue-table";
import type { Order } from "@/types/order";

export const columns: ColumnDef<Order>[] = [
    {
        header: "#",
        cell: ({ row }) => h('div', {}, row.index + 1)
    },
    {
        accessorKey: 'orderName',
        header: 'Name',
        cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('orderName')),
    },
    {
        accessorKey: 'orderStatus',
        header: 'Status',
        cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('orderStatus')),
    },
    {
        accessorKey: 'createdDate',
        header: 'Created At',
        cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('createdDate')),
    },
]