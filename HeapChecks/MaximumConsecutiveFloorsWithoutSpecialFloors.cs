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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class MaximumConsecutiveFloorsWithoutSpecialFloors
{
    public class Solution
    {
        public int MaxConsecutive(int bottom, int top, int[] special)
        {
            var max = 0;
            var last = bottom;
            foreach (var i in special.OrderBy(f => f))
            {
                var count = i - last;
                max = Math.Max(max, count);
                last = i + 1;
            }

            if (last <= top)
                max = Math.Max(max, top - last + 1);

            return max;
        }
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal(3, solution.MaxConsecutive(2, 9, new[] { 4, 6 }));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal(0, solution.MaxConsecutive(6, 8, new[] { 7, 6, 8 }));
    }

    [Fact]
    public void Test1()
    {
        var solution = new Solution();
        Assert.Equal(2, solution.MaxConsecutive(1, 3, new[] { 3 }));
    }

    [Fact]
    public void Test2()
    {
        var solution = new Solution();
        Assert.Equal(1, solution.MaxConsecutive(1, 3, new[] { 1, 3 }));
    }

    [Fact]
    public void Test3()
    {
        var solution = new Solution();
        Assert.Equal(2, solution.MaxConsecutive(1, 3, new[] { 1 }));
    }
}