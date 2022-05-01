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

namespace HeapChecks;

public class MatrixMultiply
{
    public static int[,] Multiply(int[,] a, int[,] b)
    {
        if (a.GetLength(1) != b.GetLength(0))
            throw new ArgumentException("Incompatible dimensions");

        var c = new int[a.GetLength(0), b.GetLength(1)];
        for (var i = 0; i < a.GetLength(0); i++)
        for (var j = 0; j < b.GetLength(1); j++)
        for (var k = 0; k < a.GetLength(1); k++)
            c[i, j] += a[i, k] * b[k, j];

        return c;
    }

    public static (int[,] m, int[,] s) MatrixChainOrder(int[] p)
    {
        var n = p.Length - 1;
        var m = new int[n + 1, n + 1];
        var s = new int[n + 1, n + 1];
        for (var l = 2; l <= n; l++)
        for (var i = 1; i <= n - l + 1; i++)
        {
            var j = i + l - 1;
            m[i, j] = int.MaxValue;
            for (var k = i; k <= j - 1; k++)
            {
                var q = m[i, k] + m[k + 1, j] + p[i - 1] * p[k] * p[j];
                if (q >= m[i, j]) continue;
                m[i, j] = q;
                s[i, j] = k;
            }
        }

        return (m, s);
    }

    public static int[,] MatrixChainMultiply(int[][,] a, int[,] s, int i, int j)
    {
        if (j <= i) return a[i];
        var x = MatrixChainMultiply(a, s, i, s[i, j]);
        var y = MatrixChainMultiply(a, s, s[i, j] + 1, j);
        return Multiply(x, y);
    }
}