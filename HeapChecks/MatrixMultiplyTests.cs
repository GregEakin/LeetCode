using Xunit;

namespace HeapChecks;

public class MatrixMultiplyTests
{
    [Fact]
    public void MultiplyTest()
    {
        var a = new[,] { { 1, 2, 3 }, { 4, 5, 6 } };
        var b = new[,] { { 7, 8 }, { 9, 10 }, { 11, 12 } };
        var c = MatrixMultiply.Multiply(a, b);
        Assert.Equal(new[,] { { 58, 64 }, { 139, 154 } }, c);
    }

    [Fact]
    public void MatrixChainOrderTest()
    {
        var p = new[] { 30, 35, 15, 5, 10, 20, 25 };
        var (m, s) = MatrixMultiply.MatrixChainOrder(p);
        Assert.Equal(15125, m[1, 6]);
        Assert.Equal(7125, m[2, 5]);
    }

    [Fact]
    public void MatrixChainMultiplyTest()
    {
        var a = new int[4][,];
        a[1] = new[,] { { 0, 2, 4 }, { 1, 2, 3 } };
        a[2] = new[,] { { 1, 2 }, { -1, 0 }, { 4, 4 } };
        a[3] = new[,] { { 3, 4 }, { 5, 1 } };
        var p = new[] { a[1].GetLength(0), a[1].GetLength(1), a[2].GetLength(1), a[3].GetLength(1) };
        var (m, s) = MatrixMultiply.MatrixChainOrder(p);
        var c = MatrixMultiply.MatrixChainMultiply(a, s, 1, 3);
        Assert.Equal(new[,] { { 122, 72 }, { 103, 58 } }, c);
    }
}