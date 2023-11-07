

using System;
using System.Xml.Linq;
using Xunit;
using ZiZZi.Matter;
using Yaapii.Xml;
using Yaapii.Atoms.Enumerable;

namespace ZiZZi.Matter.Test
{
    public sealed class MeasuringXmlMediaTests
    {
        [Fact]
        public void MeasuresRootBlockBuilding()
        {
            IMedia<XNode> media = new MeasuringXmlMedia();

            media.Block("root");

            System.Threading.Thread.Sleep(1000);

            Assert.True(
                Convert.ToInt32(
                    (media.Content() as XDocument).Root.Attribute("time").Value
                ) >= 500
            );
        }

        [Fact]
        public void MeasuresBlockBuildingIndependentFromRoot()
        {
            IMedia<XNode> media = new MeasuringXmlMedia();

            var root = media.Block("root");

            System.Threading.Thread.Sleep(200);

            root.Block("block");

            System.Threading.Thread.Sleep(200);

            XNode content = media.Content();

            Assert.True(
                Convert.ToInt32(
                    (content as XDocument).Root.Element("block").Attribute("time").Value
                ) <
                Convert.ToInt32(
                    (content as XDocument).Root.Attribute("time").Value
                )
            );
        }

        [Fact]
        public void MeasuresRootArrayBuilding()
        {
            IMedia<XNode> media = new MeasuringXmlMedia();

            media.Array("root", "sub");

            System.Threading.Thread.Sleep(1000);

            Assert.True(
                Convert.ToInt32(
                    (media.Content() as XDocument).Root.Attribute("time").Value
                ) >= 500
            );
        }

        [Fact]
        public void MeasuresArrayBuildingIndependentFromRoot()
        {
            IMedia<XNode> media = new MeasuringXmlMedia();

            var root = media.Array("root", "sub");

            System.Threading.Thread.Sleep(200);

            root.Block("sub");

            System.Threading.Thread.Sleep(200);

            XNode content = media.Content();

            Assert.True(
                Convert.ToInt32(
                    (content as XDocument).Root.Element("sub").Attribute("time").Value
                ) <
                Convert.ToInt32(
                    (content as XDocument).Root.Attribute("time").Value
                )
            );
        }

        [Fact]
        public void MeasuresSubBlock()
        {
            IMedia<XNode> media = new MeasuringXmlMedia();

            var root = media.Block("root");

            System.Threading.Thread.Sleep(300);

            var sub1 = root.Block("sub");
            sub1.Prop("something").Put("some-value");

            System.Threading.Thread.Sleep(300);

            var sub2 = root.Block("sub-2");

            System.Threading.Thread.Sleep(50);

            sub2.Prop("something-else").Put("some-other-value");

            XNode content = media.Content();

            Assert.True(
                Convert.ToInt32(
                    (content as XDocument).Root.Element("sub-2").Attribute("time").Value
                ) < 150
            );
        }

        [Fact]
        public void CreatesPropertyInBlock()
        {
            var media = new MeasuringXmlMedia();

            media.Block("root")
                .Prop("key");

            Assert.Equal(
                "1",
                new XMLCursor(media.Content()).Values("count(/root/key)")[0]
            );
        }

        [Fact]
        public void RejectsPuttingPropertyToArray()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new MeasuringXmlMedia()
                    .Array("root", "item")
                    .Prop("key")
                    .Put("lock")
            );
        }

        [Fact]
        public void RejectsPuttingPropertyToRoot()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new MeasuringXmlMedia()
                    .Prop("key")
                    .Put("lock")
            );
        }

        [Fact]
        public void CreatesBlockInRoot()
        {
            var media = new MeasuringXmlMedia();
            media.Block("root")
                .Prop("key")
                .Put("value");

            Assert.Contains(
                "value",
                new XMLCursor(media.Content()).Values("/root/key/text()")
            );
        }

        [Fact]
        public void RejectsSecondBlockInRoot()
        {
            var media = new MeasuringXmlMedia();
            media.Block("root");

            Assert.Throws<InvalidOperationException>(() =>
                media.Block("root2")
            );
        }

        [Fact]
        public void CreatesBlockInProp()
        {
            var media = new MeasuringXmlMedia();
            media.Block("root")
                .Prop("my-block")
                .Block("contents");

            Assert.Contains(
                "1",
                new XMLCursor(media.Content())
                    .Values("count(/root/my-block/contents)")
            );
        }

        [Fact]
        public void CreatesBlockInArray()
        {
            var media = new MeasuringXmlMedia();
            media.Array("array", "item")
                .Block("item")
                .Prop("prop")
                .Put("eller");

            Assert.Equal(
                "eller",
                media.Content().Document.Root.Element("item").Element("prop").Value
            );
        }

        [Fact]
        public void RejectsBlockInArrayWithDifferentName()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new MeasuringXmlMedia()
                    .Array("array", "item")
                    .Block("other-name")
            );
        }

        [Fact]
        public void BuildsBlockInBlock()
        {
            var media = new MeasuringXmlMedia();
            media.Block("root")
                .Block("contents");
            Assert.Equal(
                1,
                new LengthOf(
                    media.Content().Document.Root.Elements("contents")
                ).Value()
            );
        }

        [Fact]
        public void CreatesArrayAtRoot()
        {
            var media = new MeasuringXmlMedia();
            media.Array("root", "key");

            Assert.Equal(
                "<root time=\"0\" />",
                media.Content().ToString()
            );
        }

        [Fact]
        public void CreatesArrayInBlock()
        {
            var media = new MeasuringXmlMedia();

            media
                .Block("root")
                .Array("keys", "key");

            Assert.Equal(
                1,
                new LengthOf(
                    media.Content().Document.Root.Elements("keys")
                ).Value()
            );
        }

        [Fact]
        public void CreatesArrayInArray()
        {
            var media = new MeasuringXmlMedia();

            media
                .Array("keys", "key")
                .Array("subarray", "subkey");

            Assert.Equal(
                1,
                new LengthOf(
                    media.Content().Document.Root.Elements("subarray")
                ).Value()
            );
        }

        [Fact]
        public void RejectsArrayInProp()
        {
            var media = new MeasuringXmlMedia();

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
            var media = new MeasuringXmlMedia();

            media.Block("root")
                .Prop("key")
                .Put("lock");

            Assert.Equal(
                "lock",
                new XMLCursor(media.Content()).Values("/root/key/text()")[0]
            );
        }

        [Fact]
        public void PutsValueToArray()
        {
            var media = new MeasuringXmlMedia();

            media
                .Array("items", "item")
                .Put("ei");

            Assert.Contains(
                "ei",
                new XMLCursor(media.Content()).Values("/items/item/text()")[0]
            );
        }

        [Fact]
        public void RejectsValueInBlock()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new MeasuringXmlMedia()
                    .Block("root")
                    .Put("lock")
            );
        }

        [Fact]
        public void RejectsDuplicateKeyForProp()
        {
            IMedia<XNode> media = new MeasuringXmlMedia();

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
            IMedia<XNode> media = new MeasuringXmlMedia();

            media.Block("key");

            Assert.Throws<InvalidOperationException>(() =>
                media.Block("key")
            );
        }

        [Fact]
        public void RejectsDuplicateKeyForArray()
        {
            IMedia<XNode> media = new MeasuringXmlMedia();

            media.Array("array", "item");

            Assert.Throws<InvalidOperationException>(() =>
                media.Array("array", "item")
            );
        }
    }
}
