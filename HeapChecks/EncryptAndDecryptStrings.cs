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
using System.Text;
using Xunit;

namespace HeapChecks;

public class EncryptAndDecryptStrings
{
    public class Encrypter
    {
        private readonly Dictionary<char, string> _encode = new();
        private readonly Dictionary<string, int> _count = new();

        public Encrypter(char[] keys, string[] values, string[] dictionary)
        {
            for (var i = 0; i < keys.Length; i++)
                _encode[keys[i]] = values[i];

            foreach (var w in dictionary)
            {
                var e = Encrypt(w);
                _count[e] = _count.TryGetValue(e, out var sum)
                    ? sum + 1
                    : 1;
            }
        }

        public string Encrypt(string word1)
        {
            var builder = new StringBuilder();
            foreach (var c in word1)
                builder.Append(_encode[c]);

            return builder.ToString();
        }

        public int Decrypt(string word2)
        {
            return _count.TryGetValue(word2, out var sum) ? sum : 0;
        }
    }

    [Fact]
    public void Example1Encrypt()
    {
        var keys = new[] { 'a', 'b', 'c', 'd' };
        var values = new[] { "ei", "zf", "ei", "am" };
        var dictionary = new[] { "abcd", "acbd", "adbc", "badc", "dacb", "cadb", "cbda", "abad" };

        var encrypter = new Encrypter(keys, values, dictionary);
        var cypher = encrypter.Encrypt("abcd");
        Assert.Equal("eizfeiam", cypher);
        // 'a' maps to "ei", 'b' maps to "zf", 'c' maps to "ei", and 'd' maps to "am".
    }

    [Fact]
    public void Example1Decrypt()
    {
        var keys = new[] { 'a', 'b', 'c', 'd' };
        var values = new[] { "ei", "zf", "ei", "am" };
        var dictionary = new[] { "abcd", "acbd", "adbc", "badc", "dacb", "cadb", "cbda", "abad" };

        var encrypter = new Encrypter(keys, values, dictionary);
        var count = encrypter.Decrypt("eizfeiam");
        Assert.Equal(2, count);
        // "ei" can map to 'a' or 'c', "zf" maps to 'b', and "am" maps to 'd'. 
        // Thus, the possible strings after decryption are "abad", "cbad", "abcd", and "cbcd". 
        // 2 of those strings, "abad" and "abcd", appear in dictionary, so the answer is 2.
    }

    [Fact]
    public void Answer1()
    {
        var keys = new[] { 'a', 'b', 'c', 'z' };
        var values = new[] { "aa", "bb", "cc", "zz" };
        var dictionary = new[] { "ab", "ba", "a", "zz", "zzaa", "zabc" };

        // ["Encrypter","decrypt","encrypt","decrypt","encrypt","decrypt","decrypt"]
        // [[["a","b","c","z"],["aa","bb","cc","zz"],["ab","ba","a","zz","zzaa","zabc"]],["aabbcczz"],["aabbczz"],["aa"],["zzzzaaaa"],["zzaabbcc"],["asdfghjklm"]]

        // [null,1,"aaaabbbbcczzzz",1,"zzzzzzzzaaaaaaaa",1,0]

        var encrypter = new Encrypter(keys, values, dictionary);
        Assert.Equal(0, encrypter.Decrypt("aabbcczz"));
    }

    [Fact]
    public void Answer2()
    {
        var keys = new[] { 'a', 'b', 'c', 'z' };
        var values = new[] { "aa", "bb", "cc", "zz" };
        var dictionary = new[] { "ab", "ba", "a", "zz", "zzaa", "zabc" };

        // ["Encrypter","decrypt","encrypt","decrypt","encrypt","decrypt","decrypt"]
        // [[["a","b","c","z"],["aa","bb","cc","zz"],["ab","ba","a","zz","zzaa","zabc"]],["aabbcczz"],["aabbczz"],["aa"],["zzzzaaaa"],["zzaabbcc"],["asdfghjklm"]]

        // [null,1,"aaaabbbbcczzzz",1,"zzzzzzzzaaaaaaaa",1,0]

        var encrypter = new Encrypter(keys, values, dictionary);
        var cypher = encrypter.Encrypt("aabbczz");
        Assert.Equal("aaaabbbbcczzzz", cypher);
    }

