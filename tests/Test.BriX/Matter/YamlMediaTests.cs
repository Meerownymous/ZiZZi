

using Xunit;

namespace ZiZZi.Matter.Test
{
    public sealed class YamlMediaTests
    {
        [Fact]
        public void CreatesPropertyInBlock()
        {
            var media = new YamlMedia();

            media.Block("root")
                .Prop("key")
                .Put("lock");

            Assert.Equal(
                "root:\r\n  key: lock\r\n",
                media.Content().ToString(),
                false,
                true,
                false
            );
        }

        [Fact]
        public void CreatesBlockInRoot()
        {
            var media = new YamlMedia();
            media.Block("root")
                .Prop("key")
                .Put("value");

            Assert.Contains(
                "value",
                media.Content()
            );
        }

        [Fact]
        public void CreatesBlockInProp()
        {
            var media = new YamlMedia();
            media.Block("root")
                .Prop("my-block")
                .Block("contents");

            Assert.Equal(
                "root:\r\n  my-block:  contents:\r\n",
                media.Content(),
                false,
                true,
                false
            );
        }

        [Fact]
        public void BuildsBlockInBlock()
        {
            var media = new YamlMedia();
            media.Block("root")
                .Block("contents");

            Assert.Equal(
                "root:\r\n  contents:\r\n",
                media.Content(),
                false,
                true,
                false
            );
        }

        [Fact]
        public void CreatesArrayAtRoot()
        {
            var media = new YamlMedia();
            media
                .Array("wealth", "dev")
                .Put("100 coins")
                .Put("200 coins");

            Assert.Equal(
                "wealth:\r\n  - 100 coins\r\n  - 200 coins\r\n",
                media.Content().ToString(),
                false,
                true,
                false
            );
        }

        [Fact]
        public void CreatesArrayInBlock()
        {
            var media = new YamlMedia();

            media
                .Block("root")
                .Array("keys", "key");

            Assert.Equal(
                "root:\r\n  keys:\r\n",
                media.Content(),
                false,
                true,
                false
            );
        }

        [Fact]
        public void CreatesArrayInArray()
        {
            var media = new YamlMedia();

            media
                .Array("keys", "key")
                .Array("subarray", "subkey");

            Assert.Equal(
                "keys:\r\n  subarray:\r\n",
                media.Content(),
                false,
                true,
                false
            );
        }
    }
}
