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
using Xunit;

namespace HeapChecks;

public class BulbSwitcher
{
    public class Solution
    {
        private static readonly List<int> Primes = new();

        static Solution()
        {
            var ok = new bool[32000];
            for (var i = 2; i < 32000; i++)
            {
                if (ok[i]) continue;
                Primes.Add(i);
                for (var j = i; j < 32000; j += i)
                    ok[j] = true;
            }
        }

        public static int FactorCount(int n)
        {
            var step = n;
            var factors = 1;
            for (var i = 0; Primes[i] * Primes[i] <= n; i++)
            {
                var power = 0;
                while (step % Primes[i] == 0)
                {
                    step /= Primes[i];
                    ++power;
                }

                factors *= power + 1;
            }

            if (step > 1)
                factors *= 2;

            return factors;
        }

        public int BulbSwitchSlow(int n)
        {
            var sum = 0;
            for (var i = 1; i <= n; i++)
                sum += FactorCount(i) % 2;
            return sum;
        }

        public int BulbSwitch2(int n)
        {
            var sum = 0;
            for (var i = 1; i * i <= n; i++)
                sum++;
            return sum;
        }

        public int BulbSwitch(int n)
        {
            return (int)Math.Sqrt(n);
        }
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal(1, solution.BulbSwitch(3));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal(0, solution.BulbSwitch(0));
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.Equal(1, solution.BulbSwitch(1));
    }

    [Fact]
    public void Test1()
    {
        var solution = new Solution();
        Assert.Equal(2, solution.BulbSwitch(8));
    }

    [Fact]
    public void FactorCountTest()
    {
        Assert.Equal(1, Solution.FactorCount(1));
        Assert.Equal(2, Solution.FactorCount(2));
        Assert.Equal(2, Solution.FactorCount(3));
        Assert.Equal(3, Solution.FactorCount(4));
        Assert.Equal(2, Solution.FactorCount(5));
        Assert.Equal(4, Solution.FactorCount(6));
        Assert.Equal(2, Solution.FactorCount(7));
        Assert.Equal(4, Solution.FactorCount(8));
    }

    [Fact]
    public void Answer1()
    {
        var solution = new Solution();
        Assert.Equal(3162, solution.BulbSwitch(9999999));
    }
}