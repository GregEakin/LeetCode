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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class MinimumFlipsZeroMatrix
{
    public class Solution
    {
        public static List<int[]> GetNeighbors(int[] home, int n, int m)
        {
            var neighbors = new List<int[]>();
            if (home[0] > 0) neighbors.Add(new[] { home[0] - 1, home[1] });
            if (home[1] > 0) neighbors.Add(new[] { home[0], home[1] - 1 });
            if (home[0] < n - 1) neighbors.Add(new[] { home[0] + 1, home[1] });
            if (home[1] < m - 1) neighbors.Add(new[] { home[0], home[1] + 1 });
            return neighbors;
        }

        public int MinFlips(int[][] mat)
        {
            var n = mat.Length;
            var m = mat[0].Length;
            var visited = new HashSet<string>();
            var matChars = new char[n * m];
            var zeros = string.Empty;

            for (var i = 0; i < n; i++)
            for (var j = 0; j < m; j++)
            {
                var index = m * i + j;
                matChars[index] = mat[i][j] == 0 ? '0' : '1';
                zeros += '0';
            }

            var count = -1;
            var queue = new Queue<string>();
            queue.Enqueue(new string(matChars));

            while (queue.Count > 0)
            {
                count++;
                var size = queue.Count;
                for (var i = 0; i < size; i++)
                {
                    var matrix = queue.Dequeue();
                    if (matrix == zeros)
                        return count;

                    if (visited.Contains(matrix))
                        continue;

                    visited.Add(matrix);
                    for (var j = 0; j < matrix.Length; j++)
                    {
                        var newMatrix = matrix.ToCharArray();
                        newMatrix[j] = newMatrix[j] == '0' ? '1' : '0';
                        var neighbors = GetNeighbors(new[] { j / m, j % m }, n, m);
                        foreach (var nIdx in neighbors.Select(neighbor => m * neighbor[0] + neighbor[1]))
                            newMatrix[nIdx] = newMatrix[nIdx] == '0' ? '1' : '0';

                        var newMatrixString = new string(newMatrix);
                        if (!visited.Contains(newMatrixString))
                            queue.Enqueue(newMatrixString);
                    }
                }
            }

            return -1;
        }
    }

    [Fact]
    public void Example1()
    {
        var mat = new[] { new[] { 0, 0 }, new[] { 0, 1 } };
        var solution = new Solution();
        Assert.Equal(3, solution.MinFlips(mat));
    }

    [Fact]
    public void Example2()
    {
        var mat = new[] { new[] { 0 } };
        var solution = new Solution();
        Assert.Equal(0, solution.MinFlips(mat));
    }

    [Fact]
    public void Example3()
    {
        var mat = new[] { new[] { 1, 0, 0 }, new[] { 1, 0, 0 } };
        var solution = new Solution();
        Assert.Equal(-1, solution.MinFlips(mat));
    }
}