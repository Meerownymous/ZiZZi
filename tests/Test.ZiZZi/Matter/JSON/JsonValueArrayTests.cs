using System;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ZiZZi.Matter.JSON.Test
{
    public sealed class JsonValueArrayTests
    {
        [Fact]
        public void CreatesArray()
        {
            var root = new JObject();
            var media = new JsonValueArray(new BytesAsToken(), root, "Todos");

            Assert.Equal(
                "[]",
                media.Content().ToString()
            );
        }

        [Fact]
        public void PutsTextIntoArray()
        {
            var root = new JObject();
            var media = new JsonValueArray(new BytesAsToken(), root, "Todos");

            media.Put("Todo", () => "Nothing");

            Assert.Equal(
                """{"Todos":["Nothing"]}""",
                root.ToString(Newtonsoft.Json.Formatting.None)
            );
        }

        [Fact]
        public void PutsByteBasedContentIntoArray()
        {
            var root = new JObject();
            new JsonValueArray(new BytesAsToken(), root, "Todos")
                .Put("Todo", "double", () => BitConverter.GetBytes(12.12));

            Assert.Equal(
                """{"Todos":[12.12]}""",
                root.ToString(Newtonsoft.Json.Formatting.None)
            );
        }
    }
}

