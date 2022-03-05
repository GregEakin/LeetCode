//    Copyright 2022 Gregory Eakin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace HeapChecks;

public class Heap<T> : IEnumerable<T> where T : IComparable<T>
{
    private readonly List<T> _data;

    public static int Parent(int i) => (i - 1) / 2;
    public static int Left(int i) => 2 * i + 1;
    public static int Right(int i) => 2 * i + 2;

    public int Count => _data.Count;
    public int Capacity => _data.Capacity;
    public List<T> RawData => _data;

    public T this[int index]
    {
        get => _data[index];
        set => _data[index] = value;
    }

    public Heap()
    {
        _data = new List<T>();
    }

    public Heap(IEnumerable<T> a)
    {
        _data = new List<T>(a);
        for (var i = (_data.Count - 2) / 2; i >= 0; i--)
            MaxHeapify(i);
    }

    public IEnumerator<T> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(T item)
    {
        var i = _data.Count;
        _data.Add(item);
        while (i > 0)
        {
            var parent = Parent(i);
            if (_data[parent].CompareTo(_data[i]) > 0) return;
            (_data[i], _data[parent]) = (_data[parent], _data[i]);
            i = parent;
        }
    }

    public T RemoveMax()
    {
        if (_data.Count == 0) throw new Exception("heap underflow");
        var max = _data[0];
        var last = _data[^1];
        _data.RemoveAt(_data.Count - 1);
        if (_data.Count == 0) return max;

        _data[0] = last;
        MaxHeapify(0);
        return max;
    }

    public T PushPopMax(T item)
    {
        if (_data.Count == 0 || item.CompareTo(_data[0]) > 0)
            return item;

        var max = _data[0];
        _data[0] = item;
        MaxHeapify(0);
        return max;
    }

    public void MaxHeapify(int i)
    {
        var left = Left(i);
        var right = Right(i);
        var largest = i;
        if (left < Count && _data[left].CompareTo(_data[largest]) > 0) largest = left;
        if (right < Count && _data[right].CompareTo(_data[largest]) > 0) largest = right;
        if (largest == i) return;
        (_data[i], _data[largest]) = (_data[largest], _data[i]);
        MaxHeapify(largest);
    }

    public bool HeapProperty()
    {
        for (var i = 1; i < Count; i++)
        {
            var parent = Parent(i);
            if (_data[parent].CompareTo(_data[i]) < 0) return false;
        }

        return true;
    }
}

public class HeapTests
{
    private readonly int[] _answer = { 16, 14, 10, 8, 7, 9, 3, 2, 4, 1 };

    [Fact]
    public void Test71()
    {
        var data = new[] { 16, 14, 10, 8, 7, 9, 3, 2, 4, 1 };
        var heap = new Heap<int>(data);

        Assert.Equal(_answer, heap.RawData);
        Assert.True(heap.HeapProperty());
    }

    [Fact]
    public void Test72()
    {
        var data = new[] { 16, 4, 10, 14, 7, 9, 3, 2, 8, 1 };
        var heap = new Heap<int>(data);

        heap.MaxHeapify(1);
        Assert.Equal(_answer, heap.RawData);
    }

    [Fact]
    public void Test73()
    {
        var data = new[] { 4, 1, 3, 2, 16, 9, 10, 14, 8, 7 };
        var heap = new Heap<int>(data);

        heap.MaxHeapify(1);
        Assert.Equal(_answer, heap.RawData);
    }

    [Fact]
    public void HeapCtorEmpty()
    {
        var heap = new Heap<int>(Array.Empty<int>());
        Assert.Equal(0, heap.Count);
    }

    [Fact]
    public void HeapCtorOne()
    {
        var heap = new Heap<int>(new[] { 0 });
        Assert.Equal(new[] { 0 }, heap);
    }

    [Fact]
    public void HeapCtorTwo()
    {
        var heap = new Heap<int>(new[] { 0, 1 });
        Assert.Equal(new[] { 1, 0 }, heap);
    }

    [Fact]
    public void HeapCtorThree()
    {
        var heap = new Heap<int>(new[] { 0, 1, 2 });
        Assert.Equal(new[] { 2, 1, 0 }, heap);
    }

    [Fact]
    public void HeapCtorFour()
    {
        var heap = new Heap<int>(new[] { 0, 1, 2, 3 });
        Assert.Equal(new[] { 3, 1, 2, 0 }, heap);
    }

    [Fact]
    public void TestAdd()
    {
        var data = new[] { 16, 14, 10, 8, 7, 9, 3, 2, 4 };
        var heap = new Heap<int>(data) { 1 };

        Assert.Equal(_answer, heap.RawData);
    }

    [Fact]
    public void TestAdd2()
    {
        var heap = new Heap<int> { 16, 14, 10, 8, 7, 9, 3, 2, 4, 1 };

        Assert.Equal(_answer, heap.RawData);
    }

    [Fact]
    public void ExtractTest()
    {
        var heap = new Heap<int> { 1, 2, 3, 4, 7, 8, 9, 10, 14, 16 };
        var data = new[] { 16, 14, 10, 9, 8, 7, 4, 3, 2, 1 };
        foreach (var max in data)
            Assert.Equal(max, heap.RemoveMax());

        Assert.Equal(0, heap.Count);
    }

    [Fact]
    public void PushPopTest()
    {
        var heap = new Heap<int> { 1, 2, 3, 4, 7, 8, 9, 10, 14, 16 };
        Assert.Equal(20, heap.PushPopMax(20));
        Assert.Equal(16, heap.PushPopMax(12));
        Assert.Equal(14, heap.PushPopMax(6));
        Assert.Equal(12, heap.PushPopMax(0));
        Assert.Equal(10, heap.PushPopMax(5));

        var data = new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
        foreach (var max in data)
            Assert.Equal(max, heap.RemoveMax());

        Assert.Equal(0, heap.Count);
    }

    [Fact]
    public void StartLoopCheck()
    {
        static int LowestTree(int count) => (count - 2) / 2;

        Assert.Equal(-1, LowestTree(0));
        Assert.Equal(0, LowestTree(1));
        Assert.Equal(0, LowestTree(2));
        Assert.Equal(0, LowestTree(3));
        Assert.Equal(1, LowestTree(4));
        Assert.Equal(1, LowestTree(5));
        Assert.Equal(2, LowestTree(6));
    }

    [Fact]
    public void HeapSortTest()
    {
        static void MaxHeapify(IList<int> data, int count, int i)
        {
            var left = 2 * i + 1;
            var right = 2 * i + 2;
            var largest = i;
            if (left < count && data[left].CompareTo(data[largest]) > 0) largest = left;
            if (right < count && data[right].CompareTo(data[largest]) > 0) largest = right;
            if (largest == i) return;
            (data[i], data[largest]) = (data[largest], data[i]);
            MaxHeapify(data, count, largest);
        }

        var heap = new Heap<int> { 1, 2, 3, 4, 7, 8, 9, 10, 14, 16 };
        var data = new List<int>(heap.RawData);
        var count = data.Count;
        for (var i = data.Count - 1; i > 0; i--)
        {
            (data[0], data[i]) = (data[i], data[0]);
            count--;
            MaxHeapify(data, count, 0);
        }

        Assert.Equal(new[] { 1, 2, 3, 4, 7, 8, 9, 10, 14, 16 }, data);
    }
}