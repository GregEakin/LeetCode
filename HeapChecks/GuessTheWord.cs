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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HeapChecks;

public class GuessTheWord
{
    class Master
    {
        private readonly string[] _wordList;
        private readonly string _secret;
        private readonly int _count;
        private bool _guessed;
        private int _guesses;

        public Master(string[] wordList, string secret, int count)
        {
            _wordList = wordList;
            _secret = secret;
            _count = count;
        }

        public int Guesses => _guesses;
        public bool Guessed => _guesses <= _count && _guessed;

        public int Guess(string word)
        {
            _guesses++;
            if (!_wordList.Contains(word))
                return -1;

            if (word == _secret)
            {
                _guessed = true;
                return word.Length;
            }

            var count = 0;
            for (var i = 0; i < 6; i++)
                if (word[i] == _secret[i])
                    count++;

            return count;
        }
    }

    class SolutionNew
    {
        public static int MatchLetters(string word1, string word2)
        {
            var count = 0;
            for (var i = 0; i < 6; i++)
                if (word1[i] == word2[i])
                    count++;

            return count;
        }

        public void FindSecretWord(string[] wordlist, Master master)
        {
            var seen = new BitArray(wordlist.Length);
            for (var i = 0; i < 10; i++)
            {
                var min = int.MaxValue;
                var chooseWord = 0;
                for (var j = 0; j < wordlist.Length - 1; j++)
                {
                    if (seen[j]) continue;
                    var matchList = new int[7];
                    for (var k = j + 1; k < wordlist.Length; k++)
                        if (!seen[k])
                            matchList[MatchLetters(wordlist[j], wordlist[k])]++;

                    var maxNumber = matchList.Max();
                    if (min <= maxNumber) continue;
                    min = maxNumber;
                    chooseWord = j;
                }

                seen[chooseWord] = true;
                var result = master.Guess(wordlist[chooseWord]);
                if (result == 6) return;
                for (var j = 0; j < wordlist.Length; j++)
                    if (!seen[j] && MatchLetters(wordlist[j], wordlist[chooseWord]) != result)
                        seen[j] = true;
            }
        }
    }

    class SolutionGuess
    {
        private static int MatchLetters(string word1, string word2)
        {
            var count = 0;
            for (var i = 0; i < 6; i++)
                if (word1[i] == word2[i])
                    count++;

            return count;
        }

        public void FindSecretWord(string[] wordlist, Master master)
        {
            var dict = new Dictionary<string, HashSet<string>>[6];
            for (var i = 0; i < 6; i++)
                dict[i] = new Dictionary<string, HashSet<string>>();

            for (var i = 0; i < wordlist.Length - 1; i++)
            {
                var word1 = wordlist[i];
                for (var j = i + 1; j < wordlist.Length; j++)
                {
                    var word2 = wordlist[j];
                    var matches = MatchLetters(word1, word2);

                    if (!dict[matches].TryGetValue(word1, out var set1))
                    {
                        set1 = new HashSet<string>();
                        dict[matches][word1] = set1;
                    }

                    if (!dict[matches].TryGetValue(word2, out var set2))
                    {
                        set2 = new HashSet<string>();
                        dict[matches][word2] = set2;
                    }

                    set1.Add(word2);
                    set2.Add(word1);
                }
            }

            var list = new HashSet<string>(wordlist);
            while (list.Count > 0)
            {
                var word = list.ElementAt(0);
                var count = master.Guess(word);
                if (count == 6)
                    return;

                list.IntersectWith(dict[count][word]);
            }
        }
    }

    class Solution
    {
        private const int Size = 6;

        private string MostCommonWord(string[] wordlist)
        {
            var count = new int[Size][];
            for (var i = 0; i < Size; i++)
                count[i] = new int[26];

            foreach (var word in wordlist)
                for (var i = 0; i < Size; i++)
                    count[i][word[i] - 'a']++;

            var best = int.MinValue;
            var common = string.Empty;
            foreach (var word in wordlist)
            {
                var score = 0;
                for (var i = 0; i < Size; i++)
                    score += count[i][word[i] - 'a'];

                if (best >= score) continue;
                best = score;
                common = word;
            }

            return common;
        }

        private static int MatchLetters(string word1, string word2)
        {
            var count = 0;
            for (var i = 0; i < Size; i++)
                if (word1[i] == word2[i])
                    count++;

            return count;
        }

        public void FindSecretWord(string[] wordlist, Master master)
        {
            while (wordlist.Length > 0)
            {
                var guess = MostCommonWord(wordlist);
                var matches = master.Guess(guess);
                if (matches == Size)
                    return;

                wordlist = wordlist
                    .Where(word => word != guess)
                    .Where(word => matches == MatchLetters(guess, word))
                    .ToArray();
            }
        }
    }

