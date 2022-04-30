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
using Xunit;

namespace HeapChecks;

public class InMemoryFileSystemTests
{
    public class FileSystem
    {
        public class File
        {
            public string Name { get; }
            public string Data { get; private set; }

            public File(string name, string data)
            {
                Name = name;
                Data = data;
            }

            public void Append(string data)
            {
                Data += data;
            }
        }

        public class Dir
        {
            public string Name { get; }
            public Dictionary<string, Dir> SubDirs { get; } = new();
            public Dictionary<string, File> Files { get; } = new();

            public Dir(string name)
            {
                Name = name;
            }
        }

        private readonly Dir _root = new("/");

        private (Dir?, File?, string?) FindDir(string path)
        {
            if (path == "/") return (_root, null, null);

            var dir = _root;
            var p = path.Split("/");
            for (var i = 1; i < p.Length - 1; i++)
                dir = dir.SubDirs[p[i]];

            if (dir.SubDirs.TryGetValue(p[^1], out var subDir))
                return (subDir, null, null);

            if (dir.Files.TryGetValue(p[^1], out var file))
                return (null, file, null);

            return (dir, null, p[^1]);
        }

        public IList<string> Ls(string path)
        {
            var (dir, file, _) = FindDir(path);

            return file != null
                ? new List<string> { file.Name }
                : dir!.SubDirs.Keys.Union(dir.Files.Keys).OrderBy(k => k).ToList();
        }

        public void Mkdir(string path)
        {
            if (path == "/") return;

            var dir = _root;
            var parts = path.Split("/");
            for (var i = 1; i < parts.Length; i++)
            {
                if (!dir.SubDirs.TryGetValue(parts[i], out var subDir))
                {
                    subDir = new Dir(parts[i]);
                    dir.SubDirs.Add(parts[i], subDir);
                }

                dir = subDir;
            }
        }

        public void AddContentToFile(string filePath, string content)
        {
            var (dir, file, name) = FindDir(filePath);
            if (file != null)
            {
                file.Append(content);
                return;
            }

            file = new File(name!, content);
            dir!.Files.Add(name!, file);
        }

        public string ReadContentFromFile(string filePath)
        {
            var (_, file, _) = FindDir(filePath);
            return file!.Data;
        }
    }

    [Fact]
    public void Answer1()
    {
        var input = new[]
        {
            "FileSystem", "mkdir", "ls", "ls", "mkdir", "ls", "ls", "addContentToFile", "ls", "ls", "ls"
        };
        var data = new[]
        {
            Array.Empty<string>(), new[] { "/goowmfn" }, new[] { "/goowmfn" }, new[] { "/" }, new[] { "/z" },
            new[] { "/" }, new[] { "/" }, new[] { "/goowmfn/c", "shetopcy" }, new[] { "/z" }, new[] { "/goowmfn/c" },
            new[] { "/goowmfn" }
        };

        var output = new[]
        {
            null, null, Array.Empty<string>(), new[] { "goowmfn" }, null, new[] { "goowmfn", "z" },
            new[] { "goowmfn", "z" }, null, Array.Empty<string>(), new[] { "c" }, new[] { "c" }
        };

        RunTests(input, data, output);
    }

    [Fact]
    public void Example1()
    {
        /*
        Input
        ["FileSystem", "ls", "mkdir", "addContentToFile", "ls", "readContentFromFile"]
        [[], ["/"], ["/a/b/c"], ["/a/b/c/d", "hello"], ["/"], ["/a/b/c/d"]]
        Output
        [null, [], null, null, ["a"], "hello"]

        Explanation
        FileSystem fileSystem = new FileSystem();
        fileSystem.ls("/");                         // return []
        fileSystem.mkdir("/a/b/c");
        fileSystem.addContentToFile("/a/b/c/d", "hello");
        fileSystem.ls("/");                         // return ["a"]
        fileSystem.readContentFromFile("/a/b/c/d"); // return "hello"
         */

        var input = new[] { "FileSystem", "ls", "mkdir", "addContentToFile", "ls", "readContentFromFile" };
        var data = new[]
        {
            Array.Empty<string>(), new[] { "/" }, new[] { "/a/b/c" }, new[] { "/a/b/c/d", "hello" }, new[] { "/" },
            new[] { "/a/b/c/d" }
        };

        var output = new[] { null, Array.Empty<string>(), null, null, new[] { "a" }, new[] { "hello" } };

        RunTests(input, data, output);
    }

    private static void RunTests(string[] input, string[][] data, string[]?[] output)
    {
        FileSystem? fileSystem = null;
        for (var i = 0; i < input.Length; i++)
        {
            switch (input[i])
            {
                case "FileSystem":
                    fileSystem = new FileSystem();
                    break;
                case "ls":
                    Assert.Equal(output[i], fileSystem!.Ls(data[i][0]));
                    break;
                case "mkdir":
                    fileSystem!.Mkdir(data[i][0]);
                    break;
                case "addContentToFile":
                    fileSystem!.AddContentToFile(data[i][0], data[i][1]);
                    break;
                case "readContentFromFile":
                    Assert.Equal(output[i]![0], fileSystem!.ReadContentFromFile(data[i][0]));
                    break;
            }
        }
    }
}