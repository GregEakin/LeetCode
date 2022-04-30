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

public class MatrixChainTests
{
    public static (int[,], int[,]) MatrixChainOrder(int[] p)
    {
        var n = p.Length - 1;
        var m = new int[n, n];
        var s = new int[n, n];

        for (var l = 1; l < n; l++)
        for (var i = 0; i < n - l; i++)
        {
            var j = i + l;
            m[i, j] = int.MaxValue;
            for (var k = i; k < j; k++)
            {
                var q = m[i, k] + m[k + 1, j] + p[i] * p[k + 1] * p[j + 1];
                if (q >= m[i, j]) continue;
                m[i, j] = q;
                s[i, j] = k + 1;
            }
        }

        return (m, s);
    }

    [Fact]
    public void MatrixChainOrderTest1()
    {
        var p = new[] { 30, 35, 15, 5, 10, 20, 25 };
        var (m, s) = MatrixChainOrder(p);
        Assert.Equal(7125, m[1, 4]);
    }

    public static int RecursiveMatrixChain(int[] p, int[,] m, int i, int j)
    {
        if (i == j) return 0;
        m[i, j] = int.MaxValue;
        for (var k = i; k < j; k++)
        {
            var q = RecursiveMatrixChain(p, m, i, k) + RecursiveMatrixChain(p, m, k + 1, j) +
                    p[i] * p[k + 1] * p[j + 1];
            if (q < m[i, j])
                m[i, j] = q;
        }

        return m[i, j];
    }

    [Fact]
    public void RecursiveMatrixChainTest1()
    {
        var p = new[] { 30, 35, 15, 5, 10, 20, 25 };
        var n = p.Length - 1;
        var m = new int[n, n];

        var c = RecursiveMatrixChain(p, m, 0, n - 1);
        Assert.Equal(15125, c);
        Assert.Equal(7125, m[1, 4]);
    }

    public static int MemorizedMatrixChain(int[] p)
    {
        var n = p.Length - 1;
        var m = new int[n, n];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
            m[i, j] = int.MaxValue;

        return LookupChain(p, m, 0, n - 1);
    }

    public static int LookupChain(int[] p, int[,] m, int i, int j)
    {
        if (m[i, j] < int.MaxValue) return m[i, j];

        if (i == j)
        {
            m[i, j] = 0;
            return 0;
        }

        for (var k = i; k < j; k++)
        {
            var q = LookupChain(p, m, i, k) + LookupChain(p, m, k + 1, j) + p[i] * p[k + 1] * p[j + 1];
            if (q < m[i, j])
                m[i, j] = q;
        }

        return m[i, j];
    }

    [Fact]
    public void MemorizedMatrixChainTest1()
    {
        var p = new[] { 30, 35, 15, 5, 10, 20, 25 };
        var c = MemorizedMatrixChain(p);
        Assert.Equal(15125, c);
    }
}