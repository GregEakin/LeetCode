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
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Xunit;

namespace HeapChecks;

public class TextJustification
{
    public class Solution
    {
        public IList<string> FullJustify(string[] words, int maxWidth)
        {
            var result = new List<string>();

            var start = 0;
            while (start < words.Length)
            {
                var width = words[start].Length;
                var end = start + 1;
                while (end < words.Length && width + words[end].Length + 1 <= maxWidth)
                {
                    width += words[end].Length + 1;
                    end++;
                }

                var builder = new StringBuilder(words[start]);
                var spaces = maxWidth - width;
                var count = end - start;

                var single = start + 1 == end || end >= words.Length || count <= 1;
                var a = single ? 0 : spaces / (count - 1);
                var b = single ? 0 : spaces % (count - 1);
                for (var j = start + 1; j < end; j++)
                {
                    builder.Append(' ', a + 1);
                    if (j - start - 1 < b)
                        builder.Append(' ');
                    builder.Append(words[j]);
                }

                if (single) builder.Append(' ', maxWidth - width);
                result.Add(builder.ToString());
                start = end;
            }

            return result;
        }
    }

    [Fact]
    public void Example1()
    {
        var words = new[] { "This", "is", "an", "example", "of", "text", "justification." };
        var solution = new Solution();

        var answer = new[]
        {
            "This    is    an",
            "example  of text",
            "justification.  "
        };

        Assert.Equal(answer, solution.FullJustify(words, 16));
    }

    [Fact]
    public void Example2()
    {
        var words = new[] { "What", "must", "be", "acknowledgment", "shall", "be" };
        var solution = new Solution();

        var answer = new[]
        {
            "What   must   be",
            "acknowledgment  ",
            "shall be        "
        };

        Assert.Equal(answer, solution.FullJustify(words, 16));
    }

    [Fact]
    public void Example3()
    {
        var words = new[]
        {
            "Science", "is", "what", "we", "understand", "well", "enough", "to", "explain", "to", "a", "computer.",
            "Art", "is", "everything", "else", "we", "do"
        };
        var solution = new Solution();

        var answer = new[]
        {
            "Science  is  what we",
            "understand      well",
            "enough to explain to",
            "a  computer.  Art is",
            "everything  else  we",
            "do                  "
        };

        Assert.Equal(answer, solution.FullJustify(words, 20));
    }

    [Fact]
    public void Test1()
    {
        var words = new[] { "How", "now?" };
        var solution = new Solution();

        var answer = new[]
        {
            "How now?"
        };

        Assert.Equal(answer, solution.FullJustify(words, 8));
    }

    [Fact]
    public void Test2()
    {
        var words = new[] { "a", "b", "c" };
        var solution = new Solution();

        var answer = new[]
        {
            "a b",
            "c  "
        };

        Assert.Equal(answer, solution.FullJustify(words, 3));
    }

    [Fact]
    public void Test3()
    {
        var words = new[] { "a", "b", "c", "d", "e" };
        var solution = new Solution();

        var answer = new[]
        {
            "a b c",
            "d e  "
        };

        Assert.Equal(answer, solution.FullJustify(words, 5));
    }

    [Fact]
    public void Test4()
    {
        var words = new[] { "ab", "bc", "cd" };
        var solution = new Solution();

        var answer = new[]
        {
            "ab  bc",
            "cd    "
        };

        Assert.Equal(answer, solution.FullJustify(words, 6));
    }

    [Fact]
    public void Test5()
    {
        var words = new[] { "how", "a", "cow" };
        var solution = new Solution();

        var answer = new[]
        {
            "how",
            "a  ",
            "cow"
        };

        Assert.Equal(answer, solution.FullJustify(words, 3));
    }
}