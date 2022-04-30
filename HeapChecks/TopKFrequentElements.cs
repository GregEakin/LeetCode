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

public class TopKFrequentElements
{
    public class Solution
    {
        public int[] TopKFrequentWinner(int[] nums, int k)
        {
            var dic = new Dictionary<int, int>(nums.Length);
            var res = new int[k];
            foreach (var i in nums)
            {
                if (!dic.ContainsKey(i)) dic.Add(i, 0);
                dic[i]++;
            }

            var index = 0;
            foreach (var it in dic.OrderByDescending(key => key.Value))
            {
                res[index++] = it.Key;
                if (index == k) break;
            }

            return res;
        }

        public int[] TopKFrequent(int[] nums, int k)
        {
            Array.Sort(nums);
            var queue = new PriorityQueue<int, int>(nums.Length, Comparer<int>.Create((a, b) => b.CompareTo(a)));
            var current = nums[0];
            var count = 1;
            for (var i = 1; i < nums.Length; i++)
            {
                if (current == nums[i])
                {
                    count++;
                    continue;
                }

                queue.Enqueue(current, count);
                current = nums[i];
                count = 1;
            }

            queue.Enqueue(current, count);
            var result = new int[k];
            for (var i = 0; i < k; i++) result[i] = queue.Dequeue();
            return result;
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 1, 1, 1, 2, 2, 3 };
        var solution = new Solution();
        Assert.Equal(new[] { 1, 2 }, solution.TopKFrequent(nums, 2));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 1 };
        var solution = new Solution();
        Assert.Equal(new[] { 1 }, solution.TopKFrequent(nums, 1));
    }

    [Fact]
    public void TimeWinner()
    {
        var random = new Random(1023);
        var nums = new int[1000000];
        for (var i = 0; i < nums.Length; i++)
            nums[i] = random.Next();

        var solution = new Solution();
        Assert.Equal(1000, solution.TopKFrequentWinner(nums, 1000).Length);
    }

    [Fact]
    public void TimeSubmitted()
    {
        var random = new Random(1023);
        var nums = new int[100000];
        for (var i = 0; i < nums.Length; i++)
            nums[i] = random.Next();

        var solution = new Solution();
        Assert.Equal(1000, solution.TopKFrequent(nums, 1000).Length);
    }
}