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
using Xunit;

namespace HeapChecks;

public class MinMaxGame
{
    public class Solution
    {
        public int MinMaxGame(int[] nums)
        {
            var n = nums.Length;
            while (n > 1)
            {
                for (var i = 0; i < n / 2; i += 2)
                {
                    var leftEven = 2 * i;
                    var rightEven = 2 * i + 1;
                    nums[i] = Math.Min(nums[leftEven], nums[rightEven]);

                    if (i + 2 > n / 2) continue;
                    var leftOdd = 2 * (i + 1);
                    var rightOdd = 2 * (i + 1) + 1;
                    nums[i + 1] = Math.Max(nums[leftOdd], nums[rightOdd]);
                }

                n /= 2;
            }

            return nums[0];
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 1, 3, 5, 2, 4, 8, 2, 2 };
        var solution = new Solution();
        Assert.Equal(1, solution.MinMaxGame(nums));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 3 };
        var solution = new Solution();
        Assert.Equal(3, solution.MinMaxGame(nums));
    }

    [Fact]
    public void Test1()
    {
        var nums = new[] { 3, 4 };
        var solution = new Solution();
        Assert.Equal(3, solution.MinMaxGame(nums));
    }

    [Fact]
    public void Test2()
    {
        var nums = new[] { 3, 4, 5, 6 };
        var solution = new Solution();
        Assert.Equal(3, solution.MinMaxGame(nums));
    }

    [Fact]
    public void Answer1()
    {
        var nums = new[] { 70, 38, 21, 22 };
        var solution = new Solution();
        Assert.Equal(22, solution.MinMaxGame(nums));
    }

    [Fact]
    public void Answer2()
    {
        var nums = new[] { 999, 939, 892, 175, 114, 464, 850, 107 };
        var solution = new Solution();
        Assert.Equal(850, solution.MinMaxGame(nums));
    }
}