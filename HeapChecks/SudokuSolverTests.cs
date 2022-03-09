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

public static class Utilities
{
    public static char[][] EmptyArray()
    {
        var board = new char[9][];
        for (var i = 0; i < 9; i++)
            board[i] = new char[9];

        return board;
    }

    public static char[][] ParseArray(string data)
    {
        var index = 1;
        var board = new char[9][];
        for (var i = 0; i < 9; i++)
        {
            index++;
            board[i] = new char[9];
            for (var j = 0; j < 9; j++)
            {
                index++;
                board[i][j] = data[index++];
                index++;
                index++;
            }

            index++;
        }

        return board;
    }
}

public class SudokuSolverTests
{
    public class Solution
    {
        public static void SolveSudoku(char[][] board)
        {
            var candidates = SetupCandidates(board);

            for (var loop = 0; loop < 9; loop++)
            {
                for (var row = 0; row < 9; row++)
                    for (var col = 0; col < 9; col++)
                    {
                        if (candidates[row][col].Count != 1) continue;
                        var cell = candidates[row][col].ElementAt(0);
                        board[row][col] = cell;
                        RemoveSingle(candidates, row, col, cell);
                    }

                for (var index = 0; index < 9; index++)
                {
                    RemoveHiddenSingleRow(board, candidates, index);
                    RemoveHiddenSingleCol(board, candidates, index);
                    RemoveHiddenSingleBox(board, candidates, index);

                    RemoveNakedPairsRow(candidates, index);
                    RemoveNakedPairsCol(candidates, index);
                    RemoveNakedPairsBox(candidates, index);

                    RemoveHiddenPairRow(candidates, index);
                    RemoveHiddenPairCol(candidates, index);
                    RemoveHiddenPairBox(candidates, index);
                }
            }

            ;
        }

        public static HashSet<char>[][] InitCandidates()
        {
            var candidates = new HashSet<char>[9][];
            for (var row = 0; row < 9; row++)
            {
                candidates[row] = new HashSet<char>[9];
                for (var col = 0; col < 9; col++)
                    candidates[row][col] = new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            }

            return candidates;
        }

        public static HashSet<char>[][] SetupCandidates(char[][] board)
        {
            var candidates = InitCandidates();

            for (var row = 0; row < 9; row++)
                for (var col = 0; col < 9; col++)
                {
                    var cell = board[row][col];
                    if (cell == '.') continue;
                    RemoveSingle(candidates, row, col, cell);
                }

            return candidates;
        }

        public static void RemoveSingle(IReadOnlyList<HashSet<char>[]> candidates, int row, int col, char cell)
        {
            candidates[row][col].Clear();
            var box = 3 * (row / 3) + (col / 3);
            for (var i = 0; i < 9; i++)
            {
                candidates[i][col].Remove(cell);
                candidates[row][i].Remove(cell);
                var r = 3 * (box / 3) + i / 3;
                var c = 3 * (box % 3) + i % 3;
                candidates[r][c].Remove(cell);
            }
        }

        public static void RemoveHiddenSingleRow(char[][] board, IReadOnlyList<HashSet<char>[]> candidates, int row)
        {
            var counts = new int[9];
            for (var col = 0; col < 9; col++)
            {
                foreach (var cell in candidates[row][col])
                {
                    var value = (int)char.GetNumericValue(cell) - 1;
                    counts[value]++;
                }
            }

            for (var index = 0; index < 9; index++)
            {
                var count = counts[index];
                if (count != 1) continue;
                for (var col = 0; col < 9; col++)
                {
                    var cell = (char)(index + '1');
                    if (!candidates[row][col].Contains(cell)) continue;
                    board[row][col] = cell;
                    RemoveSingle(candidates, row, col, cell);
                    break;
                }
            }
        }

        public static void RemoveHiddenSingleCol(char[][] board, IReadOnlyList<HashSet<char>[]> candidates, int col)
        {
            var counts = new int[9];
            for (var row = 0; row < 9; row++)
            {
                foreach (var cell in candidates[row][col])
                {
                    var value = (int)char.GetNumericValue(cell) - 1;
                    counts[value]++;
                }
            }

            for (var index = 0; index < 9; index++)
            {
                var count = counts[index];
                if (count != 1) continue;
                for (var row = 0; row < 9; row++)
                {
                    var cell = (char)(index + '1');
                    if (!candidates[row][col].Contains(cell)) continue;
                    board[row][col] = cell;
                    RemoveSingle(candidates, row, col, cell);
                    break;
                }
            }
        }

