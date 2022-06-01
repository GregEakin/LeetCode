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

public class WordBreakII
{
    public class Solution
    {
        public class Node
        {
            public int Index { get; set; } = -1;
            public Dictionary<char, Node> Nodes { get; } = new();
        }

        public class WordList
        {
            public string Word { get; }
            public int Count { get; }
            public IList<WordList> List { get; } = new List<WordList>();

            public WordList(string word, int count)
            {
                Word = word;
                Count = count;
            }

            public WordList Add(string word, int count)
            {
                var next = new WordList(word, count);
                List.Add(next);
                return next;
            }
        }

        public static Node BuildTrie(IList<string> wordDict)
        {
            var trie = new Node();
            for (var index = 0; index < wordDict.Count; index++)
            {
                var word = wordDict[index];
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

                node.Index = index;
            }

            return trie;
        }

        public static void ParseSentences(int size, List<string> sentences, WordList wordList, List<string> words)
        {
            if (wordList.Count == size)
            {
                var sentence = new StringBuilder();
                foreach (var word in words)
                {
                    sentence.Append(word);
                    sentence.Append(' ');
                }

                sentence.Remove(sentence.Length - 1, 1);
                sentences.Add(sentence.ToString());
            }

            foreach (var node in wordList.List)
            {
                words.Add(node.Word);
                ParseSentences(size, sentences, node, words);
                words.RemoveAt(words.Count - 1);
            }
        }

        public IList<string> WordBreak(string s, IList<string> wordDict)
        {
            var trie = BuildTrie(wordDict);
            var wordList = new WordList(string.Empty, 0);
            var queue = new Queue<(int, Node, WordList)>();
            // var visited = new bool[s.Length];
            queue.Enqueue((0, trie, wordList));
            while (queue.Count > 0)
            {
                var (i, step, sentence) = queue.Dequeue();
                if (i == s.Length) continue;
                // if (visited[i] && step.Index >= 0) continue;
                if (!step.Nodes.TryGetValue(s[i], out var next)) continue;
                queue.Enqueue((i + 1, next, sentence));
                if (next.Index < 0) continue;
                // visited[i] = true;
                var nextSentence = sentence.Add(wordDict[next.Index], i + 1);
                queue.Enqueue((i + 1, trie, nextSentence));
            }

            var sentences = new List<string>();
            ParseSentences(s.Length, sentences, wordList, new List<string>());
            return sentences;
        }
    }

    [Fact]
    public void Answer1()
    {
        var s = "aaaaaaa";
        var wordDict = new[] { "aaaa", "aa" };
        var solution = new Solution();
        Assert.Equal(Array.Empty<string>(), solution.WordBreak(s, wordDict));
    }

    // [Fact]
    // public void Answer2()
    // {
    //     var s = "aaaaaaaaaaaaaaaaaaab";
    //     var wordDict = new[]
    //         { "a", "aa", "aaa", "aaaa", "aaaaa", "aaaaaa", "aaaaaaa", "aaaaaaaa", "aaaaaaaaa", "aaaaaaaaaa" };
    //     var solution = new Solution();
    //     Assert.Equal(Array.Empty<string>(), solution.WordBreak(s, wordDict));
    // }

    [Fact]
    public void Answer3()
    {
        var s = "aaaaaaa";
        var wordDict = new[] { "aaaa", "aaa" };
        var solution = new Solution();
        Assert.Equal(new[] { "aaaa aaa", "aaa aaaa" }.OrderBy(w => w),
            solution.WordBreak(s, wordDict).OrderBy(w => w));
    }

    [Fact]
    public void Answer4()
    {
        var s = "catsandogcat";
        var wordDict = new[] { "cats", "dog", "sand", "and", "cat", "an" };
        var solution = new Solution();
        Assert.Equal(new[] { "cats an dog cat" }.OrderBy(w => w),
            solution.WordBreak(s, wordDict).OrderBy(w => w));
    }

    [Fact]
    public void Example1()
    {
        var s = "catsanddog";
        var wordDict = new[] { "cat", "cats", "and", "sand", "dog" };
        var solution = new Solution();
        Assert.Equal(new[] { "cats and dog", "cat sand dog" }.OrderBy(s => s),
            solution.WordBreak(s, wordDict).OrderBy(s => s));
    }

    [Fact]
    public void Example2()
    {
        var s = "pineapplepenapple";
        var wordDict = new[] { "apple", "pen", "applepen", "pine", "pineapple" };
        var solution = new Solution();
        Assert.Equal(new[] { "pine apple pen apple", "pineapple pen apple", "pine applepen apple" }.OrderBy(w => w),
            solution.WordBreak(s, wordDict).OrderBy(w => w));
    }

    [Fact]
    public void Example3()
    {
        var s = "catsandog";
        var wordDict = new[] { "cats", "dog", "sand", "and", "cat" };
        var solution = new Solution();
        Assert.Equal(Array.Empty<string>(), solution.WordBreak(s, wordDict));
    }

    [Fact]
    public void Test1()
    {
        var s = "tootee";
        var wordDict = new[] { "to", "too", "two", "tee", "tea", "ted" };
        var solution = new Solution();
        Assert.Equal(new[] { "too tee" }.OrderBy(s => s),
            solution.WordBreak(s, wordDict).OrderBy(s => s));
    }
}