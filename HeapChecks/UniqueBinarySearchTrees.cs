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

using Xunit;

namespace HeapChecks;

public class UniqueBinarySearchTrees
{
    public class Solutionx
    {
        public static int Catalan(int n)
        {
            if (n <= 1) return 1;
            var res = 0;
            for (var i = 0; i < n; i++) res += Catalan(i) * Catalan(n - i - 1);
            return res;
        }

        public int NumTrees(int n)
        {
            var catalan = new int[n + 2];
            catalan[0] = 1;
            catalan[1] = 1;
            for (var i = 2; i <= n; i++)
            for (var j = 0; j < i; j++)
                catalan[i] += catalan[j] * catalan[i - j - 1];

            return catalan[n];
        }
    }

    public class SolutionGood
    {
        public int NumTrees(int n)
        {
            var catalan = 1L;
            for (var i = 0; i < n; ++i)
                catalan = catalan * 2 * (2 * i + 1) / (i + 2);
            return (int)catalan;
        }
    }

    public class Solution
    {
        private static readonly int[] Catalans =
        {
            1, 1, 2, 5, 14, 42, 132, 429, 1430, 4862, 16796, 58786, 208012, 742900,
            2674440, 9694845, 35357670, 129644790, 477638700, 1767263190,
        };

        public int NumTrees(int n) => Catalans[n];
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal(5, solution.NumTrees(3));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal(1, solution.NumTrees(1));
    }

    [Fact]
    public void Test6()
    {
        var solution = new Solution();
        Assert.Equal(132, solution.NumTrees(6));
    }

    [Fact]
    public void Test7()
    {
        var solution = new Solution();
        Assert.Equal(429, solution.NumTrees(7));
    }

    [Fact]
    public void Test19()
    {
        var solution = new Solution();
        Assert.Equal(1767263190, solution.NumTrees(19));
    }

    [Fact]
    public void SetupTest()
    {
        var s1 = new Solution();
        var s2 = new SolutionGood();
        for (var i = 0; i < 20; i++)
            Assert.Equal(s2.NumTrees(i), s1.NumTrees(i));
    }
}