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

using System.Collections.Generic;
using Xunit;

namespace HeapChecks;

public class MinimumSpanningTree<T>
{
    private readonly T[] _vertexes;
    private readonly Dictionary<(T u, T v), int> _edges;
}

public class MinimumSpanningTreeTests
{
    [Fact]
    public void Test1()
    {
        var vertexes = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i' };
        ((char u, char v), int)[] edges =
        {
            (('a', 'b'), 4), // found
            (('a', 'h'), 8),
            (('b', 'c'), 8), // found
            (('b', 'h'), 11),
            (('c', 'd'), 7), // found
            (('c', 'f'), 4), // found
            (('c', 'i'), 2), // found
            (('d', 'e'), 9), // found
            (('d', 'j'), 14),
            (('e', 'f'), 10),
            (('f', 'g'), 2), // found
            (('g', 'h'), 1), // found
            (('g', 'i'), 6),
            (('h', 'i'), 7),
        };
    }
}