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

public class MaximumBagsWithFullCapacityOfRocks
{
    public class Solution
    {
        public int MaximumBags(int[] capacity, int[] rocks, int additionalRocks)
        {
            var queue = new PriorityQueue<int, int>();
            var full = 0;
            for (var i = 0; i < capacity.Length; i++)
            {
                var space = capacity[i];
                var count = rocks[i];
                if (space == count)
                {
                    full++;
                    continue;
                }

                queue.Enqueue(i, space - count);
            }

            while (additionalRocks > 0 && queue.Count > 0)
            {
                var i = queue.Dequeue();
                var space = capacity[i];
                var count = rocks[i];
                if (space - count > additionalRocks)
                    break;

                additionalRocks -= space - count;
                full++;
            }

            return full;
        }
    }

    [Fact]
    public void Example1()
    {
        var capacity = new[] { 2, 3, 4, 5 };
        var rocks = new[] { 1, 2, 4, 4 };

        var solution = new Solution();
        Assert.Equal(3, solution.MaximumBags(capacity, rocks, 2));
    }

    [Fact]
    public void Example2()
    {
        var capacity = new[] { 10, 2, 2 };
        var rocks = new[] { 2, 2, 0 };

        var solution = new Solution();
        Assert.Equal(3, solution.MaximumBags(capacity, rocks, 100));
    }
}