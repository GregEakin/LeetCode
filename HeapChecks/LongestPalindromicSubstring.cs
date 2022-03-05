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

using Xunit;

namespace HeapChecks;

public class LongestPalindromicSubstring
{
    public class Solution
    {
        public static string Palindrome(string s)
        {
            for (var i = 0; i < s.Length / 2; i++)
            {
                if (s[i] != s[^(i + 1)])
                    return string.Empty;
            }
            return s;
        }

        public static string LongestPalindrome(string s)
        {
            var longest = string.Empty;
            for (var i = 0; i < s.Length; i++)
            {
                for (var j = longest.Length + 1; j <= s.Length - i; j++)
                {
                    var substring = s.Substring(i, j);
                    var palindrome = Palindrome(substring);
                    if (palindrome.Length > longest.Length)
                        longest = palindrome;
                }
            }

            return longest;
        }
    }

    [Fact]
    public void PalindromeTest1()
    {
        Assert.Equal("a", Solution.Palindrome("a"));
    }

    [Fact]
    public void PalindromeTest2()
    {
        Assert.Equal("aa", Solution.Palindrome("aa"));
    }

    [Fact]
    public void PalindromeTest3()
    {
        Assert.Equal(string.Empty, Solution.Palindrome("ab"));
    }

    [Fact]
    public void PalindromeTest4()
    {
        Assert.Equal("aveva", Solution.Palindrome("aveva"));
    }

    [Fact]
    public void Test1()
    {
        var s = "a";
        Assert.Equal("a", Solution.LongestPalindrome(s));
    }

    [Fact]
    public void Test2()
    {
        var s = "babab";
        Assert.Equal("babab", Solution.LongestPalindrome(s));
    }

    [Fact]
    public void Example1()
    {
        var s = "babad";
        Assert.Equal("bab", Solution.LongestPalindrome(s));
        // Assert.Equal("aba", Solution.LongestPalindrome(s));
    }

    [Fact]
    public void Example2()
    {
        var s = "cbbd";
        Assert.Equal("bb", Solution.LongestPalindrome(s));
    }
}