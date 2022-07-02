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

using System.Collections.Generic;
using Xunit;

namespace HeapChecks;

public class CountSubarraysWithScoreLessThanK
{
    public class SolutionFast
    {
        public long CountSubarrays(int[] nums, long k)
        {
            var i = 0;
            long res = 0;
            long sum = 0;
            for (var j = 0; j < nums.Length; j++)
            {
                sum += nums[j];
                while (sum * (j - i + 1) >= k)
                    sum -= nums[i++];

                res += j - i + 1;
            }

            return res;
        }
    }

    public class SolutionSearch
    {
        private readonly List<long> _prefix = new();
        private long _k;

        public bool Check(int l, int r)
        {
            var tot = _prefix[r] - (l - 1 >= 0 ? _prefix[l - 1] : 0);
            return tot * (r - l + 1) < _k;
        }

        public long CountSubarrays(int[] nums, long k)
        {
            _k = k;
            var n = nums.Length;
            _prefix.Capacity = n;
            _prefix[0] = nums[0];
            for (var i = 1; i < n; i++) 
                _prefix[i] = _prefix[i - 1] + nums[i];

            var count = 0L;
            for (var i = 0; i < n; i++)
            {
                var left = i;
                var right = n;
                while (left < right)
                {
                    var mid = left + right >> 1;
                    if (Check(i, mid)) left = mid + 1;
                    else right = mid;
                }

                count += left - i;
            }

            return count;
        }
    }

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