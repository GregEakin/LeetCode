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
using Xunit;

namespace HeapChecks;

public class TwoSumTests
{
    public class Solution
    {
        public static int[] TwoSum(int[] nums, int target)
        {
            var map = new Dictionary<int, int>();
            for (var i = 0; i < nums.Length; i++)
            {
                var needed = target - nums[i];
                var found = map.TryGetValue(needed, out var index);
                if (found) return new[] { index, i };
                map.TryAdd(nums[i], i);
            }

            return Array.Empty<int>();
        }

        [Fact]
        public void Example1()
        {
            var nums = new[] { 2, 7, 11, 15 };
            var target = 9;

            Assert.Equal(new[] { 0, 1 }, TwoSum(nums, target));
        }

        [Fact]
        public void Example2()
        {
            var nums = new[] { 3, 2, 4 };
            var target = 6;

            Assert.Equal(new[] { 1, 2 }, TwoSum(nums, target));
        }


        [Fact]
        public void Example3()
        {
            var nums = new[] { 3, 3 };
            var target = 6;

            Assert.Equal(new[] { 0, 1 }, TwoSum(nums, target));
        }

        [Fact]
        public void Answer1()
        {
            var nums = new[] { 1, 1, 1, 1, 1, 4, 1, 1, 1, 1, 1, 7, 1, 1, 1, 1, 1 };
            var target = 11;

            Assert.Equal(new[] { 5, 11 }, TwoSum(nums, target));
        }
    }
}