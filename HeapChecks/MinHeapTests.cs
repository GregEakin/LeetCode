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
using System.Collections.Generic;
using Xunit;

namespace HeapChecks;

public class MinHeapTests
{
    private readonly int[] _answer = { 1, 2, 3, 4, 7, 9, 10, 8, 16, 14 };

    [Fact]
    public void Test71()
    {
        var data = new[] { 16, 14, 10, 8, 7, 9, 3, 2, 4, 1 };
        var heap = new MinHeap<int>(data);

        Assert.Equal(_answer, heap.RawData);
        Assert.True(heap.HeapProperty());
    }

    // [Fact]
    // public void Test72()
    // {
    //     var data = new[] { 16, 4, 10, 14, 7, 9, 3, 2, 8, 1 };
    //     var heap = new MinHeap<int>(data);
    //
    //     heap.MaxHeapify(1);
    //     Assert.Equal(_answer, heap.RawData);
    // }
    //
    // [Fact]
    // public void Test73()
    // {
    //     var data = new[] { 4, 1, 3, 2, 16, 9, 10, 14, 8, 7 };
    //     var heap = new MinHeap<int>(data);
    //
    //     heap.MaxHeapify(1);
    //     Assert.Equal(_answer, heap.RawData);
    // }

    [Fact]
    public void HeapCtorEmpty()
    {
        var heap = new MinHeap<int>(Array.Empty<int>());
        Assert.Equal(0, heap.Count);
    }

    [Fact]
    public void HeapCtorOne()
    {
        var heap = new MinHeap<int>(new[] { 0 });
        Assert.Equal(new[] { 0 }, heap);
    }

    [Fact]
    public void HeapCtorTwo()
    {
        var heap = new MinHeap<int>(new[] { 1, 0 });
        Assert.Equal(new[] { 0, 1 }, heap);
    }

    [Fact]
    public void HeapCtorThree()
    {
        var heap = new MinHeap<int>(new[] { 2, 1, 0 });
        Assert.Equal(new[] { 0, 1, 2 }, heap);
    }

    [Fact]
    public void HeapCtorFour()
    {
        var heap = new MinHeap<int>(new[] { 3, 2, 1, 0 });
        Assert.Equal(new[] { 0, 2, 1, 3 }, heap);
    }

    // [Fact]
    // public void TestAdd()
    // {
    //     var data = new[] { 16, 14, 10, 8, 7, 9, 3, 2, 4 };
    //     var heap = new MinHeap<int>(data) { 1 };
    //
    //     Assert.Equal(_answer, heap.RawData);
    // }
    //
    // [Fact]
    // public void TestAdd2()
    // {
    //     var heap = new MinHeap<int> { 16, 14, 10, 8, 7, 9, 3, 2, 4, 1 };
    //
    //     Assert.Equal(_answer, heap.RawData);
    // }

    [Fact]
    public void ExtractTest()
    {
        var heap = new MinHeap<int> { 16, 14, 10, 9, 8, 7, 4, 3, 2, 1 };
        var data = new[] { 1, 2, 3, 4, 7, 8, 9, 10, 14, 16 };
        foreach (var min in data)
            Assert.Equal(min, heap.RemoveMin());

        Assert.Equal(0, heap.Count);
    }

    [Fact]
    public void PushPopTest()
    {
        var heap = new MinHeap<int> { 16, 14, 10, 9, 8, 7, 4, 3, 2, 1 };
        Assert.Equal(0, heap.PushPopMin(0));
        Assert.Equal(1, heap.PushPopMin(12));
        Assert.Equal(2, heap.PushPopMin(6));
        Assert.Equal(3, heap.PushPopMin(20));
        Assert.Equal(4, heap.PushPopMin(5));

        var data = new[] { 5, 6, 7, 8, 9, 10, 12, 14, 16, 20 };
        foreach (var min in data)
            Assert.Equal(min, heap.RemoveMin());

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
            if (left < count && data[left].CompareTo(data[largest]) < 0) largest = left;
            if (right < count && data[right].CompareTo(data[largest]) < 0) largest = right;
            if (largest == i) return;
            (data[i], data[largest]) = (data[largest], data[i]);
            MaxHeapify(data, count, largest);
        }

        var heap = new MinHeap<int> { 16, 14, 10, 9, 8, 7, 4, 3, 2, 1 };
        var data = new List<int>(heap.RawData);
        var count = data.Count;
        for (var i = data.Count - 1; i > 0; i--)
        {
            (data[0], data[i]) = (data[i], data[0]);
            count--;
            MaxHeapify(data, count, 0);
        }

        Assert.Equal(new[] { 16, 14, 10, 9, 8, 7, 4, 3, 2, 1 }, data);
    }
}