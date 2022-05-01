using Xunit;

namespace HeapChecks;

public class LongestCommonSubstringTests
{
    [Fact]
    public void Test1()
    {
        var s = "ABABC";
        var t = "BABCA";
        var l = LongestCommonSubstring.Substring(s, t);
        Assert.Equal(new[] { "BABC" }, l);
    }

    [Fact]
    public void Test2()
    {
        var s = "BABCA";
        var t = "ABCBA";
        var l = LongestCommonSubstring.Substring(s, t);
        Assert.Equal(new[] { "ABC" }, l);
    }

    [Fact]
    public void Test3()
    {
        var s = "ABABC";
        var t = "ABCBA";
        var l = LongestCommonSubstring.Substring(s, t);
        Assert.Equal(new[] { "ABC" }, l);
    }
}