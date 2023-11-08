

using ZiZZi.Matter;
using System.Xml.Linq;
using Xunit;
using ZiZZi.Matter.XML;

namespace ZiZZi.Test
{
    public sealed class ZiBlockTests
    {
        [Fact]
        public void WritesBlockName()
        {
            var matter = new XmlMatter();

            new ZiBlock("mein-block", new ZiProp("stockwerk", "16"))
                .Form(matter);

            Assert.Equal(
                "<mein-block><stockwerk>16</stockwerk></mein-block>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void PutsBlockInBlock()
        {
            var matter = new XmlMatter();

            new ZiBlock("mein-block", new ZiBlock("dein-block"))
                .Form(matter);

            Assert.Equal(
                "<mein-block><dein-block /></mein-block>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void CanBeInList()
        {
            var matter = new XmlMatter();

            new ZiBlockList("Hochhaussiedlung", "Strasse",
                new ZiBlock(new ZiProp("Hausnummer", 1))
            ).Form(matter);

            Assert.Equal(
                "<Hochhaussiedlung><Strasse><Hausnummer>1</Hausnummer></Strasse></Hochhaussiedlung>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}
