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

using Xunit;

namespace HeapChecks;

public class RecursivelySudokuSolverTests
{
    public class Solution
    {
        private char[][] _board;

        public bool IsInCol(int col, char num)
        {
            for (var row = 0; row < 9; row ++)
                if (_board[row][col] == num)
                    return true;
            
            return false;
        }

        public bool IsInRow(int row, char num)
        {
            for (var col = 0; col < 9; col++)
                if (_board[row][col] == num)
                    return true;
            
            return false;
        }

        public bool IsInBox(int startRow, int startCol, char num)
        {
            for (var row = 0; row < 3; row++)
                for (var col = 0; col < 3; col++)
                    if (_board[row + startRow][col + startCol] == num)
                        return true;

            return false;
        }

        public (int, int) FindEmptyCell()
        {
            for (var row = 0; row < 9; row++)
                for (var col = 0; col < 9; col++)
                    if (_board[row][col] == '.')
                        return (row, col);

            return (-1, -1);
        }

        public bool IsValidCell(int row, int col, char num)
        {
            var valid = 
                !IsInRow(row, num) && 
                !IsInCol(col, num) &&
                !IsInBox(row - row % 3, col - col % 3, num);
            
            return valid;
        }

        public bool Solver()
        {
            var (row, col) = FindEmptyCell();
            if (row < 0 || col < 0) return true;

            for (var num = '1'; num <= '9'; num++)
            {
                var isValidCell = IsValidCell(row, col, num);
                if (!isValidCell) continue;
                _board[row][col] = num;
                if (Solver()) return true;
                _board[row][col] = '.';
            }

            return false;
        }

        public void SolveSudoku(char[][] board)
        {
            _board = board;
            Solver();
        }
    }

    [Fact]
    public void Example1()
    {
        var _board =
            "[" +
            "['5','3','.','.','7','.','.','.','.'],['6','.','.','1','9','5','.','.','.'],['.','9','8','.','.','.','.','6','.']," +
            "['8','.','.','.','6','.','.','.','3'],['4','.','.','8','.','3','.','.','1'],['7','.','.','.','2','.','.','.','6']," +
            "['.','6','.','.','.','.','2','8','.'],['.','.','.','4','1','9','.','.','5'],['.','.','.','.','8','.','.','7','9']]";
        var output =
            "[" +
            "['5','3','4','6','7','8','9','1','2'],['6','7','2','1','9','5','3','4','8'],['1','9','8','3','4','2','5','6','7']," +
            "['8','5','9','7','6','1','4','2','3'],['4','2','6','8','5','3','7','9','1'],['7','1','3','9','2','4','8','5','6']," +
            "['9','6','1','5','3','7','2','8','4'],['2','8','7','4','1','9','6','3','5'],['3','4','5','2','8','6','1','7','9']]";

        var input = Utilities.ParseArray(_board);
        var solution = new Solution();
        solution.SolveSudoku(input);
        Assert.Equal(Utilities.ParseArray(output), input);
    }

    [Fact]
    public void Answer1()
    {
        var _board =
            "[" +
            "['.','.','9','7','4','8','.','.','.'],['7','.','.','.','.','.','.','.','.'],['.','2','.','1','.','9','.','.','.']," +
            "['.','.','7','.','.','.','2','4','.'],['.','6','4','.','1','.','5','9','.'],['.','9','8','.','.','.','3','.','.']," +
            "['.','.','.','8','.','3','.','2','.'],['.','.','.','.','.','.','.','.','6'],['.','.','.','2','7','5','9','.','.']]";
        var output =
            "[" +
            "['5','1','9','7','4','8','6','3','2'],['7','8','3','6','5','2','4','1','9'],['4','2','6','1','3','9','8','7','5']," +
            "['3','5','7','9','8','6','2','4','1'],['2','6','4','3','1','7','5','9','8'],['1','9','8','5','2','4','3','6','7']," +
            "['9','7','5','8','6','3','1','2','4'],['8','3','2','4','9','1','7','5','6'],['6','4','1','2','7','5','9','8','3']]";
        
        var input = Utilities.ParseArray(_board);
        var solution = new Solution();
        solution.SolveSudoku(input);
        Assert.Equal(Utilities.ParseArray(output), input);
    }
}