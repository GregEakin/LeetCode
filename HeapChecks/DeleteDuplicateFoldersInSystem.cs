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
using System.Text;
using Xunit;

namespace HeapChecks;

public class DeleteDuplicateFoldersInSystem
{
    public class Trie
    {
        public string Name { get; }
        public Dictionary<string, Trie> Children { get; } = new();
        public string Code { get; private set; } = string.Empty;

        public Trie(string name)
        {
            Name = name;
        }

        public Trie AddChild(string folder)
        {
            if (Children.TryGetValue(folder, out var dir))
                return dir;

            var newChild = new Trie(folder);
            Children.Add(folder, newChild);
            return newChild;
        }

        public string GetCode(Dictionary<string, int> keyMap)
        {
            if (Children.Count == 0) return string.Empty;
            if (!string.IsNullOrEmpty(Code)) return Code;
            var builder = new StringBuilder();
            foreach (var (key, child) in Children.OrderBy(pair => pair.Key))
            {
                var childKey = child.GetCode(keyMap);
                builder.Append(key).Append('/').Append(childKey);
            }

            Code = builder.Append('&').ToString();
            if (!keyMap.TryAdd(Code, 1))
                keyMap[Code]++;
            return Code;
        }
    }

    public class Solution
    {
        public IList<IList<string>> DeleteDuplicateFolder(IList<IList<string>> paths)
        {
            var root = new Trie(string.Empty);
            foreach (var path in paths)
            {
                var _ = path.Aggregate(root, (current, folder) => current.AddChild(folder));
            }

            var keyMap = new Dictionary<string, int>();
            root.GetCode(keyMap);

            var ans = new List<IList<string>>();
            var stack = new Stack<(Trie, IList<string>)>();
            stack.Push((root, new List<string>()));
            while (stack.Count > 0)
            {
                var (node, dir) = stack.Pop();
                foreach (var child in node.Children.Values.Where(c => c.Children.Count == 0 || keyMap[c.Code] == 1))
                    stack.Push((child, new List<string>(dir) { child.Name }));

                if (dir.Count > 0)
                    ans.Add(dir);
            }

            return ans;
        }
    }

    [Fact]
    public void Test1()
    {
        var paths = new List<IList<string>>
        {
            new List<string> { "a" }, new List<string> { "a", "x" }, new List<string> { "a", "x", "y" },
            new List<string> { "a", "z" },
            new List<string> { "b" }, new List<string> { "b", "x" }, new List<string> { "b", "x", "y" },
            new List<string> { "b", "z" }, new List<string> { "b", "z", "trick" },
        };
        var output = new List<IList<string>>
        {
            new List<string> { "a" }, new List<string> { "a", "z" },
            new List<string> { "b" }, new List<string> { "b", "z" }, new List<string> { "b", "z", "trick" },
        };

        var solution = new Solution();
        Assert.Equal(output, solution.DeleteDuplicateFolder(paths).OrderBy(t => t.First()));
    }

    [Fact]
    public void Test2()
    {
        var paths = new List<IList<string>>
        {
            new List<string> { "y" },
            new List<string> { "y", "a" },
            new List<string> { "y", "a", "b" },
            new List<string> { "y", "a", "c" },
            new List<string> { "y", "x" },
            new List<string> { "y", "x", "a" },
            new List<string> { "y", "x", "a", "b" },
            new List<string> { "y", "x", "a", "c" },
            new List<string> { "y", "x", "z" },
            new List<string> { "y", "x", "z", "a" },
            new List<string> { "y", "x", "z", "a", "b" },
            new List<string> { "y", "x", "z", "a", "c" },
        };
        var output = new List<IList<string>>
        {
            new List<string> { "y" },
            new List<string> { "y", "x" },
            new List<string> { "y", "x", "z" },
        };

        var solution = new Solution();
        Assert.Equal(output, solution.DeleteDuplicateFolder(paths).OrderBy(t => t.First()));
    }

