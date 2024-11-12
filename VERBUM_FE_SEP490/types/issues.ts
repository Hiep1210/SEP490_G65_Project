export type IssueAttachments = {
  issueId: string
  attachmentUrl: string
  tag:string
  isDeleted: boolean
}

export type Issue = {
  issueId: string
  issueName: string
  createdAt: string
  updatedAt: string
  status: string
  clientName: string
  orderId: string
  orderName: string
  issueDescription: string
  assigneeName: string
  assigneeId: string
  issueAttachments: IssueAttachments[]
  cancelResponse: string
  rejectResponse: string
}
