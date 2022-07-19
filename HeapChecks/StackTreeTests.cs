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
using Xunit;
using static HeapChecks.StackTreeTests.Solution;

namespace HeapChecks;

public class StackTreeTests
{
    public class Solution
    {
        private readonly Stack<(TreeNode? node, int target, Side side)> _stack = new();

        public enum Side
        {
            Left,
            Right
        }

        public void PushNull(int target, Side side)
        {
            _stack.Push((null, target, side));
        }

        public void PushLeaf(int value, int target, Side side, string note)
        {
            var node = new TreeNode(value, null, null);
            _stack.Push((node, target, side));
        }

        public void PushNode(int value, TreeNode? left, TreeNode? right, int target, Side side, string note)
        {
            var node = new TreeNode(value, left, right);
            _stack.Push((node, target, side));
        }

        public TreeNode? PopNull(int target, Side side)
        {
            var (node, t, s) = _stack.Pop();
            Assert.Null(node);
            Assert.Equal(target, t);
            Assert.Equal(side, s);
            return null;
        }

        public TreeNode? PopNode(int value, int target, Side side)
        {
            var (node, t, s) = _stack.Pop();
            Assert.Equal(value, node!.val);
            Assert.Equal(target, t);
            Assert.Equal(side, s);
            return node;
        }

        public TreeNode? Root(int value, TreeNode? left, TreeNode? right, string note)
        {
            var node = new TreeNode(value, left, right);
            return node;
        }
    }

    // ================
    // test 1
    //
    //    0
    //      1
    //        2
    [Fact]
    public void Test1()
    {
        var preorder = new[] { 0, 1, 2 };
        var inorder = new[] { 0, 1, 2 };
        var solution = new Solution();

        // p0: 0
        {
            var (start, len) = (0, 2);
            var i = 0;

            // p2: 2
            Assert.Equal((0, -1), (start, i - 1));

            // p1: 1, push (1,2)
            Assert.Equal((1, 2), (i + 1, len));
        }

        solution.PushLeaf(preorder[2], 1, Side.Right, "p2");
        solution.PushNull(1, Side.Left);

        var l1 = solution.PopNull(1, Side.Left);
        var r1 = solution.PopNode(2, 1, Side.Right);
        solution.PushNode(preorder[1], l1, r1, 0, Side.Right, "p1");
        solution.PushNull(0, Side.Left);

        var l0 = solution.PopNull(0, Side.Left);
        var r0 = solution.PopNode(1, 0, Side.Right);
        var tree = solution.Root(preorder[0], l0, r0, "p0");

        Assert.Equal("[0,null,1,null,2]", tree!.ToString());
    }

    // ================
    // test 2
    //
    //      0
    //    1
    //  2
    [Fact]
    public void Test2()
    {
        var preorder = new[] { 0, 1, 2 };
        var inorder = new[] { 2, 1, 0 };
        var solution = new Solution();

        // p0: 0
        {
            var (start, len) = (0, 2);
            // var v = 0;
            // var p = 0;
            var i = 2;
            // var right = i[ 3, 2 ] = (i+1, len)
            // var left = i[ 0, 1] = (start, i-1)
            Assert.Equal((3, 2), (i + 1, len));
            Assert.Equal((0, 1), (start, i - 1));
        }

        solution.PushNull(0, Side.Right);
        solution.PushNull(1, Side.Right);
        solution.PushLeaf(preorder[2], 1, Side.Left, "p2");

        var l1 = solution.PopNode(2, 1, Side.Left);
        var r1 = solution.PopNull(1, Side.Right);
        solution.PushNode(preorder[1], l1, r1, 0, Side.Left, "p1");

        var l0 = solution.PopNode(1, 0, Side.Left);
        var r0 = solution.PopNull(0, Side.Right);
        var tree = solution.Root(preorder[0], l0, r0, "p0");

        Assert.Equal("[0,1,null,2]", tree!.ToString());
    }

