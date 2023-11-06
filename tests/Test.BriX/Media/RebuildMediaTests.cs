﻿

using System;
using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.Text;
using Yaapii.Xml;

namespace BLox.Shape.Test
{
    public sealed class RebuildMediaTests
    {
        [Fact]
        public void CreatesPropertyInBlock()
        {
            var media = new RebuildMedia();

            media.Block("root")
                .Prop("key");

            Assert.Equal(
                "1",
                new XMLCursor(
                    media.Content()
                ).Values("count(/root/key)")[0]
            );
        }

        [Fact]
        public void RejectsPuttingPropertyToArray()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new RebuildMedia()
                    .Array("root", "item")
                    .Prop("key")
                    .Put("lock")
            );
        }

        [Fact]
        public void RejectsPuttingPropertyToRoot()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new RebuildMedia()
                    .Prop("key")
                    .Put("lock")
            );
        }

        [Fact]
        public void CreatesBlockInRoot()
        {
            var media = new RebuildMedia();
            media.Block("root")
                .Prop("key")
                .Put("value");

            Assert.Contains(
                "value",
                new XMLCursor(
                    media.Content()
                ).Values("/root/key/text()")
            );
        }

        [Fact]
        public void RejectsSecondBlockInRoot()
        {
            var media = new RebuildMedia();
            media.Block("root");

            Assert.Throws<InvalidOperationException>(() =>
                media.Block("root2")
            );
        }

        [Fact]
        public void CreatesBlockInProp()
        {
            var media = new RebuildMedia();
            media.Block("root")
                .Prop("my-block")
                .Block("contents");

            Assert.Contains(
                "1",
                new XMLCursor(
                    media.Content()
                ).Values("count(/root/my-block/contents)")
            );
        }

        [Fact]
        public void CreatesBlockInArray()
        {
            var media = new RebuildMedia();
            media.Array("array", "item")
                .Block("item")
                .Prop("prop")
                .Put("eller");

            Assert.Equal(
                "<array bx-type=\"array\" bx-array-item-name=\"item\"><item bx-type=\"block\"><prop bx-type=\"prop\">eller</prop></item></array>",
                media.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void RejectsBlockInArrayWithDifferentName()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new RebuildMedia()
                    .Array("array", "item")
                    .Block("other-name")
            );
        }

        [Fact]
        public void BuildsBlockInBlock()
        {
            var media = new RebuildMedia();
            media.Block("root")
                .Block("contents");
            Assert.Equal(
                "<root bx-type=\"block\"><contents bx-type=\"block\" /></root>",
                media.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void CreatesArrayAtRoot()
        {
            var media = new RebuildMedia();
            media.Array("root", "key");

            Assert.Equal(
                "<root bx-type=\"array\" bx-array-item-name=\"key\" />",
                media.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void CreatesArrayInBlock()
        {
            var media = new RebuildMedia();

            media
                .Block("root")
                .Array("keys", "key");

            Assert.Equal(
                "<root bx-type=\"block\"><keys bx-type=\"array\" bx-array-item-name=\"key\" /></root>",
                media.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void CreatesArrayInArray()
        {
            var media = new RebuildMedia();

            media
                .Array("keys", "key")
                .Array("subarray", "subkey");

            Assert.Equal(
                "<keys bx-type=\"array\" bx-array-item-name=\"key\"><subarray bx-type=\"array\" bx-array-item-name=\"subkey\" /></keys>",
                media.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void RejectsArrayInProp()
        {
            var media = new RebuildMedia();

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
            var media = new RebuildMedia();

            media.Block("root")
                .Prop("key")
                .Put("lock");

            Assert.Equal(
                "lock",
                new XMLCursor(
                    media.Content()
                ).Values("/root/key/text()")[0]
            );
        }

        [Fact]
        public void PutsValueToArray()
        {
            var media = new RebuildMedia();

            media
                .Array("items", "item")
                .Put("ei");

            Assert.Contains(
                "ei",
                new XMLCursor(
                    media.Content()
                ).Values("/items/item/text()")[0]
            );
        }

        [Fact]
        public void RejectsValueInBlock()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new RebuildMedia()
                    .Block("root")
                    .Put("lock")
            );
        }

        [Fact]
        public void RejectsDuplicateKeyForProp()
        {
            var media = new RebuildMedia();

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
            var media = new RebuildMedia();

            media.Block("key");

            Assert.Throws<InvalidOperationException>(() =>
                media.Block("key")
            );
        }

        [Fact]
        public void RejectsDuplicateKeyForArray()
        {
            var media = new RebuildMedia();

            media.Array("array", "item");

            Assert.Throws<InvalidOperationException>(() =>
                media.Array("array", "item")
            );
        }

        [Fact]
        public void RejectsEmptyBlockAsRoot()
        {
            var media = new RebuildMedia();
            Assert.Contains("You are trying to make a block without a name",
                Assert.Throws<InvalidOperationException>(() =>
                    media.Block("")
                ).Message
            );
        }
    }
}
