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

public class MaximumNumberVisiblePoints
{
    public class Solution
    {
        public int VisiblePoints(IList<IList<int>> points, int angle, IList<int> location)
        {
            var pointAngles = new List<double>(points.Count);
            var onPoint = 0;
            foreach (var point in points)
            {
                if (point[1] == location[1] && point[0] == location[0])
                {
                    onPoint++;
                    continue;
                }

                var x = (double)(point[0] - location[0]);
                var y = (double)(point[1] - location[1]);
                var theta = x >= 0.0
                    ? Math.Atan(y / x) * 180.0 / Math.PI
                    : Math.Atan(y / x) * 180.0 / Math.PI + 180.0;
                pointAngles.Add(theta);
            }

            pointAngles.Sort();

            var max = 0;
            for (int i = 0, j = 1; i < pointAngles.Count; i++)
            {
                if (i == j) j++;
                while (j - i < pointAngles.Count)
                {
                    var diff = (pointAngles[j % pointAngles.Count] - pointAngles[i] + 360) % 360;
                    if (diff > angle) break;
                    j++;
                }

                if (max < j - i) max = j - i;
            }

            return onPoint + max;
        }
    }

    [Fact]
    public void Answer1()
    {
        var points = new List<IList<int>>
            { new List<int> { 1, 1 }, new List<int> { 2, 2 }, new List<int> { 1, 2 }, new List<int> { 2, 1 } };
        var angle = 0;
        var location = new List<int> { 1, 1 };
        var solution = new Solution();
        Assert.Equal(2, solution.VisiblePoints(points, angle, location));
    }

    [Fact]
    public void Answer2()
    {
        var points = new List<IList<int>>
        {
            new List<int> { 20, 22 },
            new List<int> { 71, 38 },
            new List<int> { 65, 69 },
            new List<int> { 63, 69 },
            new List<int> { 80, 2 },
            new List<int> { 67, 31 },
            new List<int> { 65, 81 },
            new List<int> { 4, 58 },
            new List<int> { 46, 60 },
            new List<int> { 32, 20 },
            new List<int> { 29, 86 },
            new List<int> { 74, 73 },
            new List<int> { 3, 67 },
            new List<int> { 26, 0 },
            new List<int> { 71, 33 },
            new List<int> { 76, 84 },
            new List<int> { 63, 4 },
            new List<int> { 36, 12 },
            new List<int> { 28, 99 },
            new List<int> { 27, 85 },
            new List<int> { 94, 56 },
            new List<int> { 32, 78 },
            new List<int> { 56, 49 },
            new List<int> { 63, 27 },
            new List<int> { 41, 21 },
            new List<int> { 91, 96 },
            new List<int> { 34, 37 },
            new List<int> { 9, 24 },
            new List<int> { 59, 51 },
            new List<int> { 82, 6 },
            new List<int> { 94, 38 },
            new List<int> { 70, 87 },
            new List<int> { 24, 88 },
            new List<int> { 42, 18 },
            new List<int> { 57, 46 },
            new List<int> { 69, 47 },
            new List<int> { 10, 1 },
            new List<int> { 34, 67 },
            new List<int> { 55, 99 },
            new List<int> { 81, 23 },
            new List<int> { 12, 63 },
            new List<int> { 24, 75 },
            new List<int> { 39, 5 },
            new List<int> { 41, 42 },
            new List<int> { 70, 70 },
            new List<int> { 7, 86 },
            new List<int> { 94, 45 },
            new List<int> { 28, 81 },
            new List<int> { 22, 14 },
            new List<int> { 80, 87 },
            new List<int> { 2, 10 },
            new List<int> { 26, 88 },
            new List<int> { 64, 72 },
            new List<int> { 92, 69 },
            new List<int> { 74, 58 },
            new List<int> { 44, 38 },
            new List<int> { 59, 53 },
            new List<int> { 10, 67 },
            new List<int> { 59, 21 },
            new List<int> { 17, 54 },
            new List<int> { 51, 89 },
            new List<int> { 8, 37 },
            new List<int> { 40, 72 },
            new List<int> { 71, 31 },
            new List<int> { 93, 5 },
            new List<int> { 57, 88 },
            new List<int> { 60, 21 },
            new List<int> { 47, 40 },
            new List<int> { 44, 49 },
            new List<int> { 16, 14 },
            new List<int> { 84, 37 },
            new List<int> { 38, 1 },
            new List<int> { 29, 81 },
            new List<int> { 79, 38 },
            new List<int> { 91, 21 },
            new List<int> { 4, 42 },
            new List<int> { 86, 45 },
            new List<int> { 62, 81 },
            new List<int> { 29, 69 },
            new List<int> { 22, 71 },
            new List<int> { 45, 10 },
            new List<int> { 28, 80 },
            new List<int> { 43, 71 },
            new List<int> { 25, 87 },
            new List<int> { 8, 87 },
            new List<int> { 89, 42 },
            new List<int> { 76, 69 },
            new List<int> { 97, 9 },
            new List<int> { 3, 26 },
            new List<int> { 81, 19 },
            new List<int> { 5, 36 },
            new List<int> { 31, 100 },
            new List<int> { 40, 31 },
            new List<int> { 23, 12 },
            new List<int> { 23, 45 },
        };
        var angle = 26;
        var location = new List<int> { 61, 94 };
        var solution = new Solution();
        Assert.Equal(27, solution.VisiblePoints(points, angle, location));
    }

