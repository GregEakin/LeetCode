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

public class BasicCalculator
{
    public class Solution
    {
        public class Calculator
        {
            public enum Operator
            {
                None,
                Add,
                Sub
            }

            public int Acc { get; private set; }
            public int X { get; private set; }
            public Operator Op { get; private set; }

            public void AddDigit(char c)
            {
                Acc = 10 * Acc + c - '0';
            }

            public void SetValue(int value)
            {
                Acc = value;
            }

            public void AddOperator(char c)
            {
                Acc = Calculate();

                X = Acc;
                Acc = 0;
                Op = c switch
                {
                    '+' => Operator.Add,
                    '-' => Operator.Sub,
                    _ => Operator.None,
                };
            }

            public int Calculate()
            {
                Acc = Op switch
                {
                    Operator.Add => X + Acc,
                    Operator.Sub => X - Acc,
                    _ => Acc,
                };

                X = 0;
                Op = Operator.None;
                return Acc;
            }
        }

        public int Calculate(string s)
        {
            var stack = new Stack<Calculator>();
            var state = new Calculator();
            foreach (var c in s)
            {
                switch (c)
                {
                    case ' ':
                        continue;
                    case >= '0' and <= '9':
                        state.AddDigit(c);
                        continue;
                    case '+':
                    case '-':
                        state.AddOperator(c);
                        continue;
                    case '(':
                        stack.Push(state);
                        state = new Calculator();
                        continue;
                    case ')':
                        var calc = state.Calculate();
                        state = stack.Pop();
                        state.SetValue(calc);
                        continue;
                }
            }

            return state.Calculate();
        }
    }

    [Fact]
    public void Test1()
    {
        var s = "- 1 + 1";
        var solution = new Solution();
        Assert.Equal(0, solution.Calculate(s));
    }

    [Fact]
    public void Test2()
    {
        var s = " ( 1 + 1 ) + 10";
        var solution = new Solution();
        Assert.Equal(12, solution.Calculate(s));
    }

    [Fact]
    public void Test3()
    {
        var s = "-(1 + 1)";
        var solution = new Solution();
        Assert.Equal(-2, solution.Calculate(s));
    }

    [Fact]
    public void Test4()
    {
        var s = "(-(1 + 1) + 2)";
        var solution = new Solution();
        Assert.Equal(0, solution.Calculate(s));
    }

    [Fact]
    public void Test5()
    {
        var s = "(- 1+( 1 + 1)+2)";
        var solution = new Solution();
        Assert.Equal(3, solution.Calculate(s));
    }

    [Fact]
    public void Example1()
    {
        var s = "1 + 1";
        var solution = new Solution();
        Assert.Equal(2, solution.Calculate(s));
    }

    [Fact]
    public void Example2()
    {
        var s = "2-1 + 2 ";
        var solution = new Solution();
        Assert.Equal(3, solution.Calculate(s));
    }

    [Fact]
    public void Example3()
    {
        var s = "(1+(4+5+2)-3)+(6+8)";
        var solution = new Solution();
        Assert.Equal(23, solution.Calculate(s));
    }
}