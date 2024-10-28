export type IssueAttachment = {
  issueId: string
  attachmentUrl: string
}

export type Issue = {
  issueId: string
  issueName: string
  createdAt: string
  updatedAt: string
  status: string
  clientName: string
  orderId: string
  issueDescription: string
  assigneeName: string
  assigneeId: string
  issueAttachments: IssueAttachment[]
}
