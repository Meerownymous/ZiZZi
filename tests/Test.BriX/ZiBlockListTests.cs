using System;
using Xunit;
using ZiZZi.Matter.XML;

namespace ZiZZi.Test
{
    public sealed class ZiBlockListTests
    {
        [Fact]
        public void MakesBlockList()
        {
            var matter = new XmlMatter();

            new ZiBlockList("H-Blockx", "H-Item",
                new ZiBlock("H-Item", new ZiProp("move", "ya")),
                new ZiBlock("H-Item", new ZiProp("move-move", "ya"))
            ).Form(matter);

            Assert.Equal(
                "<H-Blockx><H-Item><move>ya</move></H-Item><H-Item><move-move>ya</move-move></H-Item></H-Blockx>",
                matter.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void WritesBlockListFromUnnamedBlock()
        {
            var matter = new XmlMatter();

            new ZiBlockList("H-Blockx", "H-Item",
                new ZiBlock(new ZiProp("move", "ya")),
                new ZiBlock(new ZiProp("move-move", "ya"))
            ).Form(matter);

            Assert.Equal(
                "<H-Blockx><H-Item><move>ya</move></H-Item><H-Item><move-move>ya</move-move></H-Item></H-Blockx>",
                matter.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void RejectsBlockWithDifferentName()
        {
            Assert.Throws<ArgumentException>(() =>
                new ZiBlockList("H-Blockx", "H-Item",
                    new ZiBlock("H-Item", new ZiProp("move", "ya")),
                    new ZiBlock("X-Item", new ZiProp("move-move", "ya"))
                ).Form(new XmlMatter())
            );
        }
    }
}