//    Copyright 2022 Gregory Eakin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using Xunit;
using static HeapChecks.DeleteDuplicateFoldersInSystem;

namespace HeapChecks;

public class JumpGameVI
{
    // ❌ Solution - I (Brute-Force)
    // Time Complexity: O(k^N)
    // Space Complexity: O(N)
    public class SolutionBruteForce
    {
        public int MaxResult(int[] nums, int k, int i = 0)
        {
            if (i >= nums.Length - 1)
                return nums[^1];

            var score = int.MinValue;
            for (var j = 1; j <= k; j++)
                score = Math.Max(score, nums[i] + MaxResult(nums, k, i + j));

            return score;
        }
    }

    // ❌ Solution - II (Dynamic Programming (Memorization)- Top-Down Approach)
    // Time Complexity: O(k*N)
    // Space Complexity: O(N)
    public class SolutionTopDown
    {
        // recursive solver which finds max score to reach n-1 starting from ith index
        private int Solve(int[] nums, int[] dp, int k, int i)
        {
            if (dp[i] != int.MinValue)
                return dp[i]; // already calculated result for index i

            // try jumps of all length and choose the one which maximizes the score
            for (var j = 1; j <= k; j++)
                if (i + j < nums.Length)
                    dp[i] = Math.Max(dp[i], nums[i] + Solve(nums, dp, k, i + j));

            return dp[i];
        }

        public int MaxResult(int[] nums, int k)
        {
            var dp = new int[nums.Length];
            Array.Fill(dp, int.MinValue);
            dp[^1] = nums[^1];
            return Solve(nums, dp, k, 0);
        }
    }

    // ❌ Solution - III (Dynamic Programming (Tabulation) - Bottom-Up Approach)
    // Time Complexity: O(k*N)
    // Space Complexity: O(N)
    public class SolutionBottomUp
    {
        public int MaxResult(int[] nums, int k)
        {
            var dp = new int[nums.Length];
            Array.Fill(dp, int.MinValue);
            dp[0] = nums[0];
            for (var i = 1; i < nums.Length; i++)
            for (var j = 1; j <= k && i - j >= 0; j++) // try all jumps length
                dp[i] = Math.Max(dp[i],
                    dp[i - j] + nums[i]); // choose the jump from previous index which maximises score       

            return dp[^1];
        }
    }

    // ❌ Solution - IV
    // Time Complexity: O(N*log(k))
    // Space Complexity: O(N)
    public class SolutionOptimized
    {
        public int MaxResult(int[] nums, int k)
        {
            var dp = new int[nums.Length];
            Array.Fill(dp, int.MinValue);
            dp[0] = nums[0];

            // var s = new HashSet<int> { nums[0] };
            // for (var i = 1; i < nums.Length; i++)
            // {
            //     if (i > k)
            //         s.Remove(dp[i - k - 1]); // erase elements from which we cant jump to current index
            //
            //     s.Add(dp[i] = *rbegin(s) + nums[i]); // choose element with max score and jump from that to the current index
            // }

            return dp[^1];
        }
    }

    // ❌ Solution - V
    // Time Complexity: O(N)
    // Space Complexity: O(N)
    public class Solution
    {
        public int MaxResult(int[] nums, int k)
        {
            var n = nums.Length;
            var dp = new int[n];
            var q = new LinkedList<int>();
            q.AddLast(0);
            dp[0] = nums[0];
            for (var i = 1; i < n; i++)
            {
                if (q.First!.Value < i - k) 
                    q.RemoveFirst();                        // can't reach current index from index stored in q     

                dp[i] = nums[i] + dp[q.First!.Value];       // update max score for current index

                while (q.Count > 0 && dp[q.Last!.Value] <= dp[i]) 
                    q.RemoveLast();                         // pop indices which won't be ever chosen in the future

                q.AddLast(i);                               // insert current index
            }

            return dp[^1];
        }
    }

    [Fact]
    public void Example1()
    {
        var nums = new[] { 1, -1, -2, 4, -7, 3 };
        var solution = new Solution();
        Assert.Equal(7, solution.MaxResult(nums, 2));
    }

    [Fact]
    public void Example2()
    {
        var nums = new[] { 10, -5, -2, 4, 0, 3 };
        var solution = new Solution();
        Assert.Equal(17, solution.MaxResult(nums, 3));
    }

    [Fact]
    public void Example3()
    {
        var nums = new[] { 1, -5, -20, 4, -1, 3, -6, -3 };
        var solution = new Solution();
        Assert.Equal(0, solution.MaxResult(nums, 2));
    }

