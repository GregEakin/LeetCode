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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xunit;

namespace HeapChecks;

public class ShortestPathInGridWithObstaclesElimination
{
    public class Solution1
    {
        private int[][] _grid;
        private int _k;
        private int _n;
        private int _m;

        private Queue<(int, int)>[] _queue;
        private int[][][] _dist;

        private void Setup()
        {
            _n = _grid.Length;
            _m = _grid[0].Length;

            _queue = new Queue<(int, int)>[_k + 1];
            _dist = new int[_k + 1][][];
            for (var level = 0; level <= _k; level++)
            {
                _queue[level] = new Queue<(int, int)>(_m * _n / 2);
                _dist[level] = new int[_n][];
                for (var y = 0; y < _n; y++)
                {
                    _dist[level][y] = new int[_m];
                    for (var x = 0; x < _m; x++)
                        _dist[level][y][x] = int.MaxValue;
                }
            }
        }

        private void ProcessNeighbor(int level, int y, int x, int py, int px)
        {
            if (y < 0 || y >= _n) return;
            if (x < 0 || x >= _m) return;

            var next = level + _grid[y][x];
            if (next > _k) return;

            var nextDist = _dist[level][py][px] + 1;
            var currDist = _dist[next][y][x];
            if (currDist <= nextDist) return;

            _dist[next][y][x] = nextDist;
            _queue[next].Enqueue((y, x));
        }

        public int ShortestPath(int[][] grid, int k)
        {
            _grid = grid;
            _k = k;
            Setup();

            _queue[0].Enqueue((0, 0));
            _dist[0][0][0] = 0;

            for (var level = 0; level <= k; level++)
                while (_queue[level].Count > 0)
                {
                    var (y, x) = _queue[level].Dequeue();

                    ProcessNeighbor(level, y - 1, x, y, x);
                    ProcessNeighbor(level, y + 1, x, y, x);
                    ProcessNeighbor(level, y, x - 1, y, x);
                    ProcessNeighbor(level, y, x + 1, y, x);
                }

            var min = int.MaxValue;
            for (var level = 0; level <= k; level++)
                min = Math.Min(min, _dist[level][_n - 1][_m - 1]);
            return min < int.MaxValue ? min : -1;
        }
    }

    public class Solution2
    {
        private int[][] _grid;
        private int _k;
        private int _n;
        private int _m;

        private readonly Queue<(int, int)>[] _queue = new Queue<(int, int)>[2];
        private readonly int[][][] _dist = new int[2][][];

        private void Setup()
        {
            _n = _grid.Length;
            _m = _grid[0].Length;

            _queue[1] = new Queue<(int, int)>(_m * _n / 2);
            _dist[1] = new int[_n][];
            for (var y = 0; y < _n; y++)
            {
                _dist[1][y] = new int[_m];
                for (var x = 0; x < _m; x++)
                    _dist[1][y][x] = int.MaxValue;
            }
        }

        private void ProcessNeighbor(int level, int y, int x, int py, int px)
        {
            if (y < 0 || y >= _n) return;
            if (x < 0 || x >= _m) return;

            var next = _grid[y][x];
            if (next + level > _k) return;

            var nextDist = _dist[0][py][px] + 1;
            if (_dist[next][y][x] <= nextDist) return;

            _dist[next][y][x] = nextDist;
            _queue[next].Enqueue((y, x));
        }

        public int ShortestPath(int[][] grid, int k)
        {
            _grid = grid;
            _k = k;
            Setup();

            _queue[1].Enqueue((0, 0));
            _dist[1][0][0] = 0;
            var min = int.MaxValue;

            for (var level = 0; level <= k; level++)
            {
                _queue[0] = _queue[1];
                _queue[1] = new Queue<(int, int)>();

                _dist[0] = _dist[1];
                _dist[1] = new int[_n][];
                for (var y = 0; y < _n; y++)
                {
                    _dist[1][y] = new int[_m];
                    for (var x = 0; x < _m; x++)
                        _dist[1][y][x] = int.MaxValue;
                }

                while (_queue[0].Count > 0)
                {
                    var (y, x) = _queue[0].Dequeue();

                    ProcessNeighbor(level, y - 1, x, y, x);
                    ProcessNeighbor(level, y + 1, x, y, x);
                    ProcessNeighbor(level, y, x - 1, y, x);
                    ProcessNeighbor(level, y, x + 1, y, x);
                }

                min = Math.Min(min, _dist[0][_n - 1][_m - 1]);
            }

            return min < int.MaxValue ? min : -1;
        }
    }

