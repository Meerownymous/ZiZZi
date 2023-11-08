using ZiZZi.Matter.JSON;
using ZiZZi.Matter.Validated;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ZiZZi.Matter.Test
{
    public sealed class LoggingTests
    {
        [Fact]
        public void Logs()
        {
            var result = string.Empty;
            var matter =
                new Logging<JContainer>(
                    new JsonMatter(),
                    new BytesAsToken(),
                    message => result += message + "\r\n"
                );
                var block = matter.Open("block", "Street");
                block.Put("Name", "Oosterhaven");
                block.Open("block", "Location")
                    .Put("Number", "2");


        }
    }
}

