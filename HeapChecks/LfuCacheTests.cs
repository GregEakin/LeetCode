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

public class LfuCacheTests
{
    public class LFUCache
    {
        public class Node
        {
            public int Key { get; }
            public int Value { get; set; }
            public int Count { get; set; }
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

            public bool IsEmpty => _back == null;

            public Node AddNodeToFront(Node node)
            {
                if (_front == null && _back == null)
                    _front = _back = node;
                else
                {
                    node.Next = _front;
                    _front!.Prev = node;
                    _front = node;
                }

                return node;
            }

            public void RemoveNode(Node node)
            {
                if (ReferenceEquals(_front, _back))
                {
                    _front = _back = null;
                    return;
                }

                if (ReferenceEquals(node, _front))
                {
                    _front = _front.Next;
                    _front!.Prev = null;
                    return;
                }

                if (ReferenceEquals(node, _back))
                {
                    _back = _back.Prev;
                    _back!.Next = null;
                    return;
                }

                node.Prev!.Next = node.Next;
                node.Next!.Prev = node.Prev;
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
                    _back = _back.Prev;
                    _back!.Next = null;
                }

                return back;
            }
        }

        private readonly Dictionary<int, DoublyLinkedList> _age = new();
        private readonly Dictionary<int, Node> _map;
        private readonly int _capacity;
        private int _minCount;

        public LFUCache(int capacity)
        {
            _map = new Dictionary<int, Node>(capacity);
            _capacity = capacity;

            var list = new DoublyLinkedList();
            _age.Add(0, list);
        }

        private void IncNodeCount(Node node)
        {
            var oldList = _age[node.Count];
            oldList.RemoveNode(node);

            if (_minCount == node.Count && oldList.IsEmpty) _minCount++;

            if (!_age.TryGetValue(node.Count + 1, out var list))
            {
                list = new DoublyLinkedList();
                _age.Add(node.Count + 1, list);
            }

            list.AddNodeToFront(node);
            node.Count++;
        }

        public int Get(int key)
        {
            if (_capacity == 0 || !_map.TryGetValue(key, out var node))
                return -1;

            IncNodeCount(node);
            return node.Value;
        }

        public void Put(int key, int value)
        {
            if (_capacity == 0) return;

            if (_map.TryGetValue(key, out var node))
            {
                node.Value = value;
                IncNodeCount(node);
                return;
            }

            if (_map.Count >= _capacity)
            {
                node = _age[_minCount].RemoveBackNode();
                _map.Remove(node!.Key);
            }

            node = new Node(key, value);
            _age[0].AddNodeToFront(node);
            _map[key] = node;
            _minCount = 0;
        }
    }

    [Fact]
    public void Example1()
    {
        /*
        Input
        ["LFUCache", "put", "put", "get", "put", "get", "get", "put", "get", "get", "get"]
        [[2], [1, 1], [2, 2], [1], [3, 3], [2], [3], [4, 4], [1], [3], [4]]
        Output
        [null, null, null, 1, null, -1, 3, null, -1, 3, 4]

        Explanation
        // cnt(x) = the use counter for key x
        // cache=[] will show the last used order for tiebreakers (leftmost element is  most recent)
        LFUCache lfu = new LFUCache(2);
        lfu.put(1, 1);   // cache=[1,_], cnt(1)=1
        lfu.put(2, 2);   // cache=[2,1], cnt(2)=1, cnt(1)=1
        lfu.get(1);      // return 1
                         // cache=[1,2], cnt(2)=1, cnt(1)=2
        lfu.put(3, 3);   // 2 is the LFU key because cnt(2)=1 is the smallest, invalidate 2.
                         // cache=[3,1], cnt(3)=1, cnt(1)=2
        lfu.get(2);      // return -1 (not found)
        lfu.get(3);      // return 3
                         // cache=[3,1], cnt(3)=2, cnt(1)=2
        lfu.put(4, 4);   // Both 1 and 3 have the same cnt, but 1 is LRU, invalidate 1.
                         // cache=[4,3], cnt(4)=1, cnt(3)=2
        lfu.get(1);      // return -1 (not found)
        lfu.get(3);      // return 3
                         // cache=[3,4], cnt(4)=1, cnt(3)=3
        lfu.get(4);      // return 4
                         // cache=[4,3], cnt(4)=2, cnt(3)=3
         */

        var input = new[] { "LFUCache", "put", "put", "get", "put", "get", "get", "put", "get", "get", "get" };
        var data = new[]
        {
            new[] { 2 }, new[] { 1, 1 }, new[] { 2, 2 }, new[] { 1 }, new[] { 3, 3 }, new[] { 2 }, new[] { 3 },
            new[] { 4, 4 }, new[] { 1 }, new[] { 3 }, new[] { 4 }
        };

        var output = new[] { -1, -1, -1, 1, -1, -1, 3, -1, -1, 3, 4 };

        LFUCache? cache = null;
        for (var i = 0; i < input.Length; i++)
        {
            switch (input[i])
            {
                case "LFUCache":
                    cache = new LFUCache(data[i][0]);
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