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

public class MaximumSplitOfPositiveEvenIntegers
{
    public class Solution1
    {
        private readonly Dictionary<long, HashSet<HashSet<long>>> _dict = new();

        public IEnumerable<HashSet<long>> FindFactors(long sum)
        {
            if (_dict.TryGetValue(sum, out var set))
                return set;

            var sums = new HashSet<HashSet<long>>();
            while (sum > 0)
            {
                var factors = new HashSet<long> { sum };
                sums.Add(factors);
                sum -= 2;
            }

            return sums;
            // return new List<HashSet<long>> { new() { 14L }, new() { 2L, 4L, 8L }, new() { 4L, 10L } };
        }

        public IList<long> MaximumEvenSplit(long finalSum)
        {
            _dict.Add(2L, new HashSet<HashSet<long>> { new() { 2L } });
            _dict.Add(4L, new HashSet<HashSet<long>> { new() { 4L } });
            _dict.Add(6L, new HashSet<HashSet<long>> { new() { 6L }, new() { 2L, 4L } });
            _dict.Add(8L, new HashSet<HashSet<long>> { new() { 8L }, new() { 2L, 6L } });
            _dict.Add(12L,
                new HashSet<HashSet<long>>
                    { new() { 12L }, new() { 2L, 10L }, new() { 4L, 8L }, new() { 2L, 4L, 6L } });

            return finalSum % 2L == 1L
                ? new List<long>()
                : FindFactors(finalSum).MaxBy(p => p.Count)!.ToList();
        }
    }

    public class Solution
    {
        public IList<long> MaximumEvenSplit(long finalSum)
        {
            var list = new List<long>();
            if (finalSum % 2 == 1) return list;

            var i = 0L;
            while (finalSum > i)
            {
                i += 2;
                finalSum -= i;
                list.Add(i);
            }

            if (finalSum > 0)
                list[^1] = i + finalSum;

            return list;
        }
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal(new[] { 2L, 4L, 6L }, solution.MaximumEvenSplit(12L));
        // Note that [2,6,4], [6,2,4], etc. are also accepted.
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal(Array.Empty<long>(), solution.MaximumEvenSplit(7L));
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.Equal(new[] { 2L, 4L, 6L, 16L }, solution.MaximumEvenSplit(28L));
        // Note that [2,4,10,12], [2,6,8,12], etc. are also accepted.
    }
}