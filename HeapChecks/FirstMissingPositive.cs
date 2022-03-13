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

using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace HeapChecks;

public class FirstMissingPositive
{
    public class Solution
    {
        // Just track the minimum value.
        public int FirstMissingPositive1(int[] nums)
        {
            var min = 1;
            foreach (var num in nums)
            {
                if (num < min) continue;
                if (num > min) continue;
                min++;
            }

            return min;
        }

        // remember the holes as we go along
        public int FirstMissingPositive2(int[] nums)
        {
            var set = new List<int>();

            var max = 0;
            foreach (var num in nums)
            {
                if (num < 1) continue;
                if (num > max)
                {
                    for (var i = max + 1; i < num; i++)
                        set.Add(i);
                    max = num;
                    continue;
                }

                set.Remove(num);
            }

            if (set.Count == 0) return max + 1;
            return set[0];
        }

        public int FirstMissingPositive(int[] nums)
        {
            var found = new BitArray(int.MaxValue);
            foreach (var num in nums)
            {
                if (num < 1) continue;
                found[num - 1] = true;
            }

            for (var i = 0; i < found.Length; i++)
                if (!found[i])
                    return i + 1;

            return 1;
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 1, 2, 0 };

        var solution = new Solution();
        Assert.Equal(3, solution.FirstMissingPositive(nums));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 3, 4, -1, 1 };

        var solution = new Solution();
        Assert.Equal(2, solution.FirstMissingPositive(nums));
    }

    [Fact]
    public void Example3()
    {
        var nums = new[] { 7, 8, 9, 11, 12 };

        var solution = new Solution();
        Assert.Equal(1, solution.FirstMissingPositive(nums));
    }

    [Fact]
    public void Test1()
    {
        var nums = new[] { 1 };

        var solution = new Solution();
        Assert.Equal(2, solution.FirstMissingPositive(nums));
    }

    [Fact]
    public void Test2()
    {
        var nums = new[] { 4, 1, 3, 5, 2 };

        var solution = new Solution();
        Assert.Equal(6, solution.FirstMissingPositive(nums));
    }

    [Fact]
    public void Test3()
    {
        var nums = new[] { 4, 7, 1 };

        var solution = new Solution();
        Assert.Equal(2, solution.FirstMissingPositive(nums));
    }

    [Fact]
    public void Test4()
    {
        var nums = new[] { -1 };

        var solution = new Solution();
        Assert.Equal(1, solution.FirstMissingPositive(nums));
    }

    [Fact]
    public void Test5()
    {
        var nums = new[] { 1, 3, 5, 7, 9, 8, 6, 4, 2 };

        var solution = new Solution();
        Assert.Equal(10, solution.FirstMissingPositive(nums));
    }

    [Fact]
    public void Test6()
    {
        var nums = new int[100000];
        for (var i = 0; i < nums.Length / 2; i++)
        {
            nums[i] = i * 2;
            nums[i + nums.Length / 2] = i * 2 + 1;
        }

        var solution = new Solution();
        Assert.Equal(100000, solution.FirstMissingPositive(nums));
    }

    [Fact]
    public void Answer1()
    {
        var nums = new[] { 2147483647 };

        var solution = new Solution();
        Assert.Equal(1, solution.FirstMissingPositive(nums));
    }
}