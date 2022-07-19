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

using System.Globalization;
using Xunit;

namespace HeapChecks;

public class SwapNodesInPairs
{
    public class SolutionRecursion
    {
        public ListNode? SwapPairs(ListNode? head)
        {
            if (head?.next == null)
                return head;

            var temp = head.next;
            head.next = SwapPairs(head.next.next);
            temp.next = head;
            return temp;
        }
    }

    public class Solution
    {
        public ListNode? SwapPairs(ListNode? head)
        {
            var ptr = new ListNode { next = head };
            var prev = ptr;
            while (head?.next != null)
            {
                var first = head;
                var second = head.next;
                prev.next = second;
                first.next = second.next;
                second.next = first;
                prev = first;
                head = first.next;
            }

            return ptr.next;
        }
    }

    [Fact]
    public void Example1()
    {
        var head = new ListNode(new[] { 1, 2, 3, 4 });
        var solution = new Solution();
        Assert.Equal(new ListNode(new[] { 2, 1, 4, 3 }), solution.SwapPairs(head));
    }

    [Fact]
    public void Example2()
    {
        var solution = new Solution();
        Assert.Equal((ListNode?)null, solution.SwapPairs(null));
    }

    [Fact]
    public void Example3()
    {
        var head = new ListNode(new[] { 1 });
        var solution = new Solution();
        Assert.Equal(new ListNode(new[] { 1 }), solution.SwapPairs(head));
    }
}

public class RemoveDuplicatesFromSortedArray
{
    public class Solution
    {
        public int RemoveDuplicates(int[] nums)
        {
            var skipped = 0;
            for (var i = 1; i < nums.Length; i++)
            {
                if (nums[i - skipped - 1] >= nums[i])
                    skipped++;
                else
                    nums[i - skipped] = nums[i];
            }

            return nums.Length - skipped;
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 1, 1, 2 };
        var solution = new Solution();
        Assert.Equal(2, solution.RemoveDuplicates(nums));
        for (var i = 1; i < 2; i++) Assert.True(nums[i - 1] < nums[i]);
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 4 };
        var solution = new Solution();
        Assert.Equal(5, solution.RemoveDuplicates(nums));
        for (var i = 1; i < 5; i++) Assert.True(nums[i - 1] < nums[i]);
    }

    [Fact]
    public void Answer1()
    {
        var nums = new[] { 1, 1, 1, 1, 1 };
        var solution = new Solution();
        Assert.Equal(1, solution.RemoveDuplicates(nums));
    }

    [Fact]
    public void Test1()
    {
        var nums = new[] { 20 };
        var solution = new Solution();
        Assert.Equal(1, solution.RemoveDuplicates(nums));
    }

    [Fact]
    public void Test2()
    {
        var nums = new[] { 10, 10, 20 };
        var solution = new Solution();
        Assert.Equal(2, solution.RemoveDuplicates(nums));
        for (var i = 1; i < 2; i++) Assert.True(nums[i - 1] < nums[i]);
    }


    [Fact]
    public void Test3()
    {
        var nums = new[] { 1, 1, 1, 10, 20, 21, 22, 23, 24, 24, 24 };
        var solution = new Solution();
        Assert.Equal(7, solution.RemoveDuplicates(nums));
        for (var i = 1; i < 7; i++) Assert.True(nums[i - 1] < nums[i]);
    }
}