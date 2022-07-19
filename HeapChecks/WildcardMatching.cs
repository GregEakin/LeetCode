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

using System.Numerics;
using System.Runtime.ConstrainedExecution;
using Xunit;

namespace HeapChecks;

public class WildcardMatching
{
    // '?' Matches any single character.
    // '*' Matches any sequence of characters(including the empty sequence).
    public class Solution
    {
        public bool IsMatch(string s, string p)
        {
            var match = -1;
            var star = -1;
            var sIndex = 0;
            var pIndex = 0;
            while (sIndex < s.Length)
            {
                if (pIndex < p.Length && p[pIndex] == '*')
                {
                    match = sIndex;
                    star = pIndex;
                    pIndex++;
                    continue;
                }

                if (pIndex < p.Length && (s[sIndex] == p[pIndex] || p[pIndex] == '?'))
                {
                    sIndex++;
                    pIndex++;
                    continue;
                }

                if (star < 0) return false;

                sIndex = match + 1;
                pIndex = star + 1;
                match++;
            }

            while (pIndex < p.Length && p[pIndex] == '*')
                pIndex++;

            return pIndex == p.Length;
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "aa";
        var p = "a";
        var solution = new Solution();
        Assert.False(solution.IsMatch(s, p));
    }

    [Fact]
    public void Example2()
    {
        var s = "aa";
        var p = "*";
        var solution = new Solution();
        Assert.True(solution.IsMatch(s, p));
    }

    [Fact]
    public void Example3()
    {
        var s = "cb";
        var p = "?a";
        var solution = new Solution();
        Assert.False(solution.IsMatch(s, p));
    }

    [Fact]
    public void Test1()
    {
        var s = "aaab";
        var p = "*b*";
        var solution = new Solution();
        Assert.True(solution.IsMatch(s, p));
    }

    [Fact]
    public void Test2()
    {
        var s = "aabaa";
        var p = "*b*";
        var solution = new Solution();
        Assert.True(solution.IsMatch(s, p));
    }

    [Fact]
    public void Test3()
    {
        var s = "baaa";
        var p = "*b*";
        var solution = new Solution();
        Assert.True(solution.IsMatch(s, p));
    }

    [Fact]
    public void Test4()
    {
        var s = "abaaba";
        var p = "*b*";
        var solution = new Solution();
        Assert.True(solution.IsMatch(s, p));
    }
}