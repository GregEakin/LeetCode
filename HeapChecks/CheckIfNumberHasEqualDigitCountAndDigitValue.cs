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

public class CheckIfNumberHasEqualDigitCountAndDigitValue
{
    public class Solution
    {
        public bool DigitCount(string num)
        {
            var digits = new int[10];
            foreach (var c in num) 
                digits[c - '0']++;

            for (var i = 0; i < num.Length; i++)
                if (num[i] - '0' != digits[i])
                    return false;

            return true;
        }
    }

    [Fact]
    public void Example1()
    {
        var num = "1210";
        var solution = new Solution();
        Assert.True(solution.DigitCount(num));
    }

    [Fact]
    public void Example2()
    {
        var num = "030";
        var solution = new Solution();
        Assert.False(solution.DigitCount(num));
    }
}