        public static void RemoveHiddenSingleBox(char[][] board, IReadOnlyList<HashSet<char>[]> candidates, int box)
        {
            var counts = new int[9];
            for (var i = 0; i < 9; i++)
            {
                var r = 3 * (box / 3) + i / 3;
                var c = 3 * (box % 3) + i % 3;
                foreach (var cell in candidates[r][c])
                {
                    var value = (int)char.GetNumericValue(cell) - 1;
                    counts[value]++;
                }
            }

            for (var index = 0; index < 9; index++)
            {
                var count = counts[index];
                if (count != 1) continue;
                for (var i = 0; i < 9; i++)
                {
                    var cell = (char)(index + '1');
                    var r = 3 * (box / 3) + i / 3;
                    var c = 3 * (box % 3) + i % 3;
                    if (!candidates[r][c].Contains(cell)) continue;
                    board[r][c] = cell;
                    RemoveSingle(candidates, r, c, cell);
                    break;
                }
            }
        }

        public static void RemoveNakedPairsRow(IReadOnlyList<HashSet<char>[]> candidates, int row)
        {
            for (var col = 0; col < 9; col++)
            {
                if (candidates[row][col].Count != 2) continue;
                for (var i = col + 1; i < 9; i++)
                {
                    if (!candidates[row][col].SetEquals(candidates[row][i])) continue;

                    for (var c1 = 0; c1 < 9; c1++)
                    {
                        if (c1 == col || c1 == i) continue;
                        foreach (var item in candidates[row][col])
                            candidates[row][c1].Remove(item);
                    }

                    var b1 = 3 * (row / 3) + (col / 3);
                    var b2 = 3 * (row / 3) + (i / 3);
                    if (b1 != b2) continue;
                    for (var j = 0; j < 9; j++)
                    {
                        var r = 3 * (b1 / 3) + j / 3;
                        var c = 3 * (b1 % 3) + j % 3;
                        if (r == row && (c == col || c == i)) continue;
                        foreach (var item in candidates[row][col])
                            candidates[r][c].Remove(item);
                    }

                    break;
                }
            }
        }

        // public static void RemoveSomeRow(char[][] board, IReadOnlyList<HashSet<char>[]> candidates, int count, int row)
        // {
        //     var counts = new int[9];
        //     for (var col = 0; col < 9; col++)
        //     {
        //         foreach (var cell in candidates[row][col])
        //         {
        //             var value = (int)char.GetNumericValue(cell) - 1;
        //             counts[value]++;
        //         }
        //     }
        //
        //     var dict = new Dictionary<Tuple<int, int>, int>();
        //
        //     for (var i = 0; i < 9; i++)
        //     {
        //         if (counts[i] == 1)
        //             Console.WriteLine("Single [{0}][{1}]", row, i);
        //
        //         if (counts[i] == 2)
        //         {
        //             var added = dict.TryAdd()
        //         }
        //     }
        // }

        public static void RemoveNakedPairsCol(IReadOnlyList<HashSet<char>[]> candidates, int col)
        {
            for (var row = 0; row < 9; row++)
            {
                if (candidates[row][col].Count != 2) continue;
                for (var i = row + 1; i < 9; i++)
                {
                    if (!candidates[row][col].SetEquals(candidates[i][col])) continue;

                    for (var r1 = 0; r1 < 9; r1++)
                    {
                        if (r1 == row || r1 == i) continue;
                        foreach (var item in candidates[row][col])
                            candidates[r1][col].Remove(item);
                    }

                    var b1 = 3 * (row / 3) + (col / 3);
                    var b2 = 3 * (i / 3) + (col / 3);
                    if (b1 != b2) continue;
                    for (var j = 0; j < 9; j++)
                    {
                        var r = 3 * (b1 / 3) + j / 3;
                        var c = 3 * (b1 % 3) + j % 3;
                        if ((r == row || r == i) && c == col) continue;
                        foreach (var item in candidates[row][col])
                            candidates[r][c].Remove(item);
                    }

                    break;
                }
            }
        }

