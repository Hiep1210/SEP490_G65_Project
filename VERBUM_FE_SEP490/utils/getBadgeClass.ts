export const getIssueBadgeClass = (status: string) => {
    switch (status) {
      case 'OPEN':
        return 'bg-red-500 text-white'
      case 'ACCEPTED':
        return 'bg-yellow-500 text-black'
      case 'RESOLVE':
        return 'bg-green-500 text-black'
      case 'CANCEL':
        return 'bg-gray-500 text-white'
      default:
        return 'bg-gray-300 text-black'
    }
  }