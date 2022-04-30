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
using System.Linq;
using System.Text;
using Xunit;

namespace HeapChecks;

public class FindAndReplaceInString
{
    public class Solution
    {
        public string FindReplaceString(string s, int[] indices, string[] sources, string[] targets)
        {
            var k = indices.Length;
            var replacements = new List<(int, int, string)>();
            for (var i = 0; i < k; i++)
            {
                if (s.Length < indices[i] + sources[i].Length) continue;
                var substring = s.Substring(indices[i], sources[i].Length);
                if (substring == sources[i])
                    replacements.Add((indices[i], sources[i].Length, targets[i]));
            }

            var result = new StringBuilder(s);
            foreach (var (start, length, target) in replacements.OrderByDescending(r => r.Item1))
            {
                result.Remove(start, length);
                result.Insert(start, target);
            }

            return result.ToString();
        }
    }

    [Fact]
    public void Answer1()
    {
        var s = "abcde";
        var indices = new[] { 2, 2 };
        var sources = new[] { "cdef", "bc" };
        var targets = new[] { "f", "fe" };
        var solution = new Solution();
        Assert.Equal("abcde", solution.FindReplaceString(s, indices, sources, targets));
    }

    [Fact]
    public void Example1()
    {
        var s = "abcd";
        var indices = new[] { 0, 2 };
        var sources = new[] { "a", "cd" };
        var targets = new[] { "eee", "ffff" };
        var solution = new Solution();
        Assert.Equal("eeebffff", solution.FindReplaceString(s, indices, sources, targets));
    }

    [Fact]
    public void Example2()
    {
        var s = "abcd";
        var indices = new[] { 0, 2 };
        var sources = new[] { "ab", "ec" };
        var targets = new[] { "eee", "ffff" };
        var solution = new Solution();
        Assert.Equal("eeecd", solution.FindReplaceString(s, indices, sources, targets));
    }
}