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
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace HeapChecks;

public class TilingARectangleWithFewestSquares
{
    private readonly ITestOutputHelper _output;

    public TilingARectangleWithFewestSquares(ITestOutputHelper output)
    {
        _output = output;
    }

    public class SolutionGreedy
    {
        public int TilingRectangle(int n, int m)
        {
            var count = 0;
            while (n > 0 && m > 0)
            {
                count++;
                if (n <= m)
                    m -= n;
                else
                    n -= m;
            }

            return count;
        }
    }

    public class SolutionAnswer
    {
        private int[,] _cache;

        private int TrUtil(int n, int m)
        {
            if (n > m) (n, m) = (m, n);

            if (_cache[n, m] != 0)
                return _cache[n, m];

            if (n == 0)
            {
                _cache[0, m] = 0;
                return 0;
            }

            if (n == 1)
            {
                _cache[n, m] = m;
                return m;
            }

            if (n == m)
            {
                _cache[n, m] = 1;
                return 1;
            }

            if (m % n == 0)
            {
                _cache[n, m] = m / n;
                return m / n;
            }

            if (m > 2 * n)
            {
                var num = m / n - 1;
                var newM = m - num * n;
                _cache[n, m] = num + TrUtil(n, newM);
                return _cache[n, m];
            }

            _cache[n, m] = 1 + TrUtil(Math.Min(n, m - n), Math.Max(n, m - n));

            for (var i = (m + 1) / 2; i < n; i++)
            for (var j = 0; j <= m - i; j++)
                _cache[n, m] = Math.Min(_cache[n, m], 2
                                                      + TrUtil(n - i, i + j)
                                                      + TrUtil(n - (m - i), m - i - j)
                                                      + TrUtil(j, i - (m - i)));

            return _cache[n, m];
        }

        public int TilingRectangle(int n, int m)
        {
            (n, m) = (Math.Min(n, m), Math.Max(n, m));
            _cache = new int[n + 1, m + 1];
            return TrUtil(n, m);
        }
    }
    
    public class Solution1
    {
        public static int GreedySolve(int rn, int rm, int n, int m, int max)
        {
            var count = 1;
            while (n > 0 && m > 0 && count < max)
            {
                // Is there anything left?
                if (n == rn && m == rm)
                    break;

                if (n == rn)
                {
                    m -= rn;
                    rn = 0;
                    rm = 0;
                }

                if (m == rm)
                {
                    n -= rm;
                    rn = 0;
                    rm = 0;
                }

                count++;
                var n1 = n - rn;
                var m1 = m - rm;

                // Can we take a byte out of the right?
                if (m1 >= n)
                {
                    m -= n;
                    if (m == rm)
                    {
                        n = n1;
                        rn = 0;
                        rm = 0;
                    }

                    continue;
                }

                // Can we take a byte out of the top?
                if (n1 >= m)
                {
                    n -= m;
                    if (n == rn)
                    {
                        m = m1;
                        rn = 0;
                        rm = 0;
                    }

                    continue;
                }

                // Can we take a byte out of bottom right?
                if (m1 <= rn)
                {
                    n -= m1;
                    rn -= m1;
                    if (rn == 0)
                        rm = 0;
                    continue;
                }

                // Can we take a byte out of top left?
                if (n1 <= rm)
                {
                    m -= n1;
                    rm -= n1;
                    if (rm == 0)
                        rn = 0;
                    continue;
                }

                // Then, grow the (rn, rm) rectangle
                if (rn >= rm)
                    rm += Math.Min(rn, m1);
                else
                    rn += Math.Min(rm, n1);
            }

            return count;
        }

        public int TilingRectangle(int n, int m)
        {
            // var dict = new Dictionary<int, int>();

            var min = Math.Min(n, m);
            var count = Math.Max(n, m);
            for (var r = min; r >= (min - 1) / 2 + 1; r--)
            {
                var guess = GreedySolve(r, r, n, m, count);
                // dict.Add(r, guess);
                count = Math.Min(count, guess);
            }

            return count;
        }
    }

    public class Solution
    {
        private readonly Dictionary<(int, int), int> _dict = new();

        public int TilingRectangle(int n, int m)
        {
            if (n > m) (n, m) = (m, n);
            if (n == 0) return 0;
            if (n == 1) return m;
            if (m % n == 0) return m / n;
            if (_dict.TryGetValue((n, m), out var answer)) return answer;
            if (m > 2 * n)
            {
                var squares = m / n - 1;
                var leftover = m - squares * n;
                return _dict[(n, m)] = squares + TilingRectangle(n, leftover);
            }

            var min = 1 + TilingRectangle(m - n, n);
            for (var i = (m + 1) / 2; i < n; i++)
            for (var j = 0; j <= m - i; j++)
            {
                var sum = 2
                          + TilingRectangle(n - i, i + j)
                          + TilingRectangle(m - i - j, i - m + n)
                          + TilingRectangle(j, 2 * i - m);
                min = Math.Min(min, sum);
            }

            return _dict[(n, m)] = min;
        }
    }

    public class SolutionPrint
    {
        private readonly Dictionary<(int, int), int> _dict = new();

