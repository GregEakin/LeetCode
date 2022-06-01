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
using System.Text;
using Xunit;

namespace HeapChecks;

public class BinarySearchTree<T> where T : IComparable<T>
{
    public class Node
    {
        public T Key { get; }
        public Node? Parent { get; set; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }

        public Node(Node? parent, T key)
        {
            Key = key;
            Parent = parent;
        }

        public override int GetHashCode() => Key.GetHashCode();

        public override bool Equals(object? obj) => obj is Node p && Key.CompareTo(p.Key) == 0;
        // public override string ToString() => Key?.ToString() ?? "null";
    }

    public Node? Root { get; set; }

    public static Node? TreeSearch(Node? x, T key)
    {
        while (x != null && key.CompareTo(x.Key) != 0)
            x = key.CompareTo(x.Key) < 0 ? x.Left : x.Right;
        return x;
    }

    public static Node TreeMinimum(Node x)
    {
        while (x.Left != null)
            x = x.Left;
        return x;
    }

    public static Node TreeMaximum(Node x)
    {
        while (x.Right != null)
            x = x.Right;
        return x;
    }

    public static Node? TreeSuccessor(Node x)
    {
        if (x.Right != null)
            return TreeMinimum(x.Right);
        var y = x.Parent;
        while (y != null && ReferenceEquals(x, y.Right))
        {
            x = y;
            y = y.Parent;
        }

        return y;
    }

    public static Node? TreePredecessor(Node x)
    {
        if (x.Left != null)
            return TreeMaximum(x.Left);
        var y = x.Parent;
        while (y != null && ReferenceEquals(x, y.Left))
        {
            x = y;
            y = y.Parent;
        }

        return y;
    }

    public void TreeInsert(Node z)
    {
        var y = (Node?)null;
        var x = Root;
        while (x != null)
        {
            y = x;
            x = z.Key.CompareTo(x.Key) < 0 ? x.Left : x.Right;
        }

        z.Parent = y;
        if (y == null)
            Root = z;
        else if (z.Key.CompareTo(y.Key) < 0)
            y.Left = z;
        else
            y.Right = z;
    }

    public void SubtreeShift(Node u, Node? v)
    {
        if (u.Parent == null)
            Root = v;
        else if (ReferenceEquals(u, u.Parent.Left))
            u.Parent.Left = v;
        else
            u.Parent.Right = v;

        if (v != null)
            v.Parent = u.Parent;
    }

    public void TreeDelete(Node z)
    {
        if (z.Left == null)
            SubtreeShift(z, z.Right);
        else if (z.Right == null)
            SubtreeShift(z, z.Left);
        else
        {
            var y = TreeSuccessor(z)!;
            if (!ReferenceEquals(y.Parent, z))
            {
                SubtreeShift(y, y.Right);
                y.Right = z.Right;
                y.Right.Parent = y;
            }

            SubtreeShift(z, y);
            y.Left = z.Left;
            y.Left.Parent = y;
        }
    }

    public static IEnumerable<Node> InorderTreeWalk(Node? x)
    {
        if (x == null) yield break;
        foreach (var node in InorderTreeWalk(x.Left))
            yield return node;
        yield return x;
        foreach (var node in InorderTreeWalk(x.Right))
            yield return node;
    }

    public static IEnumerable<Node> PreorderTreeWalk(Node? x)
    {
        if (x == null) yield break;
        yield return x;
        foreach (var node in PreorderTreeWalk(x.Left))
            yield return node;
        foreach (var node in PreorderTreeWalk(x.Right))
            yield return node;
    }

    public static IEnumerable<Node> PostorderTreeWalk(Node? x)
    {
        if (x == null) yield break;
        foreach (var node in PostorderTreeWalk(x.Left))
            yield return node;
        foreach (var node in PostorderTreeWalk(x.Right))
            yield return node;
        yield return x;
    }

    public override string ToString()
    {
        var queue = new Queue<Node?>();
        var builder = new StringBuilder("[");
        if (Root != null)
        {
            queue.Enqueue(Root);
            builder.Append(Root.Key);
            builder.Append(',');
        }

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node == null) continue;
            if (node.Left == null) builder.Append("null,");
            else
            {
                builder.Append(node.Left.Key);
                builder.Append(',');
            }

            if (node.Right == null) builder.Append("null,");
            else
            {
                builder.Append(node.Right.Key);
                builder.Append(',');
            }

