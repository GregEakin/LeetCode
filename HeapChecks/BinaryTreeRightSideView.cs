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
using System.Linq;
using Xunit;

namespace HeapChecks;

public class BinaryTreeRightSideView
{
    public class SolutionBad
    {
        public IList<int> RightSideView(TreeNode? root)
        {
            var values = new List<int>();
            while (root != null)
            {
                values.Add(root.val);
                root = root.right;
            }

            return values;
        }
    }

    public class SolutionCheat
    {
        public IList<int> RightSideView(TreeNode? root)
        {
            var result = new List<int>();
            RightView(root, result, 0);
            return result;
        }

        public void RightView(TreeNode? curr, IList<int> result, int currDepth)
        {
            if (curr == null)
                return;

            if (currDepth == result.Count) 
                result.Add(curr.val);

            RightView(curr.right, result, currDepth + 1);
            RightView(curr.left, result, currDepth + 1);
        }
    }

    // Solution 1: Recursive, combine right and left: 5 lines, 56 ms
    // O(n^2)
    public class Solution1
    {
        public IList<int> RightSideView(TreeNode? root)
        {
            if (root == null) return Array.Empty<int>();

            var right = RightSideView(root.right);
            var left = RightSideView(root.left);
            var list = new List<int>();
            list.Add(root.val);
            list.AddRange(right);
            list.AddRange(left.Skip(right.Count));
            return list;
        }
    }

    // Solution 2: Recursive, first come first serve: 9 lines, 48 ms
    // O(n)
    public class Solution2
    {
        private IList<int>? view = null;
        public void Collect(TreeNode? node, int depth)
        {
            if (node == null) return;
            if (depth == view!.Count)
                view.Add(node.val);

            Collect(node.right, depth + 1);
            Collect(node.left, depth + 1);
        }

        public IList<int> RightSideView(TreeNode? root)
        {
            view = new List<int>();
            Collect(root, 0);
            return view;
        }
    }

    // Solution 3: Iterative, level-by-level: 7 lines, 48 ms
    // O(n)
    public class Solution
    {
        public IList<int> RightSideView(TreeNode? root)
        {
            var view = new List<int>();
            if (root == null) 
                return view;

            var queue = new Queue<TreeNode>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var count = queue.Count;
                for (var i = 0; i < count; i++)
                {
                    var next = queue.Dequeue();
                    if (i == 0) 
                        view.Add(next.val);
                    if (next.right != null)
                        queue.Enqueue(next.right);
                    if (next.left != null)
                        queue.Enqueue(next.left);
                }
            }

            return view;
        }
    }

    [Fact]
    public void Example1()
    {
        var root = new int?[] { 1, 2, 3, null, 5, null, 4 };
        var solution = new Solution();
        Assert.Equal(new[] { 1, 3, 4 }, solution.RightSideView(TreeNode.Builder(root)!));
    }

    [Fact]
    public void Example2()
    {
        var root = new int?[] { 1, null, 3 };
        var solution = new Solution();
        Assert.Equal(new[] { 1, 3 }, solution.RightSideView(TreeNode.Builder(root)!));
    }

    [Fact]
    public void Example3()
    {
        var solution = new Solution();
        Assert.Equal(Array.Empty<int>(), solution.RightSideView(null));
    }

    [Fact]
    public void Answer1()
    {
        var root = new int?[] { 1, 2 };
        var solution = new Solution();
        Assert.Equal(new[] { 1, 2 }, solution.RightSideView(TreeNode.Builder(root)!));
    }

    [Fact]
    public void Test1()
    {
        var root = new int?[] { 1, 2, 3, null, 5 };
        var solution = new Solution();
        Assert.Equal(new[] { 1, 3, 5 }, solution.RightSideView(TreeNode.Builder(root)!));
    }
}