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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class MinimumTimeDifference
{
    public class SolutionForce
    {
        public static int Minutes(string time)
        {
            var parts = time.Split(':');
            var hours = int.Parse(parts[0]);
            var minutes = int.Parse(parts[1]);
            return hours * 60 + minutes;
        }

        public int FindMinDifference(IList<string> timePoints)
        {
            var minutes = timePoints.Select(Minutes).ToArray();

            var min = int.MaxValue;
            for (var i = 0; i < minutes.Length; i++)
            for (var j = i + 1; j < minutes.Length; j++)
            {
                var diff = Math.Abs(minutes[i] - minutes[j]);
                if (diff > 12 * 60) diff = 24 * 60 - diff;
                if (min > diff) min = diff;
            }

            return min;
        }
    }

    public class Solution
    {
        private const int Max = 24 * 60;

        public static int Minutes(string time)
        {
            var parts = time.Split(':');
            var hours = int.Parse(parts[0]);
            var minutes = int.Parse(parts[1]);
            return hours * 60 + minutes;
        }

        public int FindMinDifference(IList<string> timePoints)
        {
            var freq = new int[Max];
            foreach (var timePoint in timePoints)
            {
                var minutes = Minutes(timePoint);
                freq[minutes]++;

                if (freq[minutes] > 1)
                    return 0;
            }

            var mn = int.MaxValue;
            for (var i = 0; i < Max; i++)
            {
                if (freq[i % Max] <= 0)
                    continue;

                var j = i + 1;
                var cnt = 1;
                while (i != j % Max && freq[j % Max] == 0)
                {
                    cnt++;
                    j++;
                }

                i += cnt - 1;

                mn = Math.Min(cnt, mn);
            }

            return mn;
        }
    }


    [Fact]
    public void Example1()
    {
        var timePoints = new[] { "23:59", "00:00" };
        var solution = new Solution();
        Assert.Equal(1, solution.FindMinDifference(timePoints));
    }

    [Fact]
    public void Example2()
    {
        var timePoints = new[] { "00:00", "23:59", "00:00" };
        var solution = new Solution();
        Assert.Equal(0, solution.FindMinDifference(timePoints));
    }

    [Fact]
    public void Test1()
    {
        var timePoints = new[] { "00:00", "23:59" };
        var solution = new Solution();
        Assert.Equal(1, solution.FindMinDifference(timePoints));
    }

    [Fact]
    public void Answer1()
    {
        var timePoints = new[] { "01:01", "02:01", "03:00" };
        var solution = new Solution();
        Assert.Equal(59, solution.FindMinDifference(timePoints));
    }
}