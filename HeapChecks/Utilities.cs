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