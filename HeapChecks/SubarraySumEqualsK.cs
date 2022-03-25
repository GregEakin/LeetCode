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

public class SubarraySumEqualsK
{
    public class Solution
    {
        public int SubarraySum(int[] nums, int k)
        {
            var count = 0;
            for (var i = 0; i < nums.Length; i++)
            {
                var sum = 0;
                for (var j = 0; j < nums.Length - i; j++)
                {
                    sum += nums[i + j];
                    if (sum == k) count++;
                }
            }

            return count;
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 1, 1, 1 };
        var k = 2;
        var solution = new Solution();
        Assert.Equal(2, solution.SubarraySum(nums, k));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 1, 2, 3 };
        var k = 3;
        var solution = new Solution();
        Assert.Equal(2, solution.SubarraySum(nums, k));
    }

    [Fact]
    public void Test1()
    {
        var nums = new[] { 1, 0, 0, 1 };
        var k = 1;
        var solution = new Solution();
        Assert.Equal(6, solution.SubarraySum(nums, k));

    }

    [Fact]
    public void Test2()
    {
        var nums = new[] { 1, 0, -1, 0, 1 };
        var k = 0;
        var solution = new Solution();
        Assert.Equal(6, solution.SubarraySum(nums, k));

    }
}
