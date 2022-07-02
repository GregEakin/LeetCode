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
using System.Runtime.InteropServices.ComTypes;
using Xunit;

namespace HeapChecks;

public class PartitionArraySuchThatMaximumDifferenceIsK
{
    public class Solution
    {
        public int PartitionArray(int[] nums, int k)
        {
            Array.Sort(nums);
            var count = 1;
            var min = nums[0];
            foreach (var num in nums)
            {
                if (num <= min + k) continue;
                count++;
                min = num;
            }

            return count;
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 3, 6, 1, 2, 5 };
        var solution = new Solution();
        Assert.Equal(2, solution.PartitionArray(nums, 2));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 1, 2, 3 };
        var solution = new Solution();
        Assert.Equal(2, solution.PartitionArray(nums, 1));
    }

    [Fact]
    public void Example3()
    {
        var nums = new[] { 2, 2, 4, 5 };
        var solution = new Solution();
        Assert.Equal(3, solution.PartitionArray(nums, 0));
    }
}