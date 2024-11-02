/* eslint-disable @typescript-eslint/no-unused-vars */

import type { ColumnDef } from "@tanstack/vue-table";
import Checkbox from "../ui/checkbox/Checkbox.vue";
import type { Work } from "~/composables/useWorks";




export const columns:ColumnDef<Work>[] = [
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
        accessorKey: 'workName',
        header: 'Work Name',
        cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('workName')),
    },
    {
        accessorKey: 'sourceLanguageId',
        header: 'Source Language',
        cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('sourceLanguageId')),
    },
    {
        accessorKey: 'targetLanguageId',
        header: 'Target Language',
        cell: ({ row }) => {
            const targetLanguages: string[] = row.getValue('targetLanguageId');
            return h('div', { class: 'capitalize' }, targetLanguages.length > 0 ? targetLanguages.join(', ') : '');
        },
    },
    {
        accessorKey: 'orderStatus',
        header: 'Status',
        cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('orderStatus')),
    },
]