        public static void RemoveNakedPairsBox(IReadOnlyList<HashSet<char>[]> candidates, int box)
        {
            for (var bIndex = 0; bIndex < 9; bIndex++)
            {
                var row = 3 * (box / 3) + bIndex / 3;
                var col = 3 * (box % 3) + bIndex % 3;
                if (candidates[row][col].Count != 2) continue;
                for (var i = bIndex + 1; i < 9; i++)
                {
                    var r1 = 3 * (box / 3) + i / 3;
                    var c1 = 3 * (box % 3) + i % 3;
                    if (!candidates[row][col].SetEquals(candidates[r1][c1])) continue;
                    for (var j = 0; j < 9; j++)
                    {
                        var r2 = 3 * (box / 3) + j / 3;
                        var c2 = 3 * (box % 3) + j % 3;
                        if ((r2 == row && c2 == col) || (r2 == r1 && c2 == c1)) continue;
                        foreach (var item in candidates[row][col])
                            candidates[r2][c2].Remove(item);
                    }

                    if (row == r1)
                        for (var c3 = 0; c3 < 9; c3++)
                        {
                            if (c3 == col || c3 == c1) continue;
                            foreach (var item in candidates[row][col])
                                candidates[row][c3].Remove(item);
                        }

                    if (col == c1)
                        for (var r3 = 0; r3 < 9; r3++)
                        {
                            if (r3 == row || r3 == r1) continue;
                            foreach (var item in candidates[row][col])
                                candidates[r3][col].Remove(item);
                        }


                    break;
                }
            }
        }

        public static void RemoveHiddenPairRow(IReadOnlyList<HashSet<char>[]> candidates, int row)
        {
            var indices = new HashSet<int>[9];
            for (var i = 0; i < 9; i++)
                indices[i] = new HashSet<int>();

            for (var col = 0; col < 9; col++)
            {
                foreach (var item in candidates[row][col])
                {
                    var value = (int)char.GetNumericValue(item) - 1;
                    indices[value].Add(col);
                }
            }

            for (var i = 0; i < 9; i++)
            {
                if (indices[i].Count != 2) continue;
                for (var j = i + 1; j < 9; j++)
                {
                    if (indices[j].Count != 2) continue;
                    if (!indices[i].SetEquals(indices[j])) continue;

                    var v1 = (char)(i + '1');
                    var v2 = (char)(j + '1');
                    var c1 = indices[i].ElementAt(0);
                    var c2 = indices[i].ElementAt(1);

                    candidates[row][c1] = new HashSet<char> { v1, v2 };
                    candidates[row][c2] = new HashSet<char> { v1, v2 };
                }
            }
        }

        public static void RemoveHiddenPairCol(IReadOnlyList<HashSet<char>[]> candidates, int col)
        {
            var indices = new HashSet<int>[9];
            for (var i = 0; i < 9; i++)
                indices[i] = new HashSet<int>();

            for (var row = 0; row < 9; row++)
            {
                foreach (var item in candidates[row][col])
                {
                    var value = (int)char.GetNumericValue(item) - 1;
                    indices[value].Add(row);
                }
            }

            for (var i = 0; i < 9; i++)
            {
                if (indices[i].Count != 2) continue;
                for (var j = i + 1; j < 9; j++)
                {
                    if (indices[j].Count != 2) continue;
                    if (!indices[i].SetEquals(indices[j])) continue;

                    var v1 = (char)(i + '1');
                    var v2 = (char)(j + '1');
                    var r1 = indices[i].ElementAt(0);
                    var r2 = indices[i].ElementAt(1);

                    candidates[r1][col] = new HashSet<char> { v1, v2 };
                    candidates[r2][col] = new HashSet<char> { v1, v2 };
                }
            }
        }
        public static void RemoveHiddenPairBox(IReadOnlyList<HashSet<char>[]> candidates, int box)
        {
            var indices = new HashSet<int>[9];
            for (var i = 0; i < 9; i++)
                indices[i] = new HashSet<int>();

            for (var bIndex = 0; bIndex < 9; bIndex++)
            {
                var row = 3 * (box / 3) + bIndex / 3;
                var col = 3 * (box % 3) + bIndex % 3;

                foreach (var item in candidates[row][col])
                {
                    var value = (int)char.GetNumericValue(item) - 1;
                    indices[value].Add(bIndex);
                }
            }

            for (var i = 0; i < 9; i++)
            {
                if (indices[i].Count != 2) continue;
                for (var j = i + 1; j < 9; j++)
                {
                    if (indices[j].Count != 2) continue;
                    if (!indices[i].SetEquals(indices[j])) continue;
                    var value1 = (char)(i + '1');
                    var value2 = (char)(j + '1');

                    foreach (var index in indices[i])
                    {
                        var row = 3 * (box / 3) + index / 3;
                        var col = 3 * (box % 3) + index % 3;
                        candidates[row][col].Clear();
                        candidates[row][col].Add(value1);
                        candidates[row][col].Add(value2);
                    }
                }
            }
        }

