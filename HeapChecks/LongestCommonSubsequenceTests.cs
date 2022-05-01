using Xunit;

namespace HeapChecks;

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