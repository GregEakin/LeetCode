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
    public class TextEditorSlow
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

    public class TextEditor {
        private readonly StringBuilder _left = new();
        private readonly StringBuilder _right = new();

        public void AddText(string text) {
            _left.Append(text);
        }

        public int DeleteText(int k) {
            var length = Math.Min(_left.Length, k);
            _left.Remove(_left.Length - length, length);
            return length;
        }

        public string CursorLeft(int k) {
            var count = Math.Min(_left.Length, k);
            while (count-- > 0) {
                _right.Append(_left[^1]);
                _left.Length--;
            }
            
            return GetResult();
        }

        public string CursorRight(int k) {
            var count = Math.Min(_right.Length, k);
            while (count-- > 0) {
                _left.Append(_right[^1]);
                _right.Length--;
            }
            
            return GetResult();
        }

        private string GetResult() {
            var start = Math.Max(0, _left.Length - 10);
            var length = _left.Length - start;
            return _left.ToString(start, length);
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