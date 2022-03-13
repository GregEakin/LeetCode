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

public class MinimumWindowSubstring
{
    public class Solution
    {
        public string MinWindow(string s, string t)
        {
            var m = s.Length;
            var n = t.Length;
            if (m < n) return string.Empty;

            var counts = new int[0x40];
            foreach (var t1 in t)
                counts[t1 - 0x40]++;

            var need = 0;
            foreach (var c1 in counts)
                if (c1 > 0)
                    need++;

            if (need <= 0) return string.Empty;

            (int Start, int Length) result = (0, int.MaxValue); // max is 100,000
            var current = new int[0x40];
            var count = 0;
            var left = 0;
            for (var right = 0; right < m; right++)
            {
                current[s[right] - 0x40]++;
                if (current[s[right] - 0x40] == counts[s[right] - 0x40])
                    count++;

                while (left <= right && current[s[left] - 0x40] > counts[s[left] - 0x40])
                {
                    current[s[left] - 0x40]--;
                    left++;
                }

                if (count == need && right - left + 1 < result.Length)
                    result = (left, right - left + 1);
            }

            return count == need
                ? s.Substring(result.Start, result.Length)
                : string.Empty;
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "ADOBECODEBANC";
        var t = "ABC";

        var solution = new Solution();
        Assert.Equal("BANC", solution.MinWindow(s, t));
    }

    [Fact]
    public void Example2()
    {
        var s = "a";
        var t = "a";

        var solution = new Solution();
        Assert.Equal("a", solution.MinWindow(s, t));
    }

    [Fact]
    public void Example3()
    {
        var s = "a";
        var t = "aa";

        var solution = new Solution();
        Assert.Equal(string.Empty, solution.MinWindow(s, t));
    }

    [Fact]
    public void Test1()
    {
        var s = "ABaCABbcC";
        var t = "ABC";

        var solution = new Solution();
        Assert.Equal("CAB", solution.MinWindow(s, t));
    }

    [Fact]
    public void Test2()
    {
        var s = string.Empty;
        var t = string.Empty;

        var solution = new Solution();
        Assert.Equal(string.Empty, solution.MinWindow(s, t));
    }

    [Fact]
    public void Test3()
    {
        var s = "abc";
        var t = "ABC";

        var solution = new Solution();
        Assert.Equal(string.Empty, solution.MinWindow(s, t));
    }
}