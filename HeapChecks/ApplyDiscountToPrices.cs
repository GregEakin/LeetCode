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

public class ApplyDiscountToPrices
{
    public class Solution
    {
        public string DiscountPrices(string sentence, int discount)
        {
            var words = sentence.Split(' ');
            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];
                if (word[0] != '$') continue;

                var number = word.Substring(1);
                if (!double.TryParse(number, out var value)) continue;

                var percent = value * (1.0 - discount / 100.0);
                words[i] = $"${percent:F2}";
            }

            return string.Join(' ', words);
        }
    }

    [Fact]
    public void Example1()
    {
        var sentence = "there are $1 $2 and 5$ candies in the shop";
        var discount = 50;
        var solution = new Solution();
        Assert.Equal("there are $0.50 $1.00 and 5$ candies in the shop", solution.DiscountPrices(sentence, discount));
    }

    [Fact]
    public void Example2()
    {
        var sentence = "1 2 $3 4 $5 $6 7 8$ $9 $10$";
        var discount = 100;
        var solution = new Solution();
        Assert.Equal("1 2 $0.00 4 $0.00 $0.00 7 8$ $0.00 $10$", solution.DiscountPrices(sentence, discount));
    }

    [Fact]
    public void Test1()
    {
        var sentence = "1 2 $3 4 $5 $6 7 8$ $9 $10$";
        var discount = 0;
        var solution = new Solution();
        Assert.Equal("1 2 $3.00 4 $5.00 $6.00 7 8$ $9.00 $10$", solution.DiscountPrices(sentence, discount));
    }
}