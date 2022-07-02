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
using System.Text;
using Xunit;

namespace HeapChecks;

public class TextEditorTests
{
    public class TextEditor
    {
        private readonly StringBuilder _buffer = new();
        private int _index;

        public void AddText(string text)
        {
            _buffer.Insert(_index, text);
            _index += text.Length;
        }

        public int DeleteText(int k)
        {
            var start = Math.Max(0, _index - k);
            var len = Math.Min(k, _index);
            _buffer.Remove(start, len);
            _index -= len;
            return len;
        }

        public string CursorLeft(int k)
        {
            _index = Math.Max(0, _index - k);
            var start = Math.Max(0, _index - 10);
            var len = Math.Min(10, _index);
            return _buffer.ToString(start, len);
        }

        public string CursorRight(int k)
        {
            _index = Math.Min(_buffer.Length, _index + k);
            var start = Math.Max(0, _index - 10);
            var len = Math.Min(10, _index);
            return _buffer.ToString(start, len);
        }
    }

    public class TextEditorBad
    {
        class DoubleNode
        {
            public DoubleNode? Next { get; set; }
            public DoubleNode? Prev { get; set; }
            public char Buffer { get; set; }
        }

        private DoubleNode? _head;
        private DoubleNode? _ptr;
        private int _index;

        public void AddText(string text)
        {
            for (var i = text.Length - 1; i >= 0; i--)
            {
                var prev = _ptr?.Prev;
                var node = new DoubleNode()
                {
                    Next = _ptr,
                    Prev = prev,
                    Buffer = text[i],
                };

                _ptr = node;
            }

            if (_index == 0) 
                _head = _ptr;

            _index += text.Length;
        }

        public int DeleteText(int k)
        {
            var len = Math.Min(k, _index);
            for (var i = 0; i < len; i++)
            {
                var prev = _ptr?.Prev;
                if (prev != null)
                    prev.Next = _ptr?.Next;
                if (_ptr?.Next != null)
                    _ptr.Next.Prev = prev;

                _index--;
                if (_index == 0)
                    _head = _ptr;
            }
            _index -= len;
            return len;
        }

        public string CursorLeft(int k)
        {
            var newIndex = Math.Max(0, _index - k);
            while (_index > newIndex)
            {
                if (_ptr != null)
                    _ptr = _ptr.Prev;
                _index--;
            }

            var buffer = new StringBuilder();
            var start = Math.Max(0, _index - 10);
            var len = Math.Min(10, _index);
            var ptr = _ptr;
            for (var i = 0; i < len; i++)
            {
                if (ptr != null)
                    buffer.Insert(0, ptr.Buffer);
                ptr = ptr?.Prev;
            }

            return buffer.ToString();
        }

        public string CursorRight(int k)
        {
            for (var i = 0; i < k && _ptr?.Next != null; i++)
            {
                _ptr = _ptr.Next;
                _index++;
            }

            var buffer = new StringBuilder();
            var start = Math.Max(0, _index - 10);
            var len = Math.Min(10, _index);
            var ptr = _ptr;
            for (var i = 0; i < len; i++)
            {
                if (ptr != null)
                    buffer.Insert(0, ptr.Buffer);
                ptr = ptr?.Prev;
            }

            return buffer.ToString();
        }
    }

    [Fact]
    public void Example1()
    {
        var commands = new[]
        {
            "TextEditor", "addText", "deleteText", "addText", "cursorRight", "cursorLeft", "deleteText", "cursorLeft",
            "cursorRight"
        };
        var data = new object?[] { null, "leetcode", 4, "practice", 3, 8, 10, 2, 6 };
        var output = new object?[] { null, null, 4, null, "etpractice", "leet", 4, "", "practi" };

        TextEditor? solution = null;
        for (var i = 0; i < commands.Length; i++)
        {
            var command = commands[i];
            switch (command)
            {
                case "TextEditor":
                    solution = new TextEditor();
                    break;
                case "addText":
                    solution!.AddText((string)data[i]);
                    break;
                case "deleteText":
                    var k = solution!.DeleteText((int)data[i]);
                    Assert.Equal(output[i], k);
                    break;
                case "cursorRight":
                    var buffer1 = solution!.CursorRight((int)data[i]);
                    Assert.Equal(output[i], buffer1);
                    break;
                case "cursorLeft":
                    var buffer2 = solution!.CursorLeft((int)data[i]);
                    Assert.Equal(output[i], buffer2);
                    break;
            }
        }
    }
}