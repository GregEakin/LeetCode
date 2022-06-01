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
using Xunit;

namespace HeapChecks;

public class FindResultantArrayAfterRemovingAnagrams
{
    public class Solution
    {
        public IList<string> RemoveAnagrams(string[] words)
        {
            var wordList = words.ToList();
            for (var i = words.Length - 2; i >= 0; i--)
            {
                var w1 = new string(wordList[i].ToCharArray().OrderBy(c => c).ToArray());
                var w2 = new string(wordList[i + 1].ToCharArray().OrderBy(c => c).ToArray());
                if (w1 == w2)
                    wordList.RemoveAt(i + 1);
            }

            return wordList;
        }
    }

    [Fact]
    public void Example1()
    {
        var words = new[] { "abba", "baba", "bbaa", "cd", "cd" };
        var solution = new Solution();
        Assert.Equal(new[] { "abba", "cd" }, solution.RemoveAnagrams(words));
    }

    [Fact]
    public void Example2()
    {
        var words = new[] { "a", "b", "c", "d", "e" };
        var solution = new Solution();
        Assert.Equal(new[] { "a", "b", "c", "d", "e" }, solution.RemoveAnagrams(words));
    }
}