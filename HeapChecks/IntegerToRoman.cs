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

using System.Text;
using Xunit;

namespace HeapChecks;

public class IntegerToRoman
{
    public class Solution
    {
        public static readonly (char symbol, int value, char four, int subtraction)[] Symobls =
        {
            ('M', 1000, 'C', 100),
            ('D', 500, 'C', 100),
            ('C', 100, 'X', 10),
            ('L', 50, 'X', 10),
            ('X', 10, 'I', 1),
            ('V', 5, 'I', 1),
            ('I', 1, ' ', 0),
        };

        public string IntToRoman(int num)
        {
            var answer = new StringBuilder();
            foreach (var (symbol, value, four, subtraction) in Symobls)
            {
                while (num - value >= 0)
                {
                    answer.Append(symbol);
                    num -= value;
                }

                if (num - (value - subtraction) < 0) 
                    continue;

                answer.Append(four);
                answer.Append(symbol);
                num -= value - subtraction;
            }

            return answer.ToString();
        }
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal("III", solution.IntToRoman(3));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal("LVIII", solution.IntToRoman(58));
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.Equal("MCMXCIV", solution.IntToRoman(1994));
    }
}