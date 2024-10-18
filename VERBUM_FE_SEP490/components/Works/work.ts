import type { Category } from "../Category/category";

export interface Work {
    workId: string;
    orderName: string;
    orderStatus: string;
    sourceLanguageId: string;
    targetLanguageId: string[] | string;
    createdDate: string;
    dueDate: string;
    files: string;
    translationFileUrls: string;
    newCategory: Category[] | Category;
  }