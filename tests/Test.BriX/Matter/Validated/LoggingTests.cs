using ZiZZi.Matter;
using ZiZZi.Matter.JSON;
using ZiZZi.Matter.Validated;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ZiZZi.Media.Wrap.Test
{
    public sealed class LoggingTests
    {
        [Fact]
        public void Logs()
        {
            var result = string.Empty;
            var media =
                new Logging<JContainer>(
                    new JsonMatter(),
                    new BytesAsToken(),
                    message => result += message + "\r\n"
                );
                var block = media.Open("block", "Street");
                block.Put("Name", "Oosterhaven");
                block.Open("block", "Location")
                    .Put("Number", "2");


        }
    }
}