    [Fact]
    public void Test1()
    {
        var nums = new[]
        {
            -6204, 3659, -430, -6002, -4838, 6239, -1125, -1458, -3401, -4215, 1367, 2797, 1719, -4385, -3287, 751,
            2603, -4638, -6323, 7897, 5373, 1557, -3906, 1847, -6723, -7032, -4051, -2323, -4654, -413, -1255, -7193,
            4270, -3089, -1045, 5504, 5952, -256, 9301, 7278, -7893, 1696, -4123, 3584, -1893, 4857, 7552, 8510, -5504,
            250, 3922, 7512, 1147, 7819, 9835, 7226, -2193, 3624, 3220, -3047, 5, -9303, 4102, -7422, 9552, 4623, 1987,
            -2145, -5144, -9055, -1682, -7562, 9139, -1326, 8534, -1081, 5043, 1216, 1404, -7, -9091, 8256, 2807, -232,
            80, 766, 3221, -2305, -8238, -3036, -2866, -1003, -8682, 6515, -8654, -4154, 6832, -660, 7971, -473, 9038,
            2424, -6264, -3526, -9367, -8450, 6598, -5425, 6722, 3076, -6264, -7961, 6195, 8553, -91, -2523, 2249, 2666,
            2571, 8371, 3611, 180, 3863, -3667, 3315, 2343, 331, 6927, 2285, -5903, 9326, 9605, 8492, 3454, -6728, 3959,
            -864, -6292, -6191, -2429, 888, 5536, -868, -5147, -4173, 189, -7761, 1705, 1926, -7846, -5893, -2568, 8147,
            4787, -1045, -842, 2147, -3250, -2736, -6613, 1830, 4457, 539, 8863, 273, 9730, -7161, 6688, -8048, 9652,
            -7524, 1378, -1617, -7725, -294, 593, -3620, 2290, 1568, -8553, 5654, -4022, 3070, -7582, -8660, -6761,
            3077, -6441, 4807, -3806, 7916, 4454, -1544, -8190, -6753, 4949, -9084, -6404, -3377, -3432, -7533, -2575,
            -7064, 5790, 5572, 3140, -6315, 4376, 9890, 5008, 6987, -5399, -2272, -6441, -8983, -5994, -4902, 7875,
            -1747, -9093, 1616, -4970, 9331, -1597, 939, 7158, 8440, 2725, -6174, 5712, -2905, -4774, 4993, -9034,
            -2520, 3743, -1005, 5734, -2674, 8736, 2189, -3237, 6392, -5179, -5000, 6005, -5889, 9857, 3577, 6196,
            -2420, -1166, -5222, -6889, -724, -9828, -9068, 6328, -5092, 4234, -2626, 5914, -1529, -4521, -911, 4906,
            -2983, 9839, 6520, -8624, -7425, -9599, -9209, 2953, 9847, -4849, 4180, 2035, 3589, -8213, -3953, -9608,
            1933, -5592, -1322, -9509, -5084, -5500, -693, 8476, 8403, 3478, -5789, 3888, -6879, 4219, -3721, 2328,
            4216, 8978, 9551, -9003, -9151, 8338, 8707, 6974, -2041, 6230, 6686, 8265, 9547, -3982, 4906, 8591, -5813,
            -1674, -6957, 9370, -6023, 3081, -463, 7542, -3095, 4572, 9175, 7914, 2501, 7095, 1644, -1171, 1644, 4640,
            988, -1407, 5285, -5530, 5055, -4337, -8151, -5918, 7064, -6916, -8579, 1011, -7964, 1789, -2974, 9666,
            6015, -2953, 3439, 3557, 4203, -851, 8884, 2338, -4464, 4883, 825, -9348, 7881, 365, 2992, 9122, 1614,
            -2410, -9909, -3779, -689, 1187, -2956, -1400, -960, -5631, -4836, 7969, 8736, -983, -7872, 5031, 837, 6572,
            -2984, -216, -3640, 3406, -1748, -456, 7380, 2869, -1858, 1370, -4612, 344, 4460, -6255, 9548, 9254, 3954,
            -148, -6332, -7194, -3106, 8278, -4617, 2087, -8001, -8700, -6853, 1959, 2925, 5625, 4868, 2606, -6114,
            -8959, -7935, 6069, -9276, 9603, -7799, 1907, -4575, -7827, 5078, -6900, -8757, -9362, -3052, -3832, -661,
            -6137, 2529, -622, -1555, -2839, -1191, 6322, -9224, 3492, -2767, -5805, 1822, -9663, -5404, -27, 1084,
            8134, 5315, 448, 5724, 9566, -2351, 4914, -5755, 9542, 2371, 3732, 2124, 631, 2473, -8248, -3883, 7424,
            -2075, 1725, 138, -3775, -5655, -538, -9385, 5250, 5490, 3796, 7253, 3796, -9621, -8205, -5263, -5652, 4817,
            -9610, 6317, -8923, -985, 6452, 5333, 27, 5067, -353, -1321, -9321, -5957, -6705, -2549, -6572, 7681, 5013,
            -5761, -947, -9547, -6377, -8222, -5263, -3432, -9943, 7127, -4826, 8269, 3754, 3017, 1911, -8083, -8364,
            7369, 5915, -6435, 1606, 806, -9386, -4136, 1766, 356, 5489, 6705, -7333, 5534, 8994, 3397, -266, 8393,
            -710, 9856, -7039, 9175, -4295, -6872, -2153, -8649, -1719, -9877, 6436, -3789, 1153, 133, -8731, 5828,
            1800, 6300, 1104, -6472, -3947, -2707, -4630, -8133, 1389, -2234, -4529, -717, 8117, 7491, -3375, -2596,
            8852, -6385, -2960, -2279, 4664, -6948, -8555, 5130, 8728, -5028, 1873, 2271, -7353, 4368, -2832, 2351,
            1853, 1529, -7051, -7084, 7172, 8776, 2099, 8501, -4457, -8091, 5990, -1381, -7820, 2778, 3904, 5937, -853,
            151, -7072, 4699, -804, 8325, -3431, -2543, -7353, 8961, 7128, -5028, -6415, 6771, -6140, -8934, -6568,
            -4319, 1240, 8496, 9823, 8677, -6781, 5637, -8350, 3622, -8760, -3945, -6332, 1958, 5515, 1185, 9905, -915,
            1202, 6343, -4357, 3790, 9280, -825, -5352, 4744, -5492, 2045, 4445, -2175, -2717, 7880, 5760, -2676, -9004,
            -5248, 8328, 7736, 1062, -2780, 1206, 5717, -9229, 8316, -9290, 235, 6244, 2246, -3319, 5746, -4442, 1075,
            -8407, -1689, 9629, -4533, 5250, -3493, -6593, -944, 4652, 5720, -8350, -3481, 3167, -9438, 1379, 7427,
            4644, 7191, -3840, 29, 4333, -5029, -2463, -8284, 7553, -3330, 8640, 6949, -5821, -2994, 2021, -4794, 2367,
            2146, 6922, -1670, 1705, -4506, -4616, 4895, 6246, 1286, -2273, 7321, 8706, -2248, -1726, -5745, -8342, 867,
            3123, -5174, -1540, 2927, 3635, 8779, 1729, -4325, -2258, 6040, -209, 122, -145, 5415, -9758, 3467, 9822,
            -6925, -2151, -9835, 1678, 1705, -4250, -5643, -6058, -7327, 3416, -6223, -2195, 2636, 5313, -2163, -6508,
            -3316, -6012, -5480, 6177, 8557, -8993, -5370, 54, -1021, 6178, 5935, 2776, -654, 3812, -8319, -8717, -120,
            -5817, -5430, 40, -9513, 985, -4966, -4037, 7227, 2935, -4004, 5007, 2187, 1737, -3392, 640, -2596, 1845,
            898, -8655, -172, 1438, -9558, 2736, 1625, -610, 8039, -2431, 4714, -2489, -5816, -8741, -5380, 6726, -1412,
            -4427, 7585, -7792, -5293, 8895, 6510, -3903, -4787, 4296, -2057, -8930, -8918, -7936, -7166, -7764, -9199,
            -3844, -7279, 4819, -1487, -677, -4554, 476, -8931, -2540, -6353, -7855, -7536, -684, -4233, -4478, 2926,
            -8084, 2202, -2998, 3013, 4715, 7291, -406, -8243, -9586, -6115, -3546, -7243, 175, -4075, -8745, -2092,
            3443, 119, 8721, -8474, 848, -8105, -4614, 5712, 9540, -3476, 9060, 2098, 1178, 3840, -9675, -8005, 8086,
            8228, 4210, -9189, 904, -4075, -5159, 1218, -2360, 123, -2535, -9497, 3755, 2094, -8500, 7486, -144, 9208,
            -7342, 2204, 2321, 3053, 1725, 9924, 7473, -9538, -6344, 6660, 7833, 9668, 6134, 4407, -6497, -7948, -9014,
            -7277, -2114, 1092, -1260, -4372, 7441, 9401, 7693, -174, 4014, -9316, -1901, 480, 7431, 7060, 762, 3285,
            3390, -5782, 6113, -3776, -8221, -1276, 2130, -8558, 4411, 1575, 5529, 8600, 6478, -5256, 2834, -9839, 6999,
            -1625, 2446, -9132, -6614, 5049, 9137, -8986, -3537, 9924, 6818, 6351, 7016, 6140, -3671, 9320, 6443, 1632,
            -9529, 3014, 4538, 16, 5123, -3338, 2656, 2297, -2292, -8477, -8754, -8681, -5016, -9813, 8951, 9153, 8648,
            -8268, 8357, 2510, 7922, 2233, 4282, 161, -3851, 4630, -3679, 1567, -6702, 6057, -3830, 9188, -8831, -6134,
            5125, -6339, -4158, -8125, 5865, -2560, 3550, -6717, 8122, 1856, -5858, 4464, 8273, -9617, -1394, 1477,
            -9983, -3044, 2086
        };
        var solution = new Solution();
        Assert.Equal(2357772, solution.MaxResult(nums, 10));
    }
}