    public class SolutionGood
    {
        private readonly Queue<(int, int)>[] _queue = new Queue<(int, int)>[2];
        private readonly Dictionary<(int, int), int>[] _dist = new Dictionary<(int, int), int>[2];

        private int[][] _grid;
        private int _n;
        private int _m;
        private int _k;

        private void ProcessNeighbor(int level, int y, int x, int py, int px)
        {
            if (y < 0 || y >= _n) return;
            if (x < 0 || x >= _m) return;

            var next = _grid[y][x];
            if (next + level > _k) return;

            var prevPos = (py, px);
            if (!_dist[0].TryGetValue(prevPos, out var prevDist))
                prevDist = int.MaxValue;

            var nextPos = (y, x);
            if (!_dist[next].TryGetValue(nextPos, out var nextDist))
                nextDist = int.MaxValue;

            if (nextDist <= prevDist + 1) return;
            _dist[next][nextPos] = prevDist + 1;
            _queue[next].Enqueue(nextPos);
        }

        public int ShortestPath(int[][] grid, int k)
        {
            _grid = grid;
            _n = _grid.Length;
            _m = _grid[0].Length;
            _k = k;

            _queue[1] = new Queue<(int, int)>(_m * _n / 2);
            _dist[1] = new Dictionary<(int, int), int>(_m * _n / 2);

            _queue[1].Enqueue((0, 0));
            _dist[1][(0, 0)] = 0;
            var min = int.MaxValue;

            for (var level = 0; level <= _k; level++)
            {
                _queue[0] = _queue[1];
                _queue[1] = new Queue<(int, int)>(_m * _n / 2);

                _dist[0] = _dist[1];
                _dist[1] = new Dictionary<(int, int), int>(_m * _n / 2);

                while (_queue[0].Count > 0)
                {
                    var (y, x) = _queue[0].Dequeue();

                    ProcessNeighbor(level, y - 1, x, y, x);
                    ProcessNeighbor(level, y + 1, x, y, x);
                    ProcessNeighbor(level, y, x - 1, y, x);
                    ProcessNeighbor(level, y, x + 1, y, x);
                }

                if (_dist[0].TryGetValue((_n - 1, _m - 1), out var dist))
                    min = Math.Min(min, dist);

                if (min == _n + _m - 2) break;
            }

            return min < int.MaxValue ? min : -1;
        }
    }

    public class SolutionFast
    {
        private static readonly (int dx, int dy)[] Directions =
        {
            (0, -1),
            (-1, 0),
            (0, 1),
            (1, 0),
        };

        private int _rows = -1;
        private int _cols = -1;

        public int ShortestPath(int[][] grid, int k)
        {
            var steps = 0;
            _rows = grid.Length;
            _cols = grid[0].Length;

            var visited = new int[_rows, _cols];
            var q = new Queue<(int, int, int)>();

            for (var i = 0; i < _rows; i++)
            for (var j = 0; j < _cols; j++)
                visited[i, j] = int.MaxValue;

            visited[0, 0] = 0;
            q.Enqueue((0, 0, 0));
            while (q.Count > 0)
            {
                var count = q.Count;
                for (var i = 0; i < count; i++)
                {
                    var (curX, curY, curK) = q.Dequeue();
                    if (IsTarget(curX, curY))
                        return steps;

                    foreach (var (dx, dy) in Directions)
                    {
                        var newX = curX + dx;
                        var newY = curY + dy;

                        if (!IsSafe(newX, newY)) continue;
                        var currentObstacle = grid[newX][newY] + curK;

                        if (currentObstacle >= visited[newX, newY] || currentObstacle > k)
                            continue;

                        visited[newX, newY] = currentObstacle;
                        q.Enqueue((newX, newY, currentObstacle));
                    }
                }

                steps++;
            }

            return -1;
        }

        private bool IsTarget(int x, int y) => x == _rows - 1 && y == _cols - 1;
        private bool IsSafe(int x, int y) => x >= 0 && y >= 0 && x < _rows && y < _cols;
    }


