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

namespace HeapChecks;

public class ListNode
{
    public int Val { get; }
    public ListNode? Next { get; set; }

    public ListNode(int val = 0, ListNode? next = null)
    {
        Val = val;
        Next = next;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ListNode node)
            return false;
        return ReferenceEquals(this, node) || Equals(node);
    }

    protected bool Equals(ListNode other)
    {
        return Val == other.Val && ((Next == null && other.Next == null) || (Next != null && other.Next != null && Next.Equals(other.Next)));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Val, Next?.GetHashCode());
    }

    public ListNode(IReadOnlyList<int> data, int index = 0)
    {
        if (data.Count == 0)
            throw new ArgumentException("Empty data");

        if (index < 0 || index >= data.Count)
            throw new ArgumentException("index out of range");

        Val = data[index];
        Next = index == data.Count - 1
            ? null
            : new ListNode(data, index + 1);
    }
}