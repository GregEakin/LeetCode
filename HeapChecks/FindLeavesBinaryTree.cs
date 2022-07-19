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

public class FindLeavesBinaryTree
{
    public class Solution
    {
        public IList<IList<int>> FindLeaves(TreeNode root)
        {
            var result = new List<IList<int>>();
            var node = root;
            do
            {
                var leafs = new List<int>();
                if (Visit(node, leafs)) node = null;
                result.Add(leafs);
            } while (node != null);

            return result;
        }

        public static bool Visit(TreeNode node, IList<int> leafs)
        {
            if (node.left == null && node.right == null)
            {
                leafs.Add(node.val);
                return true;
            }

            if (node.left != null)
                if (Visit(node.left, leafs))
                    node.left = null;

            if (node.right != null)
                if (Visit(node.right, leafs))
                    node.right = null;

            return false;
        }
    }

    [Fact]
    public void Example1()
    {
        var root = TreeNode.Builder(new int?[] { 1, 2, 3, 4, 5 });
        var solution = new Solution();
        Assert.Equal(new List<List<int>> { new() { 4, 5, 3 }, new() { 2 }, new() { 1 } }, solution.FindLeaves(root!));
    }

    [Fact]
    public void Example2()
    {
        var root = TreeNode.Builder(new int?[] { 1 });
        var solution = new Solution();
        Assert.Equal(new List<List<int>> { new() { 1 } }, solution.FindLeaves(root!));
    }
}