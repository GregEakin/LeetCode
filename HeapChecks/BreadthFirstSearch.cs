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
using System.Linq;

namespace HeapChecks;

public class BreadthFirstSearch<T> where T : notnull
{
    public enum Color { White, Gray, Black }

    private readonly T[] _vertexes;
    private readonly (T u, T v)[] _edges;
    private readonly Dictionary<T, Color> _color;
    private readonly Dictionary<T, T?> _pi;
    private readonly Dictionary<T, int> _d;

    public BreadthFirstSearch(T[] vertexes, (T u, T v)[] edges)
    {
        _vertexes = vertexes;
        _edges = edges;
        _color = new Dictionary<T, Color>(vertexes.Length);  // progress indicator
        _pi = new Dictionary<T, T?>(vertexes.Length);        // predecessor field
        _d = new Dictionary<T, int>(vertexes.Length);        // distance from source
    }

    public void Search(T s)
    {
        foreach (var u in _vertexes)
        {
            _color[u] = Color.White;
            _d[u] = int.MaxValue;
            _pi[u] = default;
        }

        _color[s] = Color.Gray;
        _d[s] = 0;
        _pi[s] = default;

        var queue = new Queue<T>();
        queue.Enqueue(s);
        while (queue.Count > 0)
        {
            var u = queue.Dequeue();
            var items = _edges.Where(t => t.u.Equals(u)).Select(t => t.v).Concat(_edges.Where(t => t.v.Equals(u)).Select(t => t.u));
            foreach (var v in items)
            {
                if (_color[v] != Color.White) continue;
                _color[v] = Color.Gray;
                _d[v] = _d[u] + 1;
                _pi[v] = u;
                queue.Enqueue(v);
            }

            _color[u] = Color.Black;
        }
    }

    public void PrintPath(T s, T? v)
    {
        if (s.Equals(v))
            System.Console.WriteLine(s);
        else
        {
            if (v == null || _pi[v] == null)
                System.Console.WriteLine("No path from {0} to {1}", s, v);
            else
            {
                PrintPath(s, _pi[v]);
                System.Console.WriteLine(v);
            }
        }
    }
}