using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace OptimizedLinkedList.Benchmarks;

[Orderer(SummaryOrderPolicy.Declared, MethodOrderPolicy.Declared)]
[MemoryDiagnoser]
[SimpleJob(invocationCount: COUNT)]
public class RemoveBenchmark
{
    // Enough invocations per iteration for test accurracy
    private const int COUNT = 10_000_000;

    private LinkedList<int> _linkedListBcl;
    private OptimizedLinkedList<int> _linkedListOptimized;

    [IterationSetup(Targets = new[] { nameof(RemoveFirstBCL), nameof(RemoveLastBCL) })]
    public void SetupBCL()
    {
        _linkedListOptimized = new OptimizedLinkedList<int>(COUNT);

        for (int i = 0; i < COUNT; i++)
        {
            _linkedListOptimized.AddFirst(1);
        }
    }

    [IterationSetup(Targets = new[] { nameof(RemoveFirstOptimized), nameof(RemoveLastOptimized) })]
    public void SetupOptimized()
    {
        _linkedListBcl = new LinkedList<int>();

        for (int i = 0; i < COUNT; i++)
        {
            _linkedListBcl.AddFirst(1);
        }
    }

    [Benchmark]
    public void RemoveFirstBCL()
    {
        _linkedListBcl.Remove(_linkedListBcl.First);
    }

    [Benchmark]
    public void RemoveFirstOptimized()
    {
        _linkedListOptimized.Remove(_linkedListOptimized.FirstIndex);
    }

    [Benchmark]
    public void RemoveLastBCL()
    {
        _linkedListBcl.Remove(_linkedListBcl.Last);
    }

    [Benchmark]
    public void RemoveLastOptimized()
    {
        _linkedListOptimized.Remove(_linkedListOptimized.LastIndex);
    }
}