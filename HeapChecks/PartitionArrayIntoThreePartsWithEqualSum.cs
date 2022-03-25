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

using Xunit;

namespace HeapChecks;

public class PartitionArrayIntoThreePartsWithEqualSum
{
    // sum0 = arr[0] + arr[1] + ... + arr[i] 
    // sum1 = arr[i + 1] + arr[i + 2] + ... + arr[j - 1] 
    // sum2 = arr[j] + arr[j + 1] + ... + arr[arr.length - 1]

    public class Solution1
    {
        public bool CanThreePartsEqualSum(int[] arr)
        {
            var sum0 = 0;
            var sum1 = 0;
            for (var x = 0; x < arr.Length; x++)
                sum1 += arr[x];

            for (var i = 0; i < arr.Length - 2; i++)
            {
                sum0 += arr[i];
                sum1 -= arr[i];
                var s1 = sum1;
                var s2 = 0;
                for (var j = arr.Length - 1; j > i + 1; j--)
                {
                    s1 -= arr[j];
                    s2 += arr[j];
                    if (sum0 == s1 && s1 == s2)
                        return true;
                }
            }

            return false;
        }
    }

    public class Solution
    {
        public bool CanThreePartsEqualSum(int[] arr)
        {
            var sum = 0;
            foreach (var v in arr)
                sum += v;

            if (sum % 3 != 0) return false;
            sum /= 3;
            var p = 0;
            var a = 0;
            foreach (var v in arr)
            {
                p += v;
                if (p == sum)
                {
                    p = 0;
                    a++;
                }
            }

            return a >= 3;
        }
    }

    [Fact]
    public void Example1()
    {
        var arr = new[] { 0, 2, 1, -6, 6, -7, 9, 1, 2, 0, 1 };
        var solution = new Solution();
        Assert.True(solution.CanThreePartsEqualSum(arr));
    }

    [Fact]
    public void Example2()
    {
        var arr = new[] { 0, 2, 1, -6, 6, 7, 9, -1, 2, 0, 1 };
        var solution = new Solution();
        Assert.False(solution.CanThreePartsEqualSum(arr));
    }

    [Fact]
    public void Example3()
    {
        var arr = new[] { 3, 3, 6, 5, -2, 2, 5, 1, -9, 4 };
        var solution = new Solution();
        Assert.True(solution.CanThreePartsEqualSum(arr));
    }

    [Fact]
    public void Answer1()
    {
        var arr = new[] { 1, -1, 1, -1 };
        var solution = new Solution();
        Assert.False(solution.CanThreePartsEqualSum(arr));
    }
}
