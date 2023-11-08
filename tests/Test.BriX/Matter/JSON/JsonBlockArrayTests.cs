using System;
using Newtonsoft.Json.Linq;
using Xunit;
using Yaapii.JSON;

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
                    new BytesAsToken(),
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
                    new BytesAsToken(),
                    root,
                    "Worlds"
                );

            matter.Open("block", "UpsideDown");

            Assert.Equal(
                "{}",
                new JSONOf(root).Node("$.Worlds[0]").Token().ToString()
            );
        }
    }
}

