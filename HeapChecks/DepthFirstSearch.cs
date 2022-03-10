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

public class DepthFirstSearch<T> where T : notnull
{
    public enum Color { White, Gray, Black }

    private readonly (T u, T v)[] _tree;
    private readonly Dictionary<T, Color> _color;
    private readonly Dictionary<T, T?> _pi;
    private readonly Dictionary<T, int> _d;
    private readonly Dictionary<T, int> _f;
    private int _time;

    public DepthFirstSearch((T u, T v)[] tree)
    {
        _tree = tree;
        _color = new Dictionary<T, Color>(tree.Length);  // progress indicator
        _pi = new Dictionary<T, T?>(tree.Length);      // predecessor field
        _d = new Dictionary<T, int>(tree.Length);        // discover time
        _f = new Dictionary<T, int>(tree.Length);        // finished searching time
    }

    public int Time => _time;

    public Dictionary<T, int> FinishTimes => _f;

    public void Search()
    {
        foreach (var (u, _) in _tree)
        {
            _color[u] = Color.White;
            _pi[u] = default;
        }

        _time = 0;

        foreach (var (u, _) in _tree)
        {
            if (_color[u] == Color.White)
                Visit(u);
        }
    }


    public void Search(IOrderedEnumerable<KeyValuePair<T, int>> stuff)
    {
        foreach (var (u, _) in _tree)
        {
            _color[u] = Color.White;
            _pi[u] = default;
        }

        _time = 0;

        foreach (var (u, _) in stuff)
        {
            if (_color[u] == Color.White)
                Visit(u);
        }
    }

    public void Visit(T u)
    {
        _color[u] = Color.Gray;
        _d[u] = ++_time;

        foreach (var (_, v) in _tree.Where(t => t.u.Equals(u) && _color[t.v] == Color.White))
        {
            _pi[v] = u;
            Visit(v);
        }

        _color[u] = Color.Black;
        _f[u] = ++_time;
    }
}
