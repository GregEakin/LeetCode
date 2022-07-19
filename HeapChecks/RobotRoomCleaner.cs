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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class RobotRoomCleaner
{
    // This is the robot's control interface.
    // You should not implement it, or speculate about its implementation
    interface Robot
    {
        // Returns true if the cell in front is open and robot moves into the cell.
        // Returns false if the cell in front is blocked and robot stays in the current cell.
        public bool Move();

        // Robot will stay in the same cell after calling turnLeft/turnRight.
        // Each turn will be 90 degrees.
        public void TurnLeft();
        public void TurnRight();

        // Clean the current cell.
        public void Clean();
    }

    public class RobotTester : Robot
    {
        private enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }

        private readonly int[][] _room;
        private int _row;
        private int _col;
        private Direction _direction = Direction.Up;

        public RobotTester(int[][] room, int row, int col)
        {
            _room = room;
            _row = row;
            _col = col;

            if (row < 0 || row >= room.Length) throw new ArgumentException("Row out of range.");
            if (col < 0 || col >= room[row].Length) throw new ArgumentException("Col out of range.");
        }

        public bool Move()
        {
            var row = _row;
            var col = _col;
            switch (_direction)
            {
                case Direction.Up:
                    row = Math.Max(0, row - 1);
                    break;

                case Direction.Down:
                    row = Math.Min(_room.Length - 1, row + 1);
                    break;

                case Direction.Left:
                    col = Math.Max(0, col - 1);
                    break;

                case Direction.Right:
                    col = Math.Min(_room[row].Length - 1, col + 1);
                    break;
            }

            if ((row == _row && col == _col) || _room[row][col] == 0)
                return false;

            _row = row;
            _col = col;
            return true;
        }

        public void TurnLeft() => _direction = (Direction)(((int)_direction + 1) % 4);
        public void TurnRight() => _direction = (Direction)(((int)_direction + 3) % 4);
        public void Clean() => _room[_row][_col] = 2;
        public bool CleanedAllRooms => _room.All(row => row.All(col => col != 1));
    }

    // Depth first search
    // Recursive
    class SolutionRecursive
    {
        private static readonly (int row, int col)[] Steps = { (-1, 0), (0, 1), (1, 0), (0, -1) };
        private readonly HashSet<(int row, int col)> _visited = new();
        private int _dir;

        void BackTracking((int row, int col) pos, Robot robot)
        {
            robot.Clean();
            _visited.Add(pos);
            for (var i = 0; i < Steps.Length; ++i)
            {
                var next = (pos.row + Steps[_dir].row, pos.col + Steps[_dir].col);
                if (!_visited.Contains(next) && robot.Move())
                {
                    BackTracking(next, robot);
                    robot.TurnRight();
                    robot.TurnRight();
                    robot.Move();
                    robot.TurnRight();
                    robot.TurnRight();
                }

                robot.TurnRight();
                _dir = (_dir + 1) % Steps.Length;
            }
        }

        public void CleanRoom(Robot robot)
        {
            BackTracking((0, 0), robot);
        }
    }

    // Depth first search
    // non-recursive
    class Solution
    {
        private static readonly (int row, int col)[] Steps = { (-1, 0), (0, 1), (1, 0), (0, -1) };

        public void CleanRoom(Robot robot)
        {
            var visited = new HashSet<(int row, int col)>();
            var stack = new Stack<((int row, int col), int turn)>();
            var dir = 0;
            stack.Push(((0, 0), 0));
            visited.Add((0, 0));
            while (stack.Count > 0)
            {
                var (pos, turn) = stack.Pop();

                Top:
                for (var i = turn; i < Steps.Length; i++)
                {
                    var next = (pos.row + Steps[dir].row, pos.col + Steps[dir].col);
                    if (!visited.Contains(next) && robot.Move())
                    {
                        stack.Push((pos, i));
                        visited.Add(next);
                        pos = next;
                        turn = 0;
                        goto Top;
                    }

                    robot.TurnRight();
                    dir = (dir + 1) % Steps.Length;
                }

                robot.Clean();
                robot.TurnRight();
                robot.TurnRight();
                robot.Move();
                robot.TurnRight();
                robot.TurnRight();
            }
        }
    }

    [Fact]
    public void Example1()
    {
        var room = new[]
        {
            new[] { 1, 1, 1, 1, 1, 0, 1, 1 },
            new[] { 1, 1, 1, 1, 1, 0, 1, 1 },
            new[] { 1, 0, 1, 1, 1, 1, 1, 1 },
            new[] { 0, 0, 0, 1, 0, 0, 0, 0 },
            new[] { 1, 1, 1, 1, 1, 1, 1, 1 }
        };
        var row = 1;
        var col = 3;

        var robot = new RobotTester(room, row, col);
        var solution = new Solution();
        solution.CleanRoom(robot);
        Assert.True(robot.CleanedAllRooms);
    }

    [Fact]
    public void Example2()
    {
        var room = new[]
        {
            new[] { 1 },
        };
        var row = 0;
        var col = 0;

        var robot = new RobotTester(room, row, col);
        var solution = new Solution();
        solution.CleanRoom(robot);
        Assert.True(robot.CleanedAllRooms);
    }

    [Fact]
    public void Test1()
    {
        var room = new[]
        {
            new[] { 1, 1, 1, 1 },
            new[] { 1, 1, 1, 1 },
            new[] { 1, 1, 1, 1 },
        };
        var row = 0;
        var col = 0;

        var robot = new RobotTester(room, row, col);
        var solution = new Solution();
        solution.CleanRoom(robot);
        Assert.True(robot.CleanedAllRooms);
    }

    [Fact]
    public void Test2()
    {
        var room = new[]
        {
            new[] { 1, 1, 1, 1 },
            new[] { 1, 1, 1, 1 },
            new[] { 1, 1, 1, 1 },
        };
        var row = 2;
        var col = 3;

        var robot = new RobotTester(room, row, col);
        var solution = new Solution();
        solution.CleanRoom(robot);
        Assert.True(robot.CleanedAllRooms);
    }
}