        // public static void RemoveLockedCandidateCol(IReadOnlyList<HashSet<char>[]> candidates, int col)
        // {
        // }

        public static void RemoveNakedTripleCol(IReadOnlyList<HashSet<char>[]> candidates, int col)
        {
        }

        // hidden triples
        // naked quads
        // hidden quads
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
    public void RemoveSingeTest()
    {
        var candidates = Solution.InitCandidates();
        candidates[0][0].Clear();
        candidates[0][1].Clear();
        candidates[0][2] = new HashSet<char> { '6', '8', '9' };
        candidates[0][3] = new HashSet<char> { '6', '7', '9' };
        candidates[0][4] = new HashSet<char> { '9' };
        candidates[0][5] = new HashSet<char> { '7', '9' };
        candidates[0][6] = new HashSet<char> { '4', '6', '8', '9' };
        candidates[0][7].Clear();
        candidates[0][8].Clear();

        Solution.RemoveSingle(candidates, 0, 4, '9');

        // Row 0
        Assert.Equal(new HashSet<char> { '6', '8' }, candidates[0][2]);
        Assert.Equal(new HashSet<char> { '6', '7' }, candidates[0][3]);
        Assert.Empty(candidates[0][4]);
        Assert.Equal(new HashSet<char> { '7' }, candidates[0][5]);
        Assert.Equal(new HashSet<char> { '4', '6', '8' }, candidates[0][6]);

        // column 4
        for (var row = 1; row < 9; row++)
            Assert.Equal(8, candidates[row][4].Count);

        // box 1
        for (var row = 1; row < 3; row++)
            for (var col = 3; col < 6; col++)
                Assert.Equal(8, candidates[row][col].Count);
    }

    [Fact]
    public void RemoveHiddenSingleRowTest()
    {
        var input = Utilities.EmptyArray();

        var candidates = Solution.InitCandidates();
        candidates[0][0].Clear();
        candidates[0][1].Clear();
        candidates[0][2] = new HashSet<char> { '2', '6', '7' };
        candidates[0][3] = new HashSet<char> { '2', '6' };
        candidates[0][4].Clear();
        candidates[0][5] = new HashSet<char> { '2', '6' };
        candidates[0][6] = new HashSet<char> { '2', '5', '6' };
        candidates[0][7].Clear();
        candidates[0][8] = new HashSet<char> { '4', '5' };

        Solution.RemoveHiddenSingleRow(input, candidates, 0);

        // Row 0
        Assert.Empty(candidates[0][2]);
        Assert.Equal('7', input[0][2]);
        Assert.Equal(new HashSet<char> { '2', '6' }, candidates[0][3]);
        Assert.Empty(candidates[0][4]);
        Assert.Equal(new HashSet<char> { '2', '6' }, candidates[0][5]);
        Assert.Equal(new HashSet<char> { '2', '5', '6' }, candidates[0][6]);
        Assert.Empty(candidates[0][8]);
        Assert.Equal('4', input[0][8]);

        // column 2, 8
        for (var row = 1; row < 9; row++)
        {
            Assert.Equal(8, candidates[row][2].Count);
            Assert.Equal(8, candidates[row][8].Count);
        }

        // box 0, 3
        for (var row = 1; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
                Assert.Equal(8, candidates[row][col].Count);
            for (var col = 6; col < 9; col++)
                Assert.Equal(8, candidates[row][col].Count);
        }
    }

    [Fact]
    public void RemoveNakedPairRowSameBoxTest()
    {
        var candidates = Solution.InitCandidates();
        candidates[0][0] = new HashSet<char> { '4', '6' };
        candidates[0][1].Clear();
        candidates[0][2] = new HashSet<char> { '4', '6' };
        candidates[0][3].Clear();
        candidates[0][4].Clear();
        candidates[0][5].Clear();
        candidates[0][6] = new HashSet<char> { '3', '4', '6', '8' };
        candidates[0][7].Clear();
        candidates[0][8] = new HashSet<char> { '3', '4', '6', '8' };

        Solution.RemoveNakedPairsRow(candidates, 0);

        // Row 0
        Assert.Equal(new HashSet<char> { '4', '6' }, candidates[0][0]);
        Assert.Equal(new HashSet<char> { '4', '6' }, candidates[0][2]);
        Assert.Equal(new HashSet<char> { '3', '8' }, candidates[0][6]);
        Assert.Equal(new HashSet<char> { '3', '8' }, candidates[0][8]);

        // column 0, 2
        for (var row = 3; row < 9; row++)
        {
            Assert.Equal(9, candidates[row][0].Count);
            Assert.Equal(9, candidates[row][2].Count);
        }

        // box 0
        for (var row = 1; row < 3; row++)
            for (var col = 0; col < 3; col++)
                Assert.Equal(7, candidates[row][col].Count);
    }

