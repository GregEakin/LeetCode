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

public class CountingSortTests
{
    public class CountingSort
    {
        public static void Sort(List<int> a, List<int> b, int k)
        {
            var c = new int[k];
            foreach (var aj in a)
                c[aj] += 1;
            // C[i] now contains the number of elements equal to i
            for (var i = 1; i < k; i++)
                c[i] += c[i - 1];
            // C[i] now contains the number of element less than or equal to i
            for (var j = a.Count - 1; j >= 0; j--)
            {
                b[c[a[j]] - 1] = a[j];
                c[a[j]] -= 1;
            }
        }
    }

    [Fact]
    public void Test1()
    {
        var a = new List<int> { 2, 5, 3, 0, 2, 3, 0, 3 };
        var b = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
        CountingSort.Sort(a, b, 6);

        Assert.Equal(new[] { 0, 0, 2, 2, 3, 3, 3, 5 }, b);
    }

    [Fact]
    public void Test2()
    {
        var a = new List<int> { 6, 0, 2, 0, 1, 3, 4, 6, 1, 3, 2 };
        var b = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        CountingSort.Sort(a, b, 7);

        Assert.Equal(new[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 6, 6 }, b);
    }
}