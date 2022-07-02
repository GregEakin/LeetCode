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
using Xunit;

namespace HeapChecks;

public class SuccessfulPairsOfSpellsAndPotions
{
public class Solution
{
    public int[] SuccessfulPairs(int[] spells, int[] potions, long success)
    {
        var n = spells.Length;
        var answers = new int[n];

        Array.Sort(potions);
        for (var i = 0; i < n; i++)
        {
            var div = (int)(success / spells[i]);
            var mod = (int)(success % spells[i]);
            var index = Array.BinarySearch(potions, div);
            if (index < 0)
                index = ~index;
            else if (mod > 0) 
                index++;

            answers[i] = potions.Length - index;
        }

        return answers;
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
}