using System;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ZiZZi.Matter.JSON.Test
{
    public sealed class JsonBlockArrayTests
    {
        [Fact]
        public void CreatesArray()
        {
            var root = new JObject();
            var matter =
                new JsonBlockArray(
                    new MatterOrigin(new BytesAsToken()),
                    root,
                    "Worlds"
                );

            Assert.Equal(
                "[]",
                matter.Content().ToString()
            );
        }

        [Fact]
        public void OpensBlockInArray()
        {
            var root = new JObject();
            var matter =
                new JsonBlockArray(
                    new MatterOrigin(new BytesAsToken()),
                    root,
                    "Worlds"
                );

            matter.Open("block", "UpsideDown");

            Assert.Equal(
                """{"Worlds":[{}]}""",
                root.ToString(Newtonsoft.Json.Formatting.None)
            );
        }
    }
}

