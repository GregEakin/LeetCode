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

public class SerializeDeserializeBinaryTreeTests
{
    public class Codec
    {
        // Encodes a tree to a single string.
        public string serialize(TreeNode? root)
        {
            if (root == null) return "[]";

            var queue = new Queue<TreeNode?>();
            queue.Enqueue(root.left);
            queue.Enqueue(root.right);

            var buffer = new StringBuilder();
            buffer.Append('[');
            buffer.Append(root.val);
            while (queue.Count > 0)
            {
                buffer.Append(',');
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

            buffer.Append(']');
            return buffer.ToString();
        }

        // Decodes your encoded data to tree.
        public TreeNode? deserialize(string data)
        {
            if (string.IsNullOrEmpty(data) || data == "[]") return null;
            data = data.Substring(1, data.Length - 2);
            var nodes = data.Split(',');
            if (nodes.Length == 1 && nodes[0] == "null") return null;

            var root = new TreeNode(int.Parse(nodes[0]));
            var index = 0;
            var queue = new Queue<TreeNode?>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node == null) continue;
                if (index + 1 < nodes.Length)
                {
                    var left = nodes[index + 1];
                    if (left != "null")
                        node.left = new TreeNode(int.Parse(left));
                }

                if (index + 2 < nodes.Length)
                {
                    var right = nodes[index + 2];
                    if (right != "null")
                        node.right = new TreeNode(int.Parse(right));
                }

                index += 2;
                if (node.left == null && node.right == null) continue;
                queue.Enqueue(node.left);
                queue.Enqueue(node.right);
            }

            return root;
        }
    }

    [Fact]
    public void Answer1()
    {
        var root = TreeNode.Builder(new[] { 1, 2, 3, -1, -1, 4, 5, 6, 7 });
        var ser = new Codec();
        var deser = new Codec();
        var serialize = ser.serialize(root!);
        Assert.Equal("[1,2,3,null,null,4,5,6,7]", serialize);
        var ans = deser.deserialize(serialize);
        Assert.Equal(root?.ToString() ?? "NULL", ans?.ToString() ?? "NULL");
    }

    [Fact]
    public void Example1()
    {
        var root = TreeNode.Builder(new[] { 1, 2, 3, -1, -1, 4, 5 });
        var ser = new Codec();
        var deser = new Codec();
        var serialize = ser.serialize(root!);
        Assert.Equal("[1,2,3,null,null,4,5]", serialize);
        var ans = deser.deserialize(serialize);
        Assert.Equal(root?.ToString() ?? "NULL", ans?.ToString() ?? "NULL");
    }

    [Fact]
    public void Example2()
    {
        var root = TreeNode.Builder(Array.Empty<int>());
        var ser = new Codec();
        var deser = new Codec();
        var serialize = ser.serialize(root!);
        Assert.Equal("[]", serialize);
        var ans = deser.deserialize(serialize);
        Assert.Equal(root?.ToString() ?? "NULL", ans?.ToString() ?? "NULL");
    }

    [Fact]
    public void Test1()
    {
        var root = TreeNode.Builder(new[] { 5, 1, 2, 3, -1, 6, 4 })!;
        var ser = new Codec();
        var deser = new Codec();
        var serialize = ser.serialize(root!);
        Assert.Equal("[5,1,2,3,null,6,4]", serialize);
        var ans = deser.deserialize(serialize);
        Assert.Equal(root?.ToString() ?? "NULL", ans?.ToString() ?? "NULL");
    }
}