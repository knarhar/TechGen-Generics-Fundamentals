# TechGen Generics Fundamentals

Practice exercises for C# generics: type parameters, constraints, delegates, and generic collections.

## Ex01 - SwapSides
Swaps the two values in a tuple.
SwapSides(("string", 1)) -> (1, "string")

## Ex02 - Filter & Project
Filter<T> keeps items matching a condition (Predicate<T>).
Project<TIn, TOut> transforms each item into a new type (Func<TIn, TOut>).

Filter([1,2,3,4,5], even) -> [2,4]
Project([2,4], n => "N"+n) -> ["N2","N4"]

## Ex03 - Constraint
CreateAndInitialize<T>() creates a new T and calls Initialize() on it before returning.
Requires T to have a parameterless constructor and implement IInitializable.

## Ex04 - Top-N Buffer
Buffer<T> keeps only the top N values added so far, using a comparer.
Adding more than N items drops the smallest one.

[5,1,9,3,7,2] with N=3 -> [9,7,5]

## Ex05 - Retry Executor
Execute<T> runs an operation, retrying on failure until it succeeds or runs out of attempts.
Optional shouldRetry decides if a given failure should be retried.
Returns a Result<T> with Success, Value or Error, and Attempt count.
