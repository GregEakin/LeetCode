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

public class StrongPasswordCheckerII
{
    public class Solution
    {
        public const string SpecialChars = "!@#$%^&*()-+";

        public bool StrongPasswordCheckerII(string password)
        {
            if (password.Length < 8) return false;

            var lower = false;
            var upper = false;
            var digit = false;
            var special = false;
            var last = password[1];

            foreach (var c in password)
            {
                if (last == c) return false;
                last = c;

                lower |= char.IsLower(c);
                upper |= char.IsUpper(c);
                digit |= char.IsDigit(c);
                special |= SpecialChars.Contains(c);
            }

            return lower && upper && digit && special;
        }
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.True(solution.StrongPasswordCheckerII("IloveLe3tcode!"));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.False(solution.StrongPasswordCheckerII("Me+You--IsMyDream"));
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.False(solution.StrongPasswordCheckerII("1aB!"));
    }

    [Fact]
    public void Answer1()
    {
        var solution = new Solution();
        Assert.False(solution.StrongPasswordCheckerII("FDR+7^Z+EX)UNILV7FK)U^1@BZDQZNY"));
    }
}