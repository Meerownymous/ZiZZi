

using Newtonsoft.Json.Linq;
using System;
using Xunit;
using Yaapii.JSON;

namespace ZiZZi.Matter.Test
{
    public sealed class JsonMediaTests
    {
        [Fact]
        public void CreatesPropertyInBlock()
        {
            var media = new JsonMedia();

            media.Block("root")
                .Prop("key")
                .Put("lock");

            Assert.Equal(
                "lock",
                new JSONOf(media.Content()).Value("$.key")
            );
        }

        [Fact]
        public void RejectsPuttingPropertyToArray()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new JsonMedia()
                    .Array("array", "items")
                    .Prop("key")
                    .Put("lock")
            );
        }

        [Fact]
        public void RejectsPuttingPropertyToRoot()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new JsonMedia()
                    .Prop("key")
                    .Put("lock")
            );
        }

        [Fact]
        public void CreatesBlockInRoot()
        {
            var media = new JsonMedia();
            media.Block("root")
                .Prop("key")
                .Put("value");

            Assert.Contains(
                "value",
                new JSONOf(media.Content()).Values("$.key")
            );
        }

        [Fact]
        public void RejectsSecondBlockInRoot()
        {
            var media = new JsonMedia();
            media.Block("root");

            Assert.Throws<InvalidOperationException>(() =>
                media.Block("root2")
            );
        }

        [Fact]
        public void CreatesBlockInProp()
        {
            var media = new JsonMedia();
            media.Block("root")
                .Prop("my-block")
                .Block("contents");

            Assert.Equal(
                "{\"my-block\":{}}",
                media.Content().ToString(Newtonsoft.Json.Formatting.None)
            );
        }

        [Fact]
        public void BuildsBlockInBlock()
        {
            var media = new JsonMedia();
            media.Block("root")
                .Block("contents");

            Assert.Equal(
                "{\"contents\":{}}",
                media.Content().ToString(Newtonsoft.Json.Formatting.None)
            );
        }

        [Fact]
        public void CreatesArrayAtRoot()
        {
            var media = new JsonMedia();
            media.Array("wealth", "dev")
                .Put("100 coins");

            Assert.Equal(
                "100 coins",
                new JSONOf(media.Content()).Value("$.[0]")
            );
        }

        [Fact]
        public void CreatesArrayInBlock()
        {
            var media = new JsonMedia();

            media
                .Block("root")
                .Array("keys", "key");

            Assert.Equal(
                "{\"keys\":[]}",
                media.Content().ToString(Newtonsoft.Json.Formatting.None)
            );
        }

        [Fact]
        public void CreatesArrayInArray()
        {
            var media = new JsonMedia();

            media
                .Array("keys", "key")
                .Array("subarray", "subkey");

            Assert.Equal(
                "[[]]",
                media.Content().ToString(Newtonsoft.Json.Formatting.None)
            );
        }

        [Fact]
        public void RejectsBlockInArrayWithDifferentName()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new JsonMedia()
                    .Array("array", "item")
                    .Block("other-name")
            );
        }

        [Fact]
        public void RejectsArrayInProp()
        {
            var media = new JsonMedia();

            Assert.Throws<InvalidOperationException>(() =>
                media
                .Block("root")
                .Prop("erty")
                .Array("items", "item")
            );
        }

        [Fact]
        public void PutsValueToProp()
        {
            var media = new JsonMedia();

            media.Block("root")
                .Prop("key")
                .Put("lock");

            Assert.Equal(
                "{\"key\":\"lock\"}",
                media.Content().ToString(Newtonsoft.Json.Formatting.None)
            );
        }

        [Fact]
        public void PutsValueToArray()
        {
            var media = new JsonMedia();

            media
                .Array("items", "item")
                .Put("lock");

            Assert.Equal(
                "[\"lock\"]",
                media.Content().ToString(Newtonsoft.Json.Formatting.None)
            );
        }

        [Fact]
        public void RejectsValueInBlock()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new XmlMedia()
                    .Block("root")
                    .Put("lock")
            );
        }

        [Fact]
        public void RejectsDuplicateKeyForProp()
        {
            IMedia<JToken> media = new JsonMedia();

            var block = media.Block("root");
            block
                .Prop("key")
                .Put("lock");

            Assert.Throws<InvalidOperationException>(() =>
                block.Prop("key")
            );
        }

        [Fact]
        public void RejectsDuplicateKeyForBlock()
        {
            IMedia<JToken> media = new JsonMedia();

            media.Block("key");

            Assert.Throws<InvalidOperationException>(() =>
                media.Block("key")
            );
        }

        [Fact]
        public void RejectsDuplicateKeyForArray()
        {
            IMedia<JToken> media = new JsonMedia();

            media.Array("array", "item");

            Assert.Throws<InvalidOperationException>(() =>
                media.Array("array", "item")
            );
        }

        [Fact]
        public void RejectsEmptyBlockAsRoot()
        {
            IMedia<JToken> media = new JsonMedia();
            Assert.Contains("You are trying to make a block without a name",
                Assert.Throws<InvalidOperationException>(() =>
                    media.Block("")
                ).Message
            );
        }
    }
}
