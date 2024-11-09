import type { $Fetch, NitroFetchRequest } from 'nitropack'
import type { Order } from '@/types/order'
import type { Language } from '@/types/language'
export const repo = <T>(fetch: $Fetch<T, NitroFetchRequest>) => ({
  async getLanguages(): Promise<Language[]> {
    return fetch<Language[]>(`/lang`)
  },
  async getOrders(page: number, pageSize: number): Promise<Order[]> {
    return fetch<Order[]>(`/order/get-all?$skip=${(page - 1) * pageSize}&$top=${pageSize}&$count=true`)
  },
  async searchOrders(value: string): Promise<Order[]> {
    return fetch<Order[]>(`/order/get-all?$filter=contains(orderName, '${value}')`)
  },
  async updateOrder(order: Partial<Order>): Promise<Order | null> {
    return fetch<Order | null>(`/order/update`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(order)
    })
  }
})

