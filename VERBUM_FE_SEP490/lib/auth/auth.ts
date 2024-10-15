/* eslint-disable @typescript-eslint/no-explicit-any */
import jwt from 'jsonwebtoken'
import type { User } from '~/types/user'

const isUser = (decoded: any): decoded is User => {
  return decoded && typeof decoded.email === 'string'
}

export const decodeToken = (token: string): User | null => {
  const decoded = jwt.decode(token) as any
  const { exp, iss, aud, ...user } = decoded
  if (isUser(user)) {
    console.log(isUser(user))
    return user
  }
  return null
}
