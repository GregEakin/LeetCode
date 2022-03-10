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

public class SudokuSolverOld1Tests
{
    public class Solution
    {
        public static bool CheckSolution(char[][] board)
        {
            for (var row = 0; row < 9; row++)
            {
                var sum = 0;
                for (var col = 0; col < 9; col++)
                    sum += (int)char.GetNumericValue(board[row][col]);

                if (sum != 45) return false;
            }

            for (var col = 0; col < 9; col++)
            {
                var sum = 0;
                for (var row = 0; row < 9; row++)
                    sum += (int)char.GetNumericValue(board[row][col]);

                if (sum != 45) return false;
            }

            for (var box = 0; box < 9; box++)
            {
                var row = box / 3;
                var col = box % 3;
                var sum = 0;
                for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    sum += (int)char.GetNumericValue(board[3 * row + i][3 * col + j]);

                if (sum != 45) return false;
            }

            return true;
        }

        public static HashSet<char> CantBeValues(char[][] board, int row, int col)
        {
            var values = new HashSet<char>();
            var cell = board[row][col];
            if (cell != '.') return values;

            var box = 3 * (row / 3) + (col / 3);
            var r1 = box / 3;
            var c1 = box % 3;
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
            {
                var item = board[3 * r1 + i][3 * c1 + j];
                if (item == '.') continue;
                values.Add(item);
            }

            for (var r2 = 0; r2 < 9; r2++)
            {
                var item = board[r2][col];
                if (item == '.') continue;
                values.Add(item);
            }

            for (var c2 = 0; c2 < 9; c2++)
            {
                var item = board[row][c2];
                if (item == '.') continue;
                values.Add(item);
            }

            return values;
        }

        public static int GoAnywhereElse(char[][] board, int row, int col, char idea)
        {
            var count = 0;

            var box = 3 * (row / 3) + (col / 3);
            var r1 = box / 3;
            var c1 = box % 3;

            var p = 0;
            for (var i = 0; i < 3; i++)
            {
                if (3 * r1 + i == row) continue;
                for (var j = 0; j < 3; j++)
                {
                    if (3 * c1 + j == col) continue;
                    var x = (3 * r1 + i, 3 * c1 + j);
                    p++;
                    if (p == 1 || p == 4)
                    {
                        for (var a = 0; a < 9; a++)
                        {
                            var item = board[a][3 * c1 + j];
                            if (item != idea) continue;
                            count++;
                            break;
                        }

                        for (var b = 0; b < 9; b++)
                        {
                            var item = board[3 * r1 + i][b];
                            if (item != idea) continue;
                            count++;
                            break;
                        }
                    }

                    var y = 0;
                }
            }

            return count;
        }

        public static void SolveSudoku(char[][] board)
        {
            for (var tries = 0; tries < 9; tries++)
            {
                var p1 = new PriorityQueue<Tuple<int, int>, int>();
                for (var row = 0; row < 9; row++)
                for (var col = 0; col < 9; col++)
                {
                    var item = board[row][col];
                    if (item != '.') continue;
                    var count = CantBeValues(board, row, col);
                    p1.Enqueue(new Tuple<int, int>(row, col), 9 - count.Count);
                }

                if (p1.Count == 0) break;
                while (p1.Count > 0)
                {
                    var pair = p1.Dequeue();
                    var set = CantBeValues(board, pair.Item1, pair.Item2);
                    if (set.Count == 8)
                    {
                        for (var item = '1'; item <= '9'; item++)
                        {
                            if (set.Contains(item)) continue;
                            board[pair.Item1][pair.Item2] = item;
                            break;
                        }
                    }
                    else
                    {
                        for (var item = '1'; item <= '9'; item++)
                        {
                            if (set.Contains(item)) continue;
                            var anywhere = GoAnywhereElse(board, pair.Item1, pair.Item2, item);
                            if (anywhere < 4) continue;
                            board[pair.Item1][pair.Item2] = item;
                            break;
                        }
                    }
                }

                var x1 = 0;
            }
        }
    }

    [Fact]
    public void Test1()
    {
        var output =
            "[['5','3','4','6','7','8','9','1','2'],['6','7','2','1','9','5','3','4','8'],['1','9','8','3','4','2','5','6','7'],['8','5','9','7','6','1','4','2','3'],['4','2','6','8','5','3','7','9','1'],['7','1','3','9','2','4','8','5','6'],['9','6','1','5','3','7','2','8','4'],['2','8','7','4','1','9','6','3','5'],['3','4','5','2','8','6','1','7','9']]";
        Assert.True(Solution.CheckSolution(Utilities.ParseArray(output)));
    }

    [Fact]
    public void Test2()
    {
        // squre(6, 8) = no(1, 2, 3, 5, 6, 7, 8, 9), maybe(4, 1, 9, 8, 6)
        var board =
            "[['5','3','.','.','7','.','.','.','.'],['6','.','.','1','9','5','.','.','.'],['.','9','8','.','.','.','.','6','.'],['8','.','.','.','6','.','.','.','3'],['4','.','.','8','.','3','.','.','1'],['7','.','.','.','2','.','.','.','6'],['.','6','.','.','.','.','2','8','.'],['.','.','.','4','1','9','.','.','5'],['.','.','.','.','8','.','.','7','9']]";
        var input = Utilities.ParseArray(board);
        // Assert.Equal(1, Solution.CantBeValues(input, 6, 8));

        var set = Solution.CantBeValues(input, 4, 4);
        Assert.Equal(8, set.Count);
    }

    [Fact]
    public void Answer2()
    {
        var board =
            "[['.','.','9','7','4','8','.','.','.'],['7','.','.','6','.','2','.','.','.'],['.','2','.','1','.','9','.','.','.'],['.','.','7','9','8','6','2','4','1'],['2','6','4','3','1','7','5','9','8'],['1','9','8','5','2','4','3','6','7'],['.','.','.','8','6','3','.','2','.'],['.','.','.','4','9','1','.','.','6'],['.','.','.','2','7','5','9','.','.']]";

        var input = Utilities.ParseArray(board);
        Assert.Equal(4, Solution.GoAnywhereElse(input, 0, 8, '2'));

        var count = Solution.CantBeValues(input, 0, 8);
        Assert.Equal(new[] { '1', '8', '7', '6', '9', '4' }, count);
    }

    [Fact]
    public void Hint1()
    {
        var board =
            "[['5','1','9','7','4','8','6','3','.'],['7','8','3','6','5','2','4','1','9'],['4','2','6','1','3','9','8','7','5'],['3','5','7','9','8','6','2','4','1'],['2','6','4','3','1','7','5','9','8'],['1','9','8','5','2','4','3','6','7'],['9','7','5','8','6','3','1','2','4'],['8','3','2','4','9','1','7','5','6'],['6','4','1','2','7','5','9','8','3']]";
        var input = Utilities.ParseArray(board);
        Assert.Equal(4, Solution.GoAnywhereElse(input, 0, 8, '2'));
    }

    [Fact]
    public void Hint2()
    {
        var board =
            "[['5','1','9','7','4','8','6','.','2'],['7','8','3','6','5','2','4','1','9'],['4','2','6','1','3','9','8','7','5'],['3','5','7','9','8','6','2','4','1'],['2','6','4','3','1','7','5','9','8'],['1','9','8','5','2','4','3','6','7'],['9','7','5','8','6','3','1','2','4'],['8','3','2','4','9','1','7','5','6'],['6','4','1','2','7','5','9','8','3']]";
        var input = Utilities.ParseArray(board);
        Assert.Equal(3, Solution.GoAnywhereElse(input, 0, 8, '3'));
    }
}