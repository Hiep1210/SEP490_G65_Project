export const getIssueBadgeClass = (status: string) => {
  switch (status) {
    case 'OPEN':
      return 'bg-red-600 text-white';
    case 'IN_PROGRESS':
      return 'bg-blue-500 text-white';
    case 'CANCEL':
      return 'bg-gray-500 text-white';
    case 'SUBMITTED':
      return 'bg-purple-500 text-white';
    case 'RESOLVED':
      return 'bg-green-500 text-white';
    default:
      return 'bg-gray-300 text-black';
  }
};


export const getJobBadgeClass = (status: string) => {
  switch (status) {
    case 'OPEN':
      return 'bg-red-500 text-white'
    case 'IN_PROGRESS':
      return 'bg-yellow-500 text-black'
    case 'SUBMITTED':
      return 'bg-blue-500 text-white'
    case 'APPROVED':
      return 'bg-green-500 text-white'
    default:
      return 'bg-gray-300 text-black'
  }
}
