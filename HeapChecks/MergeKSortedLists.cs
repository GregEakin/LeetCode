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
using Xunit;

namespace HeapChecks;

public class MergeKSortedLists
{
    public class Solution
    {
        public static ListNode? MergeKLists(ListNode?[]? lists)
        {
            if (lists == null) return null;

            var queue = new PriorityQueue<int, int>();
            for (var index = 0; index < lists.Length; index++)
            {
                var listNode = lists[index];
                if (listNode == null) continue;
                queue.Enqueue(index, listNode.Val);
            }

            ListNode? r1 = null;
            ListNode? r2 = null;
            while (queue.Count > 0)
            {
                var index = queue.Dequeue();
                var value = lists[index].Val;

                if (r2 == null)
                    r1 = r2 = new ListNode(value, null);
                else
                {
                    r2.Next = new ListNode(value, null);
                    r2 = r2.Next;
                }

                var nextNode = lists[index].Next;
                lists[index] = nextNode;
                if (nextNode == null) continue;
                queue.Enqueue(index, nextNode.Val);
            }

            return r1;
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

    [Fact]
    public void Answer1()
    {
        var lists = new ListNode?[] { new(new[] { 1 }), new(new[] { 5 }), new(new[] { 2 }), new(new[] { 13 }), new(new[] { 4 }) };
        Assert.Equal(new ListNode(new[] { 1, 2, 4, 5, 13 }), Solution.MergeKLists(lists));
    }
}