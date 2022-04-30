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

public class BinaryOperatorTests
{
    public static bool Bit(byte code, int index)
    {
        return (code & (1 << index)) > 0;
    }

    [Fact]
    public void Test1()
    {
        for (var i = 0; i < 4; i++)
        {
            var x = (i & 0x02) > 0;
            var y = (i & 0x01) > 0; 

            Assert.Equal(Bit(0x00, i), false);
            Assert.Equal(Bit(0x01, i), !(x | y));
            Assert.Equal(Bit(0x02, i), !x & y);
            Assert.Equal(Bit(0x03, i), !x);
            Assert.Equal(Bit(0x04, i), x & !y);
            Assert.Equal(Bit(0x05, i), !y);
            Assert.Equal(Bit(0x06, i), x ^ y);
            Assert.Equal(Bit(0x07, i), !(x & y));
            Assert.Equal(Bit(0x08, i), x & y);
            Assert.Equal(Bit(0x09, i), x == y);
            Assert.Equal(Bit(0x0A, i), y);
            Assert.Equal(Bit(0x0B, i), !x | y);
            Assert.Equal(Bit(0x0C, i), x);
            Assert.Equal(Bit(0x0D, i), x | !y);
            Assert.Equal(Bit(0x0E, i), x | y);
            Assert.Equal(Bit(0x0F, i), true);
        }
    }
}