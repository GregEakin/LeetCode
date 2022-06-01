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
using System.Text;
using Xunit;

namespace HeapChecks;

public class CountWordsObtainedAfterAddingALetter
{
    public class Solution1
    {
        public static string SortString(string input)
        {
            var characters = input.ToCharArray();
            Array.Sort(characters);
            return new string(characters);
        }

        public int WordCount(string[] startWords, string[] targetWords)
        {
            var words = new HashSet<string>();
            foreach (var startWord in startWords) 
                words.Add(SortString(startWord));

            var count = 0;
            foreach (var targetWord in targetWords)
            {
                var target = SortString(targetWord);
                for (var i = 0; i < target.Length; i++)
                {
                    var sub = new StringBuilder(target);
                    sub.Remove(i, 1);
                    if (!words.Contains(sub.ToString())) continue;
                    count++;
                    break;
                }
            }

            return count;
        }
    }

    public class Solution
    {
        public static int GetBit(string word)
        {
            var targetSums = 0;
            foreach (var c in word) 
                targetSums |= 1 << (c - 'a');

            return targetSums;
        }

        public int WordCount(string[] startWords, string[] targetWords)
        {
            var wordValues = new HashSet<int>();
            var startLen = startWords.Length;
            for (var i = 0; i < startLen; i++)
            {
                var targetSums = GetBit(startWords[i]);
                wordValues.Add(targetSums);
            }

            var ansCount = 0;
            foreach (var word in targetWords)
            {
                var wordBit = GetBit(word);
                foreach (var c in word)
                {
                    var diff = wordBit & ~(1 << (c - 'a'));
                    if (!wordValues.Contains(diff)) continue;
                    ansCount++;
                    break;
                }
            }

            return ansCount;
        }
    }

    [Fact]
    public void Example1()
    {
        var startWords = new[] { "ant", "act", "tack" };
        var targetWords = new[] { "tack", "act", "acti" };
        var solution = new Solution();
        Assert.Equal(2, solution.WordCount(startWords, targetWords));
    }

    [Fact]
    public void Example2()
    {
        var startWords = new[] { "ab", "a" };
        var targetWords = new[] { "abc", "abcd" };
        var solution = new Solution();
        Assert.Equal(1, solution.WordCount(startWords, targetWords));
    }

    [Fact]
    public void Test1()
    {
        var startWords = new[] { "abc", "xbc", "axc", "abx" };
        var targetWords = new[] { "xabc", "axbc", "abxc", "abcx" };
        var solution = new Solution();
        Assert.Equal(4, solution.WordCount(startWords, targetWords));
    }

    [Fact]
    public void Test2()
    {
        var startWords = new[] { "abc", "abx", "acx", "bcc" };
        var targetWords = new[] { "xcba", "abcx", "bcxa", "abcd" };
        var solution = new Solution();
        Assert.Equal(4, solution.WordCount(startWords, targetWords));
    }

    [Fact]
    public void Test3()
    {
        var startWords = new[] { "aabb", "abc" };
        var targetWords = new[] { "abcde", "aabbc", "aaaa" };
        var solution = new Solution();
        Assert.Equal(1, solution.WordCount(startWords, targetWords));
    }
}