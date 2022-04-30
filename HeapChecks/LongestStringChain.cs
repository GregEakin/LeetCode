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
using System.Text;
using Xunit;

namespace HeapChecks;

public class LongestStringChain
{
    public class Solution
    {
        public int Search(string word, Dictionary<string, int> map, HashSet<string> wordSet)
        {
            if (!wordSet.Contains(word))
                return 0;

            if (word.Length == 1)
            {
                map.Add(word, 1);
                return 1;
            }

            var max = 0;
            for (var i = 0; i < word.Length; i++)
            {
                var builder = new StringBuilder(word);
                builder.Remove(i, 1);
                var newWord = builder.ToString();
                if (!wordSet.Contains(newWord)) continue;
                var subSearch = map.TryGetValue(newWord, out var count) 
                    ? count 
                    : Search(newWord, map, wordSet);
                max = Math.Max(max, subSearch);
            }

            map.Add(word, max + 1);
            return max + 1;
        }

        public int LongestStrChain(string[] words)
        {
            var wordSet = new HashSet<string>();
            foreach (var word in words)
                wordSet.Add(word);

            var map = new Dictionary<string, int>();
            return words
                .Where(word => !map.ContainsKey(word))
                .Select(word => Search(word, map, wordSet))
                .Max();
        }
    }

    [Fact]
    public void Answer1()
    {
        var words = new[]
        {
            "ksqvsyq", "ks", "kss", "czvh", "zczpzvdhx", "zczpzvh", "zczpzvhx", "zcpzvh", "zczvh", "gr", "grukmj",
            "ksqvsq", "gruj", "kssq", "ksqsq", "grukkmj", "grukj", "zczpzfvdhx", "gru"
        };
        var solution = new Solution();
        Assert.Equal(7, solution.LongestStrChain(words));
    }

    [Fact]
    public void Example1()
    {
        var words = new[] { "a", "b", "ba", "bca", "bda", "bdca" };
        var solution = new Solution();
        Assert.Equal(4, solution.LongestStrChain(words));
    }

    [Fact]
    public void Example2()
    {
        var words = new[] { "xbc", "pcxbcf", "xb", "cxbc", "pcxbc" };
        var solution = new Solution();
        Assert.Equal(5, solution.LongestStrChain(words));
    }

    [Fact]
    public void Example3()
    {
        var words = new[] { "abcd", "dbqca" };
        var solution = new Solution();
        Assert.Equal(1, solution.LongestStrChain(words));
    }

    [Fact]
    public void Test1()
    {
        var words = new[] { "a", "ab", "abcdef", "abcdeg" };
        var solution = new Solution();
        Assert.Equal(2, solution.LongestStrChain(words));
    }

    [Fact]
    public void Test2()
    {
        var words = new[] { "ghijk", "abcde", "bcde", "acde", "abcd", "hijk", "cde", "ijk", "jk" };
        var solution = new Solution();
        Assert.Equal(4, solution.LongestStrChain(words));
    }

    [Fact]
    public void Test3()
    {
        var words = new[] { "a", "ab", "ac", "abb", "add", "abdb", "adbd", "abdbd" };
        var solution = new Solution();
        Assert.Equal(5, solution.LongestStrChain(words));
    }

    [Fact]
    public void Test4()
    {
        var words = new[] { new string('a', 16) };
        var solution = new Solution();
        Assert.Equal(1, solution.LongestStrChain(words));
    }
}