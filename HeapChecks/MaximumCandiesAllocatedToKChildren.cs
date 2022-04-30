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

public class MaximumCandiesAllocatedToKChildren
{
    public class Solution
    {
        public int MaximumCandies(int[] candies, long k)
        {
            var sum = candies.Aggregate(0L, (current, candy) => current + candy);
            if (sum < k) return 0;

            var low = 1L;
            var high = sum / k;
            while (low < high)
            {
                var mid = (low + high + 1) / 2;
                var subSum = candies.Aggregate(0L, (current, candy) => current + candy / mid);
                if (subSum >= k)
                    low = mid;
                else
                    high = mid - 1;
            }

            return (int)low;
        }
    }

    [Fact]
    public void Example1()
    {
        var candies = new[] { 5, 8, 6 };
        var k = 3;

        var solution = new Solution();
        Assert.Equal(5, solution.MaximumCandies(candies, k));
    }

    [Fact]
    public void Example2()
    {
        var candies = new[] { 2, 5 };
        var k = 11;

        var solution = new Solution();
        Assert.Equal(0, solution.MaximumCandies(candies, k));
    }

    [Fact]
    public void Answer1()
    {
        var candies = new[] { 1, 2, 3, 4, 10 };
        var k = 5;

        var solution = new Solution();
        Assert.Equal(3, solution.MaximumCandies(candies, k));
    }

    [Fact]
    public void Answer2()
    {
        var candies = new[] { 1, 2, 6, 8, 6, 7, 3, 5, 2, 5 };
        var k = 3;

        var solution = new Solution();
        Assert.Equal(6, solution.MaximumCandies(candies, k));
    }

    [Fact]
    public void Test1()
    {
        var candies = new[] { 100, 200, 400 };
        var k = 3;

        var solution = new Solution();
        Assert.Equal(200, solution.MaximumCandies(candies, k));
    }

    [Fact]
    public void Test2()
    {
        var candies = new[] { 19, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        var k = 10;

        var solution = new Solution();
        Assert.Equal(2, solution.MaximumCandies(candies, k));
    }
}