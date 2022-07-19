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
using Xunit;

namespace HeapChecks;

public class LongestValidParentheses
{
    public class Solution
    {
        public int LongestValidParentheses(string s)
        {
            var max = 0;
            var stack = new Stack<int>();
            stack.Push(-1);
            for (var i = 0; i < s.Length; i++)
            {
                var c = s[i];
                if (c == '(')
                {
                    stack.Push(i);
                    continue;
                }

                stack.Pop();
                if (stack.Count <= 0)
                {
                    stack.Push(i);
                    continue;
                }

                max = Math.Max(max, i - stack.Peek());
            }

            return max;
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "(()";
        var solution = new Solution();
        Assert.Equal(2, solution.LongestValidParentheses(s));
    }

    [Fact]
    public void Example2()
    {
        var s = ")()())";
        var solution = new Solution();
        Assert.Equal(4, solution.LongestValidParentheses(s));
    }

    [Fact]
    public void Example3()
    {
        var s = string.Empty;
        var solution = new Solution();
        Assert.Equal(0, solution.LongestValidParentheses(s));
    }

    [Fact]
    public void Test1()
    {
        var s = ")(())";
        var solution = new Solution();
        Assert.Equal(4, solution.LongestValidParentheses(s));

    }

    [Fact]
    public void Test2()
    {
        var s = "(()((()";
        var solution = new Solution();
        Assert.Equal(2, solution.LongestValidParentheses(s));

    }

    [Fact]
    public void Test3()
    {
        var s = "(()()))((()";
        var solution = new Solution();
        Assert.Equal(6, solution.LongestValidParentheses(s));

    }
}