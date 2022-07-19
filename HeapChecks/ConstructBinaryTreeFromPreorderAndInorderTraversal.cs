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

public class ConstructBinaryTreeFromPreorderAndInorderTraversal
{
    public class Solution
    {
        private int[] _preorder;
        private Dictionary<int, int> _map;

        public TreeNode? Subtree(int p, int left, int right)
        {
            System.Console.WriteLine("p: {0}, left: {1}, right: {2}", p, left, right);
            if (p >= _preorder.Length || left >= right)
                return null;

            var value = _preorder[p];
            var i = _map[value];
            var leftNode = Subtree(p + 1, left, i);
            var rightNode = Subtree(p + 1 + i - left, i + 1, right);
            return new TreeNode(value, leftNode, rightNode);
        }

        public TreeNode? BuildTree(int[] preorder, int[] inorder)
        {
            _preorder = preorder;
            _map = new Dictionary<int, int>(inorder.Length);
            for (var i = 0; i < inorder.Length; i++)
                _map[inorder[i]] = i;

            return Subtree(0, 0, inorder.Length);
        }
    }


    // p 0, l 0, r 5, v = 3   , v = 
    // p 1, l 0, r 1, v = 9   , v = 
    // p 2, l 0, r 0, v = 20  , v = 9
    // p 2, l 1, r 1, v =     , v = 3
    // p 2, l 2, r 5, v =     , v = 
    // p 3, l 2, r 3, v = 15  , v = 
    // p 4, l 2, r 2, v = 7   , v = 15
    // p 4, l 3, r 3, v =     , v = 20
    // p 4, l 4, r 5, v =     , v = 
    // p 5, l 4, r 4, v = ?   , v = 7
    // p 5, l 5, r 5, v =     , v = ?
    //

    // 0: push null, 7 right
    // 0: push null, 7 left
    // 0: pop(null, 7 left)
    // 0: pop(null, 7 right)
    // push new 7, null, null, 20 right

    // 0: push null, 15 right
    // 0: push null, 15 left
    // 0: pop(null, 15 left)
    // 0: pop(null, 15 right)
    // push new 15, null, null, 20 left

    // pop(15, 20 left)
    // pop(7, 20 right)
    // push new 20, 15, 7, 3 right

    // 0: push null, 9 left
    // 0: push null, 9 right
    // 0: pop(null, 9 left)
    // 0: pop(null, 9 right)
    // push new 9, null, null, 3 left

    // pop(9, left)
    // pop(20, right)
    // push new 3, 9, 20, root


    public class Solution1
    {
        public TreeNode? BuildTree(int[] preorder, int[] inorder)
        {
            var map = new Dictionary<int, int>(inorder.Length);
            for (var i = 0; i < inorder.Length; i++)
                map[inorder[i]] = i;

            var node = (TreeNode?)null;
            var stack = new Stack<(int, int, int)>();
            var p = 0;
            var left = 0;
            while (p < preorder.Length && left < inorder.Length)
            {
                var v = preorder[p];
                var i = map[v];
                //var right = 


                // var left 
            }
            
            
            // stack.Push((0, 0, preorder.Length));
            // while (stack.Count > 0)
            // {
            //     var (p, left, right) = stack.Pop();
            //     if (left != right) stack.Push((0,0,0));
            //
            //     var leftNode = (TreeNode?)null;
            //     var rightNode = (TreeNode?)null;
            //
            //     node = new TreeNode(preorder[p], leftNode, rightNode);
            // }
            
            return node;
        }
    }


    [Fact]
    public void Example1()
    {
        var preorder = new[] { 3, 9, 20, 15, 7 };
        var inorder = new[] { 9, 3, 15, 20, 7 };

        var solution = new Solution();
        var tree = solution.BuildTree(preorder, inorder);
        Assert.Equal("[3,9,20,null,null,15,7]", tree!.ToString());
    }

    [Fact]
    public void Example2()
    {
        var preorder = new[] { -1 };
        var inorder = new[] { -1 };

        var solution = new Solution();
        var tree = solution.BuildTree(preorder, inorder);
        Assert.Equal("[-1]", tree!.ToString());
    }

    // ================
    // test 1
    //
    //    0
    //      1
    //        2

    // 0: push null, 2 right
    // 0: push null, 2 left
    // 0: pop(null, 2 left)
    // 0: pop(null, 2 right)
    // push new 2, null, null, 1 right      p2

    // push null, 1 left
    // pop(null, 1 left)
    // pop(2, 1 right)
    // push new 1, null, 2, 0 right         p1

    // push null, 0 left
    // pop(null, 0 left)
    // pop(1, 0 right)

