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
using Xunit;

namespace HeapChecks;

public class MinimumObstacleRemovalToReachCorner
{
    public class SolutionGiven
    {
        public int MinimumObstacles(int[][] grid)
        {
            return 0;
        }
    }

    public class SolutionAStar
    {
        private class StepState : IComparable<StepState>
        {
            public StepState(int row, int col, int obstacles, (int row, int col) target)
            {
                Row = row;
                Col = col;
                Obstacles = obstacles;

                var manhattanDistance = target.row - row + target.col - col;
                Estimation = manhattanDistance + obstacles;
            }

            public int Row { get; }
            public int Col { get; }
            public int Obstacles { get; }
            public int Estimation { get; }

            public override int GetHashCode() => (Row + 1) * (Col + 1) * Obstacles;
            public int CompareTo(StepState? o) => o is { } other ? Estimation - other.Estimation : -1;

            public override bool Equals(object? o) => o is StepState newState && Row == newState.Row &&
                                                      Col == newState.Col && Obstacles == newState.Obstacles;

            public override string ToString() => $"({Estimation} {Row} {Col} {Obstacles})";
        }

        public int MinimumObstacles(int[][] grid)
        {
            var queue = new PriorityQueue<StepState, int>();
            var seen = new HashSet<StepState>();
            var rows = grid.Length;
            var cols = grid[0].Length;
            var target = (rows - 1, cols - 1);

            var start = new StepState(0, 0, 0, target);
            queue.Enqueue(start, start.Estimation);
            seen.Add(start);
            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();
                if (curr.Row == rows - 1 && curr.Col == cols - 1)
                    return curr.Obstacles;

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

                    var obstacleCount = curr.Obstacles + grid[nextRow][nextCol];
                    var newState = new StepState(nextRow, nextCol, obstacleCount, target);
                    if (seen.Contains(newState)) continue;
                    seen.Add(newState);
                    queue.Enqueue(newState, newState.Estimation);
                }
            }

            return -1;
        }
    }

    public class Solution
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

        public int MinimumObstacles(int[][] grid)
        {
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
                    // if (IsTarget(curX, curY))
                    //     return curK;

                    foreach (var (dx, dy) in Directions)
                    {
                        var newX = curX + dx;
                        var newY = curY + dy;

                        if (!IsSafe(newX, newY)) continue;
                        var currentObstacle = grid[newX][newY] + curK;

                        if (currentObstacle >= visited[newX, newY])
                            continue;

                        visited[newX, newY] = currentObstacle;
                        q.Enqueue((newX, newY, currentObstacle));
                    }
                }
            }

            return visited[_rows-1, _cols-1];
        }

        private bool IsTarget(int x, int y) => x == _rows - 1 && y == _cols - 1;
        private bool IsSafe(int x, int y) => x >= 0 && y >= 0 && x < _rows && y < _cols;
    }


    [Fact]
    public void Example1()
    {
        var grid = new[] { new[] { 0, 1, 1 }, new[] { 1, 1, 0 }, new[] { 1, 1, 0 } };
        var solution = new Solution();
        Assert.Equal(2, solution.MinimumObstacles(grid));
    }

    [Fact]
    public void Example2()
    {
        var grid = new[] { new[] { 0, 1, 0, 0, 0 }, new[] { 0, 1, 0, 1, 0 }, new[] { 0, 0, 0, 1, 0 } };
        var solution = new Solution();
        Assert.Equal(0, solution.MinimumObstacles(grid));
    }
}