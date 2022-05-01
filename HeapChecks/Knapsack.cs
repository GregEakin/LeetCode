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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class Knapsack
{
    static int KnapSack1(int W, (int wt, int val)[] items, int n)
    {
        if (n == 0 || W == 0)
            return 0;

        return items[n - 1].wt > W
            ? KnapSack1(W, items, n - 1)
            : Math.Max(items[n - 1].val + KnapSack1(W - items[n - 1].wt, items, n - 1), KnapSack1(W, items, n - 1));
    }

    [Fact]
    public void GeekTest1()
    {
        var items = new[] { (1, 10), (1, 20), (1, 30) };
        Assert.Equal(50, KnapSack1(2, items, items.Length));
    }

    [Fact]
    public void GeekTest2()
    {
        var items = new[] { (10, 60), (30, 120), (20, 100) };
        Assert.Equal(220, KnapSack1(50, items, items.Length));
    }

    static int KnapSack2(int W, (int wt, int val)[] items, int n)
    {
        var k = new int[n + 1, W + 1];

        for (var i = 1; i <= n; i++)
        for (var w = 1; w <= W; w++)
        {
            if (items[i - 1].wt <= w)
                k[i, w] = Math.Max(items[i - 1].val + k[i - 1, w - items[i - 1].wt], k[i - 1, w]);
            else
                k[i, w] = k[i - 1, w];
        }

        return k[n, W];
    }

    [Fact]
    public void GeekTest3()
    {
        var items = new[] { (1, 10), (2, 15), (3, 40) };
        Assert.Equal(65, KnapSack2(6, items, items.Length));
    }

    [Fact]
    public void GeekTest4()
    {
        var items = new[] { (1, 10), (2, 15), (3, 40) };
        Assert.Equal(55, KnapSack2(5, items, items.Length));
    }

    [Fact]
    public void GeekTest4B()
    {
        var items = new[] { (1, 10), (2, 15), (1, 5), (3, 40), (2, 9) };
        Assert.Equal(55, KnapSack2(5, items, items.Length));
    }

    public static int GCD(int num1, int num2)
    {
        while (num2 != 0)
        {
            var remainder = num1 % num2;
            num1 = num2;
            num2 = remainder;
        }

        return num1;
    }

    public static int BinaryKnapsackGCD(int w, (int wt, int val)[] items)
    {
        var gcd = w;
        for (var i = 0; i < items.Length; i++) gcd = GCD(items[i].wt, gcd);

        var n = items.Length;
        var m = w / gcd;
        var c = new int[n + 1, m + 1];

        for (var i = 1; i <= n; i++)
        for (var j = 1; j <= m; j++)
        {
            if (items[i - 1].wt / gcd <= j)
            {
                var val1 = items[i - 1].val + c[i - 1, j - items[i - 1].wt / gcd];
                c[i, j] = Math.Max(val1, c[i - 1, j]);
            }
            else
                c[i, j] = c[i - 1, j];
        }

        return c[n, m];
    }

    public static int BinaryKnapsack(int w, (int wt, int val)[] items)
    {
        var n = items.Length;
        var m = w;
        var c = new int[n + 1, m + 1];

        for (var i = 1; i <= n; i++)
        for (var j = 1; j <= m; j++)
        {
            if (items[i - 1].wt <= j)
            {
                var val1 = items[i - 1].val + c[i - 1, j - items[i - 1].wt];
                c[i, j] = Math.Max(val1, c[i - 1, j]);
            }
            else
                c[i, j] = c[i - 1, j];
        }

        return c[n, m];
    }

    [Fact]
    public void Knapsack1()
    {
        var items = new[] { (2, 3), (3, 5), (4, 6) };
        Assert.Equal(14, BinaryKnapsack(9, items));
    }

    public static int BinaryKnapsackPowTwo(int t, (int wt, int val)[] items)
    {
        var max = int.MinValue;
        var n = 1 << items.Length;
        for (var i = 0; i < n; i++)
        {
            var wt = 0;
            var val = 0;
            for (var j = 0; j < items.Length; j++)
            {
                if ((i & (0x01 << j)) == 0x00) continue;
                wt += items[j].wt;
                val += items[j].val;
            }

            if (wt <= t && val > max)
                max = val;
        }

        return max;
    }

    [Fact]
    public void KnapsackPowTwo1()
    {
        var items = new[] { (2, 3), (3, 5), (4, 6) };
        Assert.Equal(14, BinaryKnapsackPowTwo(9, items));
    }

    // public static int BinaryKnapsack(int w, (int wt, int val)[] items)
    // {
    //     var gcd = w;
    //     for (var i = 0; i < items.Length; i++) gcd = GCD(items[i].wt, gcd);
    //
    //     var n = items.Length;
    //     var m = w / gcd;
    //     var c = new int[n + 1, m + 1];
    //
    //     for (var i = 1; i <= n; i++)
    //     for (var j = 1; j <= m; j++)
    //     {
    //         if (items[i - 1].wt / gcd <= j)
    //             c[i, j] = Math.Max(items[i - 1].val + c[i - 1, j - items[i - 1].wt / gcd], c[i - 1, j]);
    //         else
    //             c[i, j] = c[i - 1, j];
    //     }
    //
    //     return c[n, m];
    // }

    [Fact]
    public void GeekTest5()
    {
        var items = new[] { (1, 10), (2, 15), (3, 40) };
        Assert.Equal(65, BinaryKnapsack(6, items));
    }

    [Fact]
    public void GeekTest6()
    {
        var items = new[] { (10, 10), (20, 15), (30, 40) };
        Assert.Equal(55, BinaryKnapsack(50, items));
    }

    [Fact]
    public void GeekTest7()
    {
        var items = new[] { (5, 1), (10, 10), (20, 15), (30, 40), (5, 1) };
        Assert.Equal(55, BinaryKnapsack(50, items));
    }

    [Fact]
    public void GeekTest8()
    {
        var items = Array.Empty<(int, int)>();
        Assert.Equal(0, BinaryKnapsack(2, items));
    }

    public static IEnumerable<(int, int)> FractionalKnapsack(int pounds, (int pounds, int dollars)[] items)
    {
        var a = new List<(int, int)>();
        var stuff = new List<(int index, float dollarsPerPound)>();
        for (var i = 0; i < items.Length; i++) stuff.Add((i, (float)items[i].dollars / items[i].pounds));
        foreach (var (index, _) in stuff.OrderByDescending(s => s.dollarsPerPound))
        {
            if (pounds <= 0) break;

            if (items[index].pounds < pounds)
            {
                a.Add((index, items[index].pounds));
                pounds -= items[index].pounds;
            }
            else
            {
                a.Add((index, pounds));
                pounds = 0;
            }
        }

        return a;
    }

    [Fact]
    public void Test1()
    {
        var items = new[] { (10, 60), (30, 120), (20, 100) };
        // var binaryKnapsack = Knapsack.BinaryKnapsack(50, items).ToArray();
        // Assert.Equal(new[] { 1, 2 }, binaryKnapsack);
        // var weight = binaryKnapsack.Sum(i => items[i].Item1);
        // Assert.Equal(50, weight);
        // var profit = binaryKnapsack.Sum(i => items[i].Item2);
        // Assert.Equal(220, profit);

        var binaryKnapsack = Knapsack.BinaryKnapsack(50, items);
        Assert.Equal(220, binaryKnapsack);
    }

    [Fact]
    public void Test2()
    {
        var items = new[] { (10, 60), (30, 120), (20, 100) };
        var fractionalKnapsack = Knapsack.FractionalKnapsack(50, items).ToArray();
        Assert.Equal(new[] { (0, 10), (2, 20), (1, 20) }, fractionalKnapsack);
        var weight = fractionalKnapsack.Sum(i => i.Item2);
        Assert.Equal(50, weight);
        var profit = fractionalKnapsack.Sum(i => (double)items[i.Item1].Item2 * i.Item2 / items[i.Item1].Item1);
        Assert.Equal(240.0, profit);
    }
}