    // A* Algorithm
    public class Solution
    {
        private class StepState : IComparable<StepState>
        {
            public StepState(int steps, int row, int col, int k, (int row, int col) target)
            {
                Steps = steps;
                Row = row;
                Col = col;
                K = k;

                var manhattanDistance = target.row - row + target.col - col;
                Estimation = manhattanDistance + steps;
            }

            public int Steps { get; }
            public int Row { get; }
            public int Col { get; }
            public int K { get; }
            public int Estimation { get; }

            public override int GetHashCode() => (Row + 1) * (Col + 1) * K;
            public int CompareTo(StepState? o) => o is { } other ? Estimation - other.Estimation : -1;

            public override bool Equals(object? o) => o is StepState newState && Row == newState.Row &&
                                                      Col == newState.Col && K == newState.K;

            public override string ToString() => $"({Estimation} {Steps} {Row} {Col} {K})";
        }

        public int ShortestPath(int[][] grid, int k)
        {
            var queue = new PriorityQueue<StepState, int>();
            var seen = new HashSet<StepState>();
            var rows = grid.Length;
            var cols = grid[0].Length;
            var target = (rows - 1, cols - 1);

            var start = new StepState(0, 0, 0, k, target);
            queue.Enqueue(start, start.Estimation);
            seen.Add(start);
            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();
                var remainMinDistance = curr.Estimation - curr.Steps;
                if (remainMinDistance <= curr.K)
                    return curr.Estimation;

                var nextSteps = new[]
                {
                    (curr.Row, curr.Col + 1),
                    (curr.Row + 1, curr.Col),
                    (curr.Row, curr.Col - 1),
                    (curr.Row - 1, curr.Col)
                };

                foreach (var (nextRow, nextCol) in nextSteps)
                {
                    if (0 > nextRow || nextRow >= rows || 0 > nextCol || nextCol >= cols)
                        continue;

                    var nextElimination = curr.K - grid[nextRow][nextCol];
                    if (nextElimination < 0) continue;

                    var newState = new StepState(curr.Steps + 1, nextRow, nextCol, nextElimination, target);
                    if (seen.Contains(newState)) continue;
                    seen.Add(newState);
                    queue.Enqueue(newState, newState.Estimation);
                }
            }

