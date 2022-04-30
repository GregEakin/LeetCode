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

public class RemoveAllOnesWithRowAndColumnFlips
{
    public class Solution
    {
        public bool RemoveOnes(int[][] grid)
        {
            var m = grid.Length;
            var n = grid[0].Length;

            for (var i = 0; i < m; i++)
            {
                if (grid[i][0] != 1) continue;
                for (var j = 0; j < n; j++)
                    grid[i][j] ^= 1;
            }

            for (var j = 0; j < n; j++)
            {
                var same = true;
                for (var i = 1; i < m; i++)
                    same &= grid[0][j] == grid[i][j];
                if (!same) return false;
            }

            return true;
        }
    }

    [Fact]
    public void Example1()
    {
        var grid = new[] { new[] { 0, 1, 0 }, new[] { 1, 0, 1 }, new[] { 0, 1, 0 } };
        var solution = new Solution();
        Assert.True(solution.RemoveOnes(grid));
    }

    [Fact]
    public void Example2()
    {
        var grid = new[] { new[] { 1, 1, 0 }, new[] { 0, 0, 0 }, new[] { 0, 0, 0 } };
        var solution = new Solution();
        Assert.False(solution.RemoveOnes(grid));
    }

    [Fact]
    public void Example3()
    {
        var grid = new[] { new[] { 0 } };
        var solution = new Solution();
        Assert.True(solution.RemoveOnes(grid));
    }
}