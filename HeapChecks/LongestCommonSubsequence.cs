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

namespace HeapChecks;

public class LongestCommonSubsequence
{
    public static IEnumerable<char> CommonSubsequence(char[] x, char[] y)
    {
        throw new System.NotImplementedException();
    }

    public static (int[,] c, char[,] b) Length(string x, string y)
    {
        var m = x.Length;
        var n = y.Length;
        var c = new int[m + 1, n + 1];
        var b = new char[m + 1, n + 1];
        for (var i = 1; i <= m; i++)
        for (var j = 1; j <= n; j++)
        {
            if (x[i - 1] == y[j - 1])
            {
                c[i, j] = c[i - 1, j - 1] + 1;
                b[i, j] = '\\';
            }
            else if (c[i - 1, j] >= c[i, j - 1])
            {
                c[i, j] = c[i - 1, j];
                b[i, j] = '|';
            }
            else
            {
                c[i, j] = c[i, j - 1];
                b[i, j] = '-';
            }
        }

        return (c, b);
    }
}