            return -1;
        }
    }

    [Fact]
    public void Example1()
    {
        var grid = new[]
        {
            new[] { 0, 0, 0 },
            new[] { 1, 1, 0 },
            new[] { 0, 0, 0 },
            new[] { 0, 1, 1 },
            new[] { 0, 0, 0 }
        };
        var solution = new Solution();
        Assert.Equal(6, solution.ShortestPath(grid, 1));
    }

    [Fact]
    public void Example1B()
    {
        var grid = new[]
        {
            new[] { 0, 0, 0 },
            new[] { 1, 1, 0 },
            new[] { 0, 0, 0 },
            new[] { 0, 1, 1 },
            new[] { 0, 0, 0 }
        };
        var solution = new Solution();
        Assert.Equal(10, solution.ShortestPath(grid, 0));
    }

    [Fact]
    public void Example2()
    {
        var grid = new[]
        {
            new[] { 0, 1, 1 },
            new[] { 1, 1, 1 },
            new[] { 1, 0, 0 }
        };
        var solution = new Solution();
        Assert.Equal(-1, solution.ShortestPath(grid, 1));
    }

    [Fact]
    public void Test1()
    {
        var grid = new[]
        {
            new[] { 0, 1, 1, 0, 0, 0, 0 },
            new[] { 0, 1, 1, 1, 1, 1, 0 },
            new[] { 0, 0, 0, 0, 1, 1, 0 },
        };
        var solution = new Solution();
        Assert.Equal(12, solution.ShortestPath(grid, 1));
    }

    [Fact]
    public void Test2()
    {
        var grid = new[]
        {
            new[] { 0, 1, 1, 0, 0, 0, 0 },
            new[] { 0, 1, 1, 1, 1, 1, 0 },
            new[] { 0, 0, 0, 0, 1, 1, 0 },
        };
        var solution = new Solution();
        Assert.Equal(8, solution.ShortestPath(grid, 2));
    }

    [Fact]
    public void Test3()
    {
        var grid = new[]
        {
            new[] { 0, 1, 1, 0, 0, 0, 0 },
            new[] { 0, 1, 1, 1, 1, 1, 1 },
            new[] { 0, 0, 0, 0, 1, 1, 0 },
        };
        var solution = new Solution();
        Assert.Equal(8, solution.ShortestPath(grid, 2));
    }

    [Fact]
    public void Test4()
    {
        var grid = new[]
        {
            new[] { 0, 1, 1, 1, 0, 0, 0 },
            new[] { 0, 1, 0, 1, 1, 1, 0 },
            new[] { 0, 0, 0, 1, 1, 1, 0 },
        };
        var solution = new Solution();
        Assert.Equal(12, solution.ShortestPath(grid, 2));
    }

    [Fact]
    public void Test5()
    {
        var grid = new[]
        {
            new[] { 0, 1, 0, 1, 0, 0, 0 },
            new[] { 0, 1, 1, 1, 1, 1, 0 },
            new[] { 0, 1, 1, 1, 1, 1, 0 },
            new[] { 0, 1, 1, 1, 1, 1, 0 },
            new[] { 0, 0, 0, 1, 0, 1, 0 },
        };
        var solution = new Solution();
        Assert.Equal(10, solution.ShortestPath(grid, 20));
    }

    [Fact]
    public void Hint1()
    {
        var grid = new[]
        {
            new[] { 0, 1, 1 },
            new[] { 1, 1, 1 },
            new[] { 1, 0, 0 },
        };
        var solution = new Solution();
        Assert.Equal(4, solution.ShortestPath(grid, 2));
    }

    [Fact]
    public void Answer1()
    {
        var grid = new[]
        {
            new[]
            {
                0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1,
                1, 1, 1, 0, 1
            },
            new[]
            {
                0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 1, 1,
                0, 1, 0, 1, 0
            },
            new[]
            {
                0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0,
                1, 0, 1, 0, 1
            },
            new[]
            {
                0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                0, 0, 1, 0, 0
            },
            new[]
            {
                0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0,
                1, 0, 1, 0, 0
            },
            new[]
            {
                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1,
                0, 0, 0, 1, 1
            },
            new[]
            {
                1, 1, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1,
                1, 0, 1, 1, 0
            },
            new[]
            {
                0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1,
                0, 1, 0, 1, 1
            },
            new[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 1, 0, 0, 1,
                0, 1, 1, 0, 0
            },
            new[]
            {
                1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1,
                0, 0, 1, 0, 1
            },
            new[]
            {
                0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0,
                1, 0, 1, 1, 0
            },
            new[]
            {
                1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 1, 1,
                1, 0, 1, 1, 1
            },
            new[]
            {
                0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0,
                1, 1, 1, 0, 1
            },
            new[]
            {
                0, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0,
                0, 1, 1, 1, 1
            },
            new[]
            {
                1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 1, 1,
                1, 0, 1, 0, 1
            },
            new[]
            {
                1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0,
                1, 1, 0, 0, 0
            }
        };
        var solution = new Solution();
        Assert.Equal(54, solution.ShortestPath(grid, 257));
    }

    [Fact]
    public void Answer1B()
    {
        var grid = new[]
        {
            new[]
            {
                0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1,
                1, 1, 1, 0, 1
            },
            new[]
            {
                0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 1, 1,
                0, 1, 0, 1, 0
            },
            new[]
            {
                0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0,
                1, 0, 1, 0, 1
            },
            new[]
            {
                0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0,
                0, 0, 1, 0, 0
            },
            new[]
            {
                0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0,
                1, 0, 1, 0, 0
            },
            new[]
            {
                0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1,
                0, 0, 0, 1, 1
            },
            new[]
            {
                1, 1, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1,
                1, 0, 1, 1, 0
            },
            new[]
            {
                0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1,
                0, 1, 0, 1, 1
            },
            new[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 1, 0, 0, 1,
                0, 1, 1, 0, 0
            },
            new[]
            {
                1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1,
                0, 0, 1, 0, 1
            },
            new[]
            {
                0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0,
                1, 0, 1, 1, 0
            },
            new[]
            {
                1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 1, 1,
                1, 0, 1, 1, 1
            },
            new[]
            {
                0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0,
                1, 1, 1, 0, 1
            },
            new[]
            {
                0, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0,
                0, 1, 1, 1, 1
            },
            new[]
            {
                1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 1, 1,
                1, 0, 1, 0, 1
            },
            new[]
            {
                1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0,
                1, 1, 0, 0, 0
            }
        };
        var solution = new Solution();
        Assert.Equal(54, solution.ShortestPath(grid, 9));
    }
}