using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Tonga.Enumerable;
using Tonga.Scalar;
using Xunit;
namespace ZiZZi.Matter.JSON.Test
{
    public sealed class JsonBlockTests
    {
        [Fact]
        public void CreatesBlock()
        {
            var root = new JObject();
            var media =
                new JsonBlock(
                    new MatterOrigin(new BytesAsToken()),
                    new BytesAsToken(),
                    root,
                    "User"
                );

            media.Content();

            Assert.Equal(
                """{"User":{}}""",
                root.ToString(Newtonsoft.Json.Formatting.None)
            );
        }

        [Fact]
        public void PutsPropInBlock()
        {
            var root = new JObject();
            var media =
                new JsonBlock(
                    new MatterOrigin(new BytesAsToken()),
                    new BytesAsToken(),
                    root,
                    "User"
                );

            media.Present("Name", () => TakeContent._("Bob"));

            Assert.Equal(
                """{"User":{"Name":"Bob"}}""",
                root.ToString(Newtonsoft.Json.Formatting.None)
            );
        }

        [Fact]
        public void OpensSubBlock()
        {
            var root = new JObject();
            var media =
                new JsonBlock(
                    new MatterOrigin(new BytesAsToken()),
                    new BytesAsToken(),
                    root,
                    "User"
                );

            media.Open("block", "Skills");

            Assert.Equal(
                """{"User":{"Skills":{}}}""",
                root.ToString(Newtonsoft.Json.Formatting.None)
            );
        }
    }
}

