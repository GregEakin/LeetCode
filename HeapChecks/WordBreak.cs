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

public class WordBreak
{
    public class Solution
    {
        public class Node
        {
            public bool EndOfWord { get; set; }
            public Dictionary<char, Node> Nodes { get; } = new();
        }

        public bool WordBreak(string s, IList<string> wordDict)
        {
            var trie = new Node();
            foreach (var word in wordDict)
            {
                var node = trie;
                foreach (var c in word)
                {
                    if (!node.Nodes.TryGetValue(c, out var next))
                    {
                        next = new Node();
                        node.Nodes.Add(c, next);
                    }

                    node = next;
                }

                node.EndOfWord = true;
            }

            var queue = new Queue<(int, Node)>();
            var visited = new bool[s.Length];
            queue.Enqueue((0, trie));
            while (queue.Count > 0)
            {
                var (i, step) = queue.Dequeue();
                if (i == s.Length) continue;
                if (visited[i] && step.EndOfWord) continue;
                if (!step.Nodes.TryGetValue(s[i], out var next)) continue;
                queue.Enqueue((i + 1, next));
                if (!next.EndOfWord) continue;
                visited[i] = true;
                queue.Enqueue((i + 1, trie));
            }

            return visited[s.Length - 1];
        }
    }

    public class SolutionGood
    {
        public bool WordBreak(string s, IList<string> wordDict)
        {
            var words = wordDict.ToHashSet();
            var dp = new bool[s.Length + 1];
            dp[0] = true;
            for (var i = 1; i <= s.Length; i++)
            for (var j = 0; j < i; j++)
            {
                if (!dp[j] || !words.Contains(s.Substring(j, i - j))) continue;
                dp[i] = true;
                break;
            }

            return dp[s.Length];
        }
    }

    [Fact]
    public void Answer1()
    {
        var s = "aaaaaaa";
        var wordDict = new[] { "aaaa", "aa" };
        var solution = new Solution();
        Assert.False(solution.WordBreak(s, wordDict));
    }

    [Fact]
    public void Answer2()
    {
        var s =
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab";
        var wordDict = new[]
            { "a", "aa", "aaa", "aaaa", "aaaaa", "aaaaaa", "aaaaaaa", "aaaaaaaa", "aaaaaaaaa", "aaaaaaaaaa" };
        var solution = new Solution();
        Assert.False(solution.WordBreak(s, wordDict));
    }

    [Fact]
    public void Answer3()
    {
        var s = "aaaaaaa";
        var wordDict = new[] { "aaaa", "aaa" };
        var solution = new Solution();
        Assert.True(solution.WordBreak(s, wordDict));
    }

    [Fact]
    public void Answer4()
    {
        var s = "catsandogcat";
        var wordDict = new[] { "cats", "dog", "sand", "and", "cat", "an" };
        var solution = new Solution();
        Assert.True(solution.WordBreak(s, wordDict));
    }

    [Fact]
    public void Example1()
    {
        var s = "leetcode";
        var wordDict = new[] { "leet", "code" };
        var solution = new Solution();
        Assert.True(solution.WordBreak(s, wordDict));
    }

    [Fact]
    public void Example2()
    {
        var s = "applepenapple";
        var wordDict = new[] { "apple", "pen" };
        var solution = new Solution();
        Assert.True(solution.WordBreak(s, wordDict));
    }

    [Fact]
    public void Example3()
    {
        var s = "catsandog";
        var wordDict = new[] { "cats", "dog", "sand", "and", "cat" };
        var solution = new Solution();
        Assert.False(solution.WordBreak(s, wordDict));
    }

    [Fact]
    public void Test1()
    {
        var s = "tootee";
        var wordDict = new[] { "to", "too", "two", "tee", "tea", "ted" };
        var solution = new Solution();
        Assert.True(solution.WordBreak(s, wordDict));
    }
}