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

using System.Text;
using Xunit;

namespace HeapChecks;

public class ZigzagConversionTests
{
    public class Solution
    {
        public static string Convert(string s, int numRows)
        {
            var group = numRows <= 1 ? 1 : (numRows - 1) * 2;
            var blocks = (s.Length + group - 1) / group;

            var buffer = new StringBuilder(s.Length);
            for (var row = 0; row < numRows; row++)
            {
                for (var block = 0; block < blocks; block++)
                {
                    var index = block * group + row;
                    if (index >= s.Length) break;
                    buffer.Append(s[index]);

                    if (row <= 0 || row >= numRows - 1) continue;
                    index += group - 2 * row;
                    if (index >= s.Length) break;
                    buffer.Append(s[index]);
                }
            }

            return buffer.ToString();
        }
    }

    [Fact]
    public void Example1()
    {
        var s = "PAYPALISHIRING";
        var numRows = 3;
        Assert.Equal("PAHNAPLSIIGYIR", Solution.Convert(s, numRows));
    }

    [Fact]
    public void Example2()
    {
        var s = "PAYPALISHIRING";
        var numRows = 4;
        Assert.Equal("PINALSIGYAHRPI", Solution.Convert(s, numRows));
    }

    [Fact]
    public void Example3()
    {
        var s = "A";
        var numRows = 1;
        Assert.Equal("A", Solution.Convert(s, numRows));
    }
}