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

public class MatchSubstringAfterReplacement
{
    public class Solution
    {
        public bool MatchReplacement(string s, string sub, char[][] mappings)
        {
            var map = new Dictionary<char, List<char>>();
            foreach (var mapping in mappings)
            {
                if (!map.TryGetValue(mapping[0], out var list))
                {
                    list = new List<char>();
                    map[mapping[0]] = list;
                }

                list.Add(mapping[1]);
            }

            var n = s.Length;
            var m = sub.Length;
            for (var i = 0; i < n - m; i++)
            {

            }

            return false;
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "fool3e7bar";
        var sub = "leet";
        var mappings = new[] { new[] { 'e', '3' }, new[] { 't', '7' }, new[] { 't', '8' } };
        var solution = new Solution();
        Assert.True(solution.MatchReplacement(s, sub, mappings));
    }

    [Fact]
    public void Example2()
    {
        var s = "fooleetbar";
        var sub = "f00l";
        var mappings = new[] { new[] { 'o', '0' } };
        var solution = new Solution();
        Assert.True(solution.MatchReplacement(s, sub, mappings));
    }

    [Fact]
    public void Example3()
    {
        var s = "Fool33tbaR";
        var sub = "leetd";
        var mappings = new[]
            { new[] { 'e', '3' }, new[] { 't', '7' }, new[] { 't', '8' }, new[] { 'd', 'b' }, new[] { 'p', 'b' } };
        var solution = new Solution();
        Assert.True(solution.MatchReplacement(s, sub, mappings));
    }
}