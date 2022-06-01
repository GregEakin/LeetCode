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

public class UniquePaths
{
    public class Solution
    {
        public static IEnumerable<(int, int)> FindNeighbors(int x, int y)
        {
            if (x > 0) yield return (x - 1, y);
            if (y > 0) yield return (x, y - 1);
        }

        public int UniquePaths(int m, int n)
        {
            var matrix = new int[m, n];
            var visited = new HashSet<(int, int)>();
            var queue = new Queue<(int, int)>();
            matrix[m - 1, n - 1] = 1;
            queue.Enqueue((m - 1, n - 1));
            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();
                if (visited.Contains((x, y))) continue;
                visited.Add((x, y));
                var neighbors = FindNeighbors(x, y);
                foreach (var (nx, ny) in neighbors)
                {
                    matrix[nx, ny] += matrix[x, y];
                    if (visited.Contains((nx, ny))) continue;
                    queue.Enqueue((nx, ny));
                }
            }

            return matrix[0, 0];
        }
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal(28, solution.UniquePaths(3, 7));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal(3, solution.UniquePaths(3, 2));
    }
}