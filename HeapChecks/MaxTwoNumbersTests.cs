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

public class MaxTwoNumbersTests
{
    public static int Flip(int bit) => 0x01 ^ bit;
    public static int Sign(int a) => Flip((a >> 31) & 0x01);

    public static int GetMaxNaive(int a, int b)
    {
        var k = Sign(a - b);
        var q = Flip(k);
        return a * k + b * q;
    }

    [Fact]
    public void GetMaxNaiveTest()
    {
        Assert.Equal(5, GetMaxNaive(3, 5));
    }

    [Fact]
    public void GetMaxNaiveOverflowTest()
    {
        Assert.Equal(-15, GetMaxNaive(int.MaxValue - 2, -15));
    }

    public static int GetMax(int a, int b)
    {
        var sa = Sign(a);
        var sb = Sign(b);

        var useSa = sa ^ sb;
        var useSc = Flip(useSa);
        var k = useSa * sa + useSc * Sign(a - b);

        return a * k + b * Flip(k);
    }

    [Fact]
    public void FlipTest()
    {
        Assert.Equal(1, Flip(0));
        Assert.Equal(0, Flip(1));
        Assert.Equal(3, Flip(2));
        Assert.Equal(2, Flip(3));
        Assert.Equal(5, Flip(4));
        Assert.Equal(4, Flip(5));
    }

    [Fact]
    public void GetMaxTest()
    {
        Assert.Equal(int.MaxValue - 2, GetMax(int.MaxValue - 2, -15));
    }
}