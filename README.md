# Optimized LinkedList<T>

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