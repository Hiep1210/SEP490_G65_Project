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

export type IssueUpdatePayload = {
  issueId: string
  issueName: string
  issueDescription: string
  assigneeId: string
  issueAttachments: IssueAttachments[]
}

export type IssueReOpenPayload = {
    issueId: string
    issueName: string
    issueDescription: string
    issueAttachments: IssueAttachments[]
}
