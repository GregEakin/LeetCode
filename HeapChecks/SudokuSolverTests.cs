//    Copyright 2022 Gregory Eakin
// 
//    Licensed under the Apache License,Version 2.0 (the 'License');
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing,software
//    distributed under the License is distributed on an 'AS IS' BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HeapChecks;

public class SudokuSolverTests
{
    public class Solution
    {
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

        public static void SolveSudoku(char[][] board)
        {
            for (var loop = 0; loop < 9; loop++)
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

                while (p1.Count > 0)
                {
                    var pair = p1.Dequeue();
                    var set = CantBeValues(board, pair.Item1, pair.Item2);
                    if (set.Count < 8) break;
                    
                    for (var item = '1'; item <= '9'; item++)
                    {
                        if (set.Contains(item)) continue;
                        board[pair.Item1][pair.Item2] = item;
                        break;
                    }
                }

                if (p1.Count == 0) break;
            }
        }
    }

    [Fact]
    public void Example1()
    {
        var board =
            "[" +
            "['5','3','.','.','7','.','.','.','.'],['6','.','.','1','9','5','.','.','.'],['.','9','8','.','.','.','.','6','.']," +
            "['8','.','.','.','6','.','.','.','3'],['4','.','.','8','.','3','.','.','1'],['7','.','.','.','2','.','.','.','6']," +
            "['.','6','.','.','.','.','2','8','.'],['.','.','.','4','1','9','.','.','5'],['.','.','.','.','8','.','.','7','9']]";
        var output =
            "[" +
            "['5','3','4','6','7','8','9','1','2'],['6','7','2','1','9','5','3','4','8'],['1','9','8','3','4','2','5','6','7']," +
            "['8','5','9','7','6','1','4','2','3'],['4','2','6','8','5','3','7','9','1'],['7','1','3','9','2','4','8','5','6']," +
            "['9','6','1','5','3','7','2','8','4'],['2','8','7','4','1','9','6','3','5'],['3','4','5','2','8','6','1','7','9']]";

        var input = Utilities.ParseArray(board);
        Solution.SolveSudoku(input);
        Assert.Equal(Utilities.ParseArray(output), input);
    }

    [Fact]
    public void Answer1()
    {
        var board =
            "[" +
            "['.','.','9','7','4','8','.','.','.'],['7','.','.','.','.','.','.','.','.'],['.','2','.','1','.','9','.','.','.']," +
            "['.','.','7','.','.','.','2','4','.'],['.','6','4','.','1','.','5','9','.'],['.','9','8','.','.','.','3','.','.']," +
            "['.','.','.','8','.','3','.','2','.'],['.','.','.','.','.','.','.','.','6'],['.','.','.','2','7','5','9','.','.']]";
        var output =
            "[" +
            "['5','1','9','7','4','8','6','3','2'],['7','8','3','6','5','2','4','1','9'],['4','2','6','1','3','9','8','7','5']," +
            "['3','5','7','9','8','6','2','4','1'],['2','6','4','3','1','7','5','9','8'],['1','9','8','5','2','4','3','6','7']," +
            "['9','7','5','8','6','3','1','2','4'],['8','3','2','4','9','1','7','5','6'],['6','4','1','2','7','5','9','8','3']]";
        var input = Utilities.ParseArray(board);
        Solution.SolveSudoku(input);
        Assert.Equal(Utilities.ParseArray(output), input);
    }

    [Fact]
    public void CantBeValuesTest1()
    {
        var board =
            "[" +
            "['.','.','9','7','4','8','.','.','.'],['7','.','.','.','.','.','.','.','.'],['.','2','.','1','.','9','.','.','.']," +
            "['.','.','7','.','.','.','2','4','.'],['.','6','4','.','1','.','5','9','.'],['.','9','8','.','.','.','3','.','.']," +
            "['.','.','.','8','.','3','.','2','.'],['.','.','.','.','.','.','.','.','6'],['.','.','.','2','7','5','9','.','.']]";
        var input = Utilities.ParseArray(board);
        var takenValues = Solution.CantBeValues(input, 4, 3);
        Assert.Equal(new[] { '1', '7', '8', '2', '6', '4', '5', '9' }, takenValues);
    }

    [Fact]
    public void CantBeValuesTest2()
    {
        var board =
            "[" +
            "['.','.','9','7','4','8','.','.','.'],['7','.','.','.','.','.','.','.','.'],['.','2','.','1','.','9','.','.','.']," +
            "['.','.','7','.','.','.','2','4','.'],['.','6','4','.','1','.','5','9','.'],['.','9','8','.','.','.','3','.','.']," +
            "['.','.','.','8','.','3','.','2','.'],['.','.','.','.','.','.','.','.','6'],['.','.','.','2','7','5','9','.','.']]";
        var input = Utilities.ParseArray(board);
        var takenValues = Solution.CantBeValues(input, 7, 0);
        Assert.Equal(new[] { '7', '6' }, takenValues);
    }
}