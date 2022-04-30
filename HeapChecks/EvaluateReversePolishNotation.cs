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

public class EvaluateReversePolishNotation
{
    public class Solution
    {
        public int EvalRPN(string[] tokens)
        {
            var stack = new Stack<int>();
            foreach (var token in tokens)
            {
                if (int.TryParse(token, out var value))
                {
                    stack.Push(value);
                    continue;
                }

                var right = stack.Pop();
                var left = stack.Pop();
                var result = token switch
                {
                    "+" => left + right,
                    "-" => left - right,
                    "*" => left * right,
                    "/" => left / right,
                    _ => throw new NotSupportedException()
                };
                stack.Push(result);
            }

            return stack.Pop();
        }
    }

    [Fact]
    public void Example1()
    {
        var tokens = new[] { "2", "1", "+", "3", "*" };
        var solution = new Solution();
        Assert.Equal(9, solution.EvalRPN(tokens));
    }

    [Fact]
    public void Example2()
    {
        var tokens = new[] { "4", "13", "5", "/", "+" };
        var solution = new Solution();
        Assert.Equal(6, solution.EvalRPN(tokens));
    }

    [Fact]
    public void Example3()
    {
        var tokens = new[] { "10", "6", "9", "3", "+", "-11", "*", "/", "*", "17", "+", "5", "+" };
        var solution = new Solution();
        Assert.Equal(22, solution.EvalRPN(tokens));
    }

    [Fact]
    public void Test1()
    {
        var tokens = new[] { "-3", "2", "/" };
        var solution = new Solution();
        Assert.Equal(-1, solution.EvalRPN(tokens));
    }

    [Fact]
    public void Test2()
    {
        var tokens = new[] { "3", "2", "/" };
        var solution = new Solution();
        Assert.Equal(1, solution.EvalRPN(tokens));
    }
}