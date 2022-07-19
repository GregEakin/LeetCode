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

public class MaximalRectangle
{
    public class Solution
    {
        public int MaximalRectangle(char[][] matrix)
        {
            var rows = matrix.Length;
            var cols = matrix[0].Length;
            var left = new int[cols];
            var right = new int[cols];
            Array.Fill(right, cols);
            var height = new int[cols];
            var max = 0;
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    if (matrix[row][col] == '1') 
                        height[col]++;
                    else 
                        height[col] = 0;
                }

                var curLeft = 0;
                for (var col = 0; col < cols; col++)
                {
                    if (matrix[row][col] == '1') 
                        left[col] = Math.Max(left[col], curLeft);
                    else
                    {
                        left[col] = 0;
                        curLeft = col + 1;
                    }
                }

                var curRight = cols;
                for (var col = cols - 1; col >= 0; col--)
                {
                    if (matrix[row][col] == '1') 
                        right[col] = Math.Min(right[col], curRight);
                    else
                    {
                        right[col] = cols;
                        curRight = col;
                    }
                }

                for (var col = 0; col < cols; col++)
                    max = Math.Max(max, (right[col] - left[col]) * height[col]);
            }

            return max;
        }
    }

    [Fact]
    public void Example1()
    {
        var matrix = new[]
        {
            new[] { '1', '0', '1', '0', '0' }, new[] { '1', '0', '1', '1', '1' }, new[] { '1', '1', '1', '1', '1' },
            new[] { '1', '0', '0', '1', '0' }
        };
        var solution = new Solution();
        Assert.Equal(6, solution.MaximalRectangle(matrix));
    }

    [Fact]
    public void Example2()
    {
        var matrix = new[] { new[] { '0' } };
        var solution = new Solution();
        Assert.Equal(0, solution.MaximalRectangle(matrix));
    }

    [Fact]
    public void Example3()
    {
        var matrix = new[] { new[] { '1' } };
        var solution = new Solution();
        Assert.Equal(1, solution.MaximalRectangle(matrix));
    }

    [Fact]
    public void Test1()
    {
        var matrix = new[]
        {
            new[] { '0', '0', '0', '1', '0', '0', '0' },
            new[] { '0', '0', '1', '1', '1', '0', '0' },
            new[] { '0', '1', '1', '1', '1', '1', '0' },
        };
        var solution = new Solution();
        Assert.Equal(6, solution.MaximalRectangle(matrix));
    }

    [Fact]
    public void Test2()
    {
        var matrix = new[]
        {
            new[] { '0', '0', '0', '1', '0', '0', '0' },
            new[] { '0', '1', '1', '1', '0', '1', '1' },
            new[] { '0', '1', '1', '1', '1', '1', '0' },
        };
        var solution = new Solution();
        Assert.Equal(6, solution.MaximalRectangle(matrix));
    }
}