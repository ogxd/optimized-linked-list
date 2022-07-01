using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace OptimizedLinkedList.Benchmarks;

[Orderer(SummaryOrderPolicy.Declared, MethodOrderPolicy.Declared)]
[MemoryDiagnoser]
[SimpleJob(invocationCount: 1)]
public class AllInOneBenchmark
{
    private const int COUNT = 100_000;

    private LinkedList<string> _linkedListBcl;
    private OptimizedLinkedList<string> _linkedListOptimized;

    private List<LinkedListNode<string>> _nodesBcl;
    private List<int> _nodesOptimized;

    private Random _random;

    [IterationSetup(Target = nameof(AllInOneBCL))]
    public void SetupBCL()
    {
        _linkedListBcl = new LinkedList<string>();
        _nodesBcl = new List<LinkedListNode<string>>(COUNT);
        _random = new Random(3); // Deterministic randomness
    }

    [IterationSetup(Target = nameof(AllInOneOptimized))]
    public void SetupOptimized()
    {
        _linkedListOptimized = new OptimizedLinkedList<string>(COUNT);
        _nodesOptimized = new List<int>(COUNT);
        _random = new Random(3); // Deterministic randomness
    }

    [Benchmark]
    public void AllInOneBCL()
    {
        if (_nodesBcl.Count > COUNT)
            throw new InvalidOperationException();

        for (int i = 0; i < COUNT; i++)
        {
            switch (_random.Next(0, 5))
            {
                case 0:
                    _nodesBcl.Add(_linkedListBcl.AddFirst("test"));
                    break;
                case 1:
                    _nodesBcl.Add(_linkedListBcl.AddLast("test"));
                    break;
                case 2:
                    if (_nodesBcl.Count > 0)
                    {
                        var node = _nodesBcl[_random.Next(0, _nodesBcl.Count)];
                        _nodesBcl.Add(_linkedListBcl.AddAfter(node, "test"));
                    }
                    break;
                case 3:
                    if (_nodesBcl.Count > 0)
                    {
                        var node = _nodesBcl[_random.Next(0, _nodesBcl.Count)];
                        _nodesBcl.Add(_linkedListBcl.AddBefore(node, "test"));
                    }
                    break;
                case 4:
                    if (_nodesBcl.Count > 0)
                    {
                        var node = _nodesBcl[_random.Next(0, _nodesBcl.Count)];
                        _linkedListBcl.Remove(node);
                        _nodesBcl.Remove(node);
                    }
                    break;
            }
        }
    }

    [Benchmark]
    public void AllInOneOptimized()
    {
        if (_nodesOptimized.Count > COUNT)
            throw new InvalidOperationException();

        for (int i = 0; i < COUNT; i++)
        {
            switch (_random.Next(0, 5))
            {
                case 0:
                    _nodesOptimized.Add(_linkedListOptimized.AddFirst("test"));
                    break;
                case 1:
                    _nodesOptimized.Add(_linkedListOptimized.AddLast("test"));
                    break;
                case 2:
                    if (_nodesOptimized.Count > 0)
                    {
                        int index = _nodesOptimized[_random.Next(0, _nodesOptimized.Count)];
                        _nodesOptimized.Add(_linkedListOptimized.AddAfter("test", index));
                    }
                    break;
                case 3:
                    if (_nodesOptimized.Count > 0)
                    {
                        int index = _nodesOptimized[_random.Next(0, _nodesOptimized.Count)];
                        _nodesOptimized.Add(_linkedListOptimized.AddBefore("test", index));
                    }
                    break;
                case 4:
                    if (_nodesOptimized.Count > 0)
                    {
                        int index = _nodesOptimized[_random.Next(0, _nodesOptimized.Count)];
                        _linkedListOptimized.Remove(index);
                        _nodesOptimized.Remove(index);
                    }
                    break;
            }
        }
    }
}