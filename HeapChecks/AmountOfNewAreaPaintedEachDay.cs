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

public class AmountOfNewAreaPaintedEachDay
{
    public class SolutionSlow
    {
        public int[] AmountPainted(int[][] paint)
        {
            var painted = new BitArray(100000);
            var log = new int[paint.Length];
            for (var i = 0; i < paint.Length; i++)
            {
                var order = paint[i];
                var start = order[0];
                var end = order[1];

                for (var j = start; j < end; j++)
                {
                    if (painted[j]) continue;
                    painted[j] = true;
                    log[i]++;
                }
            }

            return log;
        }
    }

    public class SolutionFast
    {
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

        public int[] AmountPainted(int[][] paint)
        {
            var list = new SortedList<int, (int, int)>();
            var hours = new int[paint.Length];

            for (var i = 0; i < paint.Length; i++)
            {
                var start = paint[i][0];
                var end = paint[i][1];
                var index = BinarySearch(list.Keys, start);
                if (index >= 0)
                {
                    var (_, e) = list[list.Keys[index]];
                    if (start < e)
                        start = e;
                }

                var next = index + 1;
                var painted = 0;
                var paintedKey = new List<int>();
                while (next < list.Count && list[list.Keys[next]].Item2 <= end)
                {
                    painted += list[list.Keys[next]].Item2 - list[list.Keys[next]].Item1;
                    paintedKey.Add(list.Keys[next]);
                    next++;
                }

                if (next < list.Count)
                    end = Math.Min(list[list.Keys[next]].Item1, end);

                hours[i] = end > start ? end - start - painted : 0;
                if (hours[i] <= 0) continue;

                foreach (var key in paintedKey)
                    list.Remove(key);

                list.Add(start, (start, end));
            }

            return hours;
        }
    }

    public class SolutionSimple
    {
        public int[] AmountPainted(int[][] paint)
        {
            var track = new int[50001];
            var res = new int[paint.Length];
            for (var i = 0; i < paint.Length; ++i)
            {
                var start = paint[i][0];
                var end = paint[i][1];
                while (start < end)
                {
                    var jump = Math.Max(start + 1, track[start]);
                    res[i] += track[start] == 0 ? 1 : 0;
                    track[start] = Math.Max(track[start], end);
                    start = jump;
                }
            }

            return res;
        }
    }

    public class Solution
    {
        public int[] AmountPainted(int[][] paint)
        {
            var intervals = new List<int[]>();
            var hours = new int[paint.Length];
            for (var i = 0; i < paint.Length; i++)
            {
                var p = paint[i];
                var merged = new List<int[]>();
                var left = p[0];
                var right = p[1];
                var len = right - left;
                foreach (var interval in intervals)
                {
                    if (merged.Count == 0 && p[1] < interval[0] ||
                        merged.Count > 0 && p[0] > merged[^1][1] && p[1] < interval[0])
                        merged.Add(p);

                    if (interval[1] < p[0] || interval[0] > p[1])
                    {
                        merged.Add(interval);
                        continue;
                    }

                    var intersection = new int[2];
                    intersection[0] = Math.Max(interval[0], left);
                    intersection[1] = Math.Min(interval[1], right);
                    len -= intersection[1] - intersection[0];
                    p[0] = Math.Min(interval[0], p[0]);
                    p[1] = Math.Max(interval[1], p[1]);
                }

                if (merged.Count == 0 || p[0] > merged[^1][1]) merged.Add(p);
                hours[i] = len;
                intervals = merged;
            }

            return hours;
        }
    }

    [Fact]
    public void Answer1()
    {
        var paint = new[] { new[] { 1, 5 }, new[] { 2, 4 } };
        var solution = new Solution();
        Assert.Equal(new[] { 4, 0 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Answer2()
    {
        var paint = new[] { new[] { 0, 5 }, new[] { 0, 2 }, new[] { 0, 3 }, new[] { 0, 4 }, new[] { 0, 5 } };
        var solution = new Solution();
        Assert.Equal(new[] { 5, 0, 0, 0, 0 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Answer3()
    {
        var paint = new[] { new[] { 2, 5 }, new[] { 7, 10 }, new[] { 3, 9 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 3, 2 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Example1()
    {
        var paint = new[] { new[] { 1, 4 }, new[] { 4, 7 }, new[] { 5, 8 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 3, 1 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Example2()
    {
        var paint = new[] { new[] { 1, 4 }, new[] { 5, 8 }, new[] { 4, 7 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 3, 1 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Example3()
    {
        var paint = new[] { new[] { 1, 5 }, new[] { 4, 5 } };
        var solution = new Solution();
        Assert.Equal(new[] { 4, 0 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Test1()
    {
        var paint = new[] { new[] { 1, 3 }, new[] { 5, 7 }, new[] { 2, 4 } };
        var solution = new Solution();
        Assert.Equal(new[] { 2, 2, 1 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Test2()
    {
        var paint = new[] { new[] { 1, 4 }, new[] { 6, 7 }, new[] { 12, 14 }, new[] { 2, 13 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 1, 2, 7 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Test3()
    {
        var paint = new[] { new[] { 1, 4 }, new[] { 6, 7 }, new[] { 12, 14 }, new[] { 5, 13 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 1, 2, 6 }, solution.AmountPainted(paint));
    }
}