    [Fact]
    public void Answer3()
    {
        var keys = new[] { 'a', 'b', 'c', 'z' };
        var values = new[] { "aa", "bb", "cc", "zz" };
        var dictionary = new[] { "ab", "ba", "a", "zz", "zzaa", "zabc" };

        // ["Encrypter","decrypt","encrypt","decrypt","encrypt","decrypt","decrypt"]
        // [[["a","b","c","z"],["aa","bb","cc","zz"],["ab","ba","a","zz","zzaa","zabc"]],["aabbcczz"],["aabbczz"],["aa"],["zzzzaaaa"],["zzaabbcc"],["asdfghjklm"]]

        // [null,1,"aaaabbbbcczzzz",1,"zzzzzzzzaaaaaaaa",1,0]

        var encrypter = new Encrypter(keys, values, dictionary);
        Assert.Equal(1, encrypter.Decrypt("zzaabbcc"));
    }

    [Fact]
    public void Answer4()
    {
        var keys = new[] { 'a', 'b', 'c', 'z' };
        var values = new[] { "aa", "bb", "cc", "zz" };
        var dictionary = new[] { "ab", "ba", "a", "zz", "zzaa", "zabc" };

        // ["Encrypter","decrypt","encrypt","decrypt","encrypt","decrypt","decrypt"]
        // [[["a","b","c","z"],["aa","bb","cc","zz"],["ab","ba","a","zz","zzaa","zabc"]],["aabbcczz"],["aabbczz"],["aa"],["zzzzaaaa"],["zzaabbcc"],["asdfghjklm"]]

        // [null,1,"aaaabbbbcczzzz",1,"zzzzzzzzaaaaaaaa",1,0]

        var encrypter = new Encrypter(keys, values, dictionary);
        Assert.Equal("zzzzzzzzaaaaaaaa", encrypter.Encrypt("zzzzaaaa"));
    }

    [Fact]
    public void Answer5()
    {
        var keys = new[] { 'a', 'b', 'c', 'z' };
        var values = new[] { "aa", "bb", "cc", "zz" };
        var dictionary = new[] { "ab", "ba", "a", "zz", "zzaa", "zabc" };

        // ["Encrypter","decrypt","encrypt","decrypt","encrypt","decrypt","decrypt"]
        // [[["a","b","c","z"],["aa","bb","cc","zz"],["ab","ba","a","zz","zzaa","zabc"]],["aabbcczz"],["aabbczz"],["aa"],["zzzzaaaa"],["zzaabbcc"],["asdfghjklm"]]

        // [null,1,"aaaabbbbcczzzz",1,"zzzzzzzzaaaaaaaa",1,0]

        var encrypter = new Encrypter(keys, values, dictionary);
        Assert.Equal(1, encrypter.Decrypt("zzaabbcc"));
    }

    [Fact]
    public void Answer6()
    {
        var keys = new[] { 'a', 'b', 'c', 'z' };
        var values = new[] { "aa", "bb", "cc", "zz" };
        var dictionary = new[] { "ab", "ba", "a", "zz", "zzaa", "zabc" };

        // ["Encrypter","decrypt","encrypt","decrypt","encrypt","decrypt","decrypt"]
        // [[["a","b","c","z"],["aa","bb","cc","zz"],["ab","ba","a","zz","zzaa","zabc"]],["aabbcczz"],["aabbczz"],["aa"],["zzzzaaaa"],["zzaabbcc"],["asdfghjklm"]]

        // [null,1,"aaaabbbbcczzzz",1,"zzzzzzzzaaaaaaaa",1,0]

        var encrypter = new Encrypter(keys, values, dictionary);
        Assert.Equal(0, encrypter.Decrypt("asdfghjklm"));
    }
}