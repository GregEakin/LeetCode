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

public class OneThreeTwoPattern
{
    public class SolutionSlow
    {
        public bool Find132pattern(int[] nums)
        {
            for (var i = 0; i < nums.Length - 2; i++)
            for (var j = i + 1; j < nums.Length - 1; j++)
            for (var k = j + 1; k < nums.Length; k++)
                if (nums[i] < nums[k] && nums[k] < nums[j])
                    return true;

            return false;
        }
    }

    public class SolutionStack
    {
            public bool Find132pattern(int[] nums)
            {
                var n = nums.Length;
                if (n < 3)
                    return false;

                var stack = new Stack<int>();
                var min = new int[n];
                min[0] = nums[0];
                for (var i = 1; i < n; i++)
                    min[i] = Math.Min(min[i - 1], nums[i]);
            
                for (var j = n - 1; j >= 0; j--)
                {
                    if (nums[j] <= min[j]) continue;

                    while (stack.Count >0 && stack.Peek() <= min[j])
                        stack.Pop();
                        
                    if (stack.Count > 0 && stack.Peek() < nums[j])
                        return true;

                    stack.Push(nums[j]);
                }
                
                return false;
            }
        }

    public class Solution
    {
        public bool Find132pattern(int[] nums)
        {
            var n = nums.Length;
            if (n < 3)
                return false;

            var min = new int[n];
            min[0] = nums[0];
            for (var i = 1; i < n; i++)
                min[i] = Math.Min(min[i - 1], nums[i]);

            for (int j = n - 1, k = n; j >= 0; j--)
            {
                if (nums[j] <= min[j]) continue;
                while (k < n && nums[k] <= min[j])
                    k++;

                if (k < n && nums[k] < nums[j])
                    return true;

                nums[--k] = nums[j];
            }

            return false;
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 1, 2, 3, 4 };
        var solution = new Solution();
        Assert.False(solution.Find132pattern(nums));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 3, 1, 4, 2 };
        var solution = new Solution();
        Assert.True(solution.Find132pattern(nums));
    }

    [Fact]
    public void Example3()
    {
        var nums = new[] { -1, 3, 2, 0 };
        var solution = new Solution();
        Assert.True(solution.Find132pattern(nums));
    }
}