export interface Job {
    id: string;
    name: string;
    status: string;
    dueDate: string;
    createdAt: string;
    updatedAt: string;
    documentUrl: string;
    deliverableUrl: string;
    targetLanguageId: string;
    workId: string;
    assigneeNames: assigneeNames[];
    orderId: string;
}

interface assigneeNames {
    id: string;
    name: string;
    email: string;
    roleCode: string;
    revelancies: string[];
}