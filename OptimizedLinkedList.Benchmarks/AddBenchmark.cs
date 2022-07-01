using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace OptimizedLinkedList.Benchmarks;

[Orderer(SummaryOrderPolicy.Declared, MethodOrderPolicy.Declared)]
[MemoryDiagnoser]
[SimpleJob(invocationCount: COUNT)]
public class AddBenchmark
{
    // Enough invocations per iteration for test accurracy
    private const int COUNT = 10_000_000;

    private LinkedList<int> _linkedListBcl;
    private OptimizedLinkedList<int> _linkedListOptimized;

    [IterationSetup(Targets = new[] { nameof(AddFirstBCL), nameof(AddLastBCL) })]
    public void SetupBCL()
    {
        _linkedListBcl = new LinkedList<int>();
    }

    [IterationSetup(Targets = new[] { nameof(AddFirstOptimized), nameof(AddLastOptimized) })]
    public void SetupOptimized()
    {
        _linkedListOptimized = new OptimizedLinkedList<int>(COUNT);
    }

    [Benchmark]
    public void AddFirstBCL()
    {
        _linkedListBcl.AddFirst(1);
    }

    [Benchmark]
    public void AddFirstOptimized()
    {
        _linkedListOptimized.AddFirst(1);

#if DEBUG
        if (_linkedListOptimized.Count > COUNT)
            throw new InvalidOperationException("Adjust benchmark settings so that capacity isn't reached to avoid biais");
#endif
    }

    [Benchmark]
    public void AddLastBCL()
    {
        _linkedListBcl.AddLast(1);
    }

    [Benchmark]
    public void AddLastOptimized()
    {
        _linkedListOptimized.AddLast(1);

#if DEBUG
        if (_linkedListOptimized.Count > COUNT)
            throw new InvalidOperationException("Adjust benchmark settings so that capacity isn't reached to avoid biais");
#endif
    }
}