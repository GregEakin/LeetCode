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

using Xunit;

namespace HeapChecks;

public class BreadthFirstSearchTests
{
    [Fact]
    public void Test1()
    {
        var vertexes = new[] { 'r', 's', 't', 'u', 'v', 'w', 'x', 'y' };
        (char u, char v)[] edges =
        {
            ('r', 'v'),
            ('r', 's'),
            ('s', 'w'),
            ('w', 't'),
            ('w', 'x'),
            ('t', 'x'),
            ('t', 'u'),
            ('x', 'y'),
            ('u', 'x'),
        };

        var search = new BreadthFirstSearch<char>(vertexes, edges);
        search.Search('s');
        search.PrintPath('s', 'u');
    }
}