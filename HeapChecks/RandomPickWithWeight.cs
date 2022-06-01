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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class RandomPickWithWeight
{
    public class Solution 
    {
        private readonly Random _random = new Random();
        private readonly int[] _counts;
        private readonly int _total;

        public Solution(int[] w) {
            _counts = new int[w.Length];
            
            var count = 0;
            for (var i = 0; i < w.Length; ++i) {
                count += w[i];
                _counts[i] = count;
            }
            
            _total = count;
        }

        public int PickIndex() {
            var value = _total * _random.NextDouble();

            var low = 0;
            var high = _counts.Length;
            while (low < high) 
            {
                var mid = low + (high - low) / 2;
                if (value > _counts[mid])
                    low = mid + 1;
                else
                    high = mid;
            }
            
            return low;
        }
    }
    
    [Fact]
    public void Example1()
    {
        var solution = new Solution(new []{1});
        var counts = new int[1];
        // for (var i = 0; i < 10; i++)
        // {
        //     var index = solution.PickIndex();
        //     Assert.True(index >= 0 && index < 1);
        //     counts[index]++;
        // }
        //
        // Assert.Equal(1.0, counts[0]/10.0, 2);
    }
    
    [Fact]
    public void Example2()
    {
        var solution = new Solution(new []{1,3});
        var counts = new int[2];
        // for (var i = 0; i < 10000; i++)
        // {
        //     var index = solution.PickIndex();
        //     Assert.True(index >= 0 && index < 2);
        //     counts[index]++;
        // }
        //
        // Assert.Equal(0.25, counts[0]/10000.0, 2);
        // Assert.Equal(0.75, counts[1]/10000.0, 2);
    }
}