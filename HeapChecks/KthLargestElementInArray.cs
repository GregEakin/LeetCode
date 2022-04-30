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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class KthLargestElementInArray
{
    public class Solution
    {
        public int FindKthLargest(int[] nums, int k)
        {
            Array.Sort(nums, Comparer<int>.Create((a, b) => b.CompareTo(a)));
            return nums[k - 1];
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 3, 2, 1, 5, 6, 4 };
        var solution = new Solution();
        Assert.Equal(5, solution.FindKthLargest(nums, 2));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 3, 2, 3, 1, 2, 4, 5, 5, 6 };
        var solution = new Solution();
        Assert.Equal(4, solution.FindKthLargest(nums, 4));
    }

    [Fact]
    public void TimeTest1()
    {
        var random = new Random(1023);
        var nums = new int[1000000];
        for (var i = 0; i < nums.Length; i++)
            nums[i] = random.Next();

        var solution = new Solution();
        Assert.Equal(2145287100, solution.FindKthLargest(nums, 1000));
    }
}