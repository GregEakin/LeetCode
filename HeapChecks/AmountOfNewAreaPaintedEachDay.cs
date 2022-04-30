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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HeapChecks;

public class AmountOfNewAreaPaintedEachDay
{
    public class SolutionSlow
    {
        public int[] AmountPainted(int[][] paint)
        {
            var painted = new BitArray(100000);
            var log = new int[paint.Length];
            for (var i = 0; i < paint.Length; i++)
            {
                var order = paint[i];
                var start = order[0];
                var end = order[1];

                for (var j = start; j < end; j++)
                {
                    if (painted[j]) continue;
                    painted[j] = true;
                    log[i]++;
                }
            }

            return log;
        }
    }

    public class SolutionBinaryTree
    {
        public class BinaryTree
        {
            public class Node
            {
                public Node? Left { get; set; }
                public Node? Right { get; set; }
                public int Start { get; set; }
                public int End { get; set; }
            }

            private Node? Root { get; set; }

            public bool Add(int start, int end)
            {
                Node? before = null, after = Root;
                while (after != null)
                {
                    before = after;
                    if (start < after.Start)
                        after = after.Left;
                    else if (start > after.Start)
                        after = after.Right;
                    else
                        return false;
                }

                var next = new Node { Start = start, End = end };

                if (Root == null)
                    Root = next;
                else // if (before != null)
                {
                    if (start < before!.Start)
                        before.Left = next;
                    else
                        before.Right = next;
                }

                return true;
            }

            public ISet<Node> Find(int start, int end)
            {
                var list = new HashSet<Node>();
                Find(Root, start, end, list);
                return list;
            }

            private static void Find(Node? parent, int start, int end, ISet<Node> list)
            {
                if (parent == null) return;
                if (parent.Start <= start && parent.End > start) list.Add(parent);
                if (parent.Start >= start && parent.Start < end) list.Add(parent);
                Find(start < parent.Start ? parent.Left : parent.Right, start, end, list);
            }
        }

        public int[] AmountPainted(int[][] paint)
        {
            var painted = new BinaryTree();
            var log = new int[paint.Length];
            for (var i = 0; i < paint.Length; i++)
            {
                var order = paint[i];
                var start = order[0];
                var end = order[1];

                var length = end - start;
                var hours = new BitArray(length);
                var nodes = painted.Find(start, end);
                foreach (var node in nodes)
                {
                    for (var j = node.Start; j < node.End; j++)
                    {
                        var index = j - start;
                        if (index < 0 || index >= length) continue;
                        hours[index] = true;
                    }
                }

                foreach (bool hour in hours)
                    if (!hour)
                        log[i]++;

                painted.Add(start, end);
            }

            return log;
        }
    }

    public class Solution
    {
        public class BinaryTree
        {
            public class Node
            {
                public Node? Left { get; set; }
                public Node? Right { get; set; }
                public int Start { get; set; }
                public int End { get; set; }
            }

            private Node? Root { get; set; }

            public bool Add(int start, int end)
            {
                Node? before = null, after = Root;
                while (after != null)
                {
                    before = after;
                    if (start < after.Start)
                        after = after.Left;
                    else if (start > after.Start)
                        after = after.Right;
                    else
                        return false;
                }

                var next = new Node { Start = start, End = end };

                if (Root == null)
                    Root = next;
                else // if (before != null)
                {
                    if (start < before!.Start)
                        before.Left = next;
                    else
                        before.Right = next;
                }

                return true;
            }

            public ISet<Node> Find(int start, int end)
            {
                var list = new HashSet<Node>();
                Find(Root, start, end, list);
                return list;
            }

            private static void Find(Node? parent, int start, int end, ISet<Node> list)
            {
                if (parent == null) return;
                if (parent.Start <= start && parent.End > start) list.Add(parent);
                if (parent.Start >= start && parent.Start < end) list.Add(parent);
                Find(start < parent.Start ? parent.Left : parent.Right, start, end, list);
            }
        }

        public int[] AmountPainted(int[][] paint)
        {
            var painted = new BinaryTree();
            var log = new int[paint.Length];
            for (var i = 0; i < paint.Length; i++)
            {
                var order = paint[i];
                var start = order[0];
                var end = order[1];

                var length = end - start;
                var hours = new BitArray(length);
                var nodes = painted.Find(start, end);
                foreach (var node in nodes)
                {
                    for (var j = node.Start; j < node.End; j++)
                    {
                        var index = j - start;
                        if (index < 0 || index >= length) continue;
                        hours[index] = true;
                    }
                }

                foreach (bool hour in hours)
                    if (!hour)
                        log[i]++;

                painted.Add(start, end);
            }

            return log;
        }
    }

