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

public class PancakeFlips
{
    public class Solution
    {
        public IList<int> PancakeSort(int[] arr)
        {
            var answer = new List<int>();
            for (var i = arr.Length; i > 0; i--)
            {
                var position = Array.IndexOf(arr, i);
                if (position == i - 1) continue;
                if (position > 0)
                {
                    answer.Add(position + 1);
                    Array.Reverse(arr, 0, position + 1);
                }

                answer.Add(i);
                Array.Reverse(arr, 0, i);
            }
            
            return answer;
        }
    }
    
    [Fact]
    public void Example1()
    {
        var arr = new[] { 3, 2, 4, 1 };
        var solution = new Solution();
        // Assert.Equal(new[] { 4, 3, 4, 3 }, solution.PancakeSort(arr));
        Assert.Equal(new[] { 3, 4, 2, 3, 2 }, solution.PancakeSort(arr));
    }

    [Fact]
    public void Example2()
    {
        var arr = new[] { 1, 2, 3 };
        var solution = new Solution();
        Assert.Equal(Array.Empty<int>(), solution.PancakeSort(arr));
    }

    [Fact]
    public void Test1()
    {
        var arr = new[] { 3, 2, 1 };
        var solution = new Solution();
        Assert.Equal(new[] { 3 }, solution.PancakeSort(arr));
    }
}