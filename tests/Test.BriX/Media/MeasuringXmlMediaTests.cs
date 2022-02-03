//MIT License

//Copyright (c) 2020 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Xml.Linq;
using Xunit;
using BriX.Media;
using Yaapii.Xml;
using Yaapii.Atoms.Enumerable;

namespace BriX.Media.Test
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
                    (media.Content() as XDocument).Root.Attribute("building-time").Value
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
                    (content as XDocument).Root.Element("block").Attribute("building-time").Value
                ) <
                Convert.ToInt32(
                    (content as XDocument).Root.Attribute("building-time").Value
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
                    (media.Content() as XDocument).Root.Attribute("building-time").Value
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
                    (content as XDocument).Root.Element("sub").Attribute("building-time").Value
                ) <
                Convert.ToInt32(
                    (content as XDocument).Root.Attribute("building-time").Value
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
                    (content as XDocument).Root.Element("sub-2").Attribute("building-time").Value
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
                "<root building-time=\"0\" />",
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
