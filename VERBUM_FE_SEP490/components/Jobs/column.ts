/* eslint-disable @typescript-eslint/no-unused-vars */

import type { ColumnDef } from "@tanstack/vue-table";
import Checkbox from "../ui/checkbox/Checkbox.vue";
import type { Job } from "@/types/job";
import { computed } from "vue";

export function getColumns(userRole: string | undefined): ColumnDef<Job>[] {
    const columns: ColumnDef<Job>[] = [
        {
            accessorKey: 'name',
            header: 'Name',
            cell: ({ row }) => h('div', row.getValue('name')),
        },
        {
            accessorKey: 'targetLanguageId',
            header: 'Target Language',
            cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('targetLanguageId')),
        },
        {
            id: 'assigneeNames',
            header: 'Assignees',
            cell: ({ row }) => {
                const assignees = row.original.assigneeNames || []; // Assuming `row.original.assignees` contains the array
                const names = assignees.map((assignee: { name: string }) => assignee.name).join(', ');
                return h('div', { class: 'truncate', title: names }, names || 'No Assignees');
            },
        },
        {
            accessorKey: 'status',
            header: 'Status',
            cell: ({ row }) => h('div', { class: 'capitalize'}, row.getValue('status')),
        },
    ];

    return userRole === 'Linguist'
    ? columns.filter(col => col.id !== 'assigneeNames')
    : columns;
}