            queue.Enqueue(node.Left);
            queue.Enqueue(node.Right);
        }

        if (builder.Length > 1)
            builder.Remove(builder.Length - 1, 1);

        while (builder.ToString().EndsWith(",null"))
            builder.Remove(builder.Length - 5, 5);

        builder.Append(']');
        return builder.ToString();
    }

    public static BinarySearchTree<char> BuilderChar(string data)
    {
        var tree = new BinarySearchTree<char>();
        if (string.IsNullOrWhiteSpace(data) || data.Length <= 2 || data == "[null]") return tree;
        var nodes = data.Substring(1, data.Length - 2).Split(',');

        tree.Root = new BinarySearchTree<char>.Node(null, char.Parse(nodes[0]));
        var index = 0;
        var queue = new Queue<BinarySearchTree<char>.Node?>();
        queue.Enqueue(tree.Root);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node == null) continue;
            if (index + 1 < nodes.Length)
            {
                var left = nodes[index + 1];
                if (left != "null")
                    node.Left = new BinarySearchTree<char>.Node(node, char.Parse(left));
            }

            if (index + 2 < nodes.Length)
            {
                var right = nodes[index + 2];
                if (right != "null")
                    node.Right = new BinarySearchTree<char>.Node(node, char.Parse(right));
            }

            index += 2;
            if (node.Left == null && node.Right == null) continue;
            queue.Enqueue(node.Left);
            queue.Enqueue(node.Right);
        }

        return tree;
    }

    public static BinarySearchTree<int> BuilderInt(string data)
    {
        var tree = new BinarySearchTree<int>();
        if (string.IsNullOrWhiteSpace(data) || data.Length <= 2 || data == "[null]") return tree;
        var nodes = data.Substring(1, data.Length - 2).Split(',');

        tree.Root = new BinarySearchTree<int>.Node(null, int.Parse(nodes[0]));
        var index = 0;
        var queue = new Queue<BinarySearchTree<int>.Node?>();
        queue.Enqueue(tree.Root);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node == null) continue;
            if (index + 1 < nodes.Length)
            {
                var left = nodes[index + 1];
                if (left != "null")
                    node.Left = new BinarySearchTree<int>.Node(node, int.Parse(left));
            }

            if (index + 2 < nodes.Length)
            {
                var right = nodes[index + 2];
                if (right != "null")
                    node.Right = new BinarySearchTree<int>.Node(node, int.Parse(right));
            }

            index += 2;
            if (node.Left == null && node.Right == null) continue;
            queue.Enqueue(node.Left);
            queue.Enqueue(node.Right);
        }

        return tree;
    }
}

public class BinarySearchTreeTests
{
    [Fact]
    public void Test1()
    {
        var root = BinarySearchTree<int>.BuilderInt("[2,1,3]");
        Assert.Equal("[2,1,3]", root.ToString());
    }

    [Fact]
    public void Test2()
    {
        var root = BinarySearchTree<int>.BuilderInt("[8,3,10,1,6,null,14,null,null,4,7,13]");
        Assert.Equal("[8,3,10,1,6,null,14,null,null,4,7,13]", root.ToString());
    }

    [Fact]
    public void Test3()
    {
        var root = BinarySearchTree<int>.BuilderInt("[1,2,3,null,null,4,5,6,7]");
        Assert.Equal("[1,2,3,null,null,4,5,6,7]", root.ToString());
    }

    [Fact]
    public void DeleteTestA()
    {
        var tree = BinarySearchTree<char>.BuilderChar("[z,null,r]");
        Assert.Equal("[z,null,r]", tree.ToString());

        var z = BinarySearchTree<char>.TreeSearch(tree.Root, 'z');
        Assert.NotNull(z);
        tree.TreeDelete(z!);
        Assert.Equal("[r]", tree.ToString());
    }

    [Fact]
    public void DeleteTestB()
    {
        var tree = BinarySearchTree<char>.BuilderChar("[z,l]");
        Assert.Equal("[z,l]", tree.ToString());

        var z = BinarySearchTree<char>.TreeSearch(tree.Root, 'z');
        Assert.NotNull(z);
        tree.TreeDelete(z!);
        Assert.Equal("[l]", tree.ToString());
    }

    [Fact]
    public void DeleteTestC()
    {
        var tree = BinarySearchTree<char>.BuilderChar("[z,l,y,null,null,null,x]");
        Assert.Equal("[z,l,y,null,null,null,x]", tree.ToString());

        var z = BinarySearchTree<char>.TreeSearch(tree.Root, 'z');
        Assert.NotNull(z);
        tree.TreeDelete(z!);
        Assert.Equal("[y,l,x]", tree.ToString());
    }

    [Fact]
    public void DeleteTestD()
    {
        var tree = BinarySearchTree<char>.BuilderChar("[z,l,r,null,null,y,null,null,x]");
        Assert.Equal("[z,l,r,null,null,y,null,null,x]", tree.ToString());

        var z = BinarySearchTree<char>.TreeSearch(tree.Root, 'z');
        Assert.NotNull(z);
        tree.TreeDelete(z!);
        Assert.Equal("[y,l,r,null,null,x]", tree.ToString());
    }

