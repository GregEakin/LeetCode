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
using Newtonsoft.Json.Linq;
using Xunit;

namespace HeapChecks;

public class LongestCommonSubstring
{
    public static IEnumerable<string> Substring(string s, string t)
    {
        var l = new int[s.Length, t.Length];
        var z = 0;
        var ret = new HashSet<string>();

        for (var i = 0; i < s.Length; i++)
        for (var j = 0; j < t.Length; j++)
        {
            if (s[i] == t[j])
            {
                if (i == 0 || j == 0)
                    l[i, j] = 1;
                else
                    l[i, j] = l[i - 1, j - 1] + 1;

                if (l[i, j] > z)
                {
                    z = l[i, j];
                    ret.Clear();
                    ret.Add(s[(i - z + 1)..(i + 1)]);
                }
                else if (l[i, j] == z)
                    ret.Add(s[(i - z + 1)..(i + 1)]);
            }
            else
                l[i, j] = 0;
        }

        return ret;
    }
}

public class LongestCommonSubstringTests
{
    [Fact]
    public void Test1()
    {
        var s = "ABABC";
        var t = "BABCA";
        var l = LongestCommonSubstring.Substring(s, t);
        Assert.Equal(new[] { "BABC" }, l);
    }

    [Fact]
    public void Test2()
    {
        var s = "BABCA";
        var t = "ABCBA";
        var l = LongestCommonSubstring.Substring(s, t);
        Assert.Equal(new[] { "ABC" }, l);
    }

    [Fact]
    public void Test3()
    {
        var s = "ABABC";
        var t = "ABCBA";
        var l = LongestCommonSubstring.Substring(s, t);
        Assert.Equal(new[] { "ABC" }, l);
    }
}