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
using System.Text;
using Xunit;

namespace HeapChecks;

public class StepByStepDirectionsFromBinaryTreeNodeToAnother
{
    /**
     * Definition for a binary tree node.
     * public class TreeNode {
     *     public int val;
     *     public TreeNode left;
     *     public TreeNode right;
     *     public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
     *         this.val = val;
     *         this.left = left;
     *         this.right = right;
     *     }
     * }
     */
    public class Solution2
    {
        public string GetDirections(TreeNode root, int startValue, int destValue)
        {
            var steps = new Stack<(List<int>, string)>();
            var start = (new List<int>(), string.Empty);
            var end = (new List<int>(), string.Empty);

            var nodes = new Stack<TreeNode>();
            nodes.Push(root);
            steps.Push((new List<int> { root.val }, "*"));
            while (nodes.Count > 0)
            {
                var node = nodes.Pop();
                var (d1, d2) = steps.Pop();
                if (node.val == startValue || node.val == destValue)
                    if (start.Item2 == string.Empty)
                        start = (d1, d2);
                    else if (end.Item2 == string.Empty)
                        end = (d1, d2);

                if (start.Item2 != string.Empty && end.Item2 != string.Empty)
                    break;

                if (node.left != null)
                {
                    nodes.Push(node.left);
                    steps.Push((new List<int>(d1) { node.left.val }, d2 + 'L'));
                }

                if (node.right != null)
                {
                    nodes.Push(node.right);
                    steps.Push((new List<int>(d1) { node.right.val }, d2 + 'R'));
                }
            }

            var i = 0;
            while (i < start.Item1.Count && i < end.Item1.Count && start.Item1[i] == end.Item1[i]) i++;

            var buffer = new StringBuilder();
            if (start.Item1[^1] == startValue)
            {
                buffer.Append('U', start.Item2.Length - i);
                buffer.Append(end.Item2[i..]);
            }
            else if (end.Item1[^1] == startValue)
            {
                buffer.Append('U', end.Item2.Length - i);
                buffer.Append(start.Item2[i..]);
            }

            return buffer.ToString();
        }
    }

    public class Solution
    {
        public string GetDirections(TreeNode root, int startValue, int destValue)
        {
            var reverse = new Dictionary<int, (int, char)> { [root.val] = (0, 'U') };
            var nodes = new Queue<TreeNode>();
            nodes.Enqueue(root);

            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                if (node.left != null)
                {
                    nodes.Enqueue(node.left);
                    reverse[node.left.val] = (node.val, 'L');
                }

                if (node.right != null)
                {
                    nodes.Enqueue(node.right);
                    reverse[node.right.val] = (node.val, 'R');
                }
            }

            var start = new StringBuilder();
            while (startValue != 0)
            {
                var (val, dir) = reverse[startValue];
                start.Append(dir);
                startValue = val;
            }

            var dest = new StringBuilder();
            while (destValue != 0)
            {
                var (val, dir) = reverse[destValue];
                dest.Append(dir);
                destValue = val;
            }

            var i = 0;
            while (start.Length > i && dest.Length > i && start[^(i + 1)] == dest[^(i + 1)])
                i++;

            var buffer = new StringBuilder();
            buffer.Append('U', start.Length - i);
            for (var j = dest.Length - i - 1; j >= 0; j--)
                buffer.Append(dest[j]);

            return buffer.ToString();
        }
    }


    [Fact]
    public void Example1()
    {
        var root = TreeNode.Builder(new[] { 5, 1, 2, 3, -1, 6, 4 })!;
        var solution = new Solution();
        Assert.Equal("UURL", solution.GetDirections(root, 3, 6));
    }

    [Fact]
    public void Example2()
    {
        var root = TreeNode.Builder(new[] { 2, 1 })!;
        var solution = new Solution();
        Assert.Equal("L", solution.GetDirections(root, 2, 1));
    }

    [Fact]
    public void Test1()
    {
        var root = TreeNode.Builder(new[] { 2, 1 })!;
        var solution = new Solution();
        Assert.Equal("U", solution.GetDirections(root, 1, 2));
    }
}