    [Fact]
    public void Description1()
    {
        var paths = new List<IList<string>>
        {
            new List<string> { "a" }, new List<string> { "a", "x" }, new List<string> { "a", "x", "y" },
            new List<string> { "a", "z" },
            new List<string> { "b" }, new List<string> { "b", "x" }, new List<string> { "b", "x", "y" },
            new List<string> { "b", "z" },
        };
        var output = new List<IList<string>>();
        var solution = new Solution();
        Assert.Equal(output, solution.DeleteDuplicateFolder(paths));
    }

    [Fact]
    public void Description2()
    {
        var paths = new List<IList<string>>
        {
            new List<string> { "a" }, new List<string> { "a", "x" }, new List<string> { "a", "x", "y" },
            new List<string> { "a", "z" },
            new List<string> { "b" }, new List<string> { "b", "x" }, new List<string> { "b", "x", "y" },
            new List<string> { "b", "z" }, new List<string> { "b", "w" },
        };
        var output = new List<IList<string>>
        {
            new List<string> { "a" }, new List<string> { "b" }, new List<string> { "a", "z" },
            new List<string> { "b", "w" }, new List<string> { "b", "z" }
        }.OrderBy(t => t.First());
        var solution = new Solution();
        Assert.Equal(output, solution.DeleteDuplicateFolder(paths).OrderBy(t => t.First()));
    }

    [Fact]
    public void Example1()
    {
        // Input: paths = [["a"}, new List<string> {"c"}, new List<string> {"d"}, new List<string> {"a","b"}, new List<string> {"c","b"}, new List<string> {"d","a"]]
        // Output: [["d"}, new List<string> {"d","a"]]

        var paths = new List<IList<string>>
        {
            new List<string> { "a" }, new List<string> { "c" }, new List<string> { "d" },
            new List<string> { "a", "b" },
            new List<string> { "c", "b" }, new List<string> { "d", "a" }
        };
        var output = new List<IList<string>> { new List<string> { "d" }, new List<string> { "d", "a" } };
        var solution = new Solution();
        Assert.Equal(output, solution.DeleteDuplicateFolder(paths));
    }

    [Fact]
    public void Example2()
    {
        // Input: paths = [["a"}, new List<string> {"c"}, new List<string> {"a","b"}, new List<string> {"c","b"}, new List<string> {"a","b","x"}, new List<string> {"a","b","x","y"}, new List<string> {"w"}, new List<string> {"w","y"]]
        // Output: [["c"}, new List<string> {"c","b"}, new List<string> {"a"}, new List<string> {"a","b"]]

        var paths = new List<IList<string>>
        {
            new List<string> { "a" }, new List<string> { "c" }, new List<string> { "a", "b" },
            new List<string> { "c", "b" }, new List<string> { "a", "b", "x" },
            new List<string> { "a", "b", "x", "y" },
            new List<string> { "w" }, new List<string> { "w", "y" }
        };
        var output = new List<IList<string>>
        {
            new List<string> { "c" }, new List<string> { "c", "b" }, new List<string> { "a" },
            new List<string> { "a", "b" },
        };
        var solution = new Solution();
        Assert.Equal(output, solution.DeleteDuplicateFolder(paths));
    }

    [Fact]
    public void Example3()
    {
        // Input: paths = [["a","b"}, new List<string> {"c","d"}, new List<string> {"c"}, new List<string> {"a"]]
        // Output: [["c"}, new List<string> {"c","d"}, new List<string> {"a"}, new List<string> {"a","b"]]

        var paths = new List<IList<string>>
        {
            new List<string> { "a", "b" }, new List<string> { "c", "d" }, new List<string> { "c" },
            new List<string> { "a" }
        };
        var output = new List<IList<string>>
        {
            new List<string> { "c" }, new List<string> { "c", "d" }, new List<string> { "a" },
            new List<string> { "a", "b" }
        };
        var solution = new Solution();
        Assert.Equal(output, solution.DeleteDuplicateFolder(paths));
    }
}