using System;
using Xunit;

namespace HeapChecks;

public class HeapBook // : IEnumerable<T>
{
    private readonly int[] _data;

    public static int Parent(int i) => i / 2;
    public static int Left(int i) => 2 * i;
    public static int Right(int i) => 2 * i + 1;

    public int Length => _data[0];
    public int[] RawData => _data;

    public HeapBook(int[] a)
    {
        var size = a.Length;
        _data = new int[size + 1];
        _data[0] = size;
        Array.Copy(a, 0, _data, 1, size);
        for (var i = size / 2; i >= 1; i--)
            MaxHeapify(i);
    }

    public void MaxHeapify(int i)
    {
        var left = Left(i);
        var right = Right(i);
        var largest = i;
        if (left <= Length && _data[left] > _data[i]) largest = left;
        if (right <= Length && _data[right] > _data[largest]) largest = right;
        if (largest == i) return;
        (_data[i], _data[largest]) = (_data[largest], _data[i]);
        MaxHeapify(largest);
    }

    public static HeapBook HeapSort(int[] a)
    {
        var heap = new HeapBook(a);
        for (var i = a.Length; i >= 2; i--)
        {
            (heap.RawData[1], heap.RawData[i]) = (heap.RawData[i], heap.RawData[1]);
        }

        return heap;
    }
}

public class HeapBookTests
{
    private readonly int[] _answer = { 10, 16, 14, 10, 8, 7, 9, 3, 2, 4, 1 };

    [Fact]
    public void Test71()
    {
        var data = new[] { 16, 14, 10, 8, 7, 9, 3, 2, 4, 1 };
        var heap = new HeapBook(data);

        Assert.Equal(_answer, heap.RawData);
    }

    [Fact]
    public void Test72()
    {
        var data = new[] { 16, 4, 10, 14, 7, 9, 3, 2, 8, 1 };
        var heap = new HeapBook(data);

        heap.MaxHeapify(2);
        Assert.Equal(_answer, heap.RawData);
    }

    [Fact]
    public void Test73()
    {
        var data = new[] { 4, 1, 3, 2, 16, 9, 10, 14, 8, 7 };
        var heap = new HeapBook(data);

        heap.MaxHeapify(2);
        Assert.Equal(_answer, heap.RawData);
    }
}