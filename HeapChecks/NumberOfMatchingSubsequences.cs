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

public class NumberOfMatchingSubsequences
{
    public class Solution
    {
        public int NumMatchingSubseq(string s, string[] words)
        {
            var dict = new List<int>[26];
            for (var i = 0; i < dict.Length; i++)
                dict[i] = new List<int>();

            for (var i = 0; i < s.Length; i++)
                dict[s[i] - 'a'].Add(i);

            var count = 0;
            foreach (var word in words)
            {
                var position = 0;
                foreach (var c in word)
                {
                    var locations = dict[c - 'a'];
                    var search = locations.BinarySearch(position);
                    var index = search >= 0 ? search : ~search;
                    if (index >= locations.Count)
                    {
                        position = int.MaxValue;
                        break;
                    }

                    position = locations[index] + 1;
                }

                if (position <= s.Length) count++;
            }

            return count;
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "abcde";
        var words = new[] { "a", "bb", "acd", "ace" };
        // Output: 3
        // Explanation: There are three strings in words that are a subsequence of s: "a", "acd", "ace".
        var solution = new Solution();
        Assert.Equal(3, solution.NumMatchingSubseq(s, words));
    }

    [Fact]
    public void Example2()
    {
        var s = "dsahjpjauf";
        var words = new[] { "ahjpjau", "ja", "ahbwzgqnuk", "tnmlanowax" };
        var solution = new Solution();
        Assert.Equal(2, solution.NumMatchingSubseq(s, words));
    }
}