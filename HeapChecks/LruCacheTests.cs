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

public class LruCacheTests
{
    public class LRUCache
    {
        public class Node
        {
            public int Key { get; }
            public int Value { get; set; }
            public Node? Prev { get; set; }
            public Node? Next { get; set; }

            public Node(int key, int value)
            {
                Key = key;
                Value = value;
            }

            public override int GetHashCode() => Key.GetHashCode();
            public override bool Equals(object? obj) => obj is Node p && Key == p.Key;
        }

        public class DoublyLinkedList
        {
            private Node? _front;
            private Node? _back;

            public void AddNodeToFront(Node node)
            {
                if (_front == null && _back == null)
                    _front = _back = node;
                else
                {
                    node.Next = _front;
                    _front!.Prev = node;
                    _front = node;
                }
            }

            public void MoveNodeToFront(Node node)
            {
                if (ReferenceEquals(node, _front))
                    return;

                if (ReferenceEquals(node, _back))
                {
                    _back = _back.Prev;
                    _back!.Next = null;
                }
                else
                {
                    node.Prev!.Next = node.Next;
                    node.Next!.Prev = node.Prev;
                }

                node.Next = _front;
                node.Prev = null;
                _front!.Prev = node;
                _front = node;
            }

            public Node? RemoveBackNode()
            {
                if (_back == null)
                    return null;

                var back = _back;
                if (ReferenceEquals(_front, _back))
                    _front = _back = null;
                else
                {
                    _back = _back!.Prev;
                    _back!.Next = null;
                }

                return back;
            }
        }

        private readonly DoublyLinkedList _list = new();
        private readonly Dictionary<int, Node> _map;
        private readonly int _capacity;

        public LRUCache(int capacity)
        {
            _map = new Dictionary<int, Node>(capacity);
            _capacity = capacity;
        }

        public int Get(int key)
        {
            if (!_map.TryGetValue(key, out var node))
                return -1;

            _list.MoveNodeToFront(node);
            return node.Value;
        }

        public void Put(int key, int value)
        {
            if (_map.TryGetValue(key, out var node))
            {
                node.Value = value;
                _list.MoveNodeToFront(node);
                return;
            }

            if (_map.Count >= _capacity)
            {
                var back = _list.RemoveBackNode();
                _map.Remove(back!.Key);
            }

            node = new Node(key, value);
            _list.AddNodeToFront(node);
            _map[key] = node;
        }
    }

    [Fact]
    public void Example1()
    {
        /*
        Input
        ["LRUCache", "put", "put", "get", "put", "get", "put", "get", "get", "get"]
        [[2], [1, 1], [2, 2], [1], [3, 3], [2], [4, 4], [1], [3], [4]]
        Output
        [null, null, null, 1, null, -1, null, -1, 3, 4]
        Explanation
        LRUCache lRUCache = new LRUCache(2);
        lRUCache.put(1, 1); // cache is {1=1}
        lRUCache.put(2, 2); // cache is {1=1, 2=2}
        lRUCache.get(1);    // return 1
        lRUCache.put(3, 3); // LRU key was 2, evicts key 2, cache is {1=1, 3=3}
        lRUCache.get(2);    // returns -1 (not found)
        lRUCache.put(4, 4); // LRU key was 1, evicts key 1, cache is {4=4, 3=3}
        lRUCache.get(1);    // return -1 (not found)
        lRUCache.get(3);    // return 3
        lRUCache.get(4);    // return 4
         */

        var input = new[] { "LRUCache", "put", "put", "get", "put", "get", "put", "get", "get", "get" };
        var data = new[]
        {
            new[] { 2 }, new[] { 1, 1 }, new[] { 2, 2 }, new[] { 1 }, new[] { 3, 3 }, new[] { 2 }, new[] { 4, 4 },
            new[] { 1 }, new[] { 3 }, new[] { 4 }
        };

        var output = new[] { -1, -1, -1, 1, -1, -1, -1, -1, 3, 4 };

        LRUCache? cache = null;
        for (var i = 0; i < input.Length; i++)
        {
            switch (input[i])
            {
                case "LRUCache":
                    cache = new LRUCache(data[i][0]);
                    break;
                case "put":
                    cache!.Put(data[i][0], data[i][1]);
                    break;
                case "get":
                    Assert.Equal(output[i], cache!.Get(data[i][0]));
                    break;
            }
        }
    }
}