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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class MaximumTotalImportanceOfRoads
{
    public class Solution
    {
        public long MaximumImportance(int n, int[][] roads)
        {
            var counts = new int[n];
            foreach (var road in roads)
            foreach (var city in road)
                counts[city]++;

            var sorted = counts.Select((x, i) => (x, i)).OrderBy(x => x.x).Select((x, i) => (x.i, i + 1))
                .OrderBy(x => x.i).Select(x => x.Item2).ToArray();

            var sum = 0L;
            foreach (var road in roads)
            foreach (var city in road)
                sum += sorted[city];
            return sum;
        }
    }

    [Fact]
    public void Example1()
    {
        var roads = new[]
            { new[] { 0, 1 }, new[] { 1, 2 }, new[] { 2, 3 }, new[] { 0, 2 }, new[] { 1, 3 }, new[] { 2, 4 } };
        var solution = new Solution();
        Assert.Equal(43, solution.MaximumImportance(5, roads));
    }

    [Fact]
    public void Example2()
    {
        var roads = new[] { new[] { 0, 3 }, new[] { 2, 4 }, new[] { 1, 3 } };
        var solution = new Solution();
        Assert.Equal(20, solution.MaximumImportance(5, roads));
    }
}