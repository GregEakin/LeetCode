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

public class SuccessfulPairsOfSpellsAndPotions
{
    public class Solution
    {
        public int[] SuccessfulPairs(int[] spells, int[] potions, long success)
        {
            Array.Sort(potions);
            var answers = new List<int>();
            foreach (long spell in spells)
            {
                var low = 0; 
                var high = potions.Length - 1;
                while (low <= high)
                {
                    var mid = low + (high - low) / 2;
                    if (potions[mid] * spell >= success)
                        high = mid - 1;
                    else
                        low = mid + 1;
                }

                answers.Add(potions.Length - low);
            }
            
            return answers.ToArray();
        }
    }

    [Fact]
    public void Example1()
    {
        var spells = new[] { 5, 1, 3 };
        var potions = new[] { 1, 2, 3, 4, 5 };
        var solution = new Solution();
        Assert.Equal(new[] { 4, 0, 3 }, solution.SuccessfulPairs(spells, potions, 7));
    }

    [Fact]
    public void Example2()
    {
        var spells = new[] { 3, 1, 2 };
        var potions = new[] { 8, 5, 8 };
        var solution = new Solution();
        Assert.Equal(new[] { 2, 0, 2 }, solution.SuccessfulPairs(spells, potions, 16));
    }

    [Fact]
    public void Test1()
    {
        var spells = new[] { 5, 1, 3 };
        var potions = new[] { 1, 2, 3, 4, 5 };
        var solution = new Solution();
        Assert.Equal(new[] { 5, 1, 4 }, solution.SuccessfulPairs(spells, potions, 5));
    }

    [Fact]
    public void Answer1()
    {
        var spells = new[] { 1, 2, 3, 4, 5, 6, 7 };
        var potions = new[] { 1, 2, 3, 4, 5, 6, 7 };
        var solution = new Solution();
        Assert.Equal(new[] { 0, 0, 0, 1, 3, 3, 4 }, solution.SuccessfulPairs(spells, potions, 25));
    }

    [Fact]
    public void Answer2()
    {
        var spells = new[] { 15, 8, 19 };
        var potions = new[] { 38, 36, 23 };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 0, 3 }, solution.SuccessfulPairs(spells, potions, 328));
    }

    [Fact]
    public void Answer3()
    {
        var spells = new[]
        {
            13, 22, 21, 13, 11, 9, 13, 35, 7, 38, 10, 10, 38, 19, 3, 16, 13, 24, 16, 27, 20, 24, 32, 5, 16, 35, 24, 2,
            25, 32, 20, 22, 22, 3, 35, 39, 27, 26, 25, 21, 27, 40, 15, 17, 24, 40, 35, 27, 20, 40, 9, 35, 27, 19, 15,
            34, 35, 37, 17, 40, 8, 3, 33, 39, 29, 22, 30, 1, 37, 2, 16, 30, 32, 31, 24, 6, 34, 26, 36, 4, 21, 2, 29, 31,
            3, 27, 6, 24, 40, 18
        };
        var potions = new[]
        {
            33, 16, 35, 14, 26, 23, 23, 2, 37, 23, 15, 20, 25, 34, 23, 29, 4, 18, 26, 24, 16, 37, 15, 11, 33, 24, 12,
            13, 7, 24, 3, 26, 1, 3, 38, 33, 19, 3, 34, 22, 30, 39, 18, 7, 21, 4, 33, 18, 39, 5, 34, 14, 32, 5, 20, 22,
            5, 25, 15
        };
        var solution = new Solution();
        Assert.Equal(new[]
        {
            0, 21, 19, 0, 0, 0, 0, 39, 0, 42, 0, 0, 42, 16, 0, 9, 0, 28, 9, 33, 16, 28, 37, 0, 9, 39, 28, 0, 30, 37, 16,
            21, 21, 0, 39, 44, 33, 31, 30, 19, 33, 44, 5, 14, 28, 44, 39, 33, 16, 44, 0, 39, 33, 16, 5, 39, 39, 42, 14,
            44, 0, 0, 37, 44, 34, 21, 37, 0, 42, 0, 9, 37, 37, 37, 28, 0, 39, 31, 42, 0, 19, 0, 34, 37, 0, 33, 0, 28,
            44, 15
        }, solution.SuccessfulPairs(spells, potions, 533));
    }
}