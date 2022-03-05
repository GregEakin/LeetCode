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

public class MedianOfTwoSortedArraysTest
{
    public class Solution
    {
        public static double FindMedianSortedArrays(int[] nums1, int[] nums2)
        {
            if (nums2.Length < nums1.Length) return FindMedianSortedArrays(nums2, nums1);
            var n1 = nums1.Length;
            var n2 = nums2.Length;
            var low = 0;
            var high = n1;

            while (low <= high)
            {
                var cut1 = (low + high) / 2;
                var cut2 = (n1 + n2 + 1) / 2 - cut1;

                var left1 = cut1 == 0 ? int.MinValue : nums1[cut1 - 1];
                var left2 = cut2 == 0 ? int.MinValue : nums2[cut2 - 1];
                var right1 = cut1 == n1 ? int.MaxValue : nums1[cut1];
                var right2 = cut2 == n2 ? int.MaxValue : nums2[cut2];

                if (left1 <= right2 && left2 <= right1)
                    return (n1 + n2) % 2 == 0
                        ? (Math.Max(left1, left2) + Math.Min(right1, right2)) / 2.0
                        : Math.Max(left1, left2);

                if (left1 > right2)
                    high = cut1 - 1;
                else
                    low = cut1 + 1;
            }

            return 0.0;
        }
    }

    [Fact]
    public void Answer1()
    {
        var nums1 = new[] { 1 };
        var nums2 = new[] { 1 };
        Assert.Equal(1.0, Solution.FindMedianSortedArrays(nums1, nums2));
    }

    [Fact]
    public void Test1()
    {
        var nums1 = new[] { 1, 2, 3, 4 };
        var nums2 = new[] { 5, 6, 8, 9 };
        Assert.Equal(4.5, Solution.FindMedianSortedArrays(nums1, nums2));
    }

    [Fact]
    public void Test2()
    {
        var nums1 = new[] { 1 };
        var nums2 = Array.Empty<int>();
        Assert.Equal(1.0, Solution.FindMedianSortedArrays(nums1, nums2));
    }

    [Fact]
    public void Test3()
    {
        var nums1 = new[] { -1000000 };
        var nums2 = new[] { 1000000 };
        Assert.Equal(0.0, Solution.FindMedianSortedArrays(nums1, nums2));
    }

    [Fact]
    public void Test4()
    {
        var nums1 = Array.Empty<int>();
        var nums2 = new[] { 1, 2, 3, 4, 5, 6, 8, 9 };
        Assert.Equal(4.5, Solution.FindMedianSortedArrays(nums1, nums2));
    }

    [Fact]
    public void Test5()
    {
        var nums1 = new[] { 1, 3, 3, 6, 7, 8, 9 };
        var nums2 = Array.Empty<int>();
        Assert.Equal(6.0, Solution.FindMedianSortedArrays(nums1, nums2));
    }

    [Fact]
    public void PerfTest()
    {
        var nums1 = new int[1000];
        nums1[0] = -10;
        nums1[1] = -5;
        for (var i = 2; i < nums1.Length; i++)
            nums1[i] = 2 * i - 100;

        var nums2 = new int[1000];
        nums2[0] = -20;
        nums2[1] = -15;
        for (var i = 2; i < nums2.Length; i++)
            nums2[i] = 2 * i + 100;
        Assert.Equal(999.0, Solution.FindMedianSortedArrays(nums1, nums2));
    }

    [Fact]
    public void Example1()
    {
        var nums1 = new[] { 1, 3 };
        var nums2 = new[] { 2 };
        Assert.Equal(2.0, Solution.FindMedianSortedArrays(nums1, nums2));
    }

    [Fact]
    public void Example2()
    {
        var nums1 = new[] { 1, 2 };
        var nums2 = new[] { 3, 4 };
        Assert.Equal(2.5, Solution.FindMedianSortedArrays(nums1, nums2));
    }
}