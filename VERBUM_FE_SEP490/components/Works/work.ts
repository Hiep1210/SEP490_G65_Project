export interface Work {
    workId: number;
    orderName: string;
    orderStatus: string;
    sourceLanguageId: string;
    targetLanguageId: string[] | string;
    createdDate: string;
    dueDate: string;
    files: string;
    translationFileUrls: string;
  }