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
using System.Collections.Generic;
using Xunit;

namespace HeapChecks;

public class DetectSquaresTests
{
    public class DetectSquaresSimple
    {
        private readonly int[][] _cntPoints = new int[1001][];
        private readonly List<int[]> _points = new();

        public DetectSquaresSimple()
        {
            for (var i = 0; i < 1001; i++)
                _cntPoints[i] = new int[1001];
        }

        public void Add(int[] point)
        {
            _cntPoints[point[0]][point[1]]++;
            _points.Add(point);
        }

        public int Count(int[] point)
        {
            var x1 = point[0];
            var y1 = point[1];

            var sum = 0;
            foreach (var p2 in _points)
            {
                var x2 = p2[0];
                var y2 = p2[1];
                if (x2 != x1 && y2 != y1)
                    sum += _cntPoints[x1][y2] * _cntPoints[x2][y1];
            }

            return sum;
        }
    }

    public class DetectSquaresGood
    {
        private readonly Dictionary<(int, int), int> _map = new();
        private readonly Dictionary<int, HashSet<int>> _xMap = new();
        private readonly Dictionary<int, HashSet<int>> _yMap = new();

        public DetectSquaresGood()
        {
        }

        public void Add(int[] point)
        {
            var x = point[0];
            var y = point[1];

            if (!_map.TryAdd((x, y), 1))
                _map[(x, y)]++;

            if (!_xMap.TryGetValue(x, out var yMap))
                _xMap[x] = yMap = new HashSet<int>();
            yMap.Add(y);

            if (!_yMap.TryGetValue(y, out var xMap))
                _yMap[y] = xMap = new HashSet<int>();
            xMap.Add(x);
        }

        public int Count(int[] point)
        {
            var x1 = point[0];
            var y1 = point[1];

            if (!_xMap.TryGetValue(x1, out var yPoints))
                return 0;

            if (!_yMap.TryGetValue(y1, out var xPoints))
                return 0;

            var sum = 0;
            foreach (var y2 in yPoints)
            {
                if (y2 == y1) continue;
                foreach (var x2 in xPoints)
                {
                    if (x2 == x1) continue;
                    if (!_map.TryGetValue((x2, y2), out var c4)) continue;
                    var c1 = _map.TryGetValue((x1, y1), out var t1) ? t1 + 1 : 1;
                    var c2 = _map[(x2, y1)];
                    var c3 = _map[(x1, y2)];
                    sum += c1 * c2 * c3 * c4;
                }
            }

            return sum;
        }
    }

    public class DetectSquares
    {
        private readonly Dictionary<(int, int), int> _map = new();
        private readonly Dictionary<int, HashSet<int>> _xMap = new();

        public DetectSquares()
        {
        }

        public void Add(int[] point)
        {
            var x = point[0];
            var y = point[1];

            if (!_map.TryAdd((x, y), 1))
                _map[(x, y)]++;

            if (!_xMap.TryGetValue(x, out var yMap))
                _xMap[x] = yMap = new HashSet<int>();
            yMap.Add(y);
        }

        public int Count(int[] point)
        {
            var x1 = point[0];
            var y1 = point[1];

            if (!_xMap.TryGetValue(x1, out var yPoints))
                return 0;

            var sum = 0;
            foreach (var y2 in yPoints)
            {
                // Only looking for squares, then |x2 - x1| == |y2 - y1|
                var length = Math.Abs(y2 - y1);
                if (length == 0) continue;
                var c1 = _map[(x1, y2)];

                var c2R = _map.TryGetValue((x1 + length, y1), out var t2R) ? t2R : 0;
                var c3R = _map.TryGetValue((x1 + length, y2), out var t3R) ? t3R : 0;
                sum += c1 * c2R * c3R;

                var c2L = _map.TryGetValue((x1 - length, y1), out var t2L) ? t2L : 0;
                var c3L = _map.TryGetValue((x1 - length, y2), out var t3L) ? t3L : 0;
                sum += c1 * c2L * c3L;
            }

            return sum;
        }
    }

