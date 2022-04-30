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

public class LinkedListCycleII
{
    /**
     * Definition for singly-linked list.
     * public class ListNode {
     *     public int val;
     *     public ListNode next;
     *     public ListNode(int x) {
     *         val = x;
     *         next = null;
     *     }
     * }
     */
    public class Solution
    {
        public static ListNode GetIntersect(ListNode head)
        {
            var tortoise = head;
            var hare = head;
            while (hare != null && hare.next != null)
            {
                tortoise = tortoise.next;
                hare = hare.next.next;
                if (ReferenceEquals(tortoise, hare))
                    return tortoise;
            }

            return null;
        }

        public ListNode DetectCycle(ListNode head)
        {
            if (head == null) return null;

            var intersect = GetIntersect(head);
            if (intersect == null) return null;

            var ptr1 = head;
            var ptr2 = intersect;
            while (!ReferenceEquals(ptr1, ptr2))
            {
                ptr1 = ptr1.next;
                ptr2 = ptr2.next;
            }

            return ptr1;
        }
    }

    public static ListNode? CreateNodeWithLoops(int[] data, int pos)
    {
        if (data.Length == 0) return null;

        var head = new ListNode(data);
        var tail = head;
        for (var i = 0; i < data.Length - 1; i++)
            tail = tail!.next;
        if (pos < 0) return head;

        tail!.next = head;
        for (var i = 0; i < pos; i++)
            tail.next = tail.next!.next;

        return head;
    }

    public static ListNode? FindNode(ListNode? head, int pos)
    {
        if (head == null) return null;
        if (pos < 0) return null;
        for (var i = 0; i < pos; i++)
            head = head!.next;
        return head;
    }

    [Fact]
    public void Answer1()
    {
        var data = new[]
        {
            -21, 10, 17, 8, 4, 26, 5, 35, 33, -7, -16, 27, -12, 6, 29, -12, 5, 9, 20, 14, 14, 2, 13, -24, 21, 23, -21, 5
        };
        var pos = -1;
        var head = CreateNodeWithLoops(data, pos);
        var solution = new Solution();
        Assert.Equal(FindNode(head, pos), solution.DetectCycle(head));
    }

    [Fact]
    public void Answer2()
    {
        var data = Array.Empty<int>();
        var pos = -1;
        var head = CreateNodeWithLoops(data, pos);
        var solution = new Solution();
        Assert.Equal(FindNode(head, pos), solution.DetectCycle(head));
    }

    [Fact]
    public void Answer3()
    {
        var data = new[]
        {
            -21, 10, 17, 8, 4, 26, 5, 35, 33, -7, -16, 27, -12, 6, 29, -12, 5, 9, 20, 14, 14, 2, 13, -24, 21, 23, -21, 5
        };
        var pos = 24;
        var head = CreateNodeWithLoops(data, pos);
        var solution = new Solution();
        Assert.Equal(FindNode(head, pos), solution.DetectCycle(head));
    }

    [Fact]
    public void Example1()
    {
        var data = new[] { 3, 2, 0, -4 };
        var pos = 1;
        var head = CreateNodeWithLoops(data, pos);
        var solution = new Solution();
        Assert.Equal(FindNode(head, pos), solution.DetectCycle(head));
    }

    [Fact]
    public void Example2()
    {
        var data = new[] { 1, 2 };
        var pos = 0;
        var head = CreateNodeWithLoops(data, pos);
        var solution = new Solution();
        Assert.Equal(FindNode(head, pos), solution.DetectCycle(head));
    }

    [Fact]
    public void Example3()
    {
        var data = new[] { 1 };
        var pos = -1;
        var head = CreateNodeWithLoops(data, pos);
        var solution = new Solution();
        Assert.Equal(FindNode(head, pos), solution.DetectCycle(head));
    }
}