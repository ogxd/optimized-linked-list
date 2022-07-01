namespace OptimizedLinkedList.Tests;

public class OptimizedLinkedListTests
{
    [Test]
    public void Add_First_Empty()
    {
        var linkedList = new OptimizedLinkedList<string>();

        Assert.AreEqual(0, linkedList.Count);

        int helloIndex = linkedList.AddFirst("hello");

        Assert.AreEqual(1, linkedList.Count);
        Assert.AreEqual(helloIndex, linkedList.FirstIndex);
        Assert.AreEqual(helloIndex, linkedList.LastIndex);
        Assert.AreEqual("hello", linkedList[linkedList.FirstIndex].Value);
        Assert.AreEqual("hello", linkedList[linkedList.LastIndex].Value);
    }

    [Test]
    public void Add_Last_Empty()
    {
        var linkedList = new OptimizedLinkedList<string>();

        Assert.AreEqual(0, linkedList.Count);

        int helloIndex = linkedList.AddLast("hello");

        Assert.AreEqual(1, linkedList.Count);
        Assert.AreEqual(helloIndex, linkedList.FirstIndex);
        Assert.AreEqual(helloIndex, linkedList.LastIndex);
        Assert.AreEqual("hello", linkedList[linkedList.FirstIndex].Value);
        Assert.AreEqual("hello", linkedList[linkedList.LastIndex].Value);
    }

    [Test]
    public void Remove()
    {
        var linkedList = new OptimizedLinkedList<int>();

        for (int i = 0; i < 100; i++)
        {
            linkedList.AddLast(i);
        }

        Assert.IsTrue(linkedList.Remove(12));
        Assert.IsTrue(linkedList.Remove(42));
        Assert.IsTrue(linkedList.Remove(37));

        Assert.IsFalse(linkedList.Remove(37));
        Assert.IsFalse(linkedList.Remove(-5));
        Assert.IsFalse(linkedList.Remove(100));

        Assert.AreEqual(97, linkedList.Count);
        Assert.AreEqual(13, linkedList[11].After);
        Assert.AreEqual(11, linkedList[13].Before);
    }

    [Test]
    public void Contains()
    {
        var linkedList = new OptimizedLinkedList<string>();

        linkedList.AddLast("hello");
        linkedList.AddLast("dear");
        linkedList.AddLast("world");

        Assert.IsTrue(linkedList.Contains(0));
        Assert.IsTrue(linkedList.Contains(1));
        Assert.IsTrue(linkedList.Contains(2));

        Assert.IsFalse(linkedList.Contains(16));
        Assert.IsFalse(linkedList.Contains(-1));
        Assert.IsFalse(linkedList.Contains(3));
    }

    [Test]
    public void Iterate()
    {
        var linkedList = new OptimizedLinkedList<int>();

        for (int i = 0; i < 100; i++)
        {
            linkedList.AddLast(i);
        }

        int j = 0;
        foreach (int k in linkedList)
        {
            Assert.AreEqual(j, k);
            j++;
        }
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void Init_Invalid_Capacity_Does_Not_Throw(int capacity)
    {
        var linkedList = new OptimizedLinkedList<int>(capacity);
        Assert.AreEqual(1, linkedList.Capacity);
    }

    [Test]
    public void Remove_All_Does_Not_Corrupt_State()
    {
        var linkedList = new OptimizedLinkedList<string>();

        int helloIndex = linkedList.AddFirst("hello");
        int worldIndex = linkedList.AddAfter("world", helloIndex);

        Assert.IsTrue(linkedList.Remove(helloIndex));
        Assert.IsTrue(linkedList.Remove(worldIndex));

        Assert.AreEqual(0, linkedList.Count);
        Assert.AreEqual(-1, linkedList.FirstIndex);
        Assert.AreEqual(-1, linkedList.LastIndex);
    }

    [Test]
    public void Random_Operations_Does_Not_Corrupt_State()
    {
        var linkedList = new OptimizedLinkedList<string>(200000);

        List<int> indices = new List<int>();

        var random = new Random();

        for (int i = 0; i < 100_000; i++)
        {
            switch (random.Next(0, 5))
            {
                case 0:
                    indices.Add(linkedList.AddFirst("test"));
                    break;
                case 1:
                    indices.Add(linkedList.AddLast("test"));
                    break;
                case 2:
                    if (indices.Count > 0)
                    {
                        int index = indices[random.Next(0, indices.Count)];
                        indices.Add(linkedList.AddAfter("test", index));
                    }
                    break;
                case 3:
                    if (indices.Count > 0)
                    {
                        int index = indices[random.Next(0, indices.Count)];
                        indices.Add(linkedList.AddBefore("test", index));
                    }
                    break;
                case 4:
                    if (indices.Count > 0)
                    {
                        int index = indices[random.Next(0, indices.Count)];
                        Assert.IsTrue(linkedList.Remove(index));
                        Assert.IsTrue(indices.Remove(index));
                    }
                    break;
            }
        }

        Console.WriteLine("Count: " + linkedList.Count);
    }
}