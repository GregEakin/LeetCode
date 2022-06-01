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
using System.Text;

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

    public static TreeNode? Builder(int[] nodes)
    {
        if (nodes.Length == 0) return null;
        if (nodes.Length == 1 && nodes[0] == -1) return null;

        var root = new TreeNode(nodes[0]);
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
                if (left != -1)
                    node.left = new TreeNode(left);
            }

            if (index + 2 < nodes.Length)
            {
                var right = nodes[index + 2];
                if (right != -1)
                    node.right = new TreeNode(right);
            }

            index += 2;
            if (node.left == null && node.right == null) continue;
            queue.Enqueue(node.left);
            queue.Enqueue(node.right);
        }

        return root;
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