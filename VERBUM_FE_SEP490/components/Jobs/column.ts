/* eslint-disable @typescript-eslint/no-unused-vars */

import type { ColumnDef } from "@tanstack/vue-table";
import Checkbox from "../ui/checkbox/Checkbox.vue";
import type { Job } from "@/types/job";

export const columns: ColumnDef<Job>[] = [
    {
        id: 'select',
        header: ({ table }) => h(Checkbox, {
        }),
        cell: ({ row }) => h(Checkbox, {
        }),
        enableSorting: false,
        enableHiding: false,
    },
    {
        accessorKey: 'jobTitle',
        header: 'Job Title',
        cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('jobTitle')),
    },
    {
        accessorKey: 'company',
        header: 'Company',
        cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('company')),
    },
    {
        accessorKey: 'location',
        header: 'Location',
        cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('location')),
    },
    {
        accessorKey: 'jobStatus',
        header: 'Status',
        cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('jobStatus')),
    },
]