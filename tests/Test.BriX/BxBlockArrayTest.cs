

using BLox.Shape;
using System;
using Xunit;

namespace BLox.Test
{
    public sealed class BxBlocksTests
    {
        [Fact]
        public void WritesBlockArray()
        {
            var media = new XmlMedia();

            new BxBlockArray("H-Blockx", "H-Item",
                new BxBlock("H-Item", new BxProp("move", "ya")),
                new BxBlock("H-Item", new BxProp("move-move", "ya"))
            ).Print(media);

            Assert.Equal(
                "<H-Blockx><H-Item><move>ya</move></H-Item><H-Item><move-move>ya</move-move></H-Item></H-Blockx>",
                media.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void WritesBlockArrayUnnamed()
        {
            var media = new XmlMedia();

            new BxBlockArray("H-Blockx", "H-Item",
                new BxBlock(new BxProp("move", "ya")),
                new BxBlock(new BxProp("move-move", "ya"))
            ).Print(media);

            Assert.Equal(
                "<H-Blockx><H-Item><move>ya</move></H-Item><H-Item><move-move>ya</move-move></H-Item></H-Blockx>",
                media.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void RejectsBlockWithDifferentName()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new BxBlockArray("H-Blockx", "H-Item",
                    new BxBlock("H-Item", new BxProp("move", "ya")),
                    new BxBlock("X-Item", new BxProp("move-move", "ya"))
                ).Print(new XmlMedia())
            );
        }
    }
}
