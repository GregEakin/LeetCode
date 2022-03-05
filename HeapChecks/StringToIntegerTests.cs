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

public class StringToIntegerTests
{
    public class Solution
    {
        public static int MyAtoi(string s)
        {
            var start = 0;
            var sign = 1;
            while (start < s.Length && s[start] == ' ')
                start++;

            if (start < s.Length && s[start] == '+')
                start++;
            else if (start < s.Length && s[start] == '-')
            {
                start++;
                sign = -1;
            }

            var length = 0;
            while (start + length < s.Length && s[start + length] >= '0' && s[start + length] <= '9')
            {
                length++;
            }

            if (length <= 0)
                return 0;

            var sum = 0L;
            for (var i = 0; i < length; i++)
            {
                sum *= 10;
                sum += s[start + i] - '0';

                if (sign == -1 && sign * sum < int.MinValue)
                    return int.MinValue;

                if (sign == 1 && sum > int.MaxValue)
                    return int.MaxValue;
            }

            return (int)(sign * sum);
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "42";
        Assert.Equal(42, Solution.MyAtoi(s));
    }

    [Fact]
    public void Example2()
    {
        var s = "   -42";
        Assert.Equal(-42, Solution.MyAtoi(s));
    }

    [Fact]
    public void Example2B()
    {
        var s = "+42";
        Assert.Equal(42, Solution.MyAtoi(s));
    }

    [Fact]
    public void Example2C()
    {
        var s = "+ 42";
        Assert.Equal(0, Solution.MyAtoi(s));
    }

    [Fact]
    public void Example3()
    {
        var s = "4193 with words";
        Assert.Equal(4193, Solution.MyAtoi(s));
    }

    [Fact]
    public void Test1()
    {
        var s = "Words 42";
        Assert.Equal(0, Solution.MyAtoi(s));
    }

    [Fact]
    public void Test2()
    {
        var s = "  42  ";
        Assert.Equal(42, Solution.MyAtoi(s));
    }

    [Fact]
    public void Test3()
    {
        var s = "  1.99999  ";
        Assert.Equal(1, Solution.MyAtoi(s));
    }

    [Fact]
    public void Test4()
    {
        var s = "-9999999999";
        Assert.Equal(int.MinValue, Solution.MyAtoi(s));
    }

    [Fact]
    public void Test5()
    {
        var s = "9999999999";
        Assert.Equal(int.MaxValue, Solution.MyAtoi(s));
    }

    [Fact]
    public void Test6()
    {
        var s = "  a";
        Assert.Equal(0, Solution.MyAtoi(s));
    }

    [Fact]
    public void Answer1()
    {
        var s = "-2147483648";
        Assert.Equal(-2147483648, Solution.MyAtoi(s));
    }
}