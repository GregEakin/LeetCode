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
using Xunit;

namespace HeapChecks;

public class AddListNumbersTests
{
    public class Solution
    {
        public static ListNode AddTwoNumbers2(ListNode l1, ListNode l2)
        {
            var d1 = l1;
            var d2 = l2;
            var total = new Stack<int>();
            var carry = 0;
            while (d1 != null && d2 != null)
            {
                var sum = carry + d1.Val + d2.Val;
                carry = sum / 10;
                total.Push(sum % 10);
                d1 = d1.Next;
                d2 = d2.Next;
            }

            while (d1 != null)
            {
                var sum = carry + d1.Val;
                carry = sum / 10;
                total.Push(sum % 10);
                d1 = d1.Next;
            }

            while (d2 != null)
            {
                var sum = carry + d2.Val;
                carry = sum / 10;
                total.Push(sum % 10);
                d2 = d2.Next;
            }

            if (carry > 0)
                total.Push(carry % 10);

            var result = total.Aggregate<int, ListNode?>(null, (current, digit) => new ListNode(digit, current));
            return result!;
        }

        public static ListNode? AddTwoNumbers(ListNode? l1, ListNode? l2, int carry = 0)
        {
            if (l1 != null && l2 != null)
            {
                var sum = carry + l1.Val + l2.Val;
                return new ListNode(sum % 10, AddTwoNumbers(l1.Next, l2.Next, sum / 10));
            }

            if (l1 != null)
            {
                var sum = carry + l1.Val;
                return new ListNode(sum % 10, AddTwoNumbers(l1.Next, null, sum / 10));
            }

            if (l2 != null)
            {
                var sum = carry + l2.Val;
                return new ListNode(sum % 10, AddTwoNumbers(null, l2.Next, sum / 10));
            }

            if (carry > 0)
                return new ListNode(carry, null);

            return null;
        }
    }

    [Fact]
    public void Example10()
    {
        // 342 + 465 = 807
        var l1 = new ListNode(2, new ListNode(4, new ListNode(3, null)));
        var l2 = new ListNode(5, new ListNode(6, new ListNode(4, null)));
        Assert.Equal(new ListNode(7, new ListNode(0, new ListNode(8, null))), Solution.AddTwoNumbers(l1, l2));
    }

    [Fact]
    public void Example1()
    {
        // 342 + 465 = 807
        var l1 = new ListNode(new[] { 2, 4, 3 });
        var l2 = new ListNode(new[] { 5, 6, 4 });
        Assert.Equal(new ListNode(new[] { 7, 0, 8 }), Solution.AddTwoNumbers(l1, l2));
    }

    [Fact]
    public void Example2()
    {
        var l1 = new ListNode(new[] { 0 });
        var l2 = new ListNode(new[] { 0 });
        Assert.Equal(new ListNode(new[] { 0 }), Solution.AddTwoNumbers(l1, l2));
    }

    [Fact]
    public void Example3()
    {
        var l1 = new ListNode(new[] { 9, 9, 9, 9, 9, 9, 9 });
        var l2 = new ListNode(new[] { 9, 9, 9, 9 });
        Assert.Equal(new ListNode(new[] { 8, 9, 9, 9, 0, 0, 0, 1 }), Solution.AddTwoNumbers(l1, l2));
    }
}