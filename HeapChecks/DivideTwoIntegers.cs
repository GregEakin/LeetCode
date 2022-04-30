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
using System.Collections;
using System.Runtime.CompilerServices;
using Xunit;

namespace HeapChecks;

public class DivideTwoIntegers
{
    public class SolutionDivide
    {
        public int Divide(int dividend, int divisor)
        {
            return dividend / divisor;
        }
    }

    public class SolutionLog
    {
        public int Divide(int dividend, int divisor)
        {
            var negative = (dividend < 0) ^ (divisor < 0);
            var top = dividend < 0 ? 0.0 - dividend : dividend;
            var bottom = divisor < 0 ? 0.0 - divisor : divisor;
            var answer = Math.Pow(10, Math.Log10(top) - Math.Log10(bottom));
            return negative
                ? (int)(0.0 - answer)
                : (int)answer;
        }
    }

    public class SolutionPositive
    {
        public int Divide(int dividend, int divisor)
        {
            var negative = (dividend < 0) ^ (divisor < 0);
            var top = dividend < 0 ? 0 - dividend : dividend;
            var bottom = divisor < 0 ? 0 - divisor : divisor;
            var answer = top / bottom;
            return negative
                ? 0 - answer
                : answer;
        }
    }

    public class SolutionBits
    {
        // -x = ~x + 1 = ~(x - 1)
        // ~x = -x - 1
        // -~x = x + 1
        // ~-x = x - 1

        // number of trailing zeros
        public static int Ntz(uint x)
        {
            x = ~x & (x - 1);
            var n = 0;
            while (x != 0)
            {
                n++;
                x >>= 1;
            }

            return n;
        }

        public static BitArray IntToBits(int value)
        {
            if (value < 0)
                value = 0 - value;
            var bits = new BitArray(new[] { value });
            return bits;
        }

        public static int BitsToInt(bool negative, BitArray value)
        {
            var array = new int[1];
            value.CopyTo(array, 0);
            return array[0];
        }

        public int Divide(int dividend, int divisor)
        {
            var negative = (dividend < 0) ^ (divisor < 0);
            var t = IntToBits(dividend);
            var b = IntToBits(divisor);

            while (!t[0] && !b[0])
            {
                t.RightShift(1);
                b.RightShift(1);
            }

            var ba = BitsToInt(false, b);
            if (ba == 1)
            {
                return BitsToInt(negative, t);
            }

            t.RightShift(2);
            b.LeftShift(2);
            return dividend / divisor;
        }
    }

    public class SolutionHardware
    {
        public int Divide(int dividend, int divisor)
        {
            if (dividend == int.MinValue & divisor == -1) return int.MaxValue;

            var negative = (dividend < 0) ^ (divisor < 0);
            if (dividend > 0)
                dividend = -dividend;
            if (divisor < 0)
                divisor = -divisor;

            var quotient = 0;
            var remainder = 0;
            for (var i = 31; i >= 0; i--)
            {
                quotient <<= 1;
                remainder <<= 1;
                remainder |= (dividend & (1 << i)) >> i;

                if (remainder < divisor) continue;
                remainder -= divisor;
                quotient |= 1;
            }

            return negative ? -quotient : quotient;
        }
    }

    public class Solution
    {
        private const int HalfIntMin = int.MinValue >> 1;

        public int Divide(int dividend, int divisor)
        {
            if (dividend == int.MinValue & divisor == -1) return int.MaxValue;

            var positive = (dividend < 0) ^ (divisor < 0);
            if (dividend > 0)
                dividend = -dividend;
            if (divisor > 0)
                divisor = -divisor;

            var highestDouble = divisor;
            var highestPowerOfTwo = -1;
            while (highestDouble >= HalfIntMin && dividend <= highestDouble + highestDouble)
            {
                highestPowerOfTwo += highestPowerOfTwo;
                highestDouble += highestDouble;
            }

            var quotient = 0;
            while (dividend <= divisor)
            {
                if (dividend <= highestDouble)
                {
                    quotient += highestPowerOfTwo;
                    dividend -= highestDouble;
                }

                highestPowerOfTwo >>= 1;
                highestDouble >>= 1;
            }

            return positive ? quotient : -quotient;
        }
    }

    [Fact]
    public void Answer1()
    {
        var solution = new Solution();
        Assert.Equal(-2147483648, solution.Divide(-2147483648, 1));
    }

    [Fact]
    public void Test1()
    {
        Assert.Equal(-1073741824, int.MinValue >> 1);

        var solution = new Solution();
        Assert.Equal(-2, solution.Divide(-8, 4));
    }


    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal(3, solution.Divide(10, 3));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal(-2, solution.Divide(7, -3));
    }

    [Fact]
    public void OverflowTest()
    {
        var solution = new Solution();
        Assert.Equal(int.MaxValue, solution.Divide(int.MinValue, -1));
    }

    [Fact]
    public void MaxIntTest()
    {
        var solution = new Solution();
        Assert.Equal(-1, solution.Divide(int.MinValue, int.MaxValue));
        Assert.Equal(0, solution.Divide(0, int.MaxValue));
        Assert.Equal(0, solution.Divide(1, int.MaxValue));
        Assert.Equal(1, solution.Divide(int.MaxValue, int.MaxValue));
        Assert.Equal(int.MaxValue - 1, solution.Divide(int.MaxValue - 1, 1));
        Assert.Equal(int.MaxValue, solution.Divide(int.MaxValue, 1));
    }

    [Fact]
    public void MinIntTest()
    {
        var solution = new Solution();
        Assert.Equal(0, solution.Divide(int.MaxValue, int.MinValue));
        Assert.Equal(0, solution.Divide(0, int.MinValue));
        Assert.Equal(0, solution.Divide(1, int.MinValue));
        Assert.Equal(1, solution.Divide(int.MinValue, int.MinValue));
        Assert.Equal(int.MinValue, solution.Divide(int.MinValue, 1));
    }

    [Fact]
    public void MinInt2Test()
    {
        var solution = new Solution();
        Assert.Equal(-1, solution.Divide(int.MaxValue - 1, int.MinValue + 2));
        Assert.Equal(0, solution.Divide(0, int.MinValue + 2));
        Assert.Equal(0, solution.Divide(1, int.MinValue + 2));
        Assert.Equal(1, solution.Divide(int.MinValue + 2, int.MinValue + 2));
        Assert.Equal(int.MinValue + 2, solution.Divide(int.MinValue + 2, 1));
    }
}