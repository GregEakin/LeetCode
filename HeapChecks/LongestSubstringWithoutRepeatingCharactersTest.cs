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
using System.Reflection.Metadata;
using Xunit;
using Xunit.Sdk;

namespace HeapChecks;

public class LongestSubstringWithoutRepeatingCharactersTest
{
    public class Solution
    {
        public static int LengthOfLongestSubstring(string s)
        {
            var longest = 0;
            for (var i = 0; i < s.Length; i++)
            {
                var set = new HashSet<char>();
                foreach (var letter in s.Substring(i))
                {
                    if (set.Contains(letter))
                        break;

                    set.Add(letter);
                }

                if (set.Count > longest)
                {
                    longest = set.Count;
                }
            }

            return longest;
        }
    }

    [Fact]
    public void Test1()
    {
        var s = "abc";
        Assert.Equal(3, Solution.LengthOfLongestSubstring(s));
    }

    [Fact]
    public void Example1()
    {
        // Answer is "abc"
        var s = "abcabcbb";
        Assert.Equal(3, Solution.LengthOfLongestSubstring(s));
    }

    [Fact]
    public void Example2()
    {
        // Answer is "b"
        var s = "bbbbb";
        Assert.Equal(1, Solution.LengthOfLongestSubstring(s));
    }

    [Fact]
    public void Example3()
    {
        // Answer is "wke"
        // "pwke" is a subsequence and not a substring

        var s = "pwwkew";
        Assert.Equal(3, Solution.LengthOfLongestSubstring(s));
    }

    [Fact]
    public void Answer1()
    {
        var s = " ";
        Assert.Equal(1, Solution.LengthOfLongestSubstring(s));
    }
}