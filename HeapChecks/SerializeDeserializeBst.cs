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

public class SerializeDeserializeBst
{
    /**
 * Definition for a binary tree node.
 * public class TreeNode {
 *     public int val;
 *     public TreeNode left;
 *     public TreeNode right;
 *     public TreeNode(int x) { val = x; }
 * }
 */
    public class Codec
    {
        // Encodes a tree to a single string.
        public string serialize(TreeNode root)
        {
            var queue = new Queue<TreeNode?>();
            var builder = new StringBuilder("[");
            if (root != null)
            {
                queue.Enqueue(root);
                builder.Append(root.val);
                builder.Append(',');
            }

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node == null) continue;
                if (node.left == null) builder.Append("null,");
                else
                {
                    builder.Append(node.left.val);
                    builder.Append(',');
                }

                if (node.right == null) builder.Append("null,");
                else
                {
                    builder.Append(node.right.val);
                    builder.Append(',');
                }

                queue.Enqueue(node.left);
                queue.Enqueue(node.right);
            }

            if (builder.Length > 1)
                builder.Remove(builder.Length - 1, 1);

            while (builder.ToString().EndsWith(",null"))
                builder.Remove(builder.Length - 5, 5);

            builder.Append(']');
            return builder.ToString();
        }

        // Decodes your encoded data to tree.
        public TreeNode deserialize(string data)
        {
            if (string.IsNullOrWhiteSpace(data) || data.Length <= 2 || data == "[null]") return null;
            var nodes = data.Substring(1, data.Length - 2).Split(',');

            var tree = new TreeNode { val = int.Parse(nodes[0]) };
            var index = 0;
            var queue = new Queue<TreeNode?>();
            queue.Enqueue(tree);
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

            return tree;
        }
    }

    // Your Codec object will be instantiated and called as such:
    // Codec ser = new Codec();
    // Codec deser = new Codec();
    // String tree = ser.serialize(root);
    // TreeNode ans = deser.deserialize(tree);
    // return ans;

    [Fact]
    public void Example1()
    {
        var root = TreeNode.Builder(new[] { 2, 1, 3 });
        var ser = new Codec();
        var deser = new Codec();
        var tree = ser.serialize(root!);
        Assert.Equal("[2,1,3]", tree);

        var ans = deser.deserialize(tree);
        Assert.Equal(root!.ToString(), ans.ToString());
    }

    [Fact]
    public void Example2()
    {
        var root = TreeNode.Builder(Array.Empty<int>());
        var ser = new Codec();
        var deser = new Codec();
        var tree = ser.serialize(root!);
        Assert.Equal("[]", tree);

        var ans = deser.deserialize(tree);
        Assert.Null(ans);
    }

    [Fact]
    public void Test1()
    {
        var root = TreeNode.Builder(new[] { 8, 3, 10, 1, 6, 14, 4, 7, 13, -1 });
        var ser = new Codec();
        var deser = new Codec();
        var tree = ser.serialize(root!);
        Assert.Equal("[8,3,10,1,6,14,4,7,13]", tree);

        var ans = deser.deserialize(tree);
        Assert.Equal(root!.ToString(), ans.ToString());
    }

    [Fact]
    public void Test2()
    {
        var ser = new Codec();
        var deser = new Codec();
        var root = deser.deserialize("[8,3,10,1,6,null,14,null,null,4,7,13]");
        var data = ser.serialize(root);
        Assert.Equal(root.ToString(), data);
    }

    [Fact]
    public void Test3()
    {
        var ser = new Codec();
        var deser = new Codec();
        var root = deser.deserialize("[1,2,3,null,null,4,5,6,7]");
        var data = ser.serialize(root);
        Assert.Equal(root.ToString(), data);
    }
}