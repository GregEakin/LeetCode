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
using Xunit;

namespace HeapChecks;

public class BulbSwitcherII
{
    public class Solution
    {
        private readonly HashSet<ulong> _statuses = new();
        private int _n;
        private int _presses;

        public ulong Button1(ulong bulbs)
        {
            for (var i = 0; i < _n; i++)
                bulbs ^= 1UL << i;
            return bulbs;
        }

        public ulong Button2(ulong bulbs)
        {
            for (var i = 0; i < _n; i += 2)
                bulbs ^= 1UL << i;
            return bulbs;
        }

        public ulong Button3(ulong bulbs)
        {
            for (var i = 1; i < _n; i += 2)
                bulbs ^= 1UL << i;
            return bulbs;
        }

        public ulong Button4(ulong bulbs)
        {
            for (var i = 0; i < _n; i += 3)
                bulbs ^= 1UL << i;
            return bulbs;
        }

        public ulong Press(int button, ulong bulbs, Stack<int> stack)
        {
            switch (button)
            {
                case 0:
                    break;
                case 1:
                    bulbs = Button1(bulbs);
                    break;
                case 2:
                    bulbs = Button2(bulbs);
                    break;
                case 3:
                    bulbs = Button3(bulbs);
                    break;
                case 4:
                    bulbs = Button4(bulbs);
                    break;
            }

            if (stack.Count >= _presses) return bulbs;

            for (var i = 1; i < 5; i++)
            {
                stack.Push(i);
                var b = Press(i, bulbs, stack);
                if (stack.Count >= _presses)
                    _statuses.Add(b);
                stack.Pop();
            }

            return bulbs;
        }

        public int FlipLights(int n, int presses)
        {
            if (presses <= 0) return 1;
            _statuses.Clear();
            _n = n;
            _presses = presses;

            var stack = new Stack<int>();
            var bulbs = 0x0UL;
            // for (var i = 0; i < n; i++)
            //     bulbs |= 1UL << i;
            Press(0, bulbs, stack);

            return _statuses.Count;
        }

        public int FlipLights11(int n, int presses)
        {
            return presses switch
            {
                <= 0 => 1,
                1 => n switch
                {
                    1 => 2,
                    2 => 3,
                    _ => 4
                },
                2 => n switch
                {
                    1 => 2,
                    2 => 4,
                    _ => 7
                },
                _ => n switch
                {
                    1 => 2,
                    2 => 4,
                    _ => 8
                }
            };
        }
    }

    [Fact]
    public void Answer1()
    {
        var solution = new Solution();
        Assert.Equal(1, solution.FlipLights(1, 0));
    }

    [Fact]
    public void Answer2()
    {
        var solution = new Solution();
        Assert.Equal(7, solution.FlipLights(3, 2));
    }

    [Fact]
    public void Example1()
    {
        var solution = new Solution();
        Assert.Equal(2, solution.FlipLights(1, 1));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal(3, solution.FlipLights(2, 1));
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.Equal(4, solution.FlipLights(3, 1));
    }

    [Fact]
    public void ZeroButton()
    {
        var solution = new Solution();
        Assert.Equal(1, solution.FlipLights(1, 0));
        Assert.Equal(1, solution.FlipLights(2, 0));
        Assert.Equal(1, solution.FlipLights(3, 0));
        Assert.Equal(1, solution.FlipLights(4, 0));
        for (var i = 5; i < 32; i++)
            Assert.Equal(1, solution.FlipLights(i, 0));
    }

    [Fact]
    public void OneButton()
    {
        var solution = new Solution();
        Assert.Equal(2, solution.FlipLights(1, 1));
        Assert.Equal(3, solution.FlipLights(2, 1));
        Assert.Equal(4, solution.FlipLights(3, 1));
        Assert.Equal(4, solution.FlipLights(4, 1));
        for (var i = 5; i < 32; i++)
            Assert.Equal(4, solution.FlipLights(i, 1));
    }

    [Fact]
    public void TwoButtons()
    {
        var solution = new Solution();
        Assert.Equal(2, solution.FlipLights(1, 2));
        Assert.Equal(4, solution.FlipLights(2, 2));
        Assert.Equal(7, solution.FlipLights(3, 2));
        Assert.Equal(7, solution.FlipLights(4, 2));
        for (var i = 5; i < 32; i++)
            Assert.Equal(7, solution.FlipLights(i, 2));
    }

    [Fact]
    public void ThreeButtons()
    {
        var solution = new Solution();
        Assert.Equal(2, solution.FlipLights(1, 3));
        Assert.Equal(4, solution.FlipLights(2, 3));
        Assert.Equal(8, solution.FlipLights(3, 3));
        Assert.Equal(8, solution.FlipLights(4, 3));
        for (var i = 5; i < 32; i++)
            Assert.Equal(8, solution.FlipLights(i, 3));
    }

    [Fact]
    public void FourButtons()
    {
        var solution = new Solution();
        Assert.Equal(2, solution.FlipLights(1, 4));
        Assert.Equal(4, solution.FlipLights(2, 4));
        Assert.Equal(8, solution.FlipLights(3, 4));
        Assert.Equal(8, solution.FlipLights(4, 4));
        for (var i = 5; i < 32; i++)
            Assert.Equal(8, solution.FlipLights(i, 4));
    }

    [Fact]
    public void FiveButtons()
    {
        var solution = new Solution();
        Assert.Equal(2, solution.FlipLights(1, 5));
        Assert.Equal(4, solution.FlipLights(2, 5));
        Assert.Equal(8, solution.FlipLights(3, 5));
        Assert.Equal(8, solution.FlipLights(4, 5));
        for (var i = 5; i < 32; i++)
            Assert.Equal(8, solution.FlipLights(i, 5));
    }
}