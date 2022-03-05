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
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace HeapChecks;

public class MergeKSortedLists
{
    public struct PPair : IComparable<PPair>
    {
        public int Key;
        public int Value;
        public int CompareTo(PPair other) => Value.CompareTo(other.Value);
    }

    public class Solution
    {
        public static ListNode? MergeKLists(ListNode?[]? lists)
        {
            if (lists == null) return null;

            var pointers = new ListNode?[lists.Length];
            var heap = new MinHeap<PPair>();
            for (var i = 0; i < lists.Length; i++)
            {
                var listNode = lists[i];
                if (listNode == null) continue;
                heap.Add(new PPair { Key = i, Value = listNode.val });
                pointers[i] = listNode.next;
            }

            var stack = new Stack<int>();
            while (heap.Count > 0)
            {
                var pair = heap.RemoveMin();
                var index = pair.Key;
                var value = pair.Value;

                stack.Push(value);
                var listNode = pointers[index];
                if (listNode == null) continue;
                heap.Add(new PPair { Key = index, Value = listNode.val });
                pointers[index] = listNode.next;
            }

            return stack.Aggregate<int, ListNode?>(null, (current, i) => new ListNode(i, current));
        }
    }

    [Fact]
    public void Example1()
    {
        var lists = new ListNode[] { new(new[] { 1, 4, 5 }), new(new[] { 1, 3, 4 }), new(new[] { 2, 6 }) };
        Assert.Equal(new ListNode(new[] { 1, 1, 2, 3, 4, 4, 5, 6 }), Solution.MergeKLists(lists));
    }

    [Fact]
    public void Example2()
    {
        Assert.Null(Solution.MergeKLists(null));
    }

    [Fact]
    public void Example3()
    {
        var lists = Array.Empty<ListNode>();
        Assert.Null(Solution.MergeKLists(lists));
    }

    [Fact]
    public void Example4()
    {
        var lists = new ListNode?[] { null };
        Assert.Null(Solution.MergeKLists(lists));
    }

    [Fact]
    public void Test1()
    {
        var lists = new ListNode?[] { new(new[] { 1, 2 }), new(), null };
        Assert.Equal(new ListNode(new[] { 0, 1, 2 }), Solution.MergeKLists(lists));
    }
}