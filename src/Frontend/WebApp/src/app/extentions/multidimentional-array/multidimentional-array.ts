export type MultidimensionalArray<T> =
| T
| ReadonlyArray<MultidimensionalArray<T>>;