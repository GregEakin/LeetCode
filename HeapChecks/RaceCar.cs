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

public class RaceCar
{
    public class SimpleSolution
    {
        public int Racecar(int target)
        {
            var position = 0;
            var speed = 1;
            var cmds = 0;

            while (true)
            {
                var diff = target - position;
                if (diff == 0)
                    return cmds;

                cmds++;
                if ((diff < 0) ^ (speed < 0))
                {
                    speed = speed > 0 ? -1 : 1;
                    continue;
                }

                position += speed;
                speed *= 2;
            }
        }
    }

    // A* Algorithm
    public class SolutionGood
    {
        public class RaceState
        {
            public RaceState(int position, int speed, int count)
            {
                Position = position;
                Speed = speed;
                Count = count;
            }

            public int Position { get; }
            public int Speed { get; }
            public int Count { get; }
            public override bool Equals(object? obj) => obj is RaceState state && Equals(state);
            protected bool Equals(RaceState other) => Position == other.Position && Speed == other.Speed;
            public override int GetHashCode() => HashCode.Combine(Position, Speed);

            public int Estimation(int target) =>
                Count; // + (int)Math.Log2(Math.Abs((double)(target - Position) / Speed) + 1.0);
        }

        public int Racecar(int target)
        {
            var count = 0;
            var queue = new PriorityQueue<RaceState, int>();
            var seen = new HashSet<RaceState>();

            var start = new RaceState(0, 1, 0);
            queue.Enqueue(start, start.Estimation(target));
            while (queue.Count > 0)
            {
                count++;
                var curr = queue.Dequeue();
                var dist = target - curr.Position;
                if (dist == 0) return curr.Count;

                var fp = new Func<RaceState, (int, int)>[]
                {
                    state => (state.Position + state.Speed, 2 * state.Speed),
                    state => (state.Position, state.Speed > 0 ? -1 : 1),
                };

                foreach (var func in fp)
                {
                    var (p, s) = func(curr);
                    if (Math.Abs(p) > Math.Abs(2 * target)) continue;
                    var next = new RaceState(p, s, curr.Count + 1);
                    if (seen.Contains(next)) continue;
                    seen.Add(next);
                    queue.Enqueue(next, next.Estimation(target));
                }
            }

            return 0;
        }
    }

    public class Solution
    {
        public int Racecar(int target)
        {
            Queue<(int, int)> q = new();
            q.Enqueue((0, 1));
            var action = 0;

            while (q.Count > 0)
            {
                var s = q.Count;
                while (s-- > 0)
                {
                    var (pos, speed) = q.Dequeue();
                    if (pos == target) return action;

                    // simulate A
                    q.Enqueue((pos + speed, speed * 2));

                    // simulate R - only if we are "over"
                    if (speed > 0 && pos + speed > target
                        || speed < 0 && pos + speed < target)
                        q.Enqueue((pos, speed > 0 ? -1 : 1));
                }

                action++;
            }

            return -1;
        }
    }

    [Fact]
    public void Example1()
    {
        // Instructions: "AA"
        // Position: 0 --> 1 --> 3
        // Speed:    1 --> 2 --> 4
        var solution = new Solution();
        Assert.Equal(2, solution.Racecar(3));
    }

    [Fact]
    public void Example2()
    {
        // Instructions: "AAARA"
        // Position: 0 --> 1 --> 3 --> 7 -->  7 -->  6
        // Speed:    1 --> 2 --> 4 --> 8 --> -1 --> -2
        var solution = new Solution();
        Assert.Equal(5, solution.Racecar(6));
    }

    [Fact]
    public void Test2()
    {
        // Instructions: "AARA"
        // Position: 0 --> 1 --> 3 -->  3 -->  2
        // Speed:    1 --> 2 --> 4 --> -1 --> -2

        var solution = new Solution();
        Assert.Equal(4, solution.Racecar(2));
    }

    [Fact]
    public void Test4()
    {
        // Instructions: "AAARAA"
        // Position: 0 --> 1 --> 3 --> 7 -->  7 -->  6 -->  4
        // Speed:    1 --> 2 --> 4 --> 8 --> -1 --> -2 --> -4

        // Instructions: "AARRA"
        // Position: 0 --> 1 --> 3 -->  3 --> 3 --> 4
        // Speed:    1 --> 2 --> 4 --> -1 --> 1 --> 2

        var solution = new Solution();
        Assert.Equal(5, solution.Racecar(4));
    }

    [Fact]
    public void Test5()
    {
        // Instructions: "AARARAA"
        // Position: 0 --> 1 --> 3 -->  3 -->  2 --> 2 --> 3 --> 5
        // Speed:    1 --> 2 --> 4 --> -1 --> -2 --> 1 --> 2 --> 4

        var solution = new Solution();
        Assert.Equal(7, solution.Racecar(5));
    }

    [Fact]
    public void TestMinus5()
    {
        // Instructions: "AARARAA"
        // Position: 0 --> 1 --> 3 -->  3 -->  2 --> 2 --> 3 --> 5
        // Speed:    1 --> 2 --> 4 --> -1 --> -2 --> 1 --> 2 --> 4

        var solution = new Solution();
        Assert.Equal(7, solution.Racecar(-5));
    }

    [Fact]
    public void Answer1()
    {
        var solution = new Solution();
        Assert.Equal(39, solution.Racecar(5363));
    }

    [Fact]
    public void Answer2()
    {
        var solution = new Solution();
        Assert.Equal(32, solution.Racecar(812));
    }
}