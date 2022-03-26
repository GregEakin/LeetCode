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

public class GreedyActivitySelector
{
    public static IEnumerable<int> Selector((int s, int f)[] times)
    {
        var n = times.Length;
        if (n <= 0)
            return Array.Empty<int>();

        var a = new List<int> { 0 };
        var j = 0;
        for (var i = 1; i < n; i++)
        {
            if (times[i].s < times[j].f) continue;
            a.Add(i);
            j = i;
        }

        return a;
    }

    [Fact]
    public void Figure17_1()
    {
        var times = new[]
            { (1, 4), (3, 5), (0, 6), (5, 7), (3, 8), (5, 9), (6, 10), (8, 11), (8, 12), (2, 13), (12, 14) };
        Assert.Equal(new[] { 0, 3, 7, 10 }, GreedyActivitySelector.Selector(times));
    }

    [Fact]
    public void EmptyTest()
    {
        var times = Array.Empty<(int, int)>();
        Assert.Equal(Array.Empty<int>(), GreedyActivitySelector.Selector(times));
    }
}