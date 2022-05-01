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

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HeapChecks;

public class MaxPointsOnALine
{
    public class Solution
    {
        public static (float, float) CalcLine(int[] p1, int[] p2)
        {
            if (p1[0] == p2[0]) return (p1[0], float.PositiveInfinity);
            var m = (p2[1] - (float)p1[1]) / (p2[0] - (float)p1[0]);
            var b = p2[1] - m * p2[0];
            return (m, b);
        }

        public int MaxPoints(int[][] points)
        {
            if (points.Length <= 1) return points.Length;
            var dict = new Dictionary<(float, float), HashSet<int[]>>();

            var n = points.Length;
            for (var i = 0; i < n; i++)
            for (var j = i + 1; j < n; j++)
            {
                var info = CalcLine(points[i], points[j]);
                if (!dict.TryGetValue(info, out var set))
                {
                    set = new HashSet<int[]>();
                    dict.Add(info, set);
                }

                dict[info].Add(points[i]);
                dict[info].Add(points[j]);
            }

            return dict.Values.Select(s => s.Count).Max();
        }
    }

    [Fact]
    public void Answer1()
    {
        var points = new[] { new[] { 0, 0 } };
        var solution = new Solution();
        Assert.Equal(1, solution.MaxPoints(points));
    }

    [Fact]
    public void Example1()
    {
        var points = new[] { new[] { 1, 1 }, new[] { 2, 2 }, new[] { 3, 3 } };
        var solution = new Solution();
        Assert.Equal(3, solution.MaxPoints(points));
    }

    [Fact]
    public void Example2()
    {
        var points = new[]
            { new[] { 1, 1 }, new[] { 3, 2 }, new[] { 5, 3 }, new[] { 4, 1 }, new[] { 2, 3 }, new[] { 1, 4 } };
        var solution = new Solution();
        Assert.Equal(4, solution.MaxPoints(points));
    }
}