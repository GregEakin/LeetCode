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

public class BinarySearchWithDups
{
    public class Solution
    {
        public int Search(int[] nums, int target)
        {
            var left = 0;
            var right = nums.Length - 1;
            while (left <= right)
            {
                var middle = left + (right - left) / 2;
                if (nums[middle] == target) return middle;
                if (target > nums[middle])
                    left = middle + 1;
                else
                    right = middle - 1;
            }

            return -1;
        }
    }

    [Fact]
    public void Answer1()
    {
        var nums = new[] { 1, 3, 5 };
        var solution = new Solution();
        Assert.Equal(2, solution.Search(nums, 5));
    }

    [Fact]
    public void Answer2()
    {
        var nums = new[] { 0, 1, 1, 1, 1 };
        var solution = new Solution();
        Assert.Equal(0, solution.Search(nums, 0));
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 2, 5, 6 };
        var solution = new Solution();
        Assert.Equal(nums.Length - 2, solution.Search(nums, 5));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 0, 0, 1, 2, 2, 5, 6 };
        var solution = new Solution();
        Assert.Equal(-1, solution.Search(nums, 3));
    }
}