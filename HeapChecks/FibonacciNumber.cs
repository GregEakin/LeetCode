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

public class FibonacciNumber
{
    public class Solution
    {
        public int Fib(int n)
        {
            if (n <= 1) return n;

            var cache = new int[n + 1];
            cache[1] = 1;
            for (var i = 2; i <= n; i++)
                cache[i] = cache[i - 1] + cache[i - 2];

            return cache[n];
        }
    }

    [Fact]
    public void Example1()
    {
        var n = 2;
        var solution = new Solution();
        Assert.Equal(1, solution.Fib(n));
    }

    [Fact]
    public void Example2()
    {
        var n = 3;
        var solution = new Solution();
        Assert.Equal(2, solution.Fib(n));
    }

    [Fact]
    public void Example3()
    {
        var n = 4;
        var solution = new Solution();
        Assert.Equal(3, solution.Fib(n));
    }
}