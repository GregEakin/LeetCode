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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace HeapChecks;

public class SmallestStringWithSwaps
{
    public class SolutionBFS
    {
        public string SmallestStringWithSwaps(string s, IList<IList<int>> pairs)
        {
            var queue = new Queue<string>();
            queue.Enqueue(s);
            while (queue.Count > 0)
            {
                var next = queue.Dequeue();
                if (s.CompareTo(next) > 0)
                    s = next;

                foreach (var pair in pairs)
                {
                    var sb = new StringBuilder(next);
                    (sb[pair[0]], sb[pair[1]]) = (sb[pair[1]], sb[pair[0]]);
                    var guess = sb.ToString();
                    if (next.CompareTo(guess) > 0)
                        queue.Enqueue(guess);
                }
            }

            return s;
        }
    }

    public class SolutionGoodDFS
    {
        private const int N = 100001;
        private readonly BitArray _visited = new(N);
        private readonly IList<int>[] _adj = new IList<int>[N];

        public void DFS(string s, int vertex, IList<char> characters, IList<int> indices)
        {
            characters.Add(s[vertex]);
            indices.Add(vertex);
            _visited[vertex] = true;
            foreach (var adjacent in _adj[vertex].Where(adjacent => !_visited[adjacent]))
                DFS(s, adjacent, characters, indices);
        }

        public string SmallestStringWithSwaps(string s, IList<IList<int>> pairs)
        {
            for (var i = 0; i < s.Length; i++)
                _adj[i] = new List<int>();

            foreach (var edge in pairs)
            {
                var source = edge[0];
                var dest = edge[1];

                _adj[source].Add(dest);
                _adj[dest].Add(source);
            }

            var answer = new char[s.Length];
            for (var vertex = 0; vertex < s.Length; vertex++)
            {
                if (_visited[vertex]) continue;
                var characters = new List<char>();
                var indices = new List<int>();

                DFS(s, vertex, characters, indices);
                characters.Sort();
                indices.Sort();

                for (var index = 0; index < characters.Count; index++)
                    answer[indices[index]] = characters[index];
            }

            return new string(answer);
        }
    }

    public class Solution
    {
        public class UnionFind
        {
            private readonly int[] _root;
            private readonly int[] _rank;

            public UnionFind(int size)
            {
                _root = new int[size];
                _rank = new int[size];
                for (var i = 0; i < size; i++)
                {
                    _root[i] = i;
                    _rank[i] = 1;
                }
            }

            public int Find(int x)
            {
                return x == _root[x]
                    ? x
                    : _root[x] = Find(_root[x]);
            }

            public void Union(int x, int y)
            {
                var rootX = Find(x);
                var rootY = Find(y);
                if (rootX == rootY) return;
                if (_rank[rootX] >= _rank[rootY])
                {
                    _root[rootY] = rootX;
                    _rank[rootX] += _rank[rootY];
                }
                else
                {
                    _root[rootX] = rootY;
                    _rank[rootY] += _rank[rootX];
                }
            }
        }

        public string SmallestStringWithSwaps(string s, IList<IList<int>> pairs)
        {
            var uf = new UnionFind(s.Length);
            foreach (var edge in pairs) 
                uf.Union(edge[0], edge[1]);

            var rootToComponent = new Dictionary<int, IList<int>>();
            for (var vertex = 0; vertex < s.Length; vertex++)
            {
                var root = uf.Find(vertex);
                if (!rootToComponent.TryGetValue(root, out var list))
                {
                    list = new List<int>();
                    rootToComponent[root] = list;
                }

                list.Add(vertex);
            }

            var smallestString = new char[s.Length];
            foreach (var indices in rootToComponent.Values)
            {
                var characters = indices.Select(index => s[index]).OrderBy(c => c).ToArray();
                for (var index = 0; index < indices.Count; index++)
                    smallestString[indices[index]] = characters[index];
            }

            return new string(smallestString);
        }
    }

    [Fact]
    public void Answer1()
    {
        var s = "udyyek";
        var pairs = new[]
            { new[] { 3, 3 }, new[] { 3, 0 }, new[] { 5, 1 }, new[] { 3, 1 }, new[] { 3, 4 }, new[] { 3, 5 } };
        var solution = new Solution();
        Assert.Equal("deykuy", solution.SmallestStringWithSwaps(s, pairs));
    }

    [Fact]
    public void Example1()
    {
        // Input: s = "dcab", pairs = [[0, 3],[1,2]]
        // Output: "bacd"
        // Explaination:
        // Swap s[0] and s[3], s = "bcad"
        // Swap s[1] and s[2], s = "bacd"

        var s = "dcab";
        var pairs = new[] { new[] { 0, 3 }, new[] { 1, 2 } };
        var solution = new Solution();
        Assert.Equal("bacd", solution.SmallestStringWithSwaps(s, pairs));
    }

    [Fact]
    public void Example2()
    {
        // Input: s = "dcab", pairs = [[0, 3],[1,2],[0,2]]
        // Output: "abcd"
        // Explaination:
        // Swap s[0] and s[3], s = "bcad"
        // Swap s[0] and s[2], s = "acbd"
        // Swap s[1] and s[2], s = "abcd"

        var s = "dcab";
        var pairs = new[] { new[] { 0, 3 }, new[] { 1, 2 }, new[] { 0, 2 } };
        var solution = new Solution();
        Assert.Equal("abcd", solution.SmallestStringWithSwaps(s, pairs));
    }

    [Fact]
    public void Example3()
    {
        // Input: s = "cba", pairs = [[0, 1],[1,2]]
        // Output: "abc"
        // Explaination:
        // Swap s[0] and s[1], s = "bca"
        // Swap s[1] and s[2], s = "bac"
        // Swap s[0] and s[1], s = "abc"

        var s = "cba";
        var pairs = new[] { new[] { 0, 1 }, new[] { 1, 2 } };
        var solution = new Solution();
        Assert.Equal("abc", solution.SmallestStringWithSwaps(s, pairs));
    }

    [Fact]
    public void UnionFindTest()
    {
        var pairs = new[] { (0, 3), (2, 5), (7, 2), (2, 3), (5, 7), (1, 4) };
        var uf = new Solution.UnionFind(8);
        foreach (var (x, y) in pairs) 
            uf.Union(x, y);

        Assert.Equal(1, uf.Find(1));
        Assert.Equal(1, uf.Find(4));

        Assert.Equal(2, uf.Find(0));
        Assert.Equal(2, uf.Find(2));
        Assert.Equal(2, uf.Find(3));
        Assert.Equal(2, uf.Find(5));
        Assert.Equal(2, uf.Find(7));

        Assert.Equal(6, uf.Find(6));
    }
}