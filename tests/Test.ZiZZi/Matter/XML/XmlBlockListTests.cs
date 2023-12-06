using System.Xml.Linq;
using Xunit;

namespace ZiZZi.Matter.XML.Test
{
    public sealed class XMLBlockArrayTests
    {
        [Fact]
        public void CreatesArray()
        {
            Assert.Equal(
                "<Worlds />",
                new XmlBlockList(
                    new MatterOrigin(new BytesAsXElement()),
                    new XElement("root"),
                    "Worlds"
                ).Content()
                .ToString()
            );
        }

        [Fact]
        public void OpensBlockInList()
        {
            var matter =
                new XmlBlockList(
                    new MatterOrigin(new BytesAsXElement()),
                    new XElement("root"),
                    "Worlds"
                );

            matter.Open("block", "UpsideDown");

            Assert.Equal(
                "<Worlds><UpsideDown /></Worlds>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}

