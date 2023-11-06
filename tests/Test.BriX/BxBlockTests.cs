

using BLox.Shape;
using System.Xml.Linq;
using Xunit;

namespace BLox.Test
{
    public sealed class BxBlockTests
    {
        [Fact]
        public void WritesBlockName()
        {
            var media = new XmlMedia();

            new BxBlock("mein-block", new BxProp("stockwerk", "16"))
                .Print(media);

            Assert.Equal(
                "<mein-block><stockwerk>16</stockwerk></mein-block>",
                media.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void PutsBlockInBlock()
        {
            var media = new XmlMedia();

            new BxBlock("mein-block", new BxBlock("dein-block"))
                .Print(media);

            Assert.Equal(
                "<mein-block><dein-block /></mein-block>",
                media.Content().ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}
