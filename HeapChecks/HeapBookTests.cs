using Xunit;

namespace HeapChecks;

public class HeapBookTests
{
    private readonly int[] _answer = { 10, 16, 14, 10, 8, 7, 9, 3, 2, 4, 1 };

    [Fact]
    public void Test71()
    {
        var data = new[] { 16, 14, 10, 8, 7, 9, 3, 2, 4, 1 };
        var heap = new HeapBook(data);

        Assert.Equal(_answer, heap.RawData);
    }

    [Fact]
    public void Test72()
    {
        var data = new[] { 16, 4, 10, 14, 7, 9, 3, 2, 8, 1 };
        var heap = new HeapBook(data);

        heap.MaxHeapify(2);
        Assert.Equal(_answer, heap.RawData);
    }

    [Fact]
    public void Test73()
    {
        var data = new[] { 4, 1, 3, 2, 16, 9, 10, 14, 8, 7 };
        var heap = new HeapBook(data);

        heap.MaxHeapify(2);
        Assert.Equal(_answer, heap.RawData);
    }
}