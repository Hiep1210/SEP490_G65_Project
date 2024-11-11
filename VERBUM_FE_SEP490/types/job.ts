export interface Job {
    id: string;
    name: string;
    status: string;
    dueDate: string;
    createdAt: string;
    updatedAt: string;
    wordCount: number;
    documentUrl: string;
    deliverableUrl: string;
    targetLanguageId: string;
    workId: string;
    assigneeNames: string[];
}