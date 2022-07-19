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

public class EditDistance
{
    public class Solution
    {
        public int MinDistance(string word1, string word2)
        {
            var n = word1.Length;
            var m = word2.Length;
            // if (m < n) return MinDistance(word2, word1);

            var d1 = new int[m + 1];
            for (var j = 0; j <= m; j++)
                d1[j] = j + 1;

            for (var i = 0; i < n; i++)
            {
                var d0 = d1;
                d1 = new int[m + 1];
                d1[0] = i + 2;

                for (var j = 0; j < m; j++)
                    d1[j + 1] = word1[i] == word2[j]
                        ? d0[j]
                        : 1 + Math.Min(d1[j], Math.Min(d0[j + 1], d0[j]));
            }

            return d1[m] - 1;
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "horse";
        var p = "ros";
        var solution = new Solution();
        Assert.Equal(3, solution.MinDistance(s, p));
    }

    [Fact]
    public void Example2()
    {
        var s = "intention";
        var p = "execution";
        var solution = new Solution();
        Assert.Equal(5, solution.MinDistance(s, p));
    }

    [Fact]
    public void Test1()
    {
        var s = "test";
        var p = "test";
        var solution = new Solution();
        Assert.Equal(0, solution.MinDistance(s, p));
    }

    [Fact]
    public void Test2()
    {
        var s = "test";
        var p = "stopping";
        var solution = new Solution();
        Assert.Equal(7, solution.MinDistance(s, p));
    }
}