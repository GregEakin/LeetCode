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

using Xunit;

namespace HeapChecks;

public class ReverseNodesInKGroup
{
    public class SolutionRecursive
    {
        public static ListNode? Rev(ListNode? listIn, ListNode? listOut, int k)
        {
            if (k <= 0 || listIn == null) return listOut;
            var next = listIn.next;
            listIn.next = listOut;
            return Rev(next, listIn, k - 1);
        }

        public ListNode? ReverseLinkedList(ListNode? head, int k) 
        {
            ListNode? newHead = null;
            var ptr = head;
            for (; k > 0; k--) 
            {
                var next = ptr.next;
                ptr.next = newHead;
                newHead = ptr;
                ptr = next;
            }

            return newHead;
        }
            
        public ListNode? ReverseKGroup(ListNode? head, int k) 
        {
            var ptr = head;
            for (var i = 0; i < k; i++)
            {
                if (ptr == null) return head;
                ptr = ptr.next;
            }

            var reversedHead = ReverseLinkedList(head, k);
            head!.next = ReverseKGroup(ptr, k);
            return reversedHead;
        }    
    }

    class Solution
    {
        public static ListNode? ReverseLinkedList(ListNode? head, int k)
        {
            var newHead = (ListNode?)null;
            var ptr = head;
            while (k > 0)
            {
                var nextNode = ptr.next;
                ptr.next = newHead;
                newHead = ptr;
                ptr = nextNode;
                k--;
            }

            return newHead;
        }

        public ListNode? ReverseKGroup(ListNode? head, int k)
        {
            var ptr = head;
            var tail = (ListNode?)null;
            var newHead = (ListNode?)null;
            while (ptr != null)
            {
                var count = 0;
                ptr = head;
                while (count < k && ptr != null)
                {
                    ptr = ptr.next;
                    count++;
                }

                if (count != k) continue;
                var revHead = ReverseLinkedList(head, k);
                newHead ??= revHead;

                if (tail != null)
                    tail.next = revHead;

                tail = head;
                head = ptr;
            }

            if (tail != null) 
                tail.next = head;

            return newHead ?? head;
        }
    }

    [Fact]
    public void Example1()
    {
        var data = new[] { 1, 2, 3, 4, 5 };
        var head = new ListNode(data);
        var solution = new Solution();
        Assert.Equal(new ListNode(new[] { 2, 1, 4, 3, 5 }), solution.ReverseKGroup(head, 2));
    }
    
    [Fact]
    public void Example2()
    {
        var data = new[] { 1, 2, 3, 4, 5 };
        var head = new ListNode(data);
        var solution = new Solution();
        Assert.Equal(new ListNode(new[] { 3, 2, 1, 4, 5 }), solution.ReverseKGroup(head, 3));
    }
}