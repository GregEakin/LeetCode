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

using Xunit;

namespace HeapChecks;

public class CouplesHoldingHands
{
    public class Solution
    {
        public int MinSwapsCouples(int[] row)
        {
            var loc = new int[row.Length];
            for (var i = 0; i < row.Length / 2; i++)
            {
                var side1 = row[2 * i];
                var side2 = row[2 * i + 1];

                loc[side1] = i;
                loc[side2] = i;
            }

            var swaps = 0;
            for (var i = 0; i < row.Length / 2; i++)
            {
                if (loc[2 * i] == loc[2 * i + 1]) continue;

                for (var j = i + 1; j < row.Length / 2; j++)
                {
                    if (loc[2 * i] == loc[2 * j])
                    {
                        swaps++;
                        (loc[2 * i + 1], loc[2 * j]) = (loc[2 * j], loc[2 * i + 1]);
                        break;
                    }

                    if (loc[2 * i] == loc[2 * j + 1])
                    {
                        swaps++;
                        (loc[2 * i + 1], loc[2 * j + 1]) = (loc[2 * j + 1], loc[2 * i + 1]);
                        break;
                    }
                }
            }

            return swaps;
        }
    }

    [Fact]
    public void Example1()
    {
        var row = new[] { 0, 2, 1, 3 };
        var solution = new Solution();
        Assert.Equal(1, solution.MinSwapsCouples(row));
    }

    [Fact]
    public void Example2()
    {
        var row = new[] { 3, 2, 0, 1 };
        var solution = new Solution();
        Assert.Equal(0, solution.MinSwapsCouples(row));
    }

    [Fact]
    public void Test1()
    {
        var row = new[] { 4, 3, 2, 0, 1, 5 };
        var solution = new Solution();
        Assert.Equal(2, solution.MinSwapsCouples(row));
    }

    [Fact]
    public void Test2()
    {
        var row = new[] { 0, 3, 2, 5, 4, 7, 6, 1 };
        var solution = new Solution();
        Assert.Equal(3, solution.MinSwapsCouples(row));
    }

    [Fact]
    public void Test3()
    {
        var row = new[] { 0, 7, 2, 1, 4, 3, 6, 5 };
        var solution = new Solution();
        Assert.Equal(3, solution.MinSwapsCouples(row));
    }

    [Fact]
    public void Answer1()
    {
        //                      *  *  *  *  *  *
        //                0, 0, 1, 1, 2, 2, 3, 3
        var row = new[] { 5, 4, 2, 6, 3, 1, 0, 7 };
        var solution = new Solution();
        Assert.Equal(2, solution.MinSwapsCouples(row));
    }
}