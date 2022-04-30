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

public class StrongPasswordChecker
{
    public class SolutionOld
    {
        public class Guess
        {
            private readonly Solution _solution = new();

            public Guess(string password, int changes)
            {
                Password = password;
                Changes = changes;
            }

            public string Password { get; }
            public int Changes { get; }
            public int Errors => _solution.StrongPasswordChecker(Password);
            public int Estimate => Changes + Errors;
        }

        public string StrongPasswordChecker(string password)
        {
            var seen = new HashSet<string> { password };
            var queue = new PriorityQueue<Guess, int>();
            var guess = new Guess(password, 0);
            queue.Enqueue(guess, guess.Estimate);
            while (queue.Count > 0)
            {
                guess = queue.Dequeue();
                if (guess.Errors <= 0)
                    return guess.Password;

                Func<Guess, int, string>[] functions =
                {
                    (g, i) => g.Password[..i] + g.Password[(i + 1)..],
                    (g, i) => g.Password.Insert(i, "$"),
                    (g, i) => g.Password[..i] + "$" + g.Password[(i + 1)..],
                };

                foreach (var fun in functions)
                    for (var i = 0; i < guess.Password.Length; i++)
                    {
                        var p = fun(guess, i);
                        if (seen.Contains(p)) continue;
                        seen.Add(p);
                        var next = new Guess(p, guess.Changes + 1);
                        if (next.Errors >= guess.Errors) continue;
                        queue.Enqueue(next, next.Estimate);
                    }
            }

            return string.Empty;
        }
    }

    public class Solution
    {
        public static int ContainsCheck(string password)
        {
            var number = false;
            var lower = false;
            var upper = false;

            foreach (var c in password)
            {
                number |= char.IsNumber(c);
                lower |= char.IsLower(c);
                upper |= char.IsUpper(c);
            }

            var sum = (number ? 0 : 1) + (lower ? 0 : 1) + (upper ? 0 : 1);
            return Math.Max(0, sum);
        }

        public int StrongPasswordChecker(string password)
        {
            var checks = ContainsCheck(password);
            if (password.Length < 6)
                return Math.Max(checks, 6 - password.Length);

            var one = 0;
            var two = 0;
            var rep = 0;
            for (var i = 2; i < password.Length;)
                if (password[i] == password[i - 1] && password[i] == password[i - 2])
                {
                    var length = 2;
                    while (i < password.Length && password[i] == password[i - 1])
                    {
                        length++;
                        i++;
                    }

                    rep += length / 3;
                    switch (length % 3)
                    {
                        case 0:
                            one++;
                            break;
                        case 1:
                            two += 2;
                            break;
                    }
                }
                else
                    i++;

            if (password.Length <= 20)
                return Math.Max(checks, rep);

            var del = password.Length - 20;
            rep -= Math.Min(del, one);
            rep -= Math.Min(Math.Max(del - one, 0), two) / 2;
            rep -= Math.Max(del - one - two, 0) / 3;
            return Math.Max(checks, rep) + del;
        }
    }

    [Fact]
    public void Answer1()
    {
        //              12345678901234567890123
        var password = "bbaaaaaaaaaaaaaaacccccc";
        var solution = new Solution();
        Assert.Equal(8, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Answer2()
    {
        //              1234567890123456789012
        var password = "aaaaAAAAAA000000123456";
        var solution = new Solution();
        Assert.Equal(5, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Answer3()
    {
        //              12345678901234567890123
        var password = "A1234567890aaabbbbccccc";
        var solution = new Solution();
        Assert.Equal(4, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Answer4()
    {
        var password = "hoAISJDBVWD09232UHJEPODKNLADU1";
        var solution = new Solution();
        Assert.Equal(10, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Answer5()
    {
        var password = "aaaaabbbb1234567890ABA";
        var solution = new Solution();
        Assert.Equal(3, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Answer6()
    {
        var password = "FFFFFFFFFFFFFFF11111111111111111111AAA";
        var solution = new Solution();
        Assert.Equal(23, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Example1()
    {
        var password = "a";
        var solution = new Solution();
        Assert.Equal(5, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Example2()
    {
        var password = "aA1";
        var solution = new Solution();
        Assert.Equal(3, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Example3()
    {
        var password = "1337C0d3";
        var solution = new Solution();
        Assert.Equal(0, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Test1()
    {
        var password = "aaaaa";
        var solution = new Solution();
        Assert.Equal(2, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Test2()
    {
        var password = "aB3456789012345678901";
        var solution = new Solution();
        Assert.Equal(1, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Test3()
    {
        var password = "12345678901234bbbbbbaaa";
        var solution = new Solution();
        Assert.Equal(4, solution.StrongPasswordChecker(password));
    }

    [Fact]
    public void Test4()
    {
        var password = "A1aaaaa";
        var solution = new Solution();
        Assert.Equal(1, solution.StrongPasswordChecker(password));
    }
}