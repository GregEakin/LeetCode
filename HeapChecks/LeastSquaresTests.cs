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

public class LeastSquaresTests
{
    public static class Solution
    {
        public static (double, double) LeastSquareLine((double x, double y, double f)[] data)
        {
            var xMean = 0.0;
            var yMean = 0.0;
            foreach (var (x, y, _) in data)
            {
                xMean += x;
                yMean += y;
            }

            xMean /= data.Length;
            yMean /= data.Length;

            var sumXx = 0.0;
            var sumXy = 0.0;
            foreach (var (x, y, _) in data)
            {
                sumXx += (x - xMean) * (x - xMean);
                sumXy += (x - xMean) * (y - yMean);
            }

            var a = sumXy / sumXx;
            var b = yMean - a * xMean;
            return (a, b);
        }
    }

    [Fact]
    public void Exercise1()
    {
        var data = new[] { (-2.0, 1.0, 1.2), (-1.0, 2.0, 1.9), (0.0, 3.0, 2.6), (1.0, 3.0, 3.3), (2.0, 4.0, 4.0) };
        var (a, b) = Solution.LeastSquareLine(data);
        Assert.Equal(0.70, a, 7);
        Assert.Equal(2.60, b, 7);
        foreach (var (x, _, f) in data) 
            Assert.Equal(f, a * x + b, 7);
        // E2(f) = 0.2449
    }

    [Fact]
    public void Exercise2()
    {
        var data = new[]
            { (-4.0, 1.2, 0.44), (-2.0, 2.8, 3.34), (0.0, 6.2, 6.24), (2.0, 7.8, 9.14), (4.0, 13.2, 12.04) };
        var (a, b) = Solution.LeastSquareLine(data);
        Assert.Equal(1.45, a, 7);
        Assert.Equal(6.24, b, 7);
        foreach (var (x, _, f) in data)
            Assert.Equal(f, a * x + b, 7);
        // E2(f) = 0.8958
    }

    [Fact]
    public void Exercise3()
    {
        var data = new[]
            { (-2.0, 1.0, 0.4), (0.0, 3.0, 3.3), (2.0, 6.0, 6.2), (4.0, 8.0, 9.1), (6.0, 13.0, 12.0) };
        var (a, b) = Solution.LeastSquareLine(data);
        Assert.Equal(1.45, a, 7);
        Assert.Equal(3.3, b, 7);
        foreach (var (x, _, f) in data)
            Assert.Equal(f, a * x + b, 7);
        // E2(f) = 0.7348
    }
}