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
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HeapChecks;

public class SumOfTotalStrengthOfWizards
{
    public class Solution
    {
        public int TotalStrength(int[] strength)
        {
            var sum = 0L;
            for (var i = 0; i < strength.Length; i++)
            {
                var ss = 0L;
                var min = long.MaxValue;
                for (var j = i; j < strength.Length; j++)
                {
                    min = Math.Min(min, (long)strength[j]);
                    ss += (long)strength[j];
                    sum += (min * ss) % 1000000007L;
                }
            }

            return (int)sum;
        }
    }

    public class SolutionWierd
    {
        public int TotalStrength(int[] strength)
        {
            const long MOD = 1000000007L;
            var n = strength.Length;
            var pw = new List<long>();
            var ps = new List<long>();
            for (var i = 0L; i < n; ++i)
            {
                pw.Add((pw[^1] + (i + 1) * strength[i]) % MOD);
                ps.Add((ps[^1] + strength[i]) % MOD);
            }

            var sw = new List<long>();
            var ss = new List<long>();
            for (var i = n - 1; i >= 0L; --i)
            {
                sw.Add((sw[^1] + (n - i) * strength[i]) % MOD);
                ss.Add((ss[^1] + strength[i]) % MOD);
            }

            // reverse(sw.begin(), sw.end());
            // reverse(ss.begin(), ss.end());

            var ans = 0L;
            var stk = new Stack<int>();
            //strength.Add(0);
            for (var i = 0; i < strength.Length; ++i)
            {
                while (stk.Count > 0 && strength[^1] >= strength[i])
                {
                    var mid = stk.Peek();
                    stk.Pop();
                    var lo = -1;
                    if (stk.Count > 0)  lo = stk.Peek();
                    var left = (long)strength[mid] * (pw[mid + 1] - pw[lo + 1] - (ps[mid + 1] - ps[lo + 1]) * (lo + 1) % MOD) % MOD * (i - mid) % MOD;
                    var right = (long)strength[mid] * (sw[mid + 1] - sw[i] - (ss[mid + 1] - ss[i]) * (n - i) % MOD) % MOD * (mid - lo) % MOD;
                    ans = (ans + left + right) % MOD;
                }

                stk.Push(i);
            }

            return (int)ans;
        }
    }

    [Fact]
    public void Example1()
    {
        var strength = new[] { 1, 3, 1, 2 };
        var solution = new Solution();
        Assert.Equal(44, solution.TotalStrength(strength));
    }

    [Fact]
    public void Example2()
    {
        var strength = new[] { 5, 4, 6 };
        var solution = new Solution();
        Assert.Equal(213, solution.TotalStrength(strength));
    }
}