using System;
using System.IO;
using System.Xml.Linq;
using Tonga.Bytes;
using Xunit;
using ZiZZi.Matter.XML;

namespace ZiZZi.Matter.Test
{
    public sealed class ListGuardTests
    {
        [Fact]
        public void AllowsOpeningSubMatterInBlockList()
        {
            Assert.Equal(
                "<Fact />",
                new ListGuard<XNode>(
                    new XmlMatter(),
                    "Unittests",
                    isValueArray: false
                ).Open("block", "Fact")
                .Content()
                .ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AllowsPuttingStringPropInValueList()
        {
            var matter = 
                new ListGuard<XNode>(
                    new XmlMatter(),
                    "Unittests",
                    isValueArray: false
                );

            matter.Open("block", "Box").Put("Character", () => "Jack");

            Assert.Equal(
                "<Box><Character>Jack</Character></Box>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AllowsPuttingBytePropInValueList()
        {
            var matter =
                new ListGuard<XNode>(
                    new XmlMatter(),
                    "Unittests",
                    isValueArray: false
                );

            matter.Open("block", "Box").Put("JackCount", "integer", () => AsBytes._(1337).Bytes());

            Assert.Equal(
                "<Box><JackCount>1337</JackCount></Box>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AllowsPuttingByteStreamInBlockList()
        {
            var matter =
                new ListGuard<XNode>(
                    new XmlMatter(),
                    "Unittests",
                    isValueArray: false
                );

            matter.Open("block", "Box")
                .Put("JackCount", "integer",
                    () => new MemoryStream(AsBytes._(1337).Bytes())
                );

            Assert.Equal(
                "<Box><JackCount>1337</JackCount></Box>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void RejectsOpeningSubMatterInValueList()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new ListGuard<XNode>(
                    new XmlMatter(),
                    "Unittests",
                    true
                ).Open("block", "some-name")
            );
        }

        [Fact]
        public void RejectsPuttingStringPropInBlockList()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new ListGuard<XNode>(
                    new XmlMatter(),
                    "Unittests",
                    true
                ).Put("some-thing", () => "yada")
            );
        }

        [Fact]
        public void RejectsPuttingBytePropInBlockList()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new ListGuard<XNode>(
                    new XmlMatter(),
                    "Unittests",
                    true
                ).Put("some-thing", "integer", () => new byte[0])
            );
        }

        [Fact]
        public void RejectsPuttingByteStreamInBlockList()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new ListGuard<XNode>(
                    new XmlMatter(),
                    "Unittests",
                    true
                ).Put("some-thing", "integer", () => new MemoryStream(new byte[0]))
            );
        }
    }
}

