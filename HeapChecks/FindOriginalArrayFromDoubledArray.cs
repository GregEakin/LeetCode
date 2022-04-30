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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace HeapChecks;

public class FindOriginalArrayFromDoubledArray
{
    public class Solution
    {
        public class CountedSet<T> where T : notnull
        {
            private readonly SortedDictionary<T, int> _data = new();

            public CountedSet(IEnumerable<T> values)
            {
                foreach (var value in values)
                    Add(value);
            }

            public void Add(T item)
            {
                if (_data.TryAdd(item, 1)) return;
                _data[item]++;
            }

            public bool Remove(T item)
            {
                if (!_data.TryGetValue(item, out var value)) return false;
                if (value <= 1) _data.Remove(item);
                else _data[item] = value - 1;
                return true;
            }

            public bool Empty => _data.Count == 0;

            public T First => _data.Keys.First();
        }

        public int[] FindOriginalArray(int[] changed)
        {
            if (changed.Length % 2 == 1) return Array.Empty<int>();
            var answer = new List<int>();
            var set = new CountedSet<int>(changed);
            while (!set.Empty)
            {
                var value = set.First;
                var found = set.Remove(value) && set.Remove(2 * value);
                if (!found)
                    return Array.Empty<int>();
                answer.Add(value);
            }

            return answer.ToArray();
        }
    }

    [Fact]
    public void Example1()
    {
        var changed = new[] { 1, 3, 4, 2, 6, 8 };
        var solution = new Solution();
        Assert.Equal(new[] { 1, 3, 4 }, solution.FindOriginalArray(changed));
    }

    [Fact]
    public void Example2()
    {
        var changed = new[] { 6, 3, 0, 1 };
        var solution = new Solution();
        Assert.Equal(Array.Empty<int>(), solution.FindOriginalArray(changed));
    }

    [Fact]
    public void Example3()
    {
        var changed = new[] { 1 };
        var solution = new Solution();
        Assert.Equal(Array.Empty<int>(), solution.FindOriginalArray(changed));
    }

    [Fact]
    public void Test1()
    {
        var changed = new[] { 0, 0 };
        var solution = new Solution();
        Assert.Equal(new[] { 0 }, solution.FindOriginalArray(changed));
    }
}