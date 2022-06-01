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

public class BookingConcertTicketsInGroups
{
    public class BookMyShow
    {
        private readonly int _n;
        private readonly int _m;
        private readonly int[] _seats;

        public BookMyShow(int n, int m)
        {
            _n = n;
            _m = m;
            _seats = new int[_n];
        }

        public int[] Gather(int k, int maxRow)
        {
            if (k > _m) return Array.Empty<int>();
            for (var i = 0; i <= maxRow && i < _n; i++)
            {
                if (_seats[i] + k > _m) continue;
                var first = _seats[i];
                _seats[i] += k;
                return new [] { i, first };
            }

            return Array.Empty<int>();
        }

        public bool Scatter(int k, int maxRow)
        {
            var count = 0;
            for (var i = 0; i <= maxRow && i < _n && count < k; i++)
                count += _m - _seats[i];

            if (count < k)
                return false;

            for (var i = 0; i < _n && k > 0; i++)
            {
                var space = Math.Min(k, _m - _seats[i]);
                k -= space;
                _seats[i] += space;
            }

            return true;
        }
    }

    /**
     * Your BookMyShow object will be instantiated and called as such:
     * BookMyShow obj = new BookMyShow(n, m);
     * int[] param_1 = obj.Gather(k,maxRow);
     * bool param_2 = obj.Scatter(k,maxRow);
     */
    [Fact]
    public void Example1()
    {
        var input = new[] { "BookMyShow", "gather", "gather", "scatter", "scatter" };
        var data = new[] { (2, 5), (4, 0), (2, 0), (5, 1), (5, 1) };
        var output = new object?[] { null, new[] { 0, 0 }, Array.Empty<int>(), true, false };

        NewMethod(input, data, output);
    }

    private static void NewMethod(string[] input, (int, int)[] data, object?[] output)
    {
        var show = (BookMyShow?)null;
        for (var i = 0; i < input.Length; i++)
        {
            switch (input[i])
            {
                case "BookMyShow":
                    show = new BookMyShow(data[i].Item1, data[i].Item2);
                    break;
                case "gather":
                    Assert.Equal(output[i], show!.Gather(data[i].Item1, data[i].Item2));
                    break;
                case "scatter":
                    Assert.Equal(output[i], show!.Scatter(data[i].Item1, data[i].Item2));
                    break;
            }
        }
    }

    [Fact]
    public void Answer1()
    {
        var input = new[] { "BookMyShow", "gather", "scatter", "gather", "gather", "gather" };
        var data = new[] { (5, 9), (10, 1), (3, 3), (9, 1), (10, 2), (2, 0) };
        var output = new object?[]
            { null, Array.Empty<int>(), true, new[] { 1, 0 }, Array.Empty<int>(), new[] { 0, 3 } };

        NewMethod(input, data, output);
    }
}