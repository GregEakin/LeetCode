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

public class CountIntegersInIntervals
{
    public class CountIntervalsTooSlow
    {
        private List<(int l, int r)> _intervals = new();
        private int _sum;

        public void Add(int left, int right)
        {
            var merged = new List<(int l, int r)>();
            right++;
            _sum += right - left;
            foreach (var (l, r) in _intervals)
            {
                if (merged.Count == 0 && right < l ||
                    merged.Count > 0 && left > merged[^1].r && right < l)
                    merged.Add((left, right));

                if (r < left || l > right)
                {
                    merged.Add((l, r));
                    continue;
                }

                var max = Math.Max(l, left);
                var min = Math.Min(r, right);
                _sum -= min - max;
                left = Math.Min(l, left);
                right = Math.Max(r, right);
            }

            if (merged.Count == 0 || left > merged[^1].r) merged.Add((left, right));
            _intervals = merged;
        }

        public int Count()
        {
            return _sum;
        }
    }

    public class CountIntervals
    {
        private readonly SortedList<int, (int, int)> _list = new SortedList<int, (int, int)>();
        private int _count;

        public static int BinarySearch(IList<int> keys, int value)
        {
            var left = 0;
            var right = keys.Count - 1;
            while (left <= right)
            {
                var mid = (left + right) / 2;
                if (keys[mid] == value) return mid;
                if (keys[mid] > value) right = mid - 1;
                else left = mid + 1;
            }

            return right;
        }

        public void Add(int left, int right)
        {
            right++;
            var index = BinarySearch(_list.Keys, left);
            if (index >= 0)
            {
                var (_, e) = _list[_list.Keys[index]];
                if (left < e)
                    left = e;
            }

            var next = index + 1;
            var painted = 0;
            var paintedKey = new List<int>();
            while (next < _list.Count && _list[_list.Keys[next]].Item2 <= right)
            {
                painted += _list[_list.Keys[next]].Item2 - _list[_list.Keys[next]].Item1;
                paintedKey.Add(_list.Keys[next]);
                next++;
            }

            if (next < _list.Count)
                right = Math.Min(_list[_list.Keys[next]].Item1, right);

            var c = right > left ? right - left - painted : 0;
            if (c <= 0) return;
            _count += c;

            foreach (var key in paintedKey)
                _list.Remove(key);

            _list.Add(left, (left, right));
        }

        public int Count()
        {
            return _count;
        }
    }

    /**
     * Your CountIntervals object will be instantiated and called as such:
     * CountIntervals obj = new CountIntervals();
     * obj.Add(left,right);
     * int param_2 = obj.Count();
     */
    [Fact]
    public void Example1()
    {
        // Input
        //         ["CountIntervals", "add", "add", "count", "add", "count"]
        //     [[], [2, 3], [7, 10], [], [5, 8], []]
        // Output
        //     [null, null, null, 6, null, 8]

        var input1 = new[] { "CountIntervals", "add", "add", "count", "add", "count" };
        var input2 = new[]
        {
            Array.Empty<int>(), new[] { 2, 3 }, new[] { 7, 10 }, Array.Empty<int>(), new[] { 5, 8 }, Array.Empty<int>()
        };
        var output = new[] { -1, -1, -1, 6, -1, 8 };

        CountIntervals? counter = null;
        for (var index = 0; index < input1.Length; index++)
        {
            var cmd = input1[index];
            var agr = input2[index];
            var ans = output[index];
            switch (cmd)
            {
                case "CountIntervals":
                    counter = new CountIntervals();
                    break;

                case "add":
                    counter!.Add(agr[0], agr[1]);
                    break;

                case "count":
                    Assert.Equal(ans, counter!.Count());
                    break;
            }
        }
    }
}