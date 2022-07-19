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

public class MatchSubstringAfterReplacement
{
    public class SolutionSlow
    {
        public bool MatchReplacement(string s, string sub, char[][] mappings)
        {
            var map = new Dictionary<char, List<char>>();
            foreach (var mapping in mappings)
            {
                if (!map.TryGetValue(mapping[1], out var list))
                {
                    list = new List<char>();
                    map[mapping[1]] = list;
                }

                list.Add(mapping[0]);
            }

            var n = s.Length;
            var m = sub.Length;
            for (var i = 0; i <= n - m; i++)
            {
                var found = true;
                for (var j = 0; j < m; j++)
                {
                    var g = s[i + j];
                    var c = sub[j];
                    if (c == g) 
                        continue;
                    
                    if (!map.TryGetValue(g, out var list))
                    {
                        found = false;
                        break;
                    }

                    if (!list.Contains(c))
                    {
                        found = false;
                        break;
                    }
                }

                if (found) return true;
            }

            return false;
        }
    }

    public class Solution
    {
        long Encode(char c)
        {
            var bit = c switch
            {
                >= '0' and <= '9' => (int)c - '0',
                >= 'a' and <= 'z' => 10 + (int)c - 'a',
                >= 'A' and <= 'Z' => 36 + (int)c - 'A',
                _ => throw new Exception()
            };

            return 1L << bit;
        }
        
        public bool MatchReplacement(string s, string sub, char[][] mappings)
        {
            var sBits = new long[s.Length];
            for (var i = 0; i < s.Length; i++)
                sBits[i] = Encode(s[i]);

            var map = new long[128];
            foreach (var m in mappings)
                map[(int)m[0]] |= Encode(m[1]);
            
            var subBits = new long[sub.Length];
            for (var i = 0; i < sub.Length; i++)
                subBits[i] = Encode(sub[i]) | map[(int)sub[i]];

            for (var i = 0; i <= s.Length - sub.Length; i++)
            {
                var found = true;
                for (var j = 0; j < sub.Length; j++)
                {
                    if ((subBits[j] & sBits[i + j]) > 0) 
                        continue;
                    
                    found = false;
                    break;
                }

                if (found)
                    return true;
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
        Assert.False(solution.MatchReplacement(s, sub, mappings));
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

    [Fact]
    public void Test1()
    {
        var s = "1111";
        var sub = "1ab";
        var mappings = new[] { new[] { 'a', '1' }, new[] { 'b', '1' } };
        var solution = new Solution();
        Assert.True(solution.MatchReplacement(s, sub, mappings));
    }
    
    [Fact]
    public void Test2()
    {
        var s = "f00leetbar";
        var sub = "fool";
        var mappings = new[] { new[] { 'o', '0' } };
        var solution = new Solution();
        Assert.True(solution.MatchReplacement(s, sub, mappings));
    }

    [Fact]
    public void Test3()
    {
        var s = "aaabbbd";
        var sub = "cccd";
        var mappings = new[]
            { new[] { 'a', 'c' }, new[] { 'b', 'c' } };
        var solution = new Solution();
        Assert.False(solution.MatchReplacement(s, sub, mappings));
    }
    
    [Fact]
    public void Answer1()
    {
        var s = "llllllllllllllllllllllllllllllllrllllllllllllllllllllllllllllllllrllllllllllllllllllllllllllllllllrllllllllllllllllllllllllllllllllrllllllllllllllllllllllllllllllllrllllllllllllllllllllllllllllllllrllllllllllllllllllllllllllllllllrllllllllllllllllllllllllllllllllrllllllllllllllllllllllllllllllllrlllllllllllllllllllllllllllllllll";
        var sub = "lllllllllllllllllllllllllllllllll";
        var mappings = new[]
            {new[]{'l','a'},new[]{'l','b'},new[]{'l','c'},new[]{'l','d'},new[]{'l','e'},new[]{'l','f'},new[]{'l','g'},new[]{'l','h'},new[]{'l','i'},new[]{'l','j'},new[]{'l','k'},new[]{'l','m'},new[]{'l','n'},new[]{'l','o'},new[]{'l','p'},new[]{'l','q'},new[]{'l','s'},new[]{'l','t'},new[]{'l','u'},new[]{'l','v'},new[]{'l','w'},new[]{'l','x'},new[]{'l','y'},new[]{'l','z'},new[]{'l','0'},new[]{'l','1'},new[]{'l','2'},new[]{'l','3'},new[]{'l','4'},new[]{'l','5'},new[]{'l','6'},new[]{'l','7'},new[]{'l','8'},new[]{'l','9'},new[]{'r','a'},new[]{'r','b'},new[]{'r','c'},new[]{'r','d'},new[]{'r','e'},new[]{'r','f'},new[]{'r','g'},new[]{'r','h'},new[]{'r','i'},new[]{'r','j'},new[]{'r','k'},new[]{'r','m'},new[]{'r','n'},new[]{'r','o'},new[]{'r','p'},new[]{'r','q'},new[]{'r','s'},new[]{'r','t'},new[]{'r','u'},new[]{'r','v'},new[]{'r','w'},new[]{'r','x'},new[]{'r','y'},new[]{'r','z'},new[]{'r','0'},new[]{'r','1'},new[]{'r','2'},new[]{'r','3'},new[]{'r','4'},new[]{'r','5'},new[]{'r','6'},new[]{'r','7'},new[]{'r','8'},new[]{'r','9'} };
        var solution = new Solution();
        Assert.True(solution.MatchReplacement(s, sub, mappings));
    }
}