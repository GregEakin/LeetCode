//    Copyright 2022 Gregory Eakin
// 
//    Licensed under the Apache License, Version 2.0 (the 'License');
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an 'AS IS' BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using Xunit;

namespace HeapChecks;

public class MaximalSquare
{
    public class Solution
    {
        public int MaximalSquare(char[][] matrix)
        {
            var rows = matrix.Length;
            var cols = rows > 0 ? matrix[0].Length : 0;
            var dp = new int[2][];
            dp[1] = new int[cols + 1];
            var max = 0;
            for (var i = 1; i <= rows; i++)
            {
                dp[0] = dp[1];
                dp[1] = new int[cols + 1];
                for (var j = 1; j <= cols; j++)
                {
                    if (matrix[i - 1][j - 1] != '1') continue;
                    dp[1][j] = Math.Min(Math.Min(dp[1][j - 1], dp[0][j]), dp[0][j - 1]) + 1;
                    max = Math.Max(max, dp[1][j]);
                }
            }

            return max * max;
        }
    }

    [Fact]
    public void Example1()
    {
        var matrix = new[]
        {
            new[] { '1', '0', '1', '0', '0' },
            new[] { '1', '0', '1', '1', '1' },
            new[] { '1', '1', '1', '1', '1' },
            new[] { '1', '0', '0', '1', '0' },
        };

        var solution = new Solution();
        Assert.Equal(4, solution.MaximalSquare(matrix));
    }

    [Fact]
    public void Example2()
    {
        var matrix = new[]
        {
            new[] { '0', '1' },
            new[] { '1', '0' },
        };

        var solution = new Solution();
        Assert.Equal(1, solution.MaximalSquare(matrix));
    }

    [Fact]
    public void Example3()
    {
        var matrix = new[]
        {
            new[] { '0' },
        };

        var solution = new Solution();
        Assert.Equal(0, solution.MaximalSquare(matrix));
    }
}