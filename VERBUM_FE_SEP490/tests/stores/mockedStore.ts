import type { Mock } from 'vitest'
import type { Store, StoreDefinition } from 'pinia'

export function mockedStore<TStoreDef extends () => unknown>(
  useStore: TStoreDef
): TStoreDef extends StoreDefinition<
  infer Id,
  infer State,
  infer Getters,
  infer Actions
>
  ? Store<
      Id,
      State,
      Record<string, never>,
      {
        [K in keyof Actions]: Actions[K] extends (...args: any[]) => any
          ? Mock<Actions[K]>
          : Actions[K]
      }
    > & {
      [K in keyof Getters]: Getters[K] extends ComputedRef<infer T> ? T : never
    }
  : ReturnType<TStoreDef> {
  return useStore() as any
}
