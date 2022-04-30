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

public class BaseballGame
{
    public class Solution
    {
        public int CalPoints(string[] ops)
        {
            var record = new List<int>();
            foreach (var op in ops)
            {
                if (int.TryParse(op, out var value))
                {
                    record.Add(value);
                    continue;
                }

                switch (op)
                {
                    case "C":
                        record.RemoveAt(record.Count - 1);
                        continue;
                    case "D":
                        record.Add(2 * record[^1]);
                        continue;
                    case "+":
                        record.Add(record[^1] + record[^2]);
                        continue;
                    default:
                        throw new NotSupportedException();
                }
            }

            return record.Sum();
        }
    }

    [Fact]
    public void Example1()
    {
        var ops = new[] { "5", "2", "C", "D", "+" };
        var solution = new Solution();
        Assert.Equal(30, solution.CalPoints(ops));
    }

    [Fact]
    public void Example2()
    {
        var ops = new[] { "5", "-2", "4", "C", "D", "9", "+", "+" };
        var solution = new Solution();
        Assert.Equal(27, solution.CalPoints(ops));
    }

    [Fact]
    public void Example3()
    {
        var ops = new[] { "1" };
        var solution = new Solution();
        Assert.Equal(1, solution.CalPoints(ops));
    }
}