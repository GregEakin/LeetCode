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
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace HeapChecks;

public class RearrangeCharactersToMakeTargetString
{
    public class Solution
    {
        public int RearrangeCharacters(string s, string target)
        {
            var source = new int[26];
            foreach (var c in s)
                source[c - 'a']++;

            var dest = new int[26];
            foreach (var c in target)
                dest[c - 'a']++;

            var sum = s.Length;
            for (var i = 0; i < 26; i++)
            {
                if (dest[i] == 0) continue;
                sum = Math.Min(sum, source[i] / dest[i]);
            }

            return sum;
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "ilovecodingonleetcode";
        var target = "code";
        var solution = new Solution();
        Assert.Equal(2, solution.RearrangeCharacters(s, target));
    }

    [Fact]
    public void Example2()
    {
        var s = "abcba";
        var target = "abc";
        var solution = new Solution();
        Assert.Equal(1, solution.RearrangeCharacters(s, target));
    }

    [Fact]
    public void Example3()
    {
        var s = "abbaccaddaeea";
        var target = "aaaaa";
        var solution = new Solution();
        Assert.Equal(1, solution.RearrangeCharacters(s, target));
    }
}