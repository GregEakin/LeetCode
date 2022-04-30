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

public class LinkedListCycle
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
    public class SolutionHashSet
    {
        public bool HasCycle(ListNode? head)
        {
            var map = new HashSet<ListNode>();
            while (head != null)
            {
                if (map.Contains(head)) return true;
                map.Add(head);
                head = head.next;
            }
            return false;
        }
    }

    public class Solution
    {
        // Floyd's Tortoise and Hare
        public bool HasCycle(ListNode head)
        {
            var slow = head;
            var fast = head.next;
            while (!ReferenceEquals(slow, fast))
            {
                if (fast?.next == null) return false;
                slow = slow.next;
                fast = fast.next.next;
            }

            return true;
        }
    }

    public static ListNode CreateNodeWithLoops(int[] data, int pos)
    {
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

    [Fact]
    public void Answer1()
    {
        var data = new[] { -21, 10, 17, 8, 4, 26, 5, 35, 33, -7, -16, 27, -12, 6, 29, -12, 5, 9, 20, 14, 14, 2, 13, -24, 21, 23, -21, 5 };
        var pos = -1;
        var head = CreateNodeWithLoops(data, pos);
        var solution = new Solution();
        Assert.False(solution.HasCycle(head));
    }

    [Fact]
    public void Example1()
    {
        var data = new[] { 3, 2, 0, -4 };
        var pos = 1;
        var head = CreateNodeWithLoops(data, pos);
        var solution = new Solution();
        Assert.True(solution.HasCycle(head));
    }

    [Fact]
    public void Example2()
    {
        var data = new[] { 1, 2 };
        var pos = 0;
        var head = CreateNodeWithLoops(data, pos);
        var solution = new Solution();
        Assert.True(solution.HasCycle(head));
    }

    [Fact]
    public void Example3()
    {
        var data = new[] { 1 };
        var pos = -1;
        var head = CreateNodeWithLoops(data, pos);
        var solution = new Solution();
        Assert.False(solution.HasCycle(head));
    }
}