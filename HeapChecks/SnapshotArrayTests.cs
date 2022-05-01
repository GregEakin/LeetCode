using System;
using System.Collections.Generic;
using Xunit;

namespace HeapChecks;

public class SnapshotArrayTests
{
    public class SnapshotArray
    {
        private readonly Dictionary<int, Dictionary<int, int>> _data = new();
        private int _count;

        public SnapshotArray(int length)
        {
            _data[0] = new Dictionary<int, int>(length);
            _count = 0;
        }

        public void Set(int index, int val)
        {
            _data[_count][index] = val;
        }

        public int Snap()
        {
            _count++;
            _data[_count] = new Dictionary<int, int>(_data[_count - 1]);
            return _count - 1;
        }

        public int Get(int index, int snap_id)
        {
            var found = _data[snap_id].TryGetValue(index, out var value);
            return found ? value : 0;
        }
    }

    [Fact]
    public void Answer1()
    {
        var commands = new[] { "SnapshotArray", "snap", "snap", "get", "set", "snap", "set" };
        var values = new[]
        {
            new[] { 4 }, Array.Empty<int>(), Array.Empty<int>(), new[] { 3, 1 }, new[] { 2, 4 }, Array.Empty<int>(),
            new[] { 1, 4 }
        };
        var output = new[] { -1, 0, 1, 0, -1, 2, -1 };
        ProcessSnapshots(commands, values, output);
    }

    [Fact]
    public void Answer2()
    {
        var commands = new[] { "SnapshotArray", "set", "snap", "snap", "snap", "get", "snap", "snap", "get" };
        var values = new[]
        {
            new[] { 1 }, new[] { 0, 15 }, Array.Empty<int>(), Array.Empty<int>(), Array.Empty<int>(), new[] { 0, 2 },
            Array.Empty<int>(), Array.Empty<int>(), new[] { 0, 0 }
        };
        var output = new[] { -1, -1, 0, 1, 2, 15, 3, 4, 15 };
        ProcessSnapshots(commands, values, output);
    }

    [Fact]
    public void Example1()
    {
        var commands = new[] { "SnapshotArray", "set", "snap", "set", "get" };
        var values = new[] { new[] { 3 }, new[] { 0, 5 }, Array.Empty<int>(), new[] { 0, 6 }, new[] { 0, 0 } };
        var output = new[] { -1, -1, 0, -1, 5 };
        ProcessSnapshots(commands, values, output);
    }

    private static void ProcessSnapshots(string[] commands, int[][] values, int[] output)
    {
        SnapshotArray? array = null;
        for (var i = 0; i < commands.Length; i++)
        {
            switch (commands[i])
            {
                case "SnapshotArray":
                    array = new SnapshotArray(values[i][0]);
                    break;
                case "set":
                    array!.Set(values[i][0], values[i][1]);
                    break;
                case "snap":
                    var snap = array!.Snap();
                    Assert.Equal(output[i], snap);
                    break;
                case "get":
                    var value = array!.Get(values[i][0], values[i][1]);
                    Assert.Equal(output[i], value);
                    break;
            }
        }
    }
}