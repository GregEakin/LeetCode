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
using System.Text;
using Xunit;

namespace HeapChecks;

public class TreeNode
{
    public int val;
    public TreeNode? left;
    public TreeNode? right;

    public TreeNode(int val = 0, TreeNode? left = null, TreeNode? right = null)
    {
        this.val = val;
        this.left = left;
        this.right = right;
    }

    public static TreeNode? Builder(int[] nodes, int index = 0)
    {
        return index >= nodes.Length
            ? null
            : nodes[index] < 0
                ? null
                : new TreeNode(nodes[index], Builder(nodes, 2 * index + 1), Builder(nodes, 2 * index + 2));
    }

    public override string ToString()
    {
        var queue = new Queue<TreeNode?>();
        queue.Enqueue(left);
        queue.Enqueue(right);

        var buffer = new StringBuilder();
        buffer.Append("[");
        buffer.Append(val);
        while (queue.Count > 0)
        {
            buffer.Append(",");
            var item = queue.Dequeue();
            if (item == null)
            {
                buffer.Append("null");
                continue;
            }

            buffer.Append(item.val);
            queue.Enqueue(item.left);
            queue.Enqueue(item.right);
        }

        while (buffer.Length > 6)
        {
            if (buffer.ToString()[^5..] != ",null") break;
            buffer.Remove(buffer.Length - 5, 5);
        }

        buffer.Append("]");
        return buffer.ToString();
    }
}

public class RecoverTreeFromPreorderTraversal
{
    public class Solution
    {
        public TreeNode RecoverFromPreorder(string traversal)
        {
            var stack = new Stack<TreeNode>();

            var dash = 0;
            var start = 0;
            var end = 0;
            for (var i = 0; i < traversal.Length; i++)
            {
                var c = traversal[i];
                if (c == '-')
                {
                    if (end > 0)
                    {
                        var num = traversal.Substring(start, end);
                        var val = int.Parse(num);
                        while (stack.Count > dash)
                            stack.Pop();

                        if (stack.Count == 0)
                        {
                            var node1 = new TreeNode(val);
                            stack.Push(node1);
                        }
                        else
                        {
                            var node2 = stack.Peek();
                            var newNode = new TreeNode(val);
                            if (node2.left == null) node2.left = newNode;
                            else node2.right = newNode;
                            stack.Push(newNode);
                        }

                        dash = 0;
                        end = 0;
                    }

                    dash++;
                    continue;
                }

                if (end == 0) start = i;
                end++;
            }

            var num2 = traversal.Substring(start, end);
            var val2 = int.Parse(num2);
            if (stack.Count == 0)
                return new TreeNode(val2);

            while (stack.Count > dash)
                stack.Pop();

            var node3 = stack.Peek();
            var newNode2 = new TreeNode(val2);
            if (node3.left == null) node3.left = newNode2;
            else node3.right = newNode2;
            while (stack.Count > 0)
                node3 = stack.Pop();

            return node3;
        }
    }

    [Fact]
    public void Example1()
    {
        var traversal = "1-2--3--4-5--6--7";
        var solution = new Solution();
        Assert.Equal("[1,2,5,3,4,6,7]", solution.RecoverFromPreorder(traversal).ToString());
    }

    [Fact]
    public void Example2()
    {
        var traversal = "1-2--3---4-5--6---7";
        var solution = new Solution();
        Assert.Equal("[1,2,5,3,null,6,null,4,null,7]", solution.RecoverFromPreorder(traversal).ToString());
    }

    [Fact]
    public void Example3()
    {
        var traversal = "1-401--349---90--88";
        var solution = new Solution();
        Assert.Equal("[1,401,null,349,88,90]", solution.RecoverFromPreorder(traversal).ToString());
    }

    [Fact]
    public void Test1()
    {
        var traversal = "100";
        var solution = new Solution();
        Assert.Equal("[100]", solution.RecoverFromPreorder(traversal).ToString());
    }

    [Fact]
    public void TreeTest0()
    {
        var tree = new TreeNode();
        Assert.Equal("[0]", tree.ToString());
    }

    [Fact]
    public void TreeTest1()
    {
        var t7 = new TreeNode(7);
        var t6 = new TreeNode(6);
        var t5 = new TreeNode(5, t6, t7);
        var t4 = new TreeNode(4);
        var t3 = new TreeNode(3);
        var t2 = new TreeNode(2, t3, t4);
        var t1 = new TreeNode(1, t2, t5);
        Assert.Equal("[1,2,5,3,4,6,7]", t1.ToString());
    }

    [Fact]
    public void TreeTest2()
    {
        var t7 = new TreeNode(7);
        var t6 = new TreeNode(6, t7);
        var t5 = new TreeNode(5, t6);
        var t4 = new TreeNode(4);
        var t3 = new TreeNode(3, t4);
        var t2 = new TreeNode(2, t3);
        var t1 = new TreeNode(1, t2, t5);
        Assert.Equal("[1,2,5,3,null,6,null,4,null,7]", t1.ToString());
    }

    [Fact]
    public void TreeTest3()
    {
        var t90 = new TreeNode(90);
        var t349 = new TreeNode(349, t90);
        var t88 = new TreeNode(88);
        var t401 = new TreeNode(401, t349, t88);
        var t1 = new TreeNode(1, t401);
        Assert.Equal("[1,401,null,349,88,90]", t1.ToString());
    }
}