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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HeapChecks;

public class FindAllPeopleWithSecret
{
    public class Solution
    {
        public IList<int> FindAllPeopleOld(int n, int[][] meetings, int firstPerson)
        {
            var knows = new BitArray(n)
            {
                [0] = true,
                [firstPerson] = true
            };

            var last = new int[n];
            for (var i = 0; i < n; i++)
                last[i] = -1;
            last[0] = 0;
            last[firstPerson] = 0;

            foreach (var meet in meetings.OrderBy(m => m[2]))
            {
                var x = meet[0];
                var y = meet[1];
                var t = meet[2];

                if (knows[x] || knows[y])
                {
                    knows[x] = true;
                    knows[y] = true;

                    while (last[x] == t)
                    {
                        break;
                    }

                    while (last[y] == t)
                    {
                        break;
                    }

                    last[x] = t;
                    last[y] = t;
                    continue;
                }

                if (last[x] >= 0 || last[y] >= 0)
                {
                    var lx = last[x];
                    var ly = last[y];

                    continue;
                }

                last[x] = t;
                last[y] = t;
                ;
            }

            var results = new List<int>();
            for (var i = 0; i < n; i++)
                if (knows[i])
                    results.Add(i);
            return results.ToArray();
        }

        public IList<int> FindAllPeople1(int n, int[][] meetings, int firstPerson)
        {
            var knows = new HashSet<int>(n)
            {
                0,
                firstPerson
            };

            var time = 0;
            var pool = new List<int[]>();
            foreach (var meet in meetings.OrderBy(m => m[2]))
            {
                var x = meet[0];
                var y = meet[1];
                var t = meet[2];

                if (time != t)
                {
                    time = t;
                    while (pool.Count > 0)
                    {
                        var found = false;
                        foreach (var m1 in pool)
                        {
                            var x1 = m1[0];
                            var y1 = m1[1];
                            if (!knows.Contains(x1) && !knows.Contains(y1)) continue;
                            knows.Add(x1);
                            knows.Add(y1);
                            found = true;
                            pool.Remove(m1);
                            break;
                        }

                        if (!found)
                            break;
                    }

                    pool.Clear();
                }

                if (knows.Contains(x) || knows.Contains(y))
                {
                    knows.Add(x);
                    knows.Add(y);
                }
                else
                {
                    pool.Add(meet);
                }
            }

            return knows.ToArray();
        }

        public IList<int> FindAllPeople(int n, int[][] meetings, int firstPerson)
        {
            var knows = new HashSet<int>(n)
            {
                0,
                firstPerson
            };

            var time = 0;
            var pool = new List<HashSet<int>>();
            foreach (var meet in meetings.OrderBy(m => m[2]))
            {
                var t = meet[2];
                if (time != t)
                {
                    time = t;
                    foreach (var j in pool.Where(m1 => m1.Any(i => knows.Contains(i))).SelectMany(m1 => m1))
                        knows.Add(j);

                    pool.Clear();
                }

                var x = meet[0];
                var y = meet[1];
                if (knows.Contains(x) || knows.Contains(y))
                {
                    knows.Add(x);
                    knows.Add(y);
                }
                else
                {
                    HashSet<int> cx = null;
                    HashSet<int> cy = null;
                    foreach (var pp in pool)
                    {
                        if (pp.Contains(x)) cx = pp;
                        if (pp.Contains(y)) cy = pp;
                    }

                    if (cx == null && cy == null)
                    {
                        pool.Add(new HashSet<int> { x, y });
                        continue;
                    }

                    if (cx == cy)
                        continue;

                    if (cx != null && cy != null)
                    {
                        pool.Remove(cy);
                        cx.UnionWith(cy);
                        continue;
                    }

                    cx?.Add(y);
                    cy?.Add(x);
                }
            }

            foreach (var j in pool.Where(m1 => m1.Any(i => knows.Contains(i))).SelectMany(m1 => m1))
                knows.Add(j);

            return knows.ToArray();
        }
    }

    [Fact]
    public void Answer1()
    {
        var n = 5;
        var meetings = new[] { new[] { 1, 4, 3 }, new[] { 0, 4, 3 } };
        var firstPerson = 3;
        var solution = new Solution();
        Assert.Equal(new[] { 0, 1, 3, 4 }, solution.FindAllPeople(n, meetings, firstPerson).OrderBy(t => t));
    }

    [Fact]
    public void Example1()
    {
        var n = 6;
        var meetings = new[] { new[] { 1, 2, 5 }, new[] { 2, 3, 8 }, new[] { 1, 5, 10 } };
        var firstPerson = 1;
        var solution = new Solution();
        Assert.Equal(new[] { 0, 1, 2, 3, 5 }, solution.FindAllPeople(n, meetings, firstPerson).OrderBy(t => t));
    }

    [Fact]
    public void Example2()
    {
        var n = 4;
        var meetings = new[] { new[] { 3, 1, 3 }, new[] { 1, 2, 2 }, new[] { 0, 3, 3 } };
        var firstPerson = 3;
        var solution = new Solution();
        Assert.Equal(new[] { 0, 1, 3 }, solution.FindAllPeople(n, meetings, firstPerson).OrderBy(t => t));
    }

    [Fact]
    public void Example3()
    {
        var n = 5;
        var meetings = new[] { new[] { 3, 4, 2 }, new[] { 1, 2, 1 }, new[] { 2, 3, 1 } };
        var firstPerson = 1;
        var solution = new Solution();
        Assert.Equal(new[] { 0, 1, 2, 3, 4 }, solution.FindAllPeople(n, meetings, firstPerson).OrderBy(t => t));
    }

    [Fact]
    public void Test1()
    {
        var n = 7;
        var meetings = new[] { new[] { 3, 2, 1 }, new[] { 2, 1, 1 }, new[] { 5, 4, 2 }, new[] { 5, 6, 1 } };
        var firstPerson = 1;
        var solution = new Solution();
        Assert.Equal(new[] { 0, 1, 2, 3 }, solution.FindAllPeople(n, meetings, firstPerson).OrderBy(t => t));
    }
}