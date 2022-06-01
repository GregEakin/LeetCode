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

public class LargestThreeSameDigitNumberInString
{
    public class Solution
    {
        public string LargestGoodInteger(string num)
        {
            var sum = 10 * (num[0] - '0') + (num[1] - '0');
            var max = -1;
            for (var index = 2; index < num.Length; index++)
            {
                var digit = num[index];
                var val = digit - '0';
                sum = (10 * sum + val) % 1000;
                if (sum % 111 == 0)
                    max = Math.Max(max, sum);
            }

            return max >= 0 ? max.ToString("D3") : string.Empty;
        }
    }

    [Fact]
    public void Answer1()
    {
        var num = "014455";
        var solution = new Solution();
        Assert.Equal("", solution.LargestGoodInteger(num));
    }

    [Fact]
    public void Example1()
    {
        var num = "6777133339";
        var solution = new Solution();
        Assert.Equal("777", solution.LargestGoodInteger(num));
    }

    [Fact]
    public void Example2()
    {
        var num = "2300019";
        var solution = new Solution();
        Assert.Equal("000", solution.LargestGoodInteger(num));
    }

    [Fact]
    public void Example3()
    {
        var num = "42352338";
        var solution = new Solution();
        Assert.Equal(string.Empty, solution.LargestGoodInteger(num));
    }
}