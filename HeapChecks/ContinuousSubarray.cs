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

public class ContinuousSubarray
{
    public class SolutionBad
    {
        public int FindUnsortedSubarray(int[] nums)
        {
            var left = 0;
            var right = nums.Length - 1;

            for (var i = 0; i < nums.Length - 1; i++)
            {
                if (nums[i] < nums[i + 1]) continue;
                for (var j = i; j >= 0; j--)
                {
                    if (nums[i + 1] < nums[j]) continue;
                    left = j + 1;
                    break;
                }

                break;
            }
            if (left == nums.Length - 1) return 0;
            
            for (var i = nums.Length - 2; i >= 0; i--)
            {
                if (nums[i] < nums[i + 1]) continue;
                for (var j = i; j < nums.Length; j++)
                {
                    if (nums[i + 1] < nums[j]) continue;

                    right = j + 1;
                    break;
                }

                break;
            }

            return Math.Max(0, right - left);
        }
    }

    public class Solution
    {
        public int FindUnsortedSubarray(int[] nums)
        {
            var left = 0;
            while (left < nums.Length - 1 && nums[left] <= nums[left + 1]) left++;
            if (left == nums.Length - 1) return 0;

            var right = nums.Length - 1;
            while (right > 0 && nums[right - 1] <= nums[right]) right--;

            var subMin = int.MaxValue;
            var subMax = int.MinValue;
            for (var i = left; i <= right; i++)
            {
                subMax = Math.Max(subMax, nums[i]);
                subMin = Math.Min(subMin, nums[i]);
            }

            while (left > 0 && nums[left - 1] > subMin) left--;
            while (right < nums.Length - 1 && nums[right + 1] < subMax) right++;
            return right - left + 1;
        }        
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 2, 6, 4, 8, 10, 9, 15 };
        var solution = new Solution();
        Assert.Equal(5, solution.FindUnsortedSubarray(nums));
    }
    
    [Fact]
    public void Example2()
    {
        var nums = new[] { 1, 2, 3, 4 };
        var solution = new Solution();
        Assert.Equal(0, solution.FindUnsortedSubarray(nums));
    }
    
    [Fact]
    public void Example3()
    {
        var nums = new[] { 1 };
        var solution = new Solution();
        Assert.Equal(0, solution.FindUnsortedSubarray(nums));
    }

    [Fact]
    public void Test1()
    {
        var nums = new[] { 3, 2, 1 };
        var solution = new Solution();
        Assert.Equal(3, solution.FindUnsortedSubarray(nums));
    }

    [Fact]
    public void Test2()
    {
        var nums = new[] { 2, 5, 3, 3, 3 };
        var solution = new Solution();
        Assert.Equal(4, solution.FindUnsortedSubarray(nums));
    }
}