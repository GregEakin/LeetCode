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

using Xunit;

namespace HeapChecks;

public class CountSubarraysWithScoreLessThanK
{
    public class Solution
    {
        public long CountSubarrays(int[] nums, long k)
        {
            var count = 0L;
            var sum = 0L;
            var left = 0;
            for (var right = 0; right < nums.Length; right++)
            {
                sum += nums[right];
                while (sum * (right - left + 1) >= k)
                    sum -= nums[left++];

                count += right - left + 1;
            }

            return count;
        }
    }

    [Fact]
    public void Test1()
    {
        var nums = new[] { 1, 3 };
        var solution = new Solution();
        Assert.Equal(0, solution.CountSubarrays(nums, 1));
        Assert.Equal(1, solution.CountSubarrays(nums, 3));
        Assert.Equal(2, solution.CountSubarrays(nums, 8));
        Assert.Equal(3, solution.CountSubarrays(nums, 9));
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 2, 1, 4, 3, 5 };
        var solution = new Solution();
        Assert.Equal(6, solution.CountSubarrays(nums, 10));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 1, 1, 1 };
        var solution = new Solution();
        Assert.Equal(5, solution.CountSubarrays(nums, 5));
    }

    [Fact]
    public void Test2()
    {
        var nums = new[] { 1, 1, 1 };
        var solution = new Solution();
        Assert.Equal(6, solution.CountSubarrays(nums, 10));
    }
}