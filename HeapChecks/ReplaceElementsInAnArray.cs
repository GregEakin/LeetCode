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

public class ReplaceElementsInAnArray
{
    public class Solution
    {
        public int[] ArrayChange(int[] nums, int[][] operations)
        {
            var n = nums.Length;
            var m = operations.Length;
            var indices = new int[1000001];

            for (int i = 0; i < n; i++)
                indices[nums[i]] = i;

            foreach (var operation in operations)
            {
                var index = indices[operation[0]];
                nums[index] = operation[1];
                indices[operation[1]] = index;
            }

            return nums;
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 1, 2, 4, 6 };
        var operations = new[] { new[] { 1, 3 }, new[] { 4, 7 }, new[] { 6, 1 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 2, 7, 1 }, solution.ArrayChange(nums, operations));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 1, 2 };
        var operations = new[] { new[] { 1, 3 }, new[] { 2, 1 }, new[] { 3, 2 } };
        var solution = new Solution();
        Assert.Equal(new[] { 2, 1 }, solution.ArrayChange(nums, operations));
    }

    [Fact]
    public void Answer1()
    {
        var nums = new[] { 1 };
        var operations = new[] { new[] { 1, 2 }, new[] { 2, 3 }, new[] { 3, 1000000 }, new[] { 1000000, 1 } };
        var solution = new Solution();
        Assert.Equal(new[] { 1 }, solution.ArrayChange(nums, operations));
    }
}