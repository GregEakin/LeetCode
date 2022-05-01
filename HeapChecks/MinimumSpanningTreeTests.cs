using Xunit;

namespace HeapChecks;

public class MinimumSpanningTreeTests
{
    [Fact]
    public void Test1()
    {
        var vertexes = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i' };
        ((char u, char v), int)[] edges =
        {
            (('a', 'b'), 4), // found
            (('a', 'h'), 8),
            (('b', 'c'), 8), // found
            (('b', 'h'), 11),
            (('c', 'd'), 7), // found
            (('c', 'f'), 4), // found
            (('c', 'i'), 2), // found
            (('d', 'e'), 9), // found
            (('d', 'j'), 14),
            (('e', 'f'), 10),
            (('f', 'g'), 2), // found
            (('g', 'h'), 1), // found
            (('g', 'i'), 6),
            (('h', 'i'), 7),
        };
    }
}