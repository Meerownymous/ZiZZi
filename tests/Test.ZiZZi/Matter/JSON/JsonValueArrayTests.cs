using System;
using Newtonsoft.Json.Linq;
using Xunit;
using Yaapii.JSON;

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
                "Nothing",
                new JSONOf(root).Value("$.Todos[0]")
            );
        }

        [Fact]
        public void PutsByteBasedContentIntoArray()
        {
            var root = new JObject();
            new JsonValueArray(new BytesAsToken(), root, "Todos")
                .Put("Todo", "double", () => BitConverter.GetBytes(12.12));

            Assert.Equal(
                "12.12",
                new JSONOf(root).Value("$.Todos[0]")
            );
        }
    }
}