    [Fact]
    public void Answer1()
    {
        var paint = new[] { new[] { 1, 5 }, new[] { 2, 4 } };
        var solution = new Solution();
        Assert.Equal(new[] { 4, 0 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Answer2()
    {
        var paint = new[] { new[] { 0, 5 }, new[] { 0, 2 }, new[] { 0, 3 }, new[] { 0, 4 }, new[] { 0, 5 } };
        var solution = new Solution();
        Assert.Equal(new[] { 5, 0, 0, 0, 0 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Answer3()
    {
        var paint = new[] { new[] { 2, 5 }, new[] { 7, 10 }, new[] { 3, 9 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 3, 2 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Example1()
    {
        var paint = new[] { new[] { 1, 4 }, new[] { 4, 7 }, new[] { 5, 8 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 3, 1 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Example2()
    {
        var paint = new[] { new[] { 1, 4 }, new[] { 5, 8 }, new[] { 4, 7 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 3, 1 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Example3()
    {
        var paint = new[] { new[] { 1, 5 }, new[] { 4, 5 } };
        var solution = new Solution();
        Assert.Equal(new[] { 4, 0 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Test1()
    {
        var paint = new[] { new[] { 1, 3 }, new[] { 5, 7 }, new[] { 2, 4 } };
        var solution = new Solution();
        Assert.Equal(new[] { 2, 2, 1 }, solution.AmountPainted(paint));
    }
    
    [Fact]
    public void Test2()
    {
        var paint = new[] { new[] { 1, 4 }, new[] { 6, 7 }, new[] { 12, 14 }, new[] { 2, 13 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 1, 2, 7 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void Test3()
    {
        var paint = new[] { new[] { 1, 4 }, new[] { 6, 7 }, new[] { 12, 14 }, new[] { 5, 13 } };
        var solution = new Solution();
        Assert.Equal(new[] { 3, 1, 2, 6 }, solution.AmountPainted(paint));
    }

    [Fact]
    public void TreeTest1()
    {
        var tree = new SolutionBinaryTree.BinaryTree();
        Assert.True(tree.Add(1, 4));
        Assert.True(tree.Add(4, 7));
        var nodes = tree.Find(5, 8);
        Assert.Single(nodes);
        Assert.Equal(4, nodes.First().Start);
        Assert.Equal(7, nodes.First().End);
    }

    [Fact]
    public void TreeTest2()
    {
        var tree = new SolutionBinaryTree.BinaryTree();
        Assert.True(tree.Add(1, 4));
        Assert.True(tree.Add(5, 8));
        var nodes = tree.Find(4, 7);
        Assert.Single(nodes);
        Assert.Equal(5, nodes.First().Start);
        Assert.Equal(8, nodes.First().End);
    }

    [Fact]
    public void TreeTest3()
    {
        var tree = new SolutionBinaryTree.BinaryTree();
        Assert.True(tree.Add(1, 3));
        Assert.True(tree.Add(5, 7));
        var nodes = tree.Find(2, 4);
        Assert.Single(nodes);
        Assert.Equal(1, nodes.First().Start);
        Assert.Equal(3, nodes.First().End);
    }

    [Fact]
    public void TreeTest4()
    {
        var tree = new SolutionBinaryTree.BinaryTree();
        Assert.True(tree.Add(1, 5));
        var nodes = tree.Find(2, 4);
        Assert.Single(nodes);
        Assert.Equal(1, nodes.First().Start);
        Assert.Equal(5, nodes.First().End);
    }

    [Fact]
    public void TreeTest5()
    {
        var tree = new SolutionBinaryTree.BinaryTree();
        Assert.True(tree.Add(0, 5));
        var nodes = tree.Find(0, 2);
        Assert.Single(nodes);
        Assert.Equal(0, nodes.First().Start);
        Assert.Equal(5, nodes.First().End);
    }

    [Fact]
    public void TreeTest6()
    {
        var tree = new SolutionBinaryTree.BinaryTree();
        Assert.True(tree.Add(1, 4));
        Assert.True(tree.Add(6, 7));
        Assert.True(tree.Add(12, 14));
        var nodes = tree.Find(2, 13);
        Assert.Equal(3, nodes.Count);
        // Assert.Equal(1, nodes.First().Start);
        // Assert.Equal(4, nodes.First().End);
    }

    [Fact]
    public void TreeTest7()
    {
        var tree = new SolutionBinaryTree.BinaryTree();
        Assert.True(tree.Add(1, 4));
        Assert.True(tree.Add(6, 7));
        Assert.True(tree.Add(12, 14));
        var nodes = tree.Find(5, 13);
        Assert.Equal(2, nodes.Count);
        // Assert.Equal(1, nodes.First().Start);
        // Assert.Equal(4, nodes.First().End);
    }
}