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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class MaximumNumberOfPointsWithCost
{
    public class SolutionSlow
    {
        public long MaxPoints(int[][] points)
        {
            var m = points.Length;
            var n = points[0].Length;

            var dp1 = new long[n];

            for (var r = 0; r < m; r++)
            {
                var row = points[r];

                var dp0 = dp1;
                dp1 = new long[n];
                for (var c = 0; c < n; c++)
                {
                    var max = 0L;
                    if (r > 0)
                        for (var k = 0; k < n; k++)
                        {
                            var pts = dp0[k] - Math.Abs(c - k);
                            max = Math.Max(max, pts);
                        }

                    dp1[c] = row[c] + max;
                }
            }

            var best = 0L;
            for (var col = 0; col < n; col++)
                best = Math.Max(best, dp1[col]);

            return best;
        }
    }

    public class Solution
    {
        public long MaxPoints(int[][] points)
        {
            var m = points.Length;
            var n = points[0].Length;
            var next = new long[n];
            Array.Copy(points[0], next, n);

            for (var i = 1; i < m; i++)
            {
                var row = next;
                next = new long[n];
                Array.Copy(points[i], next, n);

                for (var j = 1; j < n; j++)
                    row[j] = Math.Max(row[j], row[j - 1] - 1L);

                for (var j = n - 2; j >= 0; j--)
                    row[j] = Math.Max(row[j], row[j + 1] - 1L);

                for (var j = 0; j < n; j++)
                    next[j] += row[j];
            }

            return next.Max();
        }
    }

    [Fact]
    public void Example1()
    {
        var points = new[] { new[] { 1, 2, 3 }, new[] { 1, 5, 1 }, new[] { 3, 1, 1 } };
        var solution = new Solution();
        Assert.Equal(9, solution.MaxPoints(points));
    }

    [Fact]
    public void Example2()
    {
        var points = new[] { new[] { 1, 5 }, new[] { 2, 3 }, new[] { 4, 2 } };
        var solution = new Solution();
        Assert.Equal(11, solution.MaxPoints(points));
    }

    [Fact]
    public void Test1()
    {
        var points = new[]
        {
            new[] { 5, 1, 1, 2 },
            new[] { 1, 3, 1, 2 },
            new[] { 1, 1, 2, 2 },
            new[] { 1, 1, 1, 2 },
            new[] { 1, 1, 1, 2 },
        };
        var solution = new Solution();
        Assert.Equal(11, solution.MaxPoints(points));
    }
}