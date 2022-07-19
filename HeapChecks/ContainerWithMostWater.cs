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

public class ContainerWithMostWater
{
    public class SolutionTwoLoops
    {
        public int MaxArea(int[] height)
        {
            var area = 0;
            for (var i = 0; i < height.Length - 1; i++)
            for (var j = i + 1; j < height.Length; j++)
            {
                var min = Math.Min(height[i], height[j]);
                area = Math.Max(area, min * (j - i));
            }

            return area;
        }
    }

    public class Solution
    {
        public int MaxArea(int[] height)
        {
            var right = height.Length - 1;
            var left = 0;
            var area = 0;
            while (left < right)
            {
                var min = Math.Min(height[left], height[right]);
                area = Math.Max(area, (right - left) * min);

                if (height[left] > height[right])
                    right--;
                else
                    left++;
            }

            return area;
        }
    }

    [Fact]
    public void Example1()
    {
        var height = new[] { 1, 8, 6, 2, 5, 4, 8, 3, 7 };
        var solution = new Solution();
        Assert.Equal(49, solution.MaxArea(height));
    }

    [Fact]
    public void Example2()
    {
        var height = new[] { 1, 1 };
        var solution = new Solution();
        Assert.Equal(1, solution.MaxArea(height));
    }

    [Fact]
    public void Test1()
    {
        var height = new[]
        {
            1, 0, 9, 0, 0, 0, 0, 9, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
        };
        var solution = new Solution();
        Assert.Equal(59, solution.MaxArea(height));
    }
}