    [Fact]
    public void Answer1()
    {
        var wordList = new[]
        {
            "wichbx", "oahwep", "tpulot", "eqznzs", "vvmplb", "eywinm", "dqefpt", "kmjmxr", "ihkovg", "trbzyb",
            "xqulhc", "bcsbfw", "rwzslk", "abpjhw", "mpubps", "viyzbc", "kodlta", "ckfzjh", "phuepp", "rokoro",
            "nxcwmo", "awvqlr", "uooeon", "hhfuzz", "sajxgr", "oxgaix", "fnugyu", "lkxwru", "mhtrvb", "xxonmg",
            "tqxlbr", "euxtzg", "tjwvad", "uslult", "rtjosi", "hsygda", "vyuica", "mbnagm", "uinqur", "pikenp",
            "szgupv", "qpxmsw", "vunxdn", "jahhfn", "kmbeok", "biywow", "yvgwho", "hwzodo", "loffxk", "xavzqd",
            "vwzpfe", "uairjw", "itufkt", "kaklud", "jjinfa", "kqbttl", "zocgux", "ucwjig", "meesxb", "uysfyc",
            "kdfvtw", "vizxrv", "rpbdjh", "wynohw", "lhqxvx", "kaadty", "dxxwut", "vjtskm", "yrdswc", "byzjxm",
            "jeomdc", "saevda", "himevi", "ydltnu", "wrrpoc", "khuopg", "ooxarg", "vcvfry", "thaawc", "bssybb",
            "ccoyyo", "ajcwbj", "arwfnl", "nafmtm", "xoaumd", "vbejda", "kaefne", "swcrkh", "reeyhj", "vmcwaf",
            "chxitv", "qkwjna", "vklpkp", "xfnayl", "ktgmfn", "xrmzzm", "fgtuki", "zcffuv", "srxuus", "pydgmq"
        };
        var master = new Master(wordList, "ccoyyo", 10);

        var solution = new Solution();
        solution.FindSecretWord(wordList, master);
        Assert.True(master.Guessed);
    }

    [Fact]
    public void Answer2()
    {
        var wordList = new[]
        {
            "abcdef", "acdefg", "adefgh", "aefghi", "afghij", "aghijk", "ahijkl", "aijklm", "ajklmn", "aklmno",
            "almnoz", "anopqr", "azzzzz"
        };
        var master = new Master(wordList, "azzzzz", 10);

        var solution = new Solution();
        solution.FindSecretWord(wordList, master);
        Assert.True(master.Guessed);
    }

    [Fact]
    public void Answer3()
    {
        var wordList = new[]
        {
            "gaxckt", "trlccr", "jxwhkz", "ycbfps", "peayuf", "yiejjw", "ldzccp", "nqsjoa", "qrjasy", "pcldos",
            "acrtag", "buyeia", "ubmtpj", "drtclz", "zqderp", "snywek", "caoztp", "ibpghw", "evtkhl", "bhpfla",
            "ymqhxk", "qkvipb", "tvmued", "rvbass", "axeasm", "qolsjg", "roswcb", "vdjgxx", "bugbyv", "zipjpc",
            "tamszl", "osdifo", "dvxlxm", "iwmyfb", "wmnwhe", "hslnop", "nkrfwn", "puvgve", "rqsqpq", "jwoswl",
            "tittgf", "evqsqe", "aishiv", "pmwovj", "sorbte", "hbaczn", "coifed", "hrctvp", "vkytbw", "dizcxz",
            "arabol", "uywurk", "ppywdo", "resfls", "tmoliy", "etriev", "oanvlx", "wcsnzy", "loufkw", "onnwcy",
            "novblw", "mtxgwe", "rgrdbt", "ckolob", "kxnflb", "phonmg", "egcdab", "cykndr", "lkzobv", "ifwmwp",
            "jqmbib", "mypnvf", "lnrgnj", "clijwa", "kiioqr", "syzebr", "rqsmhg", "sczjmz", "hsdjfp", "mjcgvm",
            "ajotcx", "olgnfv", "mjyjxj", "wzgbmg", "lpcnbj", "yjjlwn", "blrogv", "bdplzs", "oxblph", "twejel",
            "rupapy", "euwrrz", "apiqzu", "ydcroj", "ldvzgq", "zailgu", "xgqpsr", "wxdyho", "alrplq", "brklfk"
        };
        var master = new Master(wordList, "hbaczn", 10);

        var solution = new Solution();
        solution.FindSecretWord(wordList, master);
        Assert.True(master.Guessed);
    }

    [Fact]
    public void Example1()
    {
        var wordList = new[] { "acckzz", "ccbazz", "eiowzz", "abcczz" };
        var master = new Master(wordList, "acckzz", 10);

        var solution = new Solution();
        solution.FindSecretWord(wordList, master);
        Assert.True(master.Guessed);
    }

    [Fact]
    public void Example2()
    {
        var wordList = new[] { "hamada", "khaled" };
        var master = new Master(wordList, "hamada", 10);

        var solution = new Solution();
        solution.FindSecretWord(wordList, master);
        Assert.True(master.Guessed);
    }

    [Fact]
    public void Test1()
    {
        var wordList = new[]
            { "kzzacc", "zzkcca", "cccccc", "dddddd", "aaaaaa", "acckzz", "ccbazz", "eiowzz", "abcczz" };
        var master = new Master(wordList, "acckzz", 10);

        var solution = new Solution();
        solution.FindSecretWord(wordList, master);
        Assert.True(master.Guessed);
    }

    [Fact]
    public void Test2()
    {
        var wordList = new[]
        {
            "bcsbfw", "abpjhw", "uysfyc", "ccoyyo", "zcffuv"
        };
        var master = new Master(wordList, "ccoyyo", 10);

        var solution = new Solution();
        solution.FindSecretWord(wordList, master);
        Assert.Equal(3, master.Guesses);
    }
}