    // ======
    // push new 0, null, 1, root            p0

    [Fact]
    public void Test1()
    {
        // right only
        var preorder = new[] { 0, 1, 2 };
        var inorder = new[] { 0, 1, 2 };

        var solution = new Solution();
        var tree = solution.BuildTree(preorder, inorder);
        Assert.Equal("[0,null,1,null,2]", tree!.ToString());
    }

    // ================
    // test 2
    //
    //      0
    //    1
    //  2
    
    // push null, 0 right                   
    // push null, 1 right                   

    // 0: push null, 2 right
    // 0: push null, 2 left
    // 0: pop(null, 2 left)
    // 0: pop(null, 2 right)
    // push new 2, null, null, 1 left       p2

    // pop(2, 1 left)
    // pop(null, 1 right)
    // push new 1, 2, null, 0 left          p1

    // pop(1, 0 left)
    // pop(null, 0 right)

    // ======
    // push new 0, 1, null, root            p0

    [Fact]
    public void Test2()
    {
        // left only
        var preorder = new[] { 0, 1, 2 };
        var inorder = new[] { 2, 1, 0 };

        var solution = new Solution();
        var tree = solution.BuildTree(preorder, inorder);
        Assert.Equal("[0,1,null,2]", tree!.ToString());
    }

    // ================
    // test 3
    //
    //   0
    //      1
    //    2

    // push null, 1 right

    // 0: push null, 2 right
    // 0: push null, 2 left
    // 0: pop(null, 2 left)
    // 0: pop(null, 2 right)
    // push new 2, null, null, 1 left       p2

    // pop(2, 1 left)
    // pop(null, 1 right)
    // push new 1, 2, null, 0 right         p1
    // push null, 0 left

    // pop(null, 0 left)
    // pop(1, 0 right)
    // push new 0, null, 1, root            p0


    [Fact]
    public void Test3()
    {
        var preorder = new[] { 0, 1, 2 };
        var inorder = new[] { 0, 2, 1 };

        var solution = new Solution();
        var tree = solution.BuildTree(preorder, inorder);
        Assert.Equal("[0,null,1,2]", tree!.ToString());
    }

    // ================
    // test 4
    // 
    //     0
    //   1
    //    2

    // push null, 0 right

    // 0: push null, 2 right
    // 0: push null, 2 left
    // 0: pop(null, 2 left)
    // 0: pop(null, 2 right)
    // push new 2, null, null, 1 right      p2

    // push null, 1 left

    // pop(null, 1 left)
    // pop(2, 1 right)
    // push new 1, 2, null, 0 left          p1

    // pop(null, 0 left)
    // pop(1, 0 right)
    // push new 0, null, 1, root            p0

    [Fact]
    public void Test4()
    {
        var preorder = new[] { 0, 1, 2 };
        var inorder = new[] { 1, 2, 0 };

        var solution = new Solution();
        var tree = solution.BuildTree(preorder, inorder);
        Assert.Equal("[0,1,null,null,2]", tree!.ToString());
    }

    // ================
    // test 5
    //
    //     0
    //   1   2

    // 0: push null, 2 right
    // 0: push null, 2 left
    // 0: pop(null, 2 left)
    // 0: pop(null, 2 right)
    // push new 2, null, null, 0 right      p2

    // 0: push null, 1 right
    // 0: push null, 1 left
    // 0: pop(null, 1 left)
    // 0: pop(null, 1 right)
    // push new 1, 2, null, 0 left          p1

    // pop(1, 0 left)
    // pop(2, 0 right)
    // push new 0, 1, 2, root               p0

    [Fact]
    public void Test5()
    {
        var preorder = new[] { 0, 1, 2 };
        var inorder = new[] { 1, 0, 2 };

        var solution = new Solution();
        var tree = solution.BuildTree(preorder, inorder);
        Assert.Equal("[0,1,2]", tree!.ToString());
    }

    // ================
    // test 7

    [Fact]
    public void Test7()
    {
        // right only
        var preorder = new[] { 0, 1, 2, 3 };
        var inorder = new[] { 0, 1, 2, 3 };

        var solution = new Solution();
        var tree = solution.BuildTree(preorder, inorder);
        Assert.Equal("[0,null,1,null,2,null,3]", tree!.ToString());
    }

    [Fact]
    public void Test8()
    {
        // left only
        var preorder = new[] { 0, 1, 2, 3 };
        var inorder = new[] { 3, 2, 1, 0 };

        var solution = new Solution();
        var tree = solution.BuildTree(preorder, inorder);
        Assert.Equal("[0,1,null,2,null,3]", tree!.ToString());
    }
}