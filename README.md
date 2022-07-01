# Optimized LinkedList<T>

![ci workflow](https://github.com/ogxd/optimized-linked-list/actions/workflows/ci/badge.svg)

Linked lists are handy structures when it comes to implementing things that needs O(1) removal while keeping an order. It is used in things like LRU caches or priority queues for instance. However, pointer based linked lists (or reference based in managed languages) tend to spread everywhere in memory. This quickly leads to lot of memory fragmentation, and can even slow down allocations and garbage collection in C# for instance by introducing a lot of additional work.    
The linked list implementation in the repository is an alternative that is based on indices. Nodes are structures (as opposed to classes in BCL's `LinkedList<T>`) and everything is contained in a single array. On top of it, it leverages a clever trick inspired from dotnet's BCL `Dictionary<K, V>` (freeList) to find new slots without any loops (true O(1)).

## Features

* Allocation free (appart from internal buffer of course)
* Add O(1)
* Remove O(1)
* Contains O(1) (from index, not `T`, but you can pair it with a `Dictionary<T, int>` if you want O(1) on Remove and Contains)
* Get First O(1)
* Get Last O(1)
* Iterate from first to last

## Benchmarks

Checkout `OptimizedLinkedList.Benchmarks`

### Adding Items

```
|            Method |       Mean |     Error |    StdDev |  Gen 0 |  Gen 1 |  Gen 2 | Allocated |
|------------------ |-----------:|----------:|----------:|-------:|-------:|-------:|----------:|
|       AddFirstBCL | 102.818 ns | 2.0356 ns | 3.5651 ns | 0.0080 | 0.0029 | 0.0004 |      48 B |
| AddFirstOptimized |   6.527 ns | 0.1034 ns | 0.0808 ns |      - |      - |      - |         - |
|        AddLastBCL | 101.499 ns | 1.2358 ns | 1.1560 ns | 0.0080 | 0.0029 | 0.0004 |      48 B |
|  AddLastOptimized |   6.476 ns | 0.1129 ns | 0.1056 ns |      - |      - |      - |         - |
```

### Removing Items

```
|               Method |     Mean |     Error |    StdDev | Allocated |
|--------------------- |---------:|----------:|----------:|----------:|
|       RemoveFirstBCL | 9.182 ns | 0.1131 ns | 0.0944 ns |         - |
| RemoveFirstOptimized | 4.909 ns | 0.1265 ns | 0.1406 ns |         - |
|        RemoveLastBCL | 8.416 ns | 0.1973 ns | 0.2111 ns |         - |
|  RemoveLastOptimized | 4.977 ns | 0.1290 ns | 0.1850 ns |         - |
```

### 100000 Operations

```
|            Method |       Mean |    Error |  StdDev |   Allocated |
|------------------ |-----------:|---------:|--------:|------------:|
|       AllInOneBCL | 1,078.8 ms | 11.05 ms | 9.79 ms | 3,844,656 B |
| AllInOneOptimized |   110.8 ms |  1.01 ms | 0.95 ms |       480 B |
```