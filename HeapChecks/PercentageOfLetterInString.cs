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

using System.Linq;
using Xunit;

namespace HeapChecks;

public class PercentageOfLetterInString
{
    public class Solution
    {
        public int PercentageLetter(string s, char letter)
        {
            var len = s.Length;
            var count = s.Count(c => c == letter);
            return 100 * count / len;
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "foobar";
        var solution = new Solution();
        Assert.Equal(33, solution.PercentageLetter(s, 'o'));
    }

    [Fact]
    public void Example2()
    {
        var s = "jjj";
        var solution = new Solution();
        Assert.Equal(0, solution.PercentageLetter(s, 'k'));
    }

    [Fact]
    public void Test1()
    {
        var s = new string('a', 99) + 'b';
        var solution = new Solution();
        Assert.Equal(99, solution.PercentageLetter(s, 'a'));
    }
}