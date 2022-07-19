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

public class TrappingRainWater
{
    public class SolutionLoops
    {
        public int Trap(int[] height)
        {
            var left = new int[height.Length];
            left[0] = height[0];
            for (var i = 1; i < height.Length; i++) 
                left[i] = Math.Max(left[i - 1], height[i]);

            var right = new int[height.Length];
            right[^1] = height[^1];
            for (var i = height.Length - 2; i >= 0; i--)
                right[i] = Math.Max(right[i + 1], height[i]);

            var amount = 0;
            for (var i = 0; i < height.Length; i++)
                amount += Math.Min(left[i], right[i]) - height[i];
            
            return amount;
        }
    }
    
    public class Solution 
    {
        public int Trap(int[] height) 
        {
            var left = 0;
            var leftMax = height[left];
            var right = height.Length - 1;
            var rightMax = height[right];
            var amount = 0;
            while (left < right) 
            {
                if (leftMax <= rightMax) 
                {
                    left++;
                    leftMax = Math.Max(height[left], leftMax);
                    amount += leftMax - height[left];
                }
                else 
                {
                    right--;
                    rightMax = Math.Max(height[right], rightMax);
                    amount += rightMax - height[right];
                }
            }
        
            return amount;
        }
    }

    [Fact]
    public void Example1()
    {
        var height = new[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 };
        var solution = new Solution();
        Assert.Equal(6, solution.Trap(height));
    }
    
    [Fact]
    public void Example2()
    {
        var height = new[] { 4, 2, 0, 3, 2, 5 };
        var solution = new Solution();
        Assert.Equal(9, solution.Trap(height));
    }

    [Fact]
    public void Test1()
    {
        var height = new[] { 0, 1, 0, 2, 0, 2, 0, 1, 0 };
        var solution = new Solution();
        Assert.Equal(4, solution.Trap(height));
    }
    
    [Fact]
    public void Test2()
    {
        var height = new[] { 0, 1, 0, 2, 0, 1, 0, 2, 0, 1, 0 };
        var solution = new Solution();
        Assert.Equal(7, solution.Trap(height));
    }
    
    [Fact]
    public void Test3()
    {
        var height = new[] { 0 };
        var solution = new Solution();
        Assert.Equal(0, solution.Trap(height));
    }
    
    [Fact]
    public void Test4()
    {
        var height = new[] { 9, 9 };
        var solution = new Solution();
        Assert.Equal(0, solution.Trap(height));
    }
}