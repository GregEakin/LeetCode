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

public class Pow
{
    public class SolutionPower
    {
        public double MyPow(double x, int n)
        {
            return Math.Pow(x, n);
        }
    }

    public class Solution
    {
        public static double FastPow(double x, long n)
        {
            if (n == 0)
                return 1.0;

            var half = FastPow(x, n / 2);
            return n % 2 == 0
                ? half * half
                : half * half * x;
        }

        public double MyPow(double x, int n)
        {
            var count = (long)n;
            return count >= 0
                ? FastPow(x, count)
                : FastPow(1 / x, -count);
        }
    }

    [Fact]
    public void Answer1()
    {
        var solution = new Solution();
        Assert.Equal(1.0, solution.MyPow(1.0, 2147483647));
    }

    [Fact]
    public void Answer2()
    {
        var solution = new Solution();
        Assert.Equal(0.0, solution.MyPow(2.0, -2147483648));
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal(1024.0, solution.MyPow(2.0, 10));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal(9.261, solution.MyPow(2.1, 3), 5);
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.Equal(0.25, solution.MyPow(2.0, -2));
    }
}