    [Fact]
    public void RemoveNakedPairRowDifferentBoxTest()
    {
        var candidates = Solution.InitCandidates();
        candidates[0][0] = new HashSet<char> { '4', '6' };
        candidates[0][1].Clear();
        candidates[0][2] = new HashSet<char> { '3', '4', '6', '8' };
        candidates[0][3].Clear();
        candidates[0][4].Clear();
        candidates[0][5].Clear();
        candidates[0][6] = new HashSet<char> { '3', '4', '6' };
        candidates[0][7].Clear();
        candidates[0][8] = new HashSet<char> { '4', '6' };

        Solution.RemoveNakedPairsRow(candidates, 0);

        // Row 0
        Assert.Equal(new HashSet<char> { '4', '6' }, candidates[0][0]);
        Assert.Equal(new HashSet<char> { '3', '8' }, candidates[0][2]);
        Assert.Equal(new HashSet<char> { '3' }, candidates[0][6]);
        Assert.Equal(new HashSet<char> { '4', '6' }, candidates[0][8]);

        // column 0, 2
        for (var row = 1; row < 9; row++)
        {
            Assert.Equal(9, candidates[row][0].Count);
            Assert.Equal(9, candidates[row][8].Count);
        }

        // box 0
        for (var row = 1; row < 3; row++)
            for (var col = 1; col < 8; col++)
                Assert.Equal(9, candidates[row][col].Count);
    }

    [Fact]
    public void RemoveHiddenPairRowTest()
    {
        var candidates = Solution.InitCandidates();
        candidates[0][0].Clear();
        candidates[0][1] = new HashSet<char> { '1', '7' };
        candidates[0][2] = new HashSet<char> { '2', '7', '9' }; // 2, 9
        candidates[0][3] = new HashSet<char> { '3', '4', '5', '7' };
        candidates[0][4] = new HashSet<char> { '3', '4', '5', '7' };
        candidates[0][5] = new HashSet<char> { '3', '4', '7' };
        candidates[0][6] = new HashSet<char> { '1', '2', '3', '5', '9' }; // 2, 9
        candidates[0][7] = new HashSet<char> { '1', '3', '5' };
        candidates[0][8].Clear();

        Solution.RemoveHiddenPairRow(candidates, 0);

        // Row 0
        Assert.Equal(new HashSet<char> { '2', '9' }, candidates[0][2]);
        Assert.Equal(new HashSet<char> { '2', '9' }, candidates[0][6]);
    }

    [Fact]
    public void RemoveHiddenPairColTest()
    {
        var candidates = Solution.InitCandidates();
        candidates[0][0].Clear();
        candidates[1][0] = new HashSet<char> { '1', '7' };
        candidates[2][0] = new HashSet<char> { '2', '7', '9' }; // 2, 9
        candidates[3][0] = new HashSet<char> { '3', '4', '5', '7' };
        candidates[4][0] = new HashSet<char> { '3', '4', '5', '7' };
        candidates[5][0] = new HashSet<char> { '3', '4', '7' };
        candidates[6][0] = new HashSet<char> { '1', '2', '3', '5', '9' }; // 2, 9
        candidates[7][0] = new HashSet<char> { '1', '3', '5' };
        candidates[8][0].Clear();

        Solution.RemoveHiddenPairCol(candidates, 0);

        // Row 0
        Assert.Equal(new HashSet<char> { '2', '9' }, candidates[2][0]);
        Assert.Equal(new HashSet<char> { '2', '9' }, candidates[6][0]);
    }