    [Fact]
    public void DeleteTest1()
    {
        var tree = BinarySearchTree<char>.BuilderChar("[y,x,z]");

        var y = BinarySearchTree<char>.TreeSearch(tree.Root, 'y');
        Assert.NotNull(y);
        tree.TreeDelete(y!);
        Assert.Equal("[z,x]", tree.ToString());
    }

    [Fact]
    public void InsertTest1()
    {
        var tree = new BinarySearchTree<int>();
        foreach (var value in new[] { 8, 3, 10, 1, 6, 14, 4, 7, 13 })
            tree.TreeInsert(new BinarySearchTree<int>.Node(null, value));

        Assert.Equal("[8,3,10,1,6,null,14,null,null,4,7,13]", tree.ToString());
    }

    [Fact]
    public void InsertTest2()
    {
        var tree = new BinarySearchTree<int>();
        foreach (var value in new[] { 1, 3, 4, 6, 7, 8, 10, 13, 14 })
            tree.TreeInsert(new BinarySearchTree<int>.Node(null, value));

        Assert.Equal("[1,null,3,null,4,null,6,null,7,null,8,null,10,null,13,null,14]", tree.ToString());
    }

    [Fact]
    public void InsertTest3()
    {
        var tree = new BinarySearchTree<int>();
        foreach (var value in new[] { 14, 13, 10, 8, 7, 6, 4, 3, 1 })
            tree.TreeInsert(new BinarySearchTree<int>.Node(null, value));

        Assert.Equal("[14,13,null,10,null,8,null,7,null,6,null,4,null,3,null,1]", tree.ToString());
    }

    [Fact]
    public void MinTest1()
    {
        var tree = new BinarySearchTree<int>();
        foreach (var value in new[] { 8, 3, 10, 1, 6, 14, 4, 7, 13 })
            tree.TreeInsert(new BinarySearchTree<int>.Node(null, value));

        Assert.Equal(1, BinarySearchTree<int>.TreeMinimum(tree.Root!).Key);
    }

    [Fact]
    public void MaxTest1()
    {
        var tree = new BinarySearchTree<int>();
        foreach (var value in new[] { 8, 3, 10, 1, 6, 14, 4, 7, 13 })
            tree.TreeInsert(new BinarySearchTree<int>.Node(null, value));

        Assert.Equal(14, BinarySearchTree<int>.TreeMaximum(tree.Root!).Key);
    }

    [Fact]
    public void TreePredecessorTest1()
    {
        var tree = BinarySearchTree<int>.BuilderInt("[8,3,10,1,6,null,14,null,null,4,7,13]");

        var node = BinarySearchTree<int>.TreeSearch(tree.Root, 1);
        Assert.NotNull(node);
        Assert.Null(BinarySearchTree<int>.TreePredecessor(node!));
    }

    [Fact]
    public void TreePredecessorTest2()
    {
        var tree = BinarySearchTree<int>.BuilderInt("[8,3,10,1,6,null,14,null,null,4,7,13]");

        var node = BinarySearchTree<int>.TreeSearch(tree.Root, 13);
        Assert.NotNull(node);
        Assert.Equal(10, BinarySearchTree<int>.TreePredecessor(node!)!.Key);
    }

    [Fact]
    public void TreeSuccessorTest1()
    {
        var tree = BinarySearchTree<int>.BuilderInt("[8,3,10,1,6,null,14,null,null,4,7,13]");

        var node = BinarySearchTree<int>.TreeSearch(tree.Root, 13);
        Assert.NotNull(node);
        Assert.Equal(14, BinarySearchTree<int>.TreeSuccessor(node!)!.Key);
    }

    [Fact]
    public void InorderTreeWalkTest1()
    {
        var root = BinarySearchTree<int>.BuilderInt("[8,3,10,1,6,null,14,null,null,4,7,13]");
        var expected = new[] { 1, 3, 4, 6, 7, 8, 10, 13, 14 };
        var answer = BinarySearchTree<int>.InorderTreeWalk(root.Root).Select(n => n.Key).ToArray();
        Assert.Equal(expected, answer);
    }

    [Fact]
    public void PreorderTreeWalkTest1()
    {
        var root = BinarySearchTree<int>.BuilderInt("[8,3,10,1,6,null,14,null,null,4,7,13]");
        var expected = new[] { 8, 3, 1, 6, 4, 7, 10, 14, 13 };
        var answer = BinarySearchTree<int>.PreorderTreeWalk(root.Root).Select(n => n.Key).ToArray();
        Assert.Equal(expected, answer);
    }

    [Fact]
    public void PostorderTreeWalkTest1()
    {
        var root = BinarySearchTree<int>.BuilderInt("[8,3,10,1,6,null,14,null,null,4,7,13]");
        var expected = new[] { 1, 4, 7, 6, 3, 13, 14, 10, 8 };
        var answer = BinarySearchTree<int>.PostorderTreeWalk(root.Root).Select(n => n.Key).ToArray();
        Assert.Equal(expected, answer);
    }
}