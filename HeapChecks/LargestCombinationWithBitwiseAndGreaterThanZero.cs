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

public class LargestCombinationWithBitwiseAndGreaterThanZero
{
    public class Solution
    {
        public int LargestCombination(int[] candidates)
        {
            var map = new Dictionary<int, List<int>>(24);
            for (var i =0; i < 24; i++)
                map.Add(i, new List<int>());

            foreach (var candidate in candidates)
                for (var i = 0; i < 24; i++)
                    if (((candidate >> i) & 0x01) != 0)
                        map[i].Add(candidate);

            return map.Max(p => p.Value.Count);
        }
    }

    [Fact]
    public void Example1()
    {
        var candidates = new[] { 16, 17, 71, 62, 12, 24, 14 };
        var solution = new Solution();
        Assert.Equal(4, solution.LargestCombination(candidates));
    }

    [Fact]
    public void Example1B()
    {
        var candidates = new[] { 62, 12, 24, 14 };
        var solution = new Solution();
        Assert.Equal(4, solution.LargestCombination(candidates));
    }

    [Fact]
    public void Example2()
    {
        var candidates = new[] { 8, 8 };
        var solution = new Solution();
        Assert.Equal(2, solution.LargestCombination(candidates));
    }
}