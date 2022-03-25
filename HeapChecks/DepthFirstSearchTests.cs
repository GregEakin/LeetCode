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

using System.Linq;
using Xunit;

namespace HeapChecks;

public class DepthFirstSearchTests
{
    [Fact]
    public void DfsTest()
    {
        var tree = new (char u, char v)[] {
            ('u', 'v'),
            ('u', 'x'),
            ('x', 'v'),
            ('v', 'y'),
            ('y', 'x'),
            ('w', 'y'),
            ('w', 'z'),
            ('z', 'z'),
        };
        var search = new DepthFirstSearch<char>(tree);
        search.Search();
        Assert.Equal(12, search.Time);
    }

    [Fact]
    public void StronglyConnectedComponentsTest()
    {
        var g = new (char u, char v)[] {
            ('c', 'g'),
            ('g', 'f'),
            ('h', 'h'),
            ('d', 'c'),
            ('g', 'h'),
            ('b', 'e'),
            ('e', 'a'),
            ('a', 'b'),
            ('b', 'f'),
            ('b', 'c'),
            ('c', 'd'),
            ('d', 'h'),
            ('e', 'f'),
            ('f', 'g'),
        };

        var gt = g.Select(pair => (pair.v, pair.u)).ToArray();
        // Assert.Equal(('b', 'a'), gt[0]);
        

        var searchG = new DepthFirstSearch<char>(g);
        searchG.Search();
        var searchGt = new DepthFirstSearch<char>(gt);
        var stuff = searchG.FinishedTimes.OrderByDescending(t => t.Value);
        // Assert.Equal(new[]{'b', 'e', 'a', 'c', 'd', 'g', 'h', 'f' }, stuff.Select(t => t.Key));
        searchGt.Search(stuff);
        Assert.Equal(16, searchG.Time);
    }

    [Fact]
    public void NakedTest()
    {
        var g = new (char u, char v)[] {
            ('c', 'i'), // 1 c, i
            ('i', 'c'),
            ('f', 'f'), // 2 f
            ('a', 'a'), // 3 a
            ('c', 'f'), // 4 c, f, i
            ('f', 'i'),
            ('i', 'c'),
            ('b', 'f'), // 5 b, f, h
            ('f', 'h'),
            ('h', 'b'),
            ('b', 'c'), // 6 b, c, e, f, i
            ('b', 'e'),
            ('b', 'f'),
            ('b', 'i'),
            ('c', 'e'),
            ('c', 'f'),
            ('c', 'i'),
            ('e', 'f'),
            ('e', 'i'),
            ('f', 'i'),
            ('d', 'd'), // 7 d
            ('g', 'g'), // 8 g
            ('c', 'e'), // 9 c, e, h, i
            ('c', 'h'),
            ('c', 'i'),
            ('e', 'h'),
            ('e', 'i'),
            ('h', 'i'),
        };

        var gt = g.Select(pair => (pair.v, pair.u)).ToArray();
        // Assert.Equal(('b', 'a'), gt[0]);


        var searchG = new DepthFirstSearch<char>(g);
        searchG.Search();
        var searchGt = new DepthFirstSearch<char>(gt);
        var stuff = searchG.FinishedTimes.OrderByDescending(t => t.Value);
        // Assert.Equal(new[]{'b', 'e', 'a', 'c', 'd', 'g', 'h', 'f' }, stuff.Select(t => t.Key));
        searchGt.Search(stuff);
        Assert.Equal(18, searchG.Time);

    }
}