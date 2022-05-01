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
using Xunit;

namespace HeapChecks;

public class BinaryTreeLevelOrderTraversal
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
    public class Solution
    {
        public IList<IList<int>> LevelOrder(TreeNode root)
        {
            var levels = new List<IList<int>>();
            if (root == null) return levels;

            var queue = new Queue<(int, TreeNode)>();
            queue.Enqueue((1, root));
            levels.Add(new List<int> { root.val });
            while (queue.Count > 0)
            {
                var (level, node) = queue.Dequeue();
                var values = new List<int>();
                if (node.left != null)
                {
                    values.Add(node.left.val);
                    queue.Enqueue((level + 1, node.left));
                }

                if (node.right != null)
                {
                    values.Add(node.right.val);
                    queue.Enqueue((level + 1, node.right));
                }

                if (values.Count <= 0) continue;
                
                if (level < levels.Count)
                    levels[level] = levels[level].Concat(values).ToList();
                else
                    levels.Add(values);
            }

            return levels;
        }
    }

    [Fact]
    public void Test1()
    {
        var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        var root = TreeNode.Builder(data);
        var solution = new Solution();
        var answer = new[]
            { new[] { 1 }, new[] { 2, 3 }, new[] { 4, 5, 6, 7 }, new[] { 8, 9, 10, 11, 12, 13, 14, 15 }, new[] { 16 } };
        Assert.Equal(answer, solution.LevelOrder(root!));
    }

    [Fact]
    public void Example1()
    {
        var data = new[] { 3, 9, 20, -1, -1, 15, 7 };
        var root = TreeNode.Builder(data);
        var solution = new Solution();
        Assert.Equal(new IList<int>[] { new[] { 3 }, new[] { 9, 20 }, new[] { 15, 7 } }, solution.LevelOrder(root!));
    }

    [Fact]
    public void Example2()
    {
        var data = new[] { 1 };
        var root = TreeNode.Builder(data);
        var solution = new Solution();
        Assert.Equal(new IList<int>[] { new[] { 1 } }, solution.LevelOrder(root!));
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.Equal(new IList<int>[] { }, solution.LevelOrder(null));
    }
}