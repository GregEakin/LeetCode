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