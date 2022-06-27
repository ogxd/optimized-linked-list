using System.Diagnostics;
using System.Collections;

namespace OptimizedLinkedList;

public class OptimizedLinkedList<T> : IEnumerable<T>
{
    private int _count;
    private int _firstIndex;
    private int _lastIndex;
    private int _firstFreeNodeIndex;
    private Node[] _array;

    public int Count => _count;

    public int Capacity => _array.Length;

    public int FirstIndex => _firstIndex;

    public int LastIndex => _lastIndex;

    public OptimizedLinkedList(int capacity = 4)
    {
        if (capacity < 1)
            capacity = 1;

        _array = new Node[capacity];

        Clear();
    }

    public Node this[int index]
    {
        get
        {
            var node = _array[index];
            if (!node.used)
                throw new ArgumentException("This index does not refer to a valid entry");
            return node;
        }
    }

    public bool Contains(int index)
    {
        return index >= 0 && index < _array.Length && _array[index].used;
    }

    public int AddAfter(T value, int afterIndex)
    {
        if (_count == 0)
        {
            afterIndex = -1;
        }
        else if (!_array[afterIndex].used)
        {
            throw new ArgumentException("This index does not refer to a valid entry");
        }

        // Create new node
        ref var newNode = ref CreateNode(value, out int index);

        // Remap links
        if (afterIndex != -1)
        {
            ref var afterNode = ref _array[afterIndex];
            newNode.before = afterIndex;
            newNode.after = afterNode.after;
            if (afterNode.after != -1)
            {
                _array[afterNode.after].before = index;
            }
            afterNode.after = index;

            // If inserted after last, it becomes last
            if (_lastIndex == afterIndex)
                _lastIndex = index;
        }
        else
        {
            _firstIndex = index;
            _lastIndex = index;
        }

        // Assign value
        newNode.value = value;

        return index;
    }

    public int AddLast(T value) => AddAfter(value, _lastIndex);

    public int AddBefore(T value, int beforeIndex)
    {
        if (_count == 0)
        {
            beforeIndex = -1;
        }
        else if (!_array[beforeIndex].used)
        {
            throw new ArgumentException("This index does not refer to a valid entry");
        }

        // Create new node
        ref var newNode = ref CreateNode(value, out int index);

        // Remap links
        if (beforeIndex != -1)
        {
            ref var beforeNode = ref _array[beforeIndex];
            newNode.before = beforeNode.before;
            newNode.after = beforeIndex;
            if (beforeNode.before != -1)
            {
                _array[beforeNode.before].after = index;
            }
            beforeNode.before = index;

            // If inserted before first, it becomes first
            if (_firstIndex == beforeIndex)
                _firstIndex = index;
        }
        else
        {
            _firstIndex = index;
            _lastIndex = index;
        }

        // Assign value
        newNode.value = value;

        return index;
    }

    public int AddFirst(T value) => AddBefore(value, _firstIndex);

    public void Clear()
    {
        FillFree(0, _array.Length);
        _count = 0;
        _firstIndex = -1;
        _lastIndex = -1;
    }

    public bool Remove(int index)
    {
        if (index < 0 || index >= _array.Length)
            return false;

        ref var node = ref _array[index];
        if (!node.used)
            return false;

        // Remap links
        if (node.before == -1)
        {
            Debug.Assert(index == _firstIndex);
            _firstIndex = node.after;
        }
        else
        {
            ref var beforeNode = ref _array[node.before];
            beforeNode.after = node.after;
        }
        if (node.after == -1)
        {
            Debug.Assert(index == _lastIndex);
            _lastIndex = node.before;
        }
        else
        {
            ref var afterNode = ref _array[node.after];
            afterNode.before = node.before;
        }

        // Mark node as free
        node.used = false;
        node.value = default; // Free reference
        _array[index].before = -1;
        _array[index].after = _firstFreeNodeIndex;
        _firstFreeNodeIndex = index;

        // Decrement count
        _count--;

        return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new Enumerator(this);
    }

    private ref Node CreateNode(T value, out int index)
    {
        if (_array.Length == _count)
        {
            Array.Resize(ref _array, _array.Length * 2);
            Debug.Assert(_firstFreeNodeIndex == -1);

            FillFree(_count, _array.Length - _count);
        }

        Debug.Assert(_firstFreeNodeIndex != -1);

        index = _firstFreeNodeIndex;
        ref var newNode = ref _array[_firstFreeNodeIndex];
        _firstFreeNodeIndex = newNode.after;
        newNode.used = true;
        newNode.after = -1;
        newNode.before = -1;
        newNode.value = value;

        _count++;

        return ref newNode;
    }

    private void FillFree(int start, int count)
    {
        int i = _firstFreeNodeIndex = start;
        for (; i < start + count - 1; i++)
        {
            _array[i].after = i + 1;
        }
        _array[start + count - 1].after = -1;
    }

    private bool CheckState()
    {
        if (_array[_firstIndex].before > -1)
            return false;

        if (_array[_lastIndex].after > -1)
            return false;

        int current = _firstIndex;
        while (current != -1)
        {
            Node node = _array[current];
            if (!node.used)
                return false;

            if (node.after != -1)
            {
                if (_array[node.after].before != current)
                {
                    return false;
                }
            }

            current = node.after;
        }

        return true;
    }

    public struct Enumerator : IEnumerator<T>
    {
        private readonly OptimizedLinkedList<T> _list;
        private int _index;

        public Enumerator(OptimizedLinkedList<T> list)
        {
            _list = list;
            _index = -2;
        }

        public T Current => _list._array[_index].Value;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_index == -1)
                return false;

            if (_index == -2)
            {
                _index = _list._firstIndex;
            }
            else
            {
                ref var node = ref _list._array[_index];
                _index = node.after;
            }

            return _index != -1;
        }

        public Enumerator GetEnumerator() => this;

        public void Reset()
        {
            _index = _list.FirstIndex;
        }

        public void Dispose() { }
    }

    public struct Node
    {
        // true if node holds an actual value, false if free
        internal bool used;
        internal int after;
        internal int before;
        internal T value;

        public int After => after;

        public int Before => before;

        public T Value => value;

        public Node()
        {
            used = false;
            after = -1;
            before = -1;
            value = default;
        }
    }
}
