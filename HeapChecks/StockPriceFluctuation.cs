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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class StockPriceFluctuation
{
    public class StockPrice
    {
        private readonly SortedList<int, int> _timestamps = new();
        private readonly SortedList<int, int> _prices = new();

        public StockPrice()
        {
        }

        public void Update(int timestamp, int price)
        {
            var found = _timestamps.TryGetValue(timestamp, out var oldPrice);
            if (found)
            {
                if (_prices[oldPrice] <= 1)
                    _prices.Remove(oldPrice);
                else
                    _prices[oldPrice]--;
            }

            _timestamps[timestamp] = price;
            var found2 = _prices.TryGetValue(price, out var count);
            if (!found2) _prices.Add(price, 1);
            else _prices[price]++;
        }

        public int Current() => _timestamps.Values[^1];
        public int Maximum() => _prices.Keys[^1];
        public int Minimum() => _prices.Keys[0];
    }

    /**
     * Your StockPrice object will be instantiated and called as such:
     * StockPrice obj = new StockPrice();
     * obj.Update(timestamp,price);
     * int param_2 = obj.Current();
     * int param_3 = obj.Maximum();
     * int param_4 = obj.Minimum();
     */
    [Fact]
    public void Example1()
    {
        var actions = new[]
            { "StockPrice", "update", "update", "current", "maximum", "update", "maximum", "update", "minimum" };
        var values = new List<int>?[]
            { null, new() { 1, 10 }, new() { 2, 5 }, null, null, new() { 1, 3 }, null, new() { 4, 2 }, null };
        var output = new int?[] { null, null, null, 5, 10, null, 5, null, 2 };

        StockPrice? stock = null;
        for (var i = 0; i < actions.Length; i++)
        {
            switch (actions[i])
            {
                case "StockPrice":
                    stock = new StockPrice();
                    break;
                case "update":
                    stock!.Update(values[i]![0], values[i]![1]);
                    break;
                case "current":
                    Assert.Equal(output[i]!, stock!.Current());
                    break;
                case "maximum":
                    Assert.Equal(output[i]!, stock!.Maximum());
                    break;
                case "minimum":
                    Assert.Equal(output[i]!, stock!.Minimum());
                    break;
            }
        }
    }

    [Fact]
    public void Test1()
    {
        var stock = new StockPrice();
        stock.Update(0, 100);
        stock.Update(1, 100);
        stock.Update(0, 100);
        stock.Update(2, 110);
        stock.Update(0, 150);

        Assert.Equal(100, stock.Minimum());
        Assert.Equal(150, stock.Maximum());
        Assert.Equal(110, stock.Current());
    }
}