    [Fact]
    public void RemoveHiddenPairBoxTest()
    {
        var candidates = Solution.InitCandidates();
        candidates[0][0].Clear();
        candidates[0][1] = new HashSet<char> { '1', '7' };
        candidates[0][2] = new HashSet<char> { '2', '7', '9' }; // 2, 9
        candidates[1][0] = new HashSet<char> { '3', '4', '5', '7' };
        candidates[1][1] = new HashSet<char> { '3', '4', '5', '7' };
        candidates[1][2] = new HashSet<char> { '3', '4', '7' };
        candidates[2][0] = new HashSet<char> { '1', '2', '3', '5', '9' }; // 2, 9
        candidates[2][1] = new HashSet<char> { '1', '3', '5' };
        candidates[2][2].Clear();

        Solution.RemoveHiddenPairBox(candidates, 0);

        // Row 0
        Assert.Equal(new HashSet<char> { '2', '9' }, candidates[0][2]);
        Assert.Equal(new HashSet<char> { '2', '9' }, candidates[2][0]);
    }

    [Fact]
    public void RemoveLockedCandidateTest()
    {
        var input =
            "[" +
            "['.','3','7','4','8','1','6','.','9'],['.','9','.','.','2','7','.','3','8'],['8','.','.','3','.','9','.','.','.']," +
            "['.','1','9','8','7','3','.','6','.'],['7','8','.','.','.','2','.','9','3'],['.','.','.','9','.','4','8','7','.']," +
            "['.','.','.','2','9','5','.','8','6'],['.','.','8','1','3','6','9','.','.'],['9','6','2','7','4','8','3','1','5']]";
        var board = Utilities.ParseArray(input);
        var candidates = Solution.SetupCandidates(board);

        for (var j = 0; j < 10; j++)
            for (var index = 0; index < 9; index++)
            {
                Solution.RemoveHiddenSingleRow(board, candidates, index);
                Solution.RemoveHiddenSingleCol(board, candidates, index);
                Solution.RemoveHiddenSingleBox(board, candidates, index);

                Solution.RemoveNakedPairsRow(candidates, index);
                Solution.RemoveNakedPairsCol(candidates, index);
                Solution.RemoveNakedPairsBox(candidates, index);

                Solution.RemoveHiddenPairRow(candidates, index);
                Solution.RemoveHiddenPairCol(candidates, index);
                Solution.RemoveHiddenPairBox(candidates, index);

                // hidden triples
                // naked quads
                // hidden quads
            }

        // Solution.RemoveLockedCandidateCol(candidates, 6);                                     // remove 5, from column 6
        Assert.Equal(new HashSet<char> { '1', '4' }, candidates[1][6]); // 1, 4, 5
        Assert.Equal(new HashSet<char> { '1', '2', '4', '7' }, candidates[2][6]); // 1, 2, 4, 5, 7
        Assert.Equal(new HashSet<char> { '2', '4', '5' }, candidates[3][6]);
        Assert.Equal(new HashSet<char> { '1', '4', '5' }, candidates[4][6]);
        Assert.Equal(new HashSet<char> { '4', '7' }, candidates[6][6]);
    }

    [Fact]
    public void RemoveNakedTripleTest()
    {
        var input =
            "[" +
            "['.','3','7','4','8','1','6','.','9'],['.','9','.','.','2','7','.','3','8'],['8','.','.','3','.','9','.','.','.']," +
            "['.','1','9','8','7','3','.','6','.'],['7','8','.','.','.','2','.','9','3'],['.','.','.','9','.','4','8','7','.']," +
            "['.','.','.','2','9','5','.','8','6'],['.','.','8','1','3','6','9','.','.'],['9','6','2','7','4','8','3','1','5']]";
        var board = Utilities.ParseArray(input);
        var candidates = Solution.SetupCandidates(board);
        Solution.RemoveNakedTripleCol(candidates, 0);
        Assert.Equal(new HashSet<char> { '2', '5' }, candidates[0][0]);
        Assert.Equal(new HashSet<char> { '1', '6' }, candidates[1][0]); // 1, 4, 5, 6
        Assert.Equal(new HashSet<char> { '2', '4', '5' }, candidates[3][0]);
        Assert.Equal(new HashSet<char> { '3', '6' }, candidates[5][0]); // 2, 3, 5, 6
        Assert.Equal(new HashSet<char> { '1', '3' }, candidates[6][0]);
        Assert.Equal(new HashSet<char> { '4', '5' }, candidates[7][0]);
    }

