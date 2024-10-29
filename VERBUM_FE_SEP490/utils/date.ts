import { format } from 'date-fns';

export function formatDate(date: Date | string, formatStr: string = 'dd MMMM yyyy') {
  const dateObj = typeof date === 'string' ? new Date(date) : date;
  return format(dateObj, formatStr);
}
