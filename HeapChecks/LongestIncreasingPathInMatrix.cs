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
using System.Text.Json;
using Xunit;

namespace HeapChecks;

public class LongestIncreasingPathInMatrix
{
    public class SolutionSlow
    {
        public static IEnumerable<(int, int)> Neighbors(int m, int n, int x, int y)
        {
            var neighbors = new List<(int, int)>();
            if (x - 1 >= 0) neighbors.Add((x - 1, y));
            if (x + 1 < m) neighbors.Add((x + 1, y));
            if (y - 1 >= 0) neighbors.Add((x, y - 1));
            if (y + 1 < n) neighbors.Add((x, y + 1));
            return neighbors;
        }

        public int LongestIncreasingPath(int[][] matrix)
        {
            var m = matrix.Length;
            var n = matrix[0].Length;
            var map = new SortedDictionary<int, List<(int, int)>>();
            for (var i = 0; i < m; i++)
            for (var j = 0; j < n; j++)
            {
                var value = matrix[i][j];
                if (!map.TryGetValue(value, out var list))
                {
                    list = new List<(int, int)>();
                    map.Add(value, list);
                }

                list.Add((i, j));
            }

            var max = 0;
            var path = new int[m, n];
            foreach (var (key, list) in map)
            foreach (var (i, j) in list)
            {
                var count = path[i, j];
                var value = key;
                foreach (var (x, y) in Neighbors(m, n, i, j))
                {
                    if (matrix[x][y] <= value) continue;
                    var next = Math.Max(path[x, y], count + 1);
                    path[x, y] = next;
                    if (max >= next) continue;
                    max = next;
                }
            }

            return max + 1;
        }
    }

    public class SolutionFast
    {
        private int GetMaxLen(int[][] matrix, int row, int col, int prev, int[,] memo)
        {
            var m = matrix.Length;
            var n = matrix[0].Length;
            if (row < 0 || col < 0 || row >= m || col >= n || prev >= matrix[row][col])
                return 0;
            if (memo[row, col] != 0)
                return memo[row, col];

            var val = matrix[row][col];
            memo[row, col] = Math.Max(memo[row, col], GetMaxLen(matrix, row + 1, col, val, memo));
            memo[row, col] = Math.Max(memo[row, col], GetMaxLen(matrix, row - 1, col, val, memo));
            memo[row, col] = Math.Max(memo[row, col], GetMaxLen(matrix, row, col + 1, val, memo));
            memo[row, col] = Math.Max(memo[row, col], GetMaxLen(matrix, row, col - 1, val, memo));
            return ++memo[row, col];
        }

        public int LongestIncreasingPath(int[][] matrix)
        {
            var m = matrix.Length;
            var n = matrix[0].Length;
            var memo = new int[m, n];
            var maxLen = 0;
            for (var row = 0; row < m; row++)
            for (var col = 0; col < n; col++)
            {
                var len = GetMaxLen(matrix, row, col, -1, memo);
                maxLen = maxLen > len ? maxLen : len;
            }

            return maxLen;
        }
    }

    public class Solution // DFS, non recursive
    {
        enum Colors
        {
            White,
            Gray,
            Black
        };

        private readonly Stack<(int, int)> _stack = new();
        private int _m;
        private int _n;
        private int[][] _matrix;
        private int[,] _counts;
        private Colors[,] _colors;
        private int _max = 0;

        public IEnumerable<(int, int)> Neighbors(int x, int y)
        {
            var neighbors = new List<(int, int)>();
            if (x - 1 >= 0) neighbors.Add((x - 1, y));
            if (x + 1 < _m) neighbors.Add((x + 1, y));
            if (y - 1 >= 0) neighbors.Add((x, y - 1));
            if (y + 1 < _n) neighbors.Add((x, y + 1));
            return neighbors;
        }

        void Visit(int row, int col)
        {
            _stack.Push((row, col));
            while (_stack.Count > 0)
            {
                var (i, j) = _stack.Peek();
                _colors[i, j] = Colors.Gray;
                var len = _stack.Count;
                var val = _matrix[i][j];
                foreach (var (x, y) in Neighbors(i, j))
                    if (_colors[x, y] == Colors.White && val < _matrix[x][y])
                        _stack.Push((x, y));

                if (len < _stack.Count) continue;
                _stack.Pop();
                _colors[i, j] = Colors.Black;
                foreach (var (x, y) in Neighbors(i, j))
                    if (val < _matrix[x][y])
                        _counts[i, j] = Math.Max(_counts[i, j], _counts[x, y]);
                _counts[i, j]++;
                _max = Math.Max(_max, _counts[i, j]);
            }
        }

        public int LongestIncreasingPath(int[][] matrix)
        {
            _m = matrix.Length;
            _n = matrix[0].Length;
            _matrix = matrix;

            _counts = new int[_m, _n];
            _colors = new Colors[_m, _n];
            for (var i = 0; i < _m; i++)
            for (var j = 0; j < _n; j++)
                if (_colors[i, j] == Colors.White)
                    Visit(i, j);

            return _max;
        }
    }

    [Fact]
    public void Answer1()
    {
        var matrix = new[] { new[] { 7, 8, 9 }, new[] { 9, 7, 6 }, new[] { 7, 2, 3 } };
        var solution = new Solution();
        Assert.Equal(6, solution.LongestIncreasingPath(matrix));
    }

    [Fact]
    public void Example1()
    {
        var matrix = new[] { new[] { 9, 9, 4 }, new[] { 6, 6, 8 }, new[] { 2, 1, 1 } };
        var solution = new Solution();
        Assert.Equal(4, solution.LongestIncreasingPath(matrix));
    }

    [Fact]
    public void Example2()
    {
        var matrix = new[] { new[] { 3, 4, 5 }, new[] { 3, 2, 6 }, new[] { 2, 2, 1 } };
        var solution = new Solution();
        Assert.Equal(4, solution.LongestIncreasingPath(matrix));
    }

    [Fact]
    public void Test1()
    {
        var matrix = new[] { new[] { 1, 2, 3 }, new[] { 8, 9, 4 }, new[] { 7, 6, 5 } };
        var solution = new Solution();
        Assert.Equal(9, solution.LongestIncreasingPath(matrix));
    }
}