    [Fact]
    public void Example1()
    {
        var points = new List<IList<int>> { new List<int> { 2, 1 }, new List<int> { 2, 2 }, new List<int> { 3, 3 } };
        var angle = 90;
        var location = new List<int> { 1, 1 };
        var solution = new Solution();
        Assert.Equal(3, solution.VisiblePoints(points, angle, location));
    }

    [Fact]
    public void Example2()
    {
        var points = new List<IList<int>>
            { new List<int> { 2, 1 }, new List<int> { 2, 2 }, new List<int> { 3, 4 }, new List<int> { 1, 1 } };
        var angle = 90;
        var location = new List<int> { 1, 1 };
        var solution = new Solution();
        Assert.Equal(4, solution.VisiblePoints(points, angle, location));
    }

    [Fact]
    public void Example3()
    {
        var points = new List<IList<int>> { new List<int> { 1, 0 }, new List<int> { 2, 1 } };
        var angle = 13;
        var location = new List<int> { 1, 1 };
        var solution = new Solution();
        Assert.Equal(1, solution.VisiblePoints(points, angle, location));
    }

    [Fact]
    public void Test1()
    {
        var points = new List<IList<int>> { new List<int> { 1, 1 } };
        var angle = 90;
        var location = new List<int> { 1, 1 };
        var solution = new Solution();
        Assert.Equal(1, solution.VisiblePoints(points, angle, location));
    }

    [Fact]
    public void Test2()
    {
        var points = new List<IList<int>> { new List<int> { 1, 2 } };
        var angle = 90;
        var location = new List<int> { 1, 1 };
        var solution = new Solution();
        Assert.Equal(1, solution.VisiblePoints(points, angle, location));
    }

    [Fact]
    public void Test3()
    {
        var points = new List<IList<int>>
            { new List<int> { 4, 0 }, new List<int> { 4, 1 }, new List<int> { 4, 2 }, new List<int> { 0, 1 } };
        var angle = 90;
        var location = new List<int> { 1, 1 };
        var solution = new Solution();
        Assert.Equal(3, solution.VisiblePoints(points, angle, location));
    }

    [Fact]
    public void Test4()
    {
        var points = new List<IList<int>>
            { new List<int> { 2, 1 }, new List<int> { 1, 2 }, new List<int> { 0, 1 }, new List<int> { 1, 0 } };
        var angle = 89;
        var location = new List<int> { 1, 1 };
        var solution = new Solution();
        Assert.Equal(1, solution.VisiblePoints(points, angle, location));
    }
}