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

public class FindPlayersWithZeroOrOneLosses
{
    public class Solution
    {
        public IList<IList<int>> FindWinners(int[][] matches)
        {
            var players = new Dictionary<int, (int, int)>();

            foreach (var pair in matches)
            {
                if (!players.ContainsKey(pair[0])) players[pair[0]] = (0, 0);
                if (!players.ContainsKey(pair[1])) players[pair[1]] = (0, 0);

                var (w1, l1) = players[pair[0]];
                players[pair[0]] = (w1 + 1, l1);

                var (w2, l2) = players[pair[1]];
                players[pair[1]] = (w2, l2 + 1);
            }

            var zeroLosses = new List<int>();
            var oneLoss = new List<int>();

            foreach (var (player, (win, loss)) in players.OrderBy(t => t.Key))
            {
                if (loss == 0) zeroLosses.Add(player);
                if (loss == 1) oneLoss.Add(player);
            }

            return new List<IList<int>> { zeroLosses, oneLoss };
        }
    }

    [Fact]
    public void Example1()
    {
        var matches = new[]
        {
            new[] { 1, 3 }, new[] { 2, 3 }, new[] { 3, 6 }, new[] { 5, 6 }, new[] { 5, 7 }, new[] { 4, 5 },
            new[] { 4, 8 }, new[] { 4, 9 }, new[] { 10, 4 }, new[] { 10, 9 }
        };
        var output = new[] { new[] { 1, 2, 10 }, new[] { 4, 5, 7, 8 } };
        var solution = new Solution();
        Assert.Equal(output, solution.FindWinners(matches));
    }

    [Fact]
    public void Example2()
    {
        var matches = new[]
        {
            new[] { 2, 3 }, new[] { 1, 3 }, new[] { 5, 4 }, new[] { 6, 4 }
        };
        var output = new[] { new[] { 1, 2, 5, 6 }, Array.Empty<int>() };
        var solution = new Solution();
        Assert.Equal(output, solution.FindWinners(matches));
    }
}