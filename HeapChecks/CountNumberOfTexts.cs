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

public class CountNumberOfTexts
{
    public class Solution
    {
        private static List<(int, int)> RunLengthEncoding(string pressedKeys)
        {
            var list = new List<(int, int)>();
            var last = 'x';
            var count = 0;
            foreach (var key in pressedKeys)
            {
                if (key == last)
                {
                    count++;
                    continue;
                }

                if (last != 'x')
                    list.Add((last, count));
                last = key;
                count = 1;
            }

            if (count > 0)
                list.Add((last, count));
            return list;
        }

        private static readonly Dictionary<long, long> Threes = new();
        private static readonly Dictionary<long, long> Fours = new();

        public static long ThreeLetter(long x)
        {
            if (Threes.TryGetValue(x, out var count))
                return count;

            // if (x == 0) return 0L;
            if (x == 1) return 1L;
            if (x == 2) return 2L;
            if (x == 3) return 4L;
            count = ThreeLetter(x - 1) + ThreeLetter(x - 2) + ThreeLetter(x - 3);
            count %= 1000000007L;
            Threes.Add(x, count);
            return count;
        }

        public static long FourLetter(long x)
        {
            if (Fours.TryGetValue(x, out var count))
                return count;

            // if (x == 0) return 0L;
            if (x == 1) return 1L;
            if (x == 2) return 2L;
            if (x == 3) return 4L;
            if (x == 4) return 8L;
            count = FourLetter(x - 1) + FourLetter(x - 2) + FourLetter(x - 3) + FourLetter(x - 4);
            count %= 1000000007L;
            Fours.Add(x, count);
            return count;
        }

        public static long Combinations(int digit, int count)
        {
            if (digit == '7' || digit == '9')
                return FourLetter(count);

            return ThreeLetter(count);
        }

        public int CountTexts(string pressedKeys)
        {
            var product = 1L;
            var list = RunLengthEncoding(pressedKeys);
            foreach (var (digit, count) in list)
                product = product * Combinations(digit, count) % 1000000007L;

            return (int)product;
        }
    }

    [Fact]
    public void Answer1()
    {
        var keys =
            "444444444444444444444444444444448888888888888888999999999999333333333333333366666666666666662222222222222222666666666666666633333333333333338888888888888888222222222222222244444444444444448888888888888222222222222222288888888888889999999999999999333333333444444664";
        var solution = new Solution();
        Assert.Equal(537551452, solution.CountTexts(keys));
    }

    [Fact]
    public void Answer2()
    {
        var keys =
            "88888888888888888888888888888999999999999999999999999999994444444444444444444444444444488888888888888888888888888888555555555555555555555555555556666666666666666666666666666666666666666666666666666666666222222222222222222222222222226666666666666666666666666666699999999999999999999999999999888888888888888888888888888885555555555555555555555555555577777777777777777777777777777444444444444444444444444444444444444444444444444444444444433333333333333333333333333333555555555555555555555555555556666666666666666666666666666644444444444444444444444444444999999999999999999999999999996666666666666666666666666666655555555555555555555555555555444444444444444444444444444448888888888888888888888888888855555555555555555555555555555555555555555555555555555555555555555555555555555555555999999999999999555555555555555555555555555554444444444444444444444444444444555";
        var solution = new Solution();
        Assert.Equal(886136986, solution.CountTexts(keys));
    }

    [Fact]
    public void TestThreeLetterWord()
    {
        Assert.Equal(1L, Solution.Combinations('2', 1));
        Assert.Equal(2L, Solution.Combinations('2', 2));
        Assert.Equal(4L, Solution.Combinations('2', 3));
        Assert.Equal(7L, Solution.Combinations('2', 4));
        Assert.Equal(13L, Solution.Combinations('2', 5));
    }

    [Fact]
    public void TestFourLetterWord()
    {
        Assert.Equal(1L, Solution.Combinations('7', 1));
        Assert.Equal(2L, Solution.Combinations('7', 2));
        Assert.Equal(4L, Solution.Combinations('7', 3));
        Assert.Equal(8L, Solution.Combinations('7', 4));
        Assert.Equal(15L, Solution.Combinations('7', 5));
    }

    [Fact]
    public void Example0()
    {
        var pressedKeys = "2266622";
        var solution = new Solution();
        Assert.Equal(16, solution.CountTexts(pressedKeys));
    }

    [Fact]
    public void Example1()
    {
        var pressedKeys = "22233";
        var solution = new Solution();
        Assert.Equal(8, solution.CountTexts(pressedKeys));
    }

    [Fact]
    public void Example2()
    {
        var pressedKeys = "222222222222222222222222222222222222";
        var solution = new Solution();
        Assert.Equal(82876089, solution.CountTexts(pressedKeys));
    }
}