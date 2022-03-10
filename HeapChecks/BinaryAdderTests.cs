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
using System.Text;
using Xunit;

namespace HeapChecks;

public class BinaryAdderTests
{
    public (char, char) FullAdder(char x, char y, char z)
    {
        var pair = FullAdder(x == '1', y == '1', z == '1');
        return (pair.Item1 ? '1' : '0', pair.Item2 ? '1' : '0');
    }

    public (bool, bool) FullAdder(bool x, bool y, bool z)
    {
        var sum = x ^ y ^ z;
        var carry = (x & z) | (y & z) | (x & y);
        return (carry, sum);
    }

    public int GetSum(int a, int b)
    {
        var aBits = Convert.ToString(a, 2).PadLeft(32, '0');
        var bBits = Convert.ToString(b, 2).PadLeft(32, '0');
        var result = new StringBuilder();

        var carry = '0';
        for (var i = 31; i >= 0; i--)
        {
            var aBit = aBits[i];
            var bBit = bBits[i];
            var sum = '0';
            (carry, sum) = FullAdder(aBit, bBit, carry);
            result.Insert(0, sum);
        }

        var total = Convert.ToInt32(result.ToString(), 2);
        return total;
    }

    [Fact]
    public void BinaryTests()
    {
        var a1 = Convert.ToString(1000, 2).PadLeft(32, '0');
        var a2 = Convert.ToInt32(a1, 2);
        Assert.Equal(1000, a2);

        var b1 = Convert.ToString(-1000, 2).PadLeft(32, '0');
        var b2 = Convert.ToInt32(b1, 2);
        Assert.Equal(-1000, b2);
    }

    [Fact]
    public void FullAdderTest()
    {
        Assert.Equal((false, false), FullAdder(false, false, false));
        Assert.Equal((false, true), FullAdder(false, false, true));
        Assert.Equal((false, true), FullAdder(false, true, false));
        Assert.Equal((true, false), FullAdder(false, true, true));
        Assert.Equal((false, true), FullAdder(true, false, false));
        Assert.Equal((true, false), FullAdder(true, false, true));
        Assert.Equal((true, false), FullAdder(true, true, false));
        Assert.Equal((true, true), FullAdder(true, true, true));
    }

    [Fact]
    public void NegTest()
    {
        Assert.Equal(3, GetSum(-1000, 1003));
    }

    [Fact]
    public void MinTest()
    {
        //  0123456789012345

        // "1111110000011000"
        //  {111100000110000}

        Assert.Equal(-2000, GetSum(-1000, -1000));
    }

    [Fact]
    public void SumTwoIntegers1()
    {
        Assert.Equal(3, GetSum(1, 2));
        Assert.Equal(5, GetSum(2, 3));
        Assert.Equal(1000, GetSum(500, 500));
        Assert.Equal(-2000, GetSum(-1000, -1000));
        Assert.Equal(0, GetSum(-5, 5));
        Assert.Equal(0, GetSum(5, -5));
    }
}