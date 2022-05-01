using Xunit;

namespace HeapChecks;

public class MinimumNumberOfOperationsToConvertTime
{
    public class Solution
    {
        public static int ParseTime(string time)
        {
            var h = int.Parse(time.Substring(0, 2));
            var m = int.Parse(time.Substring(3, 2));
            var t = h * 60 + m;
            return t;
        }

        public int ConvertTime(string current, string correct)
        {
            var c1 = ParseTime(current);
            var c2 = ParseTime(correct);

            var count = 0;
            var steps = new[] { 60, 15, 5, 1 };
            foreach (var step in steps)
            {
                while (c1 < c2)
                {
                    var time = c1 + step;
                    if (time > c2)
                        break;
                    c1 = time;
                    count++;
                }
            }

            return count;
        }
    }

    [Fact]
    public void Example1()
    {
        var current = "02:30";
        var correct = "04:35";

        var solution = new Solution();
        Assert.Equal(3, solution.ConvertTime(current, correct));
    }

    [Fact]
    public void Example2()
    {
        var current = "11:00";
        var correct = "11:01";

        var solution = new Solution();
        Assert.Equal(1, solution.ConvertTime(current, correct));
    }

    [Fact]
    public void Answer1()
    {
        var current = "00:00";
        var correct = "23:59";

        var solution = new Solution();
        Assert.Equal(32, solution.ConvertTime(current, correct));
    }
}