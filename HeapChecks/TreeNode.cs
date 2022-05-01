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