    [Fact]
    public void RemoveNakedTripleTest2()
    {
        // Naked triples are three numbers that do not have any other numbers residing in the cells with them.
        var candidates = Solution.InitCandidates();
        candidates[0][0].Clear();
        candidates[0][1] = new HashSet<char> { '5', '6' };      // Triple
        candidates[0][2] = new HashSet<char> { '1', '4', '6', '9' };
        candidates[0][3].Clear();
        candidates[0][4] = new HashSet<char> { '6', '9' };      // Triple
        candidates[0][5] = new HashSet<char> { '2', '4', '5', '6' };
        candidates[0][6].Clear();
        candidates[0][7] = new HashSet<char> { '5', '9' };      // Triple
        candidates[0][8] = new HashSet<char> { '1', '4', '6', '9' };

        Solution.RemoveNakedTripleCol(candidates, 0);

        Assert.Equal(new HashSet<char> { '5', '6' }, candidates[1][0]);
        Assert.Equal(new HashSet<char> { '1', '4' }, candidates[2][0]);     // 1, 4
        Assert.Equal(new HashSet<char> { '6', '9' }, candidates[4][0]);
        Assert.Equal(new HashSet<char> { '2', '4' }, candidates[5][0]);     // 2
        Assert.Equal(new HashSet<char> { '5', '9' }, candidates[7][0]);
        Assert.Equal(new HashSet<char> { '1', '4' }, candidates[8][0]);     // 1, 4
    }

    [Fact]
    public void RemoveHiddenTripleTest()
    {
        // Naked triples are three numbers that do not have any other numbers residing in the cells with them.
        var candidates = Solution.InitCandidates();
        candidates[0][0] = new HashSet<char> { '1', '2', '6' };
        candidates[0][1] = new HashSet<char> { '1', '2', '5', '6' };
        candidates[0][2] = new HashSet<char> { '4', '5', '8', '9' };      // Triple 4, 8, 9
        candidates[0][3].Clear();
        candidates[0][4] = new HashSet<char> { '1', '4', '6', '8' };      // Triple 4, 8, 9
        candidates[0][5] = new HashSet<char> { '2', '3', '8', '9' };      // Triple 4, 8, 9
        candidates[0][6] = new HashSet<char> { '2', '3', '5', '6' };
        candidates[0][7] = new HashSet<char> { '2', '3', '6' };      
        candidates[0][8] = new HashSet<char> { '2', '3', '5' };

        // Solution.RemoveHiddenTripleCol(candidates, 0);

        Assert.Equal(new HashSet<char> { '1', '2', '6' }, candidates[0][0]);
        Assert.Equal(new HashSet<char> { '1', '2', '5', '6' }, candidates[0][1]);     
        Assert.Equal(new HashSet<char> { '4', '8', '9' }, candidates[0][2]);
        Assert.Equal(new HashSet<char> { '4', '8' }, candidates[0][4]);     
        Assert.Equal(new HashSet<char> { '8', '9' }, candidates[0][5]);
        Assert.Equal(new HashSet<char> { '2', '3', '5', '6' }, candidates[0][6]);
        Assert.Equal(new HashSet<char> { '2', '3', '6' }, candidates[0][7]);     
        Assert.Equal(new HashSet<char> { '2', '3', '5' }, candidates[0][8]);
    }

    [Fact]
    public void VeryHard1Test()
    {
        var board =
            "[" +
            "['.','3','.','4','8','.','6','.','9'],['.','.','.','.','2','7','.','.','.'],['8','.','.','3','.','.','.','.','.']," +
            "['.','1','9','.','.','.','.','.','.'],['7','8','.','.','.','2','.','9','3'],['.','.','.','.','.','4','8','7','.']," +
            "['.','.','.','.','.','5','.','.','6'],['.','.','.','1','3','.','.','.','.'],['9','.','2','.','4','8','.','1','.']]";
        var output =
            "[" +
            "['.','3','7','4','8','1','6','.','9'],['.','9','.','.','2','7','.','3','8'],['8','.','.','3','.','9','.','.','.']," +
            "['.','1','9','8','7','3','.','6','.'],['7','8','.','.','.','2','.','9','3'],['.','.','.','9','.','4','8','7','.']," +
            "['.','.','.','2','9','5','.','8','6'],['.','.','8','1','3','6','9','.','.'],['9','6','2','7','4','8','3','1','5']]";
        var input = Utilities.ParseArray(board);
        Solution.SolveSudoku(input);
        Assert.Equal(Utilities.ParseArray(output), input);
    }

