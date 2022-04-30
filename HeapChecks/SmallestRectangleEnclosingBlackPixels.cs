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
using Xunit;

namespace HeapChecks;

public class SmallestRectangleEnclosingBlackPixels
{
    public class Solution
    {
        public class Bounds
        {
            public int X { get; }
            public int Y { get; }
            public int M { get; }
            public int N { get; }

            private int _minX;
            private int _minY;
            private int _maxX;
            private int _maxY;

            public Bounds(int x, int y, int m, int n)
            {
                X = x;
                Y = y;
                M = m;
                N = n;

                _minX = x;
                _minY = y;
                _maxX = x;
                _maxY = y;
            }

            public void Check(int x, int y)
            {
                if (_minX > x) _minX = x;
                if (_maxX < x) _maxX = x;
                if (_minY > y) _minY = y;
                if (_maxY < y) _maxY = y;
            }

            public IEnumerable<(int, int)> Neighbors(int x, int y)
            {
                var neighbors = new List<(int, int)>();
                if (x - 1 >= 0) neighbors.Add((x - 1, y));
                if (x + 1 < M) neighbors.Add((x + 1, y));
                if (y - 1 >= 0) neighbors.Add((x, y - 1));
                if (y + 1 < N) neighbors.Add((x, y + 1));
                return neighbors;
            }

            public int Area()
            {
                return (_maxX - _minX + 1) * (_maxY - _minY + 1);
            }
        }

        public int MinArea(char[][] image, int x, int y)
        {
            var m = image.Length;
            var n = image[0].Length;
            var seen = new HashSet<(int, int)>();
            var queue = new Queue<(int, int)>();
            var bounds = new Bounds(x, y, m, n);

            queue.Enqueue((x, y));
            while (queue.Count > 0)
            {
                var (gx, gy) = queue.Dequeue();
                if (seen.Contains((gx, gy))) continue;
                seen.Add((gx, gy));
                bounds.Check(gx, gy);
                foreach (var (nx, ny) in bounds.Neighbors(gx, gy))
                {
                    if (image[nx][ny] == '0') continue;
                    if (seen.Contains((nx, ny))) continue;
                    queue.Enqueue((nx, ny));
                }
            }

            return bounds.Area();
        }
    }

    [Fact]
    public void Example1()
    {
        var image = new[]
        {
            new[] { '0', '0', '1', '0' },
            new[] { '0', '1', '1', '0' },
            new[] { '0', '1', '0', '0' }
        };
        var solution = new Solution();
        Assert.Equal(6, solution.MinArea(image, 0, 2));
    }

    [Fact]
    public void Example2()
    {
        var image = new[]
        {
            new[] { '1' },
        };
        var solution = new Solution();
        Assert.Equal(1, solution.MinArea(image, 0, 0));
    }

    [Fact]
    public void BoundsTest1()
    {
        var solution = new Solution();
        var bounds = new Solution.Bounds(0, 2, 3, 4);
        bounds.Check(1, 1);
        bounds.Check(1, 2);
        bounds.Check(2, 1);
        Assert.Equal(6, bounds.Area());
    }

    [Fact]
    public void BoundsTest2()
    {
        var solution = new Solution();
        var bounds = new Solution.Bounds(0, 2, 3, 4);
        bounds.Check(1, 1);
        bounds.Check(1, 2);
        var neighbors = bounds.Neighbors(2, 1);
        Assert.Equal(new[] { (1, 1), (2, 0), (2, 2) }, bounds.Neighbors(2, 1));
    }
}