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

public class FindTheMiddleIndexInArray
{
    public class Solution
    {
        public int FindMiddleIndex(int[] nums)
        {
            var s1 = 0;
            var s2 = 0;
            foreach (var v in nums)
                s2 += v;

            for (var i = 0; i < nums.Length; i++)
            {
                if (i > 0) s1 += nums[i - 1];
                s2 -= nums[i];

                if (s1 == s2) return i;
            }

            return -1;
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 2, 3, -1, 8, 4 };
        var solution = new Solution();
        Assert.Equal(3, solution.FindMiddleIndex(nums));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 1, -1, 4 };
        var solution = new Solution();
        Assert.Equal(2, solution.FindMiddleIndex(nums));
    }

    [Fact]
    public void Example3()
    {
        var nums = new[] { 2, 5 };
        var solution = new Solution();
        Assert.Equal(-1, solution.FindMiddleIndex(nums));
    }

    [Fact]
    public void Test1()
    {
        var nums = new[] { 0 };
        var solution = new Solution();
        Assert.Equal(0, solution.FindMiddleIndex(nums));
    }

    [Fact]
    public void Test2()
    {
        var nums = new[] { 0, 0 };
        var solution = new Solution();
        Assert.Equal(0, solution.FindMiddleIndex(nums));
    }

    [Fact]
    public void Test3()
    {
        var nums = new[] { 0, 0, 0 };
        var solution = new Solution();
        Assert.Equal(0, solution.FindMiddleIndex(nums));
    }

    [Fact]
    public void Test4()
    {
        var nums = new[] { 1, 1, 1, 1, 1 };
        var solution = new Solution();
        Assert.Equal(2, solution.FindMiddleIndex(nums));
    }
}
