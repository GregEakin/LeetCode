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
using Xunit;

namespace HeapChecks;

public class MinimumTimeToRemoveAllCarsContainingIllegalGoods
{
    public class Solution
    {
        public int MinimumTime1(string s)
        {
            var leftCount = 0;
            var lefts = new int[s.Length];
            var front = 0;
            for (var left = 0; left < s.Length; left++)
            {
                if (s[left] == '0') continue;
                lefts[left] = left - front + 1;
                leftCount += left - front + 1;
                front = left + 1;
            }

            var rightCount = 0;
            var rights = new int[s.Length];
            var end = s.Length - 1;
            for (var right = s.Length - 1; right >= 0; right--)
            {
                if (s[right] == '0') continue;
                rights[right] = end - right + 1;
                rightCount += end - right + 1;
                end = right - 1;
            }

            return Math.Min(leftCount, rightCount);
        }

        public int MinimumTime2(string s)
        {
            if (s.Length == 0) return 0;

            var left = 0;
            var right = 0;
            var lefts = new int[s.Length + 1];
            var rights = new int[s.Length + 1];
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == '1') left = i + 1;
                lefts[i + 1] = left;

                if (s[s.Length - 1 - i] == '1') right = i + 1;
                rights[rights.Length - 2 - i] = right;
            }

            for (var i = 0; i < s.Length; i++)
            {
                var i1 = s.Length - 1 - i;
                var s1 = lefts[i1];
                for (var j = i1; j < s.Length; j++)
                    if (s[j] == '1')
                        s1 += 2;

                if (left > s1)
                {
                    var s2 = lefts[i1];
                    for (var j = i1; j < s.Length; j++)
                    {
                        if (s[j] == '1') s2 += 2;
                        lefts[j + 1] = s2;
                    }

                    left = s2;
                }
            }

            for (var i = 0; i < s.Length; i++)
            {
                var s1 = rights[i + 1];
                for (var j = i; j >= 0; j--)
                    if (s[j] == '1')
                        s1 += 2;

                if (right > s1)
                {
                    var s2 = rights[i + 1];
                    for (var j = i; j >= 0; j--)
                    {
                        if (s[j] == '1') s2 += 2;
                        rights[j] = s2;
                    }

                    right = s2;
                }
            }

            var min = int.MaxValue;
            for (var i = 0; i < s.Length; i++)
            {
                var count = lefts[i] + rights[i];
                if (min > count) min = count;
            }

            return min;
        }

        public int MinimumTimeX(string s)
        {
            var sa = new int[s.Length];
            for (var i = 1; i < s.Length; i++)
                sa[i] = sa[i - 1] + s[i - 1] == '1' ? 1 : 0;
            for (var i = 1; i < s.Length; i++)
            {
                var x = sa[i];
                sa[i] = 2 * x - i - 1;
            }

            var max = 0;
            var ret = 0;
            for (var i = 1; i < s.Length; i++)
            {
                var x = sa[i];
                ret = Math.Min(ret, x - max);
                max = Math.Max(x, max);
            }

            return ret + s.Length;
        }

        public int MinimumTime(string s)
        {
            var left = 0;
            var count0 = 0;
            var count1 = 0;
            var best = (left, right: s.Length - 1, count0: 0, count1: s.Length);

            for (var right = 0; right < s.Length; right++)
            {
                if (s[right] == '0')
                    count0++;
                else
                    count1++;

                while (count1 > count0)
                {
                    if (s[left] == '0')
                        count0--;
                    else
                        count1--;

                    left++;
                }

                if (count0 - count1 > best.count0 - best.count1)
                    best = (left, right, count0, count1);
            }

            return best.left + 2 * best.count1 + s.Length - 1 - best.right;
        }

        public int MinimumTimeSlim(string s)
        {
            var costLeft = 0;
            var minCost = s.Length;

            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == '1')
                    costLeft = Math.Min(costLeft + 2, i + 1);
                minCost = Math.Min(minCost, costLeft + s.Length - i - 1);
            }

            return minCost;
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "1100101";
        var solution = new Solution();
        Assert.Equal(5, solution.MinimumTime(s));
    }

    [Fact]
    public void Example2()
    {
        var s = "0010";
        var solution = new Solution();
        Assert.Equal(2, solution.MinimumTime(s));
    }

    [Fact]
    public void Answer1()
    {
        var s = "010110";
        var solution = new Solution();
        Assert.Equal(5, solution.MinimumTime(s));
    }

    [Fact]
    public void Answer2()
    {
        var s = "01011010";
        var solution = new Solution();
        Assert.Equal(7, solution.MinimumTime(s));
    }

    [Fact]
    public void Answer3()
    {
        var s = "0111001110";
        var solution = new Solution();
        Assert.Equal(8, solution.MinimumTime(s));
    }

    [Fact]
    public void Answer4()
    {
        var s = "011001111111101001010000001010011";
        var solution = new Solution();
        Assert.Equal(25, solution.MinimumTime(s));
    }

    [Fact]
    public void Answer5()
    {
        var s = "10111111100000100101010111111001011110010101011110110101001110101010000";
        var solution = new Solution();
        Assert.Equal(65, solution.MinimumTime(s));
    }

    [Fact]
    public void Test1()
    {
        var s = "";
        var solution = new Solution();
        Assert.Equal(0, solution.MinimumTime(s));
    }

    [Fact]
    public void Test2()
    {
        var s = "0";
        var solution = new Solution();
        Assert.Equal(0, solution.MinimumTime(s));
    }

    [Fact]
    public void Test3()
    {
        var s = "1";
        var solution = new Solution();
        Assert.Equal(1, solution.MinimumTime(s));
    }

    [Fact]
    public void Test4()
    {
        var s = "10";
        var solution = new Solution();
        Assert.Equal(1, solution.MinimumTime(s));
    }

    [Fact]
    public void Test5()
    {
        var s = "01";
        var solution = new Solution();
        Assert.Equal(1, solution.MinimumTime(s));
    }

    [Fact]
    public void Test6()
    {
        var s = "00100";
        var solution = new Solution();
        Assert.Equal(2, solution.MinimumTime(s));
    }

    [Fact]
    public void Test7()
    {
        var s = "11010111011";
        var solution = new Solution();
        Assert.Equal(10, solution.MinimumTime(s));
    }

    [Fact]
    public void Test8()
    {
        var s = "0111";
        var solution = new Solution();
        Assert.Equal(3, solution.MinimumTime(s));
    }

    [Fact]
    public void Test9()
    {
        var s = "00010000";
        var solution = new Solution();
        Assert.Equal(2, solution.MinimumTime(s));
    }

    [Fact]
    public void Test10()
    {
        var s = "00000111100000";
        var solution = new Solution();
        Assert.Equal(8, solution.MinimumTime(s));
    }

    [Fact]
    public void Test11()
    {
        var s = "000100010001000";
        var solution = new Solution();
        Assert.Equal(6, solution.MinimumTime(s));
    }

    [Fact]
    public void Test12()
    {
        var s = "111011101110111";
        var solution = new Solution();
        Assert.Equal(14, solution.MinimumTime(s));
    }

    [Fact]
    public void Test13()
    {
        var s = "111111";
        var solution = new Solution();
        Assert.Equal(6, solution.MinimumTime(s));
    }
}