        private readonly ITestOutputHelper _output;
        // public int this[int n, int m] => _dict.TryGetValue((n, m), out var answer) ? answer : -1;

        public SolutionPrint(ITestOutputHelper output)
        {
            _output = output;
        }

        public void PrintData(int x, int y)
        {
            _output.WriteLine($"Cache {x}, {y}");
            for (var n = 1; n <= x; n++)
            {
                var buffer = new StringBuilder();
                buffer.Append($"{n,2}:  ");
                for (var m = 1; m <= y; m++)
                {
                    var data = _dict.TryGetValue((n,m), out var d) ? $"{d,2}" : "  ";
                    buffer.Append($"{data}, ");
                }

                _output.WriteLine(buffer.ToString());
            }
        }

        private int Solve(int n, int m)
        {
            if (n > m) (n, m) = (m, n);
            if (n == 0) return 0;
            if (n == 1) return m;
            if (n == m) return 1;
            if (m % n == 0) return m / n;
            if (_dict.TryGetValue((n, m), out var answer)) return answer;
            if (m > 2 * n)
            {
                var squares = m / n - 1;
                var leftover = m - squares * n;
                return _dict[(n, m)] = squares + Solve(n, leftover);
            }

            var min = 1 + Solve(m - n, n);
            //_output.WriteLine($"-- Solve ({n}, {m}) := {min} = 1 + [{m - n}, {n}]");

            for (var i = (m + 1) / 2; i < n; i++)
            for (var j = 0; j <= m - i; j++)
            {
                var sum = 2
                          + Solve(n - i, i + j)
                          + Solve(m - i - j, i - m + n)
                          + Solve(j, 2 * i - m);
                //_output.WriteLine(
                //    $"   Solve ({n}, {m}) <{i}, {j}> := {sum} = 2 + [{n - i}, {i + j}] + [{m - i - j}, {i - m + n}] + [{j}, {2 * i - m}]");
                min = Math.Min(min, sum);
            }

            //_output.WriteLine($"===== ({n}, {m}) = {min}");
            return _dict[(n, m)] = min;
        }

        public int TilingRectangle(int n, int m)
        {
            return Solve(n, m);
        }
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal(3, solution.TilingRectangle(2, 3));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal(5, solution.TilingRectangle(5, 8));
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.Equal(6, solution.TilingRectangle(11, 13));
    }

    [Fact]
    public void Test1()
    {
        var solution = new SolutionPrint(_output);
        Assert.Equal(7, solution.TilingRectangle(8, 9));
        // solution.PrintData(8, 9);
    }

    [Fact]
    public void Test2()
    {
        var solution = new SolutionPrint(_output);
        Assert.Equal(7, solution.TilingRectangle(11, 16));
        // solution.PrintData(11, 16);
    }

    [Fact]
    public void Test3()
    {
        var solution = new SolutionPrint(_output);
        Assert.Equal(7, solution.TilingRectangle(13, 14));
        // for (var i = 1; i < 14; i++)
        // for (var j = 1; j < 14; j++)
        //     solution.TilingRectangle(i, j);
        //
        // solution.PrintData(13, 14);
    }

    [Fact]
    public void TestAll()
    {
        var answer = new SolutionAnswer();
        var solution = new Solution();
        for (var n = 1; n <= 13; n++)
        for (var m = 1; m <= 13; m++)
        {
            var count = answer.TilingRectangle(n, m);
            var result = solution.TilingRectangle(n, m);
            Assert.Equal(count, result);
        }
    }

    public static (int, int, int, int) RightBite(int rn, int rm, int n, int m)
    {
        var n1 = n - rn;
        m -= n;
        if (m <= rm)
        {
            n = n1;
            rn = 0;
            rm = 0;
        }

        return (rn, rm, n, m);
    }

    public static (int, int, int, int) TopBite(int rn, int rm, int n, int m)
    {
        var m1 = m - rm;
        n -= m;
        if (n <= rn)
        {
            m = m1;
            rn = 0;
            rm = 0;
        }

        return (rn, rm, n, m);
    }

    [Fact]
    public void RightBiteTest1()
    {
        Assert.Equal((0, 0, 3, 3), RightBite(4, 3, 7, 10));
    }

    [Fact]
    public void RightBiteTest2()
    {
        Assert.Equal((4, 3, 7, 5), RightBite(4, 3, 7, 12));
    }

    [Fact]
    public void TopBiteTest1()
    {
        Assert.Equal((0, 0, 3, 3), TopBite(3, 4, 10, 7));
    }

    [Fact]
    public void TopBiteTest2()
    {
        Assert.Equal((3, 4, 5, 7), TopBite(3, 4, 12, 7));
    }

    [Fact]
    public void GreedyPrint()
    {
        var greedy = new SolutionGreedy();
        for (var n = 1; n < 14; n++)
        {
            var buffer = new StringBuilder();
            buffer.Append($"{n}:  ");
            for (var m = 1; m < 14; m++)
                buffer.Append($"{greedy.TilingRectangle(n, m)}, ");

            _output.WriteLine(buffer.ToString());
        }
    }
}