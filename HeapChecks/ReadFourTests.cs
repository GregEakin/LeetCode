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
using Xunit;

namespace HeapChecks;

public class ReadFourTests
{
    /**
     * The Read4 API is defined in the parent class Reader4.
     *     int Read4(char[] buf4);
     */
    public class Reader4
    {
        private readonly string _data;
        private int _index;

        public Reader4(string data)
        {
            _data = data;
        }

        public int Read4(char[] buf4)
        {
            var i = 0;
            for (; i < 4 && i < buf4.Length && _index < _data.Length; i++, _index++)
                buf4[i] = _data[_index];

            return i;
        }
    }

    public class Solution : Reader4
    {
        public Solution(string data)
            : base(data)
        {
        }

        public int Read(char[] buf, int n)
        {
            var index = 0;
            while (index < n && index < buf.Length)
            {
                var buf4 = new char[4];
                var read = Read4(buf4);
                var count = Math.Min(read, n - index);
                Array.Copy(buf4, 0, buf, index, count);
                index += count;
                if (count < 4)
                    break;
            }

            return index;
        }
    }

    [Fact]
    public void Answer1()
    {
        var file = "leetcode";
        var n = 5;
        var solution = new Solution(file);
        var buffer = new char[n];
        Assert.Equal(5, solution.Read(buffer, n));
        Assert.Equal("leetc", buffer);
    }

    [Fact]
    public void Test1()
    {
        var file = "abcdABCD1234";
        var n = 4;
        var solution = new Solution(file);
        var buffer = new char[n];
        Assert.Equal(4, solution.Read(buffer, n));
        Assert.Equal("abcd", buffer);
    }

    [Fact]
    public void Example1()
    {
        var file = "abc";
        var n = 4;
        var solution = new Solution(file);
        var buffer = new char[n];
        Assert.Equal(3, solution.Read(buffer, n));
        Assert.Equal("abc\0", buffer);
    }

    [Fact]
    public void Example2()
    {
        var file = "abcde";
        var n = 5;
        var solution = new Solution(file);
        var buffer = new char[n];
        Assert.Equal(5, solution.Read(buffer, n));
        Assert.Equal("abcde", buffer);
    }

    [Fact]
    public void Example3()
    {
        var file = "abcdABCD1234";
        var n = 12;
        var solution = new Solution(file);
        var buffer = new char[n];
        Assert.Equal(12, solution.Read(buffer, n));
        Assert.Equal("abcdABCD1234", buffer);
    }
}