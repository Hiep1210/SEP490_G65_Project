import type { JobDeliverables } from "./jobDeliverables"

export interface Order {
  orderId: string
  orderName?: string
  createdDate?: string
  dueDate?: string
  sourceLanguageId?: string
  targetLanguageId?: string[]
  orderStatus?: string
  orderPrice?: string
  discountId?: string
  hasTranslateService?: boolean
  hasEditService?: boolean
  hasEvaluateService?: boolean
  reference?: string
  translationFileUrls?: string[]
  referenceFileUrls?: string[]
  jobDeliverables?: JobDeliverables[]
  paymentStatus?: string
}
