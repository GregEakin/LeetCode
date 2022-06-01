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

public class MinimumLinesToRepresentALineChart
{
    public class Solution
    {
        public int MinimumLines(int[][] stockPrices)
        {
            var data = stockPrices.OrderBy(pair => pair[0]).ToArray();
            var last = data[0];
            var lines = 0;
            var slope = 0L;
            var intercept = 0L;
            for (var i = 1; i < data.Length; i++)
            {
                var m = ((long)last[1] - data[i][1]) / ((long)last[0] - data[i][0]);
                var b = data[i][1] - m * data[i][0];
                last = data[i];
                if (lines > 0 && slope == m && intercept == b) continue;
                lines++;
                slope = m;
                intercept = b;
            }

            return lines;
        }
    }

    [Fact]
    public void Test1()
    {
        var stockPrices = new[]
        {
            new[] { 1, 1 }
        };

        var solution = new Solution();
        Assert.Equal(0, solution.MinimumLines(stockPrices));
    }

    [Fact]
    public void Example1()
    {
        var stockPrices = new[]
        {
            new[] { 1, 7 }, new[] { 2, 6 }, new[] { 3, 5 }, new[] { 4, 4 }, new[] { 5, 4 }, new[] { 6, 3 },
            new[] { 7, 2 }, new[] { 8, 1 }
        };

        var solution = new Solution();
        Assert.Equal(3, solution.MinimumLines(stockPrices));
    }

    [Fact]
    public void Example2()
    {
        var stockPrices = new[]
        {
            new[] { 3, 4 }, new[] { 1, 2 }, new[] { 7, 8 }, new[] { 2, 3 }
        };

        var solution = new Solution();
        Assert.Equal(1, solution.MinimumLines(stockPrices));
    }

    [Fact]
    public void Answer1()
    {
        var stockPrices = new[]
        {
            new[] { 1, 1 }, new[] { 2, 2 }, new[] { 3, 3 }, new[] { 4, 5 }, new[] { 5, 7 }, new[] { 6, 6 },
            new[] { 7, 5 }, new[] { 8, 4 }, new[] { 9, 4 }, new[] { 10, 4 }
        };

        var solution = new Solution();
        Assert.Equal(4, solution.MinimumLines(stockPrices));
    }

    [Fact]
    public void Answer2()
    {
        var stockPrices = new[]
        {
            new[] { 1, 1000000000 }, new[] { 1000000000, 1000000000 }, new[] { 999999999, 1 }, new[] { 2, 999999999 }
        };

        var solution = new Solution();
        Assert.Equal(3, solution.MinimumLines(stockPrices));
    }

    [Fact]
    public void Answer3()
    {
        var stockPrices = new[]
        {
            new[] { 1, 1 }, new[] { 500000000, 499999999 }, new[] { 1000000000, 999999998 }
        };

        var solution = new Solution();
        Assert.Equal(2, solution.MinimumLines(stockPrices));
    }
}