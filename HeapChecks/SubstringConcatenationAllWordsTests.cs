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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class SubstringConcatenationAllWordsTests
{
    public class Solution2
    {
        private string _s;
        private string[] _words;
        private List<int> _result;
        private int _count;
        private int _length;

        public void DoIt(IList<int> indices)
        {
            if (indices.Count == _count)
            {
                for (var i = 0; i <= _s.Length - _count * _length; i++)
                {
                    var found = true;
                    for (var j = 0; j < _count && found; j++)
                        found &= _s.Substring(i + j * _length, _length) == _words[indices[j]];

                    if (!found) continue;
                    if (_result.Contains(i)) break;
                    _result.Add(i);
                    break;
                }

                return;
            }

            for (var j = 0; j < _count; j++)
            {
                if (indices.Contains(j)) continue;
                indices.Add(j);
                DoIt(indices);
                indices.Remove(j);
            }
        }


        public IList<int> FindSubstring(string s, string[] words)
        {
            _s = s;
            _words = words;
            _result = new List<int>();
            _count = words.Length;
            _length = words[0].Length;

            DoIt(new List<int>());
            return _result;
        }
    }

    public class Solution
    {
        public IList<int> FindSubstring(string s, string[] words)
        {
            var result = new List<int>();

            var indicies = new int[words.Length];
            for (var i = 0; i < words.Length; i++)
                indicies[i] = -1;

            var length = words[0].Length;
            var count = words.Length;
            for (var i = 0; i <= s.Length - count * length; i++)
            {
                for (var j = 0; j < count; j++)
                {
                    var sub = s.Substring(i + j * length, length);
                }
            }

            return result;
        }
    }


    // [Fact]
    // public void Example1()
    // {
    //     var s = "barfoothefoobarman";
    //     var words = new[] { "foo", "bar" };
    //
    //     var solution = new Solution();
    //     Assert.Equal(new[] { 0, 9 }, solution.FindSubstring(s, words).OrderBy(t => t));
    // }

    [Fact]
    public void Example2()
    {
        var s = "wordgoodgoodgoodbestword";
        var words = new[] { "word", "good", "best", "word" };

        var solution = new Solution();
        Assert.Equal(Array.Empty<int>(), solution.FindSubstring(s, words).OrderBy(t => t));
    }

    // [Fact]
    // public void Example3()
    // {
    //     var s = "barfoofoobarthefoobarman";
    //     var words = new[] { "foo", "bar", "the" };
    //
    //     var solution = new Solution();
    //     Assert.Equal(new[] { 6, 9, 12 }, solution.FindSubstring(s, words).OrderBy(t => t));
    // }
    //
    // [Fact]
    // public void Test1()
    // {
    //     var s = "abaca";
    //     var words = new[] { "a", "b", "c", "a" };
    //
    //     var solution = new Solution();
    //     Assert.Equal(new[] { 0, 1 }, solution.FindSubstring(s, words).OrderBy(t => t));
    // }
}