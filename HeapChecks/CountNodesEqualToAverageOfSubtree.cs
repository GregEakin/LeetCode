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

public class CountNodesEqualToAverageOfSubtree
{
    public class Solution
    {
        public static (int, int, int) TreeCounter(TreeNode? node)
        {
            if (node == null) return (0, 0, 0);
            var (l1, l2, l3) = TreeCounter(node.left);
            var (r1, r2, r3) = TreeCounter(node.right);
            var n = 1 + l1 + r1;
            var sum = node.val + l2 + r2;
            var average = sum / n;
            var count = (average == node.val ? 1 : 0) + l3 + r3;
            return (n, sum, count);
        }

        public int AverageOfSubtree(TreeNode? root)
        {
            var (n, sum, count) = TreeCounter(root);
            return count;
        }
    }

    [Fact]
    public void Example1()
    {
        var data = "[4,8,5,0,1,null,6]";
        var codec = new SerializeDeserializeBst.Codec();
        var root = codec.Deserialize(data);
        var solution = new Solution();
        Assert.Equal(5, solution.AverageOfSubtree(root));
    }

    [Fact]
    public void Example2()
    {
        var data = "[1]";
        var codec = new SerializeDeserializeBst.Codec();
        var root = codec.Deserialize(data);
        var solution = new Solution();
        Assert.Equal(1, solution.AverageOfSubtree(root));
    }
}