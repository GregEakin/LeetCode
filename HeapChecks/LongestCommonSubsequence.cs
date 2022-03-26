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
using System.ComponentModel.Design;
using System.Runtime.InteropServices.ComTypes;
using Xunit;

namespace HeapChecks;

public class LongestCommonSubsequence
{
    public static IEnumerable<char> CommonSubsequence(char[] x, char[] y)
    {
        throw new System.NotImplementedException();
    }

    public static (int[,] c, char[,] b) Length(string x, string y)
    {
        var m = x.Length;
        var n = y.Length;
        var c = new int[m + 1, n + 1];
        var b = new char[m + 1, n + 1];
        for (var i = 1; i <= m; i++)
        for (var j = 1; j <= n; j++)
        {
            if (x[i - 1] == y[j - 1])
            {
                c[i, j] = c[i - 1, j - 1] + 1;
                b[i, j] = '\\';
            }
            else if (c[i - 1, j] >= c[i, j - 1])
            {
                c[i, j] = c[i - 1, j];
                b[i, j] = '|';
            }
            else
            {
                c[i, j] = c[i, j - 1];
                b[i, j] = '-';
            }
        }

        return (c, b);
    }
}

public class LongestCommonSubsequenceTests
{
    public static class Solution
    {
        public static int LongestCommonSubsequence(string text1, string text2)
        {
            var m = text1.Length;
            var n = text2.Length;
            var c1 = new int[n + 1];
            for (var i = 0; i < m; i++)
            {
                var c0 = c1;
                c1 = new int[n + 1];
                for (var j = 0; j < n; j++)
                    if (text1[i] == text2[j])
                        c1[j + 1] = c0[j] + 1;
                    else if (c0[j] >= c1[j])
                        c1[j + 1] = c0[j + 1];
                    else
                        c1[j + 1] = c1[j];
            }

            return c1[^1];
        }
    }

    [Fact]
    public void Example0()
    {
        var text1 = "ppabcdepp";
        var text2 = "eeexaxcxexfff";
        Assert.Equal(3, Solution.LongestCommonSubsequence(text1, text2));
    }

    [Fact]
    public void Example1()
    {
        var text1 = "abcde";
        var text2 = "ace";
        Assert.Equal(3, Solution.LongestCommonSubsequence(text1, text2));
    }

    [Fact]
    public void Example2()
    {
        var text1 = "abc";
        var text2 = "abc";
        Assert.Equal(3, Solution.LongestCommonSubsequence(text1, text2));
    }

    [Fact]
    public void Example3()
    {
        var text1 = "abc";
        var text2 = "def";
        Assert.Equal(0, Solution.LongestCommonSubsequence(text1, text2));
    }

    [Fact]
    public void Example4()
    {
        var text1 = "zazazaz";
        var text2 = "bababababab";
        Assert.Equal(3, Solution.LongestCommonSubsequence(text1, text2));
    }

    [Fact]
    public void Test1()
    {
        var x = "ABCBDAB";
        var y = "BDCABA";

        // Assert.Equal(new[] { 'B', 'C', 'B', 'A' }, LongestCommonSubsequence.CommonSubsequence(text1, text2));

        var length = LongestCommonSubsequence.Length(x, y);
    }
}