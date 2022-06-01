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
using System.Collections.Generic;
using Xunit;

namespace HeapChecks;

public class CheckIfThereIsAValidParenthesesStringPath
{
    public class Solution
    {
        public static IEnumerable<(int, int)> FindNeighbors(int x, int y)
        {
            if (x > 0) yield return (x - 1, y);
            if (y > 0) yield return (x, y - 1);
        }

        public bool HasValidPath(char[][] grid)
        {
            var m = grid.Length;
            var n = grid[0].Length;
            if ((m + n) % 2 == 0) return false;
            if (grid[0][0] == ')') return false;
            if (grid[m - 1][n - 1] == '(') return false;

            var matrix = new HashSet<int>[m, n];
            for(var i = 0; i < m; i++)
            for (var j = 0; j < n; j++)
                matrix[i, j] = new HashSet<int>();

            var visited = new HashSet<(int, int)>();
            var queue = new Queue<(int, int)>();
            matrix[m - 1, n - 1].Add(1);
            queue.Enqueue((m - 1, n - 1));
            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();
                if (visited.Contains((x, y))) continue;
                visited.Add((x, y));
                var neighbors = FindNeighbors(x, y);
                foreach (var (nx, ny) in neighbors)
                foreach (var count in matrix[x, y])
                {
                    var c = grid[nx][ny];
                    switch (c)
                    {
                        case '(':
                        {
                            var sum = count - 1;
                            if (sum < 0) continue;
                            matrix[nx, ny].Add(sum);
                            break;
                        }
                        case ')':
                            matrix[nx, ny].Add(count + 1);
                            break;
                    }

                    if (visited.Contains((nx, ny))) continue;
                    queue.Enqueue((nx, ny));
                }
            }

            return matrix[0,0].Contains(0);
        }
    }

    [Fact]
    public void Example1()
    {
        var grid = new[]
        {
            new[] { '(', '(', '(' },
            new[] { ')', '(', ')' },
            new[] { '(', '(', ')' },
            new[] { '(', '(', ')' }
        };

        var solution = new Solution();
        Assert.True(solution.HasValidPath(grid));
    }

    [Fact]
    public void Example2()
    {
        var grid = new[]
        {
            new[] { ')', ')' },
            new[] { '(', '(' },
        };

        var solution = new Solution();
        Assert.False(solution.HasValidPath(grid));
    }
}