using Newtonsoft.Json.Linq;
using Xunit;
using Yaapii.JSON;

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
                1,
                new JSONOf(root).Nodes("$.User").Count
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

            media.Put("Name", () => "Bob");

            Assert.Contains(
                "Bob",
                new JSONOf(root).Values("$.User.Name")
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
                1,
                new JSONOf(root).Nodes("$.User.Skills").Count
            );
        }
    }
}

