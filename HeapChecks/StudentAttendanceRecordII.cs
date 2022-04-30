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

public class StudentAttendanceRecordII
{
    public class SolutionBruteForce
    {
        private const int Max = 1000000007;
        private int _count;

        public bool CheckRecord(string s)
        {
            var count = 0;
            for (var i = 0; i < s.Length && count < 2; i++)
                if (s[i] == 'A')
                    count++;
            return s.Length > 0 && count < 2 && s.IndexOf("LLL", StringComparison.Ordinal) < 0;
        }

        public void Gen(string s, int n)
        {
            switch (n)
            {
                case 0 when CheckRecord(s):
                    _count = (_count + 1) % Max;
                    break;
                case > 0:
                    Gen(s + 'A', n - 1);
                    Gen(s + 'P', n - 1);
                    Gen(s + 'L', n - 1);
                    break;
            }
        }

        public int CheckRecord(int n)
        {
            Gen("", n);
            return _count;
        }
    }

    public class SolutionRecursive
    {
        private const int Max = 1000000007;

        public int Func(int n)
        {
            return n switch
            {
                0 => 1,
                1 => 2,
                2 => 4,
                3 => 7,
                _ => (2 * Func(n - 1) - Func(n - 4)) % Max
            };
        }

        public int CheckRecord(int n)
        {
            var f = new int[n + 1];
            f[0] = 1;
            for (var i = 1; i <= n; i++)
                f[i] = Func(i);
            var sum = Func(n);
            for (var i = 1; i <= n; i++)
                sum += f[i - 1] * f[n - i] % Max;
            return sum % Max;
        }
    }

    public class SolutionDynamicProgramming
    {
        private const long Max = 1000000007L;

        public int CheckRecord(int n)
        {
            var f = new long[n <= 5 ? 6 : n + 1];
            f[0] = 1;
            f[1] = 2;
            f[2] = 4;
            f[3] = 7;
            for (var i = 4; i <= n; i++)
                f[i] = (2L * f[i - 1] % Max + (Max - f[i - 4])) % Max;

            var sum = f[n];
            for (var i = 1; i <= n; i++)
                sum += f[i - 1] * f[n - i] % Max;

            return (int)(sum % Max);
        }
    }

    // Calculator
    public class Solution
    {
        private const long Max = 1000000007L;

        public int CheckRecord(int n)
        {
            var a0L0 = 1L;
            var a0L1 = 0L;
            var a0L2 = 0L;
            var a1L0 = 0L;
            var a1L1 = 0L;
            var a1L2 = 0L;
            for (var i = 0; i < n; i++)
            {
                var a0L0New = (a0L0 + a0L1 + a0L2) % Max;
                var a0L1New = a0L0;
                var a0L2New = a0L1;
                var a1L0New = (a0L0 + a0L1 + a0L2 + a1L0 + a1L1 + a1L2) % Max;
                var a1L1New = a1L0;
                var a1L2New = a1L1;
                a0L0 = a0L0New;
                a0L1 = a0L1New;
                a0L2 = a0L2New;
                a1L0 = a1L0New;
                a1L1 = a1L1New;
                a1L2 = a1L2New;
            }

            return (int)((a0L0 + a0L1 + a0L2 + a1L0 + a1L1 + a1L2) % Max);
        }
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal(8, solution.CheckRecord(2));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal(3, solution.CheckRecord(1));
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.Equal(183236316, solution.CheckRecord(10101));
    }

    [Fact]
    public void Test1()
    {
        var solution = new Solution();
        Assert.Equal(19, solution.CheckRecord(3));
    }

    [Fact]
    public void Test2()
    {
        var solution = new Solution();
        Assert.Equal(43, solution.CheckRecord(4));
    }
}