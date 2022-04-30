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

public class PrimeFactor
{
    public class Solution2
    {
        private const int MAXN = 101;
        private static readonly int[] spf = new int[MAXN];

        static Solution2()
        {
            for (var i = 1; i < MAXN; i++)
                spf[i] = i;

            for (var i = 4; i < MAXN; i += 2)
                spf[i] = 2;

            for (var i = 3; i * i < MAXN; i++)
            {
                if (spf[i] != i) continue;
                for (var j = i * i; j < MAXN; j += i)
                    if (spf[j] == j)
                        spf[j] = i;
            }
        }

        public static int PrimFactorizationCount(int x)
        {
            var count = 0;
            while (x > 1)
            {
                count++;
                x /= spf[x];
            }

            return count;
        }
    }

    [Fact]
    public void FactorCountTest2()
    {
        Assert.Equal(0, Solution2.PrimFactorizationCount(1));
        Assert.Equal(1, Solution2.PrimFactorizationCount(2));
        Assert.Equal(1, Solution2.PrimFactorizationCount(3));
        Assert.Equal(2, Solution2.PrimFactorizationCount(4));
        Assert.Equal(1, Solution2.PrimFactorizationCount(5));
        Assert.Equal(2, Solution2.PrimFactorizationCount(6));
        Assert.Equal(1, Solution2.PrimFactorizationCount(7));
        Assert.Equal(3, Solution2.PrimFactorizationCount(8));
    }

    [Fact]
    public void BigTest()
    {
        // Assert.Equal(3162, Solution2.PrimFactorizationCount(9999999));
    }
}