    // ================
    // test 3
    //
    //   0
    //      1
    //    2
    [Fact]
    public void Test3()
    {
        var preorder = new[] { 0, 1, 2 };
        var inorder = new[] { 0, 2, 1 };
        var solution = new Solution();

        // p0: 0
        {
            var (start, end) = (0, 2);
            // var p = 0;
            // var v = 0;
            var i = 0;

            // var left = i[ 0, -1 ] = (start, i-1)
            Assert.Equal((0, -1), (start, i - 1));
            // var right = i[ 1, 2 ] = (i+1, len)
            Assert.Equal((1, 2), (i + 1, end));
        }

        // [0, -1]
        {
            // var (start, end) = (0, -1);
            // return null;  // 1 right
            solution.PushNull(1, Side.Right);
        }

        // [1, 2]
        {
            var (start, end) = (1, 2);
            // var v = 1;
            // var p = 1;
            var i = 2;
            Assert.Equal((3, 2), (i + 1, end));
            Assert.Equal((1, 1), (start, i - 1));
        }

        solution.PushLeaf(preorder[2], 1, Side.Left, "p2");

        var l1 = solution.PopNode(2, 1, Side.Left);
        var r1 = solution.PopNull(1, Side.Right);
        solution.PushNode(preorder[1], l1, r1, 0, Side.Right, "p1");
        solution.PushNull(0, Side.Left);

        var l0 = solution.PopNull(0, Side.Left);
        var r0 = solution.PopNode(1, 0, Side.Right);
        var tree = solution.Root(preorder[0], l0, r0, "p0");

        Assert.Equal("[0,null,1,2]", tree!.ToString());
    }

    [Fact]
    public void Test3B()
    {
        var preorder = new[] { 0, 1, 2 };
        var map = new Dictionary<int, int> { { 0, 0 }, { 1, 2 }, { 2, 1 } };
        // var inorder = new[] { 0, 2, 1 };
        var solution = new Solution();
        var stack = new Stack<(int p, int left, int right)>();
        stack.Push((0, 0, preorder.Length-1));

        {
            var (p, start, end) = stack.Pop();
            var i = map[p];
            var v = preorder[p];
            stack.Push((p, i + 1, end));
            stack.Push((p, start, i - 1));
        }
        
        {
            var (p, start, end) = stack.Pop();
            if (p >= preorder.Length || start >= end)
                solution.PushNull(p, Side.Left);

            var i = map[p];
            var v = preorder[p];
            stack.Push((p, i + 1, end));
            stack.Push((p, start, i - 1));
        }
    }

    // ================
    // test 4
    // 
    //     0
    //   1
    //    2
    [Fact]
    public void Test4()
    {
        var preorder = new[] { 0, 1, 2 };
        var inorder = new[] { 1, 2, 0 };
        var solution = new Solution();

        // p0: 0 
        {
            var (start, len) = (0, 2);
            // var v = 0;
            // var p = 0;
            var i = 2;
            // var right = i[ 3, 2 ] = (i+1, len)
            // var left = i[ 0, 1 ] = (start, i-1)
            Assert.Equal((3, 2), (i + 1, len));
            Assert.Equal((0, 1), (start, i - 1));
        }

        solution.PushNull(0, Side.Right);
        solution.PushLeaf(preorder[2], 1, Side.Right, "p2");
        solution.PushNull(1, Side.Left);

        var l1 = solution.PopNull(1, Side.Left);
        var r1 = solution.PopNode(2, 1, Side.Right);
        solution.PushNode(preorder[1], l1, r1, 0, Side.Left, "p1");

        var l0 = solution.PopNode(1, 0, Side.Left);
        var r0 = solution.PopNull(0, Side.Right);
        var tree = solution.Root(preorder[0], l0, r0, "p0");

        Assert.Equal("[0,1,null,null,2]", tree!.ToString());
    }

    // ================
    // test 5
    //
    //     0
    //   1   2
    [Fact]
    public void Test5()
    {
        var preorder = new[] { 0, 1, 2 };
        var inorder = new[] { 1, 0, 2 };
        var solution = new Solution();

        // p0: 0
        {
            var (start, len) = (0, 2);
            var i = 1;

            // p1: 1, push (start, i - 1)
            Assert.Equal((0, 0), (start, i - 1));

            // p2: 2, push (i + 1, len)
            Assert.Equal((2, 2), (i + 1, len));
        }

        // pop p2: (2, 2)
        solution.PushLeaf(preorder[2], 0, Side.Right, "p2");
        // pop p1: (0, 0)
        solution.PushLeaf(preorder[1], 0, Side.Left, "p1");

        var l0 = solution.PopNode(1, 0, Side.Left);
        var r0 = solution.PopNode(2, 0, Side.Right);
        var tree = solution.Root(preorder[0], l0, r0, "p0");

        Assert.Equal("[0,1,2]", tree!.ToString());
    }
}