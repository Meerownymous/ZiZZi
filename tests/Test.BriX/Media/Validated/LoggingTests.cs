using BLox.Shape;
using BLox.Shape.JSON;
using BLox.Shape.Validated;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Test.BriX.Media.Validated
{
    public sealed class LoggingTests
    {
        [Fact]
        public void Logs()
        {
            var result = string.Empty;
            var media =
                new Logging<JContainer>(
                    new JsonMedia2(),
                    new BytePipe(),
                    message => result += message + "\r\n"
                );
                var block = media.Open("block", "Street");
                block.Put("Name", "Oosterhaven");
                block.Open("block", "Location")
                    .Put("Number", "3");


        }
    }
}

