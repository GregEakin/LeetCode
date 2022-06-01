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

public class SenderWithLargestWordCount
{
    public class Solution
    {
        public string LargestWordCount(string[] messages, string[] senders)
        {
            var counts = new Dictionary<string, int>();
            var largest = string.Empty;
            var max = 0;

            for (var i = 0; i < messages.Length; i++)
            {
                var message = messages[i];
                var sender = senders[i];

                if (!counts.TryGetValue(sender, out var count))
                    counts.Add(sender, 0);

                var words = message.Split(' ').Length;
                count += words;
                counts[sender] = count;

                if (count <= max &&
                    (count != max || String.Compare(largest, sender, StringComparison.Ordinal) >= 0)) continue;

                largest = sender;
                max = count;
            }

            return largest;
        }
    }

    [Fact]
    public void Test1()
    {
        var messages = new[] { "Hello userTwooo", "Hi userThree" };
        var senders = new[] { "Alice", "alice" };
        var solution = new Solution();
        Assert.Equal("alice", solution.LargestWordCount(messages, senders));
    }

    [Fact]
    public void Example1()
    {
        var messages = new[] { "Hello userTwooo", "Hi userThree", "Wonderful day Alice", "Nice day userThree" };
        var senders = new[] { "Alice", "userTwo", "userThree", "Alice" };
        var solution = new Solution();
        Assert.Equal("Alice", solution.LargestWordCount(messages, senders));
    }

    [Fact]
    public void Example2()
    {
        var messages = new[] { "How is leetcode for everyone", "Leetcode is useful for practice" };
        var senders = new[] { "Bob", "Charlie" };
        var solution = new Solution();
        Assert.Equal("Charlie", solution.LargestWordCount(messages, senders));
    }
}