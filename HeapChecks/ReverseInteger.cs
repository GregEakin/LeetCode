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

public class ReverseInteger
{
    public class Solution
    {
        public static int Reverse(int x)
        {
            if (x == int.MinValue) return 0;
            var array = Math.Abs(x).ToString().ToCharArray();
            Array.Reverse(array);
            var parsed = int.TryParse(array, out var value);
            return parsed ? x < 0 ? -value : value : 0;
        }
    }

    [Fact]
    public void Example1()
    {
        var x = 123;
        Assert.Equal(321, Solution.Reverse(x));
    }

    [Fact]
    public void Example2()
    {
        var x = -123;
        Assert.Equal(-321, Solution.Reverse(x));
    }

    [Fact]
    public void Example3()
    {
        var x = 120;
        Assert.Equal(21, Solution.Reverse(x));
    }

    [Fact]
    public void Answer1()
    {
        var x = int.MaxValue;
        Assert.Equal(0, Solution.Reverse(x));
    }

    [Fact]
    public void Answer2()
    {
        var x = -int.MaxValue;
        Assert.Equal(0, Solution.Reverse(x));
    }

    [Fact]
    public void Answer3()
    {
        var x = int.MinValue;
        Assert.Equal(0, Solution.Reverse(x));
    }

    [Fact]
    public void Answer4()
    {
        var x = -(int.MinValue + 1);
        Assert.Equal(0, Solution.Reverse(x));
    }
}