    [Fact]
    public void XWingTest()
    {
        var board =
            "[" +
            "['.','3','7','4','8','1','6','.','9'],['.','9','.','.','2','7','.','3','8'],['8','.','.','3','.','9','.','.','.']," +
            "['.','1','9','8','7','3','.','6','.'],['7','8','.','.','.','2','.','9','3'],['.','.','.','9','.','4','8','7','.']," +
            "['.','.','.','2','9','5','.','8','6'],['.','.','8','1','3','6','9','.','.'],['9','6','2','7','4','8','3','1','5']]";
        var input = Utilities.ParseArray(board);
        Solution.SolveSudoku(input);
        var x = 0;
    }

    [Fact]
    public void SwordfishTest()
    {
        var board =
            "[" +
            "['.','5','7','3','6','.','2','8','4'],['6','.','4','8','2','5','.','.','.'],['2','8','.','7','.','4','6','5','.']," +
            "['.','9','2','4','.','6','.','.','.'],['3','6','1','9','.','7','.','4','2'],['.','4','5','1','3','2','.','9','6']," +
            "['4','.','6','2','.','.','.','7','5'],['.','2','.','5','7','.','4','6','.'],['5','7','8','6','4','.','3','2','.']]";
        var input = Utilities.ParseArray(board);
        Solution.SolveSudoku(input);
        var x = 0;
    }

    public class Solution2
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
        Assert.True(Solution2.CheckSolution(Utilities.ParseArray(output)));
    }

    [Fact]
    public void Test2()
    {
        // squre(6, 8) = no(1, 2, 3, 5, 6, 7, 8, 9), maybe(4, 1, 9, 8, 6)
        var board =
            "[['5','3','.','.','7','.','.','.','.'],['6','.','.','1','9','5','.','.','.'],['.','9','8','.','.','.','.','6','.'],['8','.','.','.','6','.','.','.','3'],['4','.','.','8','.','3','.','.','1'],['7','.','.','.','2','.','.','.','6'],['.','6','.','.','.','.','2','8','.'],['.','.','.','4','1','9','.','.','5'],['.','.','.','.','8','.','.','7','9']]";
        var input = Utilities.ParseArray(board);
        // Assert.Equal(1, Solution2.CantBeValues(input, 6, 8));

        var set = Solution2.CantBeValues(input, 4, 4);
        Assert.Equal(8, set.Count);
    }

    [Fact]
    public void Answer2()
    {
        var board =
            "[['.','.','9','7','4','8','.','.','.'],['7','.','.','6','.','2','.','.','.'],['.','2','.','1','.','9','.','.','.'],['.','.','7','9','8','6','2','4','1'],['2','6','4','3','1','7','5','9','8'],['1','9','8','5','2','4','3','6','7'],['.','.','.','8','6','3','.','2','.'],['.','.','.','4','9','1','.','.','6'],['.','.','.','2','7','5','9','.','.']]";

        var input = Utilities.ParseArray(board);
        Assert.Equal(4, Solution2.GoAnywhereElse(input, 0, 8, '2'));

        var count = Solution2.CantBeValues(input, 0, 8);
        Assert.Equal(new[] { '1', '8', '7', '6', '9', '4' }, count);
    }

    [Fact]
    public void Hint1()
    {
        var board =
            "[['5','1','9','7','4','8','6','3','.'],['7','8','3','6','5','2','4','1','9'],['4','2','6','1','3','9','8','7','5'],['3','5','7','9','8','6','2','4','1'],['2','6','4','3','1','7','5','9','8'],['1','9','8','5','2','4','3','6','7'],['9','7','5','8','6','3','1','2','4'],['8','3','2','4','9','1','7','5','6'],['6','4','1','2','7','5','9','8','3']]";
        var input = Utilities.ParseArray(board);
        Assert.Equal(4, Solution2.GoAnywhereElse(input, 0, 8, '2'));
    }

    [Fact]
    public void Hint2()
    {
        var board =
            "[['5','1','9','7','4','8','6','.','2'],['7','8','3','6','5','2','4','1','9'],['4','2','6','1','3','9','8','7','5'],['3','5','7','9','8','6','2','4','1'],['2','6','4','3','1','7','5','9','8'],['1','9','8','5','2','4','3','6','7'],['9','7','5','8','6','3','1','2','4'],['8','3','2','4','9','1','7','5','6'],['6','4','1','2','7','5','9','8','3']]";
        var input = Utilities.ParseArray(board);
        Assert.Equal(3, Solution2.GoAnywhereElse(input, 0, 8, '3'));
    }
}