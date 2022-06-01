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

public class RandomPickWithBlacklist
{
    public class SolutionTooBig {
        private readonly Random _random = new Random();
        private readonly int[] _values;
        
        public SolutionTooBig(int n, int[] blacklist)
        {
            var total = n - blacklist.Length;
            _values = new int[total];
            
            var index = 0;
            for (var i = 0; i < _values.Length; ++i, ++index)
            {
                while (blacklist.Contains(index))
                    ++index;
                
                _values[i] = index;
            }
        }
    
        public int Pick()
        {
            var index = _values.Length * _random.NextDouble();
            return _values[(int)index];
        }
    }

    class Solution {

        private readonly Dictionary<int, int> _dict = new();
        private readonly Random _random = new();
        private readonly int _length;

        public Solution(int n, int[] blacklist) 
        {
            _length = n - blacklist.Length;
            var next = _length - 1;
            foreach (var b in blacklist.Where(i => i < _length).OrderBy(i => i))
            {
                do next++; while(blacklist.Contains(next));
                _dict.Add(b, next);
            }
        }

        public int Pick() 
        {
            var k = _random.Next(_length);
            return _dict.TryGetValue(k, out var v) ? v : k;
        }
    }
    
    [Fact]
    public void Example1()
    {
        var solution = new Solution(7,new []{2, 3, 5});
        var counts = new int[7];
        // for (var i = 0; i < 10000; i++)
        // {
        //     var index = solution.Pick();
        //     Assert.True(index >= 0 && index < 7);
        //     counts[index]++;
        // }
        //
        // Assert.Equal(0.25, counts[0]/10000.0, 2);
        // Assert.Equal(0.25, counts[1]/10000.0, 2);
        // Assert.Equal(0.0, counts[2]/10000.0, 2);
        // Assert.Equal(0.0, counts[3]/10000.0, 2);
        // Assert.Equal(0.25, counts[4]/10000.0, 2);
        // Assert.Equal(0.0, counts[5]/10000.0, 2);
        // Assert.Equal(0.25, counts[6]/10000.0, 2);
    }
}