    [Fact]
    public void Answer1()
    {
        var commands = new[]
        {
            "DetectSquares", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count",
            "add", "add", "add", "count", "add", "add", "add", "count", "add", "add", "add", "count", "add", "add",
            "add", "count", "add", "add", "add", "count", "add", "add", "add", "count"
        };
        var values = new[]
        {
            Array.Empty<int>(), new[] { 5, 10 }, new[] { 10, 5 }, new[] { 10, 10 }, new[] { 5, 5 }, new[] { 3, 0 },
            new[] { 8, 0 }, new[] { 8, 5 }, new[] { 3, 5 }, new[] { 9, 0 }, new[] { 9, 8 }, new[] { 1, 8 },
            new[] { 1, 0 }, new[] { 0, 0 }, new[] { 8, 0 }, new[] { 8, 8 }, new[] { 0, 8 }, new[] { 1, 9 },
            new[] { 2, 9 }, new[] { 2, 10 }, new[] { 1, 10 }, new[] { 7, 8 }, new[] { 2, 3 }, new[] { 2, 8 },
            new[] { 7, 3 }, new[] { 9, 10 }, new[] { 9, 5 }, new[] { 4, 5 }, new[] { 4, 10 }, new[] { 0, 9 },
            new[] { 4, 5 }, new[] { 4, 9 }, new[] { 0, 5 }, new[] { 1, 10 }, new[] { 10, 1 }, new[] { 10, 10 },
            new[] { 1, 1 }, new[] { 10, 0 }, new[] { 2, 0 }, new[] { 2, 8 }, new[] { 10, 8 }, new[] { 7, 6 },
            new[] { 4, 6 }, new[] { 4, 9 }, new[] { 7, 9 }, new[] { 10, 9 }, new[] { 10, 0 }, new[] { 1, 0 },
            new[] { 1, 9 }, new[] { 0, 9 }, new[] { 8, 1 }, new[] { 0, 1 }, new[] { 8, 9 }, new[] { 3, 9 },
            new[] { 10, 9 }, new[] { 3, 2 }, new[] { 10, 2 }, new[] { 3, 8 }, new[] { 9, 2 }, new[] { 3, 2 },
            new[] { 9, 8 }, new[] { 0, 9 }, new[] { 7, 9 }, new[] { 0, 2 }, new[] { 7, 2 }, new[] { 10, 1 },
            new[] { 1, 10 }, new[] { 10, 10 }, new[] { 1, 1 }, new[] { 6, 10 }, new[] { 2, 6 }, new[] { 6, 6 },
            new[] { 2, 10 }, new[] { 6, 0 }, new[] { 6, 2 }, new[] { 8, 2 }, new[] { 8, 0 }, new[] { 6, 5 },
            new[] { 7, 4 }, new[] { 6, 4 }, new[] { 7, 5 }, new[] { 2, 10 }, new[] { 8, 4 }, new[] { 2, 4 },
            new[] { 8, 10 }, new[] { 2, 6 }, new[] { 2, 5 }, new[] { 1, 5 }, new[] { 1, 6 }, new[] { 10, 9 },
            new[] { 10, 0 }, new[] { 1, 9 }, new[] { 1, 0 }, new[] { 0, 9 }, new[] { 5, 9 }, new[] { 0, 4 },
            new[] { 5, 4 }, new[] { 3, 6 }, new[] { 9, 0 }, new[] { 3, 0 }, new[] { 9, 6 }, new[] { 0, 2 },
            new[] { 1, 1 }, new[] { 0, 1 }, new[] { 1, 2 }, new[] { 1, 7 }, new[] { 8, 0 }, new[] { 8, 7 },
            new[] { 1, 0 }, new[] { 2, 7 }, new[] { 4, 5 }, new[] { 2, 5 }, new[] { 4, 7 }, new[] { 6, 7 },
            new[] { 3, 7 }, new[] { 6, 4 }, new[] { 3, 4 }, new[] { 10, 2 }, new[] { 2, 10 }, new[] { 2, 2 },
            new[] { 10, 10 }, new[] { 10, 1 }, new[] { 1, 10 }, new[] { 1, 1 }, new[] { 10, 10 }, new[] { 2, 10 },
            new[] { 2, 9 }, new[] { 3, 9 }, new[] { 3, 10 }, new[] { 10, 1 }, new[] { 1, 10 }, new[] { 1, 1 },
            new[] { 10, 10 }, new[] { 10, 4 }, new[] { 10, 3 }, new[] { 9, 4 }, new[] { 9, 3 }, new[] { 6, 6 },
            new[] { 6, 10 }, new[] { 10, 6 }, new[] { 10, 10 }, new[] { 9, 7 }, new[] { 4, 7 }, new[] { 9, 2 },
            new[] { 4, 2 }, new[] { 2, 3 }, new[] { 2, 1 }, new[] { 0, 3 }, new[] { 0, 1 }, new[] { 2, 8 },
            new[] { 10, 8 }, new[] { 2, 0 }, new[] { 10, 0 }, new[] { 8, 4 }, new[] { 2, 10 }, new[] { 8, 10 },
            new[] { 2, 4 }, new[] { 0, 0 }, new[] { 9, 9 }, new[] { 0, 9 }, new[] { 9, 0 }, new[] { 5, 7 },
            new[] { 5, 8 }, new[] { 4, 7 }, new[] { 4, 8 }, new[] { 10, 10 }, new[] { 10, 1 }, new[] { 1, 1 },
            new[] { 1, 10 }, new[] { 6, 8 }, new[] { 7, 8 }, new[] { 6, 9 }, new[] { 7, 9 }, new[] { 4, 6 },
            new[] { 1, 6 }, new[] { 4, 3 }, new[] { 1, 3 }, new[] { 10, 1 }, new[] { 1, 10 }, new[] { 10, 10 },
            new[] { 1, 1 }, new[] { 7, 7 }, new[] { 7, 10 }, new[] { 4, 7 }, new[] { 4, 10 }, new[] { 0, 0 },
            new[] { 8, 0 }, new[] { 0, 8 }, new[] { 8, 8 }, new[] { 3, 5 }, new[] { 2, 4 }, new[] { 3, 4 },
            new[] { 2, 5 }, new[] { 0, 6 }, new[] { 0, 2 }, new[] { 4, 2 }, new[] { 4, 6 }, new[] { 5, 2 },
            new[] { 9, 6 }, new[] { 9, 2 }, new[] { 5, 6 }, new[] { 1, 1 }, new[] { 1, 10 }, new[] { 10, 10 },
            new[] { 10, 1 }, new[] { 7, 5 }, new[] { 2, 0 }, new[] { 2, 5 }, new[] { 7, 0 }, new[] { 1, 9 },
            new[] { 1, 2 }, new[] { 8, 2 }, new[] { 8, 9 }, new[] { 3, 8 }, new[] { 3, 3 }, new[] { 8, 3 },
            new[] { 8, 8 }, new[] { 3, 10 }, new[] { 9, 10 }, new[] { 3, 4 }, new[] { 9, 4 }, new[] { 0, 2 },
            new[] { 0, 10 }, new[] { 8, 10 }, new[] { 8, 2 }, new[] { 9, 4 }, new[] { 8, 4 }, new[] { 8, 5 },
            new[] { 9, 5 }, new[] { 9, 8 }, new[] { 4, 3 }, new[] { 4, 8 }, new[] { 9, 3 }, new[] { 4, 9 },
            new[] { 0, 5 }, new[] { 0, 9 }, new[] { 4, 5 }, new[] { 1, 3 }, new[] { 3, 5 }, new[] { 1, 5 },
            new[] { 3, 3 }, new[] { 0, 0 }, new[] { 0, 8 }, new[] { 8, 0 }, new[] { 8, 8 }, new[] { 2, 8 },
            new[] { 10, 0 }, new[] { 10, 8 }, new[] { 2, 0 }, new[] { 8, 1 }, new[] { 0, 9 }, new[] { 8, 9 },
            new[] { 0, 1 }, new[] { 4, 9 }, new[] { 4, 6 }, new[] { 1, 9 }, new[] { 1, 6 }, new[] { 0, 9 },
            new[] { 0, 8 }, new[] { 1, 9 }, new[] { 1, 8 }, new[] { 5, 1 }, new[] { 5, 6 }, new[] { 10, 1 },
            new[] { 10, 6 }, new[] { 9, 2 }, new[] { 2, 2 }, new[] { 2, 9 }, new[] { 9, 9 }, new[] { 5, 5 },
            new[] { 8, 5 }, new[] { 5, 8 }, new[] { 8, 8 }, new[] { 8, 0 }, new[] { 1, 0 }, new[] { 8, 7 },
            new[] { 1, 7 }, new[] { 8, 2 }, new[] { 5, 5 }, new[] { 5, 2 }, new[] { 8, 5 }, new[] { 6, 6 },
            new[] { 6, 8 }, new[] { 8, 6 }, new[] { 8, 8 }, new[] { 2, 10 }, new[] { 10, 2 }, new[] { 2, 2 },
            new[] { 10, 10 }, new[] { 1, 9 }, new[] { 8, 2 }, new[] { 1, 2 }, new[] { 8, 9 }, new[] { 7, 4 },
            new[] { 7, 2 }, new[] { 9, 4 }, new[] { 9, 2 }, new[] { 1, 9 }, new[] { 1, 0 }, new[] { 10, 0 },
            new[] { 10, 9 }, new[] { 2, 10 }, new[] { 2, 3 }, new[] { 9, 10 }, new[] { 9, 3 }, new[] { 10, 0 },
            new[] { 1, 0 }, new[] { 1, 9 }, new[] { 10, 9 }, new[] { 8, 10 }, new[] { 1, 10 }, new[] { 1, 3 },
            new[] { 8, 3 }, new[] { 0, 9 }, new[] { 9, 9 }, new[] { 0, 0 }, new[] { 9, 0 }, new[] { 7, 9 },
            new[] { 8, 9 }, new[] { 7, 8 }, new[] { 8, 8 }, new[] { 3, 1 }, new[] { 9, 7 }, new[] { 9, 1 },
            new[] { 3, 7 }, new[] { 5, 9 }, new[] { 6, 9 }, new[] { 5, 8 }, new[] { 6, 8 }, new[] { 0, 1 },
            new[] { 0, 10 }, new[] { 9, 10 }, new[] { 9, 1 }, new[] { 8, 0 }, new[] { 8, 2 }, new[] { 10, 2 },
            new[] { 10, 0 }, new[] { 8, 0 }, new[] { 0, 8 }, new[] { 8, 8 }, new[] { 0, 0 }, new[] { 6, 7 },
            new[] { 5, 8 }, new[] { 5, 7 }, new[] { 6, 8 }, new[] { 0, 9 }, new[] { 0, 2 }, new[] { 7, 9 },
            new[] { 7, 2 }, new[] { 5, 0 }, new[] { 5, 5 }, new[] { 10, 0 }, new[] { 10, 5 }, new[] { 1, 10 },
            new[] { 10, 10 }, new[] { 10, 1 }, new[] { 1, 1 }, new[] { 9, 2 }, new[] { 9, 10 }, new[] { 1, 2 },
            new[] { 1, 10 }, new[] { 1, 10 }, new[] { 10, 1 }, new[] { 10, 10 }, new[] { 1, 1 }, new[] { 9, 9 },
            new[] { 0, 9 }, new[] { 0, 0 }, new[] { 9, 0 }, new[] { 9, 6 }, new[] { 9, 3 }, new[] { 6, 3 },
            new[] { 6, 6 }, new[] { 10, 4 }, new[] { 6, 0 }, new[] { 10, 0 }, new[] { 6, 4 }, new[] { 6, 8 },
            new[] { 0, 2 }, new[] { 0, 8 }, new[] { 6, 2 }, new[] { 7, 9 }, new[] { 0, 9 }, new[] { 7, 2 },
            new[] { 0, 2 }, new[] { 9, 1 }, new[] { 9, 10 }, new[] { 0, 10 }, new[] { 0, 1 }, new[] { 10, 0 },
            new[] { 10, 9 }, new[] { 1, 9 }, new[] { 1, 0 }, new[] { 1, 6 }, new[] { 1, 9 }, new[] { 4, 9 },
            new[] { 4, 6 }, new[] { 0, 8 }, new[] { 1, 9 }, new[] { 0, 9 }, new[] { 1, 8 }, new[] { 1, 1 },
            new[] { 9, 1 }, new[] { 1, 9 }, new[] { 9, 9 }, new[] { 2, 5 }, new[] { 2, 9 }, new[] { 6, 5 },
            new[] { 6, 9 }, new[] { 7, 3 }, new[] { 2, 3 }, new[] { 2, 8 }, new[] { 7, 8 }, new[] { 9, 4 },
            new[] { 4, 4 }, new[] { 9, 9 }, new[] { 4, 9 }, new[] { 4, 4 }, new[] { 2, 4 }, new[] { 4, 2 },
            new[] { 2, 2 }, new[] { 0, 3 }, new[] { 0, 2 }, new[] { 1, 3 }, new[] { 1, 2 }, new[] { 10, 9 },
            new[] { 10, 2 }, new[] { 3, 2 }, new[] { 3, 9 }, new[] { 5, 6 }, new[] { 10, 6 }, new[] { 10, 1 },
            new[] { 5, 1 }, new[] { 9, 0 }, new[] { 0, 9 }, new[] { 9, 9 }, new[] { 0, 0 }, new[] { 5, 6 },
            new[] { 9, 2 }, new[] { 9, 6 }, new[] { 5, 2 }, new[] { 3, 3 }, new[] { 10, 3 }, new[] { 10, 10 },
            new[] { 3, 10 }, new[] { 2, 4 }, new[] { 2, 10 }, new[] { 8, 4 }, new[] { 8, 10 }, new[] { 4, 9 },
            new[] { 1, 9 }, new[] { 4, 6 }, new[] { 1, 6 }, new[] { 1, 8 }, new[] { 9, 0 }, new[] { 1, 0 },
            new[] { 9, 8 }, new[] { 10, 3 }, new[] { 5, 8 }, new[] { 5, 3 }, new[] { 10, 8 }, new[] { 8, 2 },
            new[] { 0, 10 }, new[] { 8, 10 }, new[] { 0, 2 }, new[] { 9, 0 }, new[] { 2, 7 }, new[] { 9, 7 },
            new[] { 2, 0 }, new[] { 0, 4 }, new[] { 5, 9 }, new[] { 0, 9 }, new[] { 5, 4 }, new[] { 5, 3 },
            new[] { 10, 3 }, new[] { 5, 8 }, new[] { 10, 8 }, new[] { 6, 4 }, new[] { 7, 4 }, new[] { 6, 5 },
            new[] { 7, 5 }, new[] { 9, 1 }, new[] { 0, 1 }, new[] { 9, 10 }, new[] { 0, 10 }, new[] { 5, 10 },
            new[] { 5, 7 }, new[] { 8, 7 }, new[] { 8, 10 }, new[] { 8, 0 }, new[] { 8, 7 }, new[] { 1, 7 },
            new[] { 1, 0 }, new[] { 1, 1 }, new[] { 9, 9 }, new[] { 1, 9 }, new[] { 9, 1 }, new[] { 3, 1 },
            new[] { 3, 5 }, new[] { 7, 5 }, new[] { 7, 1 }, new[] { 5, 8 }, new[] { 5, 3 }, new[] { 10, 8 },
            new[] { 10, 3 }, new[] { 0, 9 }, new[] { 2, 7 }, new[] { 2, 9 }, new[] { 0, 7 }, new[] { 9, 3 },
            new[] { 9, 7 }, new[] { 5, 3 }, new[] { 5, 7 }, new[] { 0, 0 }, new[] { 9, 0 }, new[] { 9, 9 },
            new[] { 0, 9 }, new[] { 6, 4 }, new[] { 4, 2 }, new[] { 4, 4 }, new[] { 6, 2 }, new[] { 1, 9 },
            new[] { 1, 5 }, new[] { 5, 5 }, new[] { 5, 9 }, new[] { 7, 7 }, new[] { 0, 7 }, new[] { 0, 0 },
            new[] { 7, 0 }, new[] { 1, 3 }, new[] { 1, 9 }, new[] { 7, 3 }, new[] { 7, 9 }, new[] { 0, 9 },
            new[] { 9, 9 }, new[] { 9, 0 }, new[] { 0, 0 }, new[] { 1, 8 }, new[] { 3, 6 }, new[] { 3, 8 },
            new[] { 1, 6 },
        };

        /*
         */

        var output = new[]
        {
            -1, -1, -1, -1, 1, -1, -1, -1, 1, -1, -1, -1, 1, -1, -1, -1, 2, -1, -1, -1, 1, -1, -1, -1, 1, -1, -1, -1, 1,
            -1, -1, -1, 2, -1, -1, -1, 2, -1, -1, -1, 2, -1, -1, -1, 2, -1, -1, -1, 5, -1, -1, -1, 6, -1, -1, -1, 2, -1,
            -1, -1, 3, -1, -1, -1, 3, -1, -1, -1, 14, -1, -1, -1, 3, -1, -1, -1, 1, -1, -1, -1, 2, -1, -1, -1, 2, -1,
            -1, -1, 4, -1, -1, -1, 20, -1, -1, -1, 4, -1, -1, -1, 5, -1, -1, -1, 10, -1, -1, -1, 26, -1, -1, -1, 8, -1,
            -1, -1, 3, -1, -1, -1, 7, -1, -1, -1, 21, -1, -1, -1, 20, -1, -1, -1, 52, -1, -1, -1, 6, -1, -1, -1, 56, -1,
            -1, -1, 2, -1, -1, -1, 5, -1, -1, -1, 17, -1, -1, -1, 18, -1, -1, -1, 13, -1, -1, -1, 19, -1, -1, -1, 102,
            -1, -1, -1, 9, -1, -1, -1, 2, -1, -1, -1, 157, -1, -1, -1, 23, -1, -1, -1, 29, -1, -1, -1, 23, -1, -1, -1,
            15, -1, -1, -1, 24, -1, -1, -1, 186, -1, -1, -1, 12, -1, -1, -1, 32, -1, -1, -1, 36, -1, -1, -1, 10, -1, -1,
            -1, 35, -1, -1, -1, 20, -1, -1, -1, 43, -1, -1, -1, 48, -1, -1, -1, 35, -1, -1, -1, 73, -1, -1, -1, 59, -1,
            -1, -1, 56, -1, -1, -1, 72, -1, -1, -1, 198, -1, -1, -1, 37, -1, -1, -1, 145, -1, -1, -1, 130, -1, -1, -1,
            45, -1, -1, -1, 68, -1, -1, -1, 172, -1, -1, -1, 281, -1, -1, -1, 147, -1, -1, -1, 53, -1, -1, -1, 160, -1,
            -1, -1, 105, -1, -1, -1, 253, -1, -1, -1, 82, -1, -1, -1, 103, -1, -1, -1, 248, -1, -1, -1, 75, -1, -1, -1,
            86, -1, -1, -1, 312, -1, -1, -1, 301, -1, -1, -1, 273, -1, -1, -1, 119, -1, -1, -1, 191, -1, -1, -1, 61, -1,
            -1, -1, 584, -1, -1, -1, 696, -1, -1, -1, 802, -1, -1, -1, 293, -1, -1, -1, 104, -1, -1, -1, 114, -1, -1,
            -1, 242, -1, -1, -1, 259, -1, -1, -1, 300, -1, -1, -1, 465, -1, -1, -1, 180, -1, -1, -1, 1082, -1, -1, -1,
            697, -1, -1, -1, 187, -1, -1, -1, 113, -1, -1, -1, 201, -1, -1, -1, 520, -1, -1, -1, 652, -1, -1, -1, 197,
            -1, -1, -1, 91, -1, -1, -1, 670, -1, -1, -1, 159, -1, -1, -1, 189, -1, -1, -1, 386, -1, -1, -1, 403, -1, -1,
            -1, 204, -1, -1, -1, 301, -1, -1, -1, 378, -1, -1, -1, 314, -1, -1, -1, 292, -1, -1, -1, 352, -1, -1, -1,
            174, -1, -1, -1, 2778, -1, -1, -1, 473, -1, -1, -1, 869, -1, -1, -1, 1568, -1, -1, -1, 190, -1, -1, -1, 198,
            -1, -1, -1, 342, -1, -1, -1, 286, -1, -1, -1, 1062, -1, -1, -1, 475, -1, -1, -1, 354, -1, -1, -1, 174, -1,
            -1, -1, 574, -1, -1, -1, 1605, -1, -1, -1, 547
        };

        DetectSquares? detectSquares = null;
        for (var i = 0; i < commands.Length; i++)
        {
            switch (commands[i])
            {
                case "DetectSquares":
                    detectSquares = new DetectSquares();
                    break;
                case "add":
                    detectSquares!.Add(values[i]);
                    break;
                case "count":
                    var e1 = output[i];
                    var o1 = detectSquares!.Count(values[i]);
                    Assert.Equal(e1, o1);
                    break;
            }
        }
    }

    /*
// Expected: 2, Pass: 2
["DetectSquares","add","add","add","add","count"]
[[],[[8,0]],[[8,0]],[[0,0]],[[8,8]],[[0,8]]]

// Expected: 1, Fail: 0
["DetectSquares","add","add","add","count"]
[[],[[9,0]],[[0,0]],[[9,8]],[[0,8]]]
     
     */

    /**
     * Your DetectSquares object will be instantiated and called as such:
     * DetectSquares obj = new DetectSquares();
     * obj.Add(point);
     * int param_2 = obj.Count(point);
     */
    [Fact]
    public void Example1()
    {
        var commands = new[] { "DetectSquares", "add", "add", "add", "count", "count", "add", "count" };
        var values = new[]
        {
            Array.Empty<int>(), new[] { 3, 10 }, new[] { 11, 2 }, new[] { 3, 2 }, new[] { 11, 10 }, new[] { 14, 8 },
            new[] { 11, 2 }, new[] { 11, 10 }
        };
        var output = new[] { -1, -1, -1, -1, 1, 0, -1, 2 };

        DetectSquares? detectSquares = null;
        for (var i = 0; i < commands.Length; i++)
        {
            switch (commands[i])
            {
                case "DetectSquares":
                    detectSquares = new DetectSquares();
                    break;
                case "add":
                    detectSquares!.Add(values[i]);
                    break;
                case "count":
                    Assert.Equal(output[i], detectSquares!.Count(values[i]));
                    break;
            }
        }
    }

    [Fact]
    public void Test1()
    {
        var square = new DetectSquares();
        square.Add(new[] { 3, 10 });
        square.Add(new[] { 11, 2 });
        square.Add(new[] { 3, 2 });
        Assert.Equal(1, square.Count(new[] { 11, 10 }));
    }

    [Fact]
    public void Test2()
    {
        // ["DetectSquares","add","add","add","add","count"]
        // [[],[[8,0]],[[8,0]],[[0,0]],[[8,8]],[[0,8]]]

        var square = new DetectSquares();
        square.Add(new[] { 8, 0 });
        square.Add(new[] { 8, 0 });
        square.Add(new[] { 0, 0 });
        square.Add(new[] { 8, 8 });
        Assert.Equal(2, square.Count(new[] { 0, 8 }));
    }

    [Fact]
    public void Test3()
    {
        // ["DetectSquares","add","add","add","count"]
        // [[],[[9,0]],[[0,0]],[[9,8]],[[0,8]]]

        var square = new DetectSquares();
        square.Add(new[] { 9, 0 });
        square.Add(new[] { 0, 0 });
        square.Add(new[] { 9, 8 });
        Assert.Equal(0, square.Count(new[] { 0, 8 }));
    }
}