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

using System.Collections.Generic;
using Xunit;

namespace HeapChecks;

public class ValidNumber
{
    public class SolutionParser
    {
        public bool IsNumber(string s)
        {
            var i = 0;
            if (s[i] == '+' || s[i] == '-') i++;

            var dots = 0;
            var mantissa = 0;
            var exponent = 0;
            while (i < s.Length)
            {
                if (char.IsDigit(s[i]))
                {
                    mantissa++;
                    i++;
                    continue;
                }

                if (s[i] == '.')
                {
                    dots++;
                    i++;
                    continue;
                }

                break;
            }

            if (mantissa == 0 || dots > 1) return false;
            if (i == s.Length) return true;

            if (s[i] != 'e' && s[i] != 'E') return false;
            i++;
            if (i < s.Length && (s[i] == '+' || s[i] == '-')) i++;
            while (i < s.Length)
            {
                if (char.IsDigit(s[i]))
                {
                    exponent++;
                    i++;
                    continue;
                }

                break;
            }

            return i == s.Length && exponent > 0;
        }
    }

    public class Solution
    {
        private static readonly List<Dictionary<string, int>> States = new()
        {
            new Dictionary<string, int> { { "digit", 1 }, { "sign", 2 }, { "dot", 3 } },
            new Dictionary<string, int> { { "digit", 1 }, { "dot", 4 }, { "exponent", 5 } },
            new Dictionary<string, int> { { "digit", 1 }, { "dot", 3 } },
            new Dictionary<string, int> { { "digit", 4 } },
            new Dictionary<string, int> { { "digit", 4 }, { "exponent", 5 } },
            new Dictionary<string, int> { { "sign", 6 }, { "digit", 7 } },
            new Dictionary<string, int> { { "digit", 7 } },
            new Dictionary<string, int> { { "digit", 7 } }
        };

        private static readonly HashSet<int> FinalStates = new() { 1, 4, 7 };

        public bool IsNumber(string s)
        {
            var current = 0;
            foreach (var c in s)
            {
                string group;
                switch (c)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        group = "digit";
                        break;
                    case '+':
                    case '-':
                        group = "sign";
                        break;
                    case 'e':
                    case 'E':
                        group = "exponent";
                        break;
                    case '.':
                        group = "dot";
                        break;
                    default:
                        return false;
                }

                if (!States[current].TryGetValue(group, out current))
                    return false;
            }

            return FinalStates.Contains(current);
        }
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.True(solution.IsNumber("0"));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.False(solution.IsNumber("e"));
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.False(solution.IsNumber("."));
    }

    [Fact]
    public void Test1()
    {
        var values = new[]
            { "2", "0089", "-0.1", "+3.14", "4.", "-.9", "2e10", "-90E3", "3e+7", "+6e-1", "53.5e93", "-123.456e789" };
        var solution = new Solution();
        foreach (var value in values)
        {
            Assert.True(solution.IsNumber(value));
        }
    }

    [Fact]
    public void Test2()
    {
        var values = new[] { "abc", "1a", "1e", "e3", "99e2.5", "--6", "-+3", "95a54e53" };
        var solution = new Solution();
        foreach (var value in values)
        {
            Assert.False(solution.IsNumber(value));
        }
    }
}