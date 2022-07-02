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
using System.Runtime.InteropServices.ComTypes;
using Xunit;

namespace HeapChecks;

public class StepsToMakeArrayNonDecreasing
{
    public class Solution2
    {
        public int TotalSteps(int[] nums)
        {
            if (nums.Length < 2) return 0;

            var list = new LinkedList<int>(nums);
            var count = 0;
            while (list.Count > 0)
            {
                var found = false;
                var last = list.First.Value;
                foreach (var next in list)
                {
                    if (last <= next)
                    {
                        last = next;
                        continue;
                    }

                    list.Remove(next);
                    found = true;
                }

                if (!found) break;
            }

            return count;
        }
    }

    public class Solution3
    {
        public int TotalSteps(int[] nums)
        {
            var count = 0;
            var step = 0;
            var max = 0;
            var last = 0;
            foreach (var value in nums)
            {
                if (max > value)
                {
                    if (last > value)
                    {
                        last = value;
                        count = Math.Max(count, step);
                        step = 0;
                        continue;
                    }

                    last = value;
                    step++;
                    count = Math.Max(count, step);
                    continue;
                }

                max = value;
                last = value;
                count = Math.Max(count, step);
                step = 0;
            }

            return count;
        }
    }

    public class Solution
    {
        public int TotalSteps(int[] nums)
        {
            var count = 0;
            var step = 0;
            var stack = new Stack<int>();
            var last = 0;
            foreach (var value in nums)
            {
                var peek = last > value;
                last = value;
                if (peek)
                {
                    stack.Push(last);
                    step = 0;
                    continue;
                }

                if (stack.Count == 0)
                    continue;
                
                var max = stack.Peek();
                if (max <= value)
                {
                    stack.Pop();
                    stack.Push(value);
                    step++;
                    count = Math.Max(step, count);
                    step = 0;
                    continue;
                }

                step++;
                count = Math.Max(count, step);
            }

            return count;
        }
    }

    // [Fact]
    // public void Example1()
    // {
    //     var nums = new[] { 5, 3, 4, 4, 7, 3, 6, 11, 8, 5, 11 };
    //     var solution = new Solution();
    //     Assert.Equal(3, solution.TotalSteps(nums));
    // }
    //
    // [Fact]
    // public void Example2()
    // {
    //     var nums = new[] { 4, 5, 7, 7, 13 };
    //     var solution = new Solution();
    //     Assert.Equal(0, solution.TotalSteps(nums));
    // }
    //
    // [Fact]
    // public void Answer1()
    // {
    //     var nums = new[] { 10, 1, 2, 3, 4, 5, 6, 1, 2, 3 };
    //     var solution = new Solution();
    //     Assert.Equal(6, solution.TotalSteps(nums));
    // }
    //
    // [Fact]
    // public void Answer2()
    // {
    //     var nums = new[] { 7, 14, 4, 14, 13, 2, 6, 13 };
    //     var solution = new Solution();
    //     Assert.Equal(3, solution.TotalSteps(nums));
    // }
    //
    // [Fact]
    // public void Answer3()
    // {
    //     var nums = new[] { 5, 14, 15, 2, 11, 5, 13, 15 };
    //     var solution = new Solution();
    //     Assert.Equal(3, solution.TotalSteps(nums));
    // }
    //
    // [Fact]
    // public void Test1()
    // {
    //     var nums = new[] { 1, 1, 1 ,1 };
    //     var solution = new Solution();
    //     Assert.Equal(0, solution.TotalSteps(nums));
    // }
    //
    // [Fact]
    // public void Test2()
    // {
    //     var nums = new[] { 10, 9, 8, 7 };
    //     var solution = new Solution();
    //     Assert.Equal(1, solution.TotalSteps(nums));
    // }
}