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
using Xunit;

namespace HeapChecks;

public class LoggerRateLimiter
{
    public class Logger {

        private readonly Dictionary<string, int> _dict = new();
        
        public bool ShouldPrintMessage(int timestamp, string message)
        {
            if (_dict.TryAdd(message, timestamp))
                return true;

            var time = _dict[message];
            if (timestamp < time + 10) return false;

            _dict[message] = timestamp;
            return true;
        }
    }

    [Fact]
    public void Example1()
    {
        var input = new[]
        {
            "Logger", "shouldPrintMessage", "shouldPrintMessage", "shouldPrintMessage", "shouldPrintMessage",
            "shouldPrintMessage", "shouldPrintMessage"
        };

        var data = new[]
        {
            (0, ""), (1, "foo"), (2, "bar"), (3, "foo"), (8, "bar"), (10, "foo"), (11, "foo")
        };

        var output = new[] { false, true, true, false, false, false, true };

        Logger? logger = null;
        for (var i = 0; i < input.Length; i++)
        {
            switch (input[i])
            {
                case "Logger":
                    logger = new Logger();
                    break;
                
                case "shouldPrintMessage":
                    var (timestamp, message) = data[i];
                    Assert.Equal(output[i] , logger!.ShouldPrintMessage(timestamp, message));
                    break;
            }
        }
    }
}