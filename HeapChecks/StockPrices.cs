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

public class StockPrices
{
    public static class Solution
    {
        public static int GetMaxProfit(int[] prices)
        {
            if (prices.Length < 2) return 0;
            var min = prices[0];
            var maxProfit = prices[1] - prices[0];

            for (var i = 1; i < prices.Length; i++)
            {
                maxProfit = Math.Max(maxProfit, prices[i] - min);
                min = Math.Min(min, prices[i]);
            }

            return maxProfit;
        }
    }

    [Fact]
    public void Example1()
    {
        var prices = new[] { 10, 7, 5, 8, 11, 9 };
        Assert.Equal(6, Solution.GetMaxProfit(prices));
    }

    [Fact]
    public void Example2()
    {
        var prices = new[] { 10, 8, 6, 5, 3, 1 };
        Assert.Equal(-1, Solution.GetMaxProfit(prices));
    }

    [Fact]
    public void Test1()
    {
        var prices = new[] { 10, 9, 11, 5, 7, 6, 10 };
        Assert.Equal(5, Solution.GetMaxProfit(prices));
    }
}