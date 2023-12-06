using System.Xml.Linq;
using Tonga.Bytes;
using Xunit;

namespace ZiZZi.Matter.XML.Test
{
    public sealed class XMLBlockTests
    {
        [Fact]
        public void CreatesBlock()
        {
            var root = new XElement("root");
            var matter =
                new XmlBlock(
                    new MatterOrigin(new BytesAsXElement()),
                    new BytesAsXElement(),
                    root,
                    "User"
                );

            Assert.Equal(
                "<User />",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void PutsPropInBlock()
        {
            var root = new XElement("Root");
            var matter =
                new XmlBlock(
                    new MatterOrigin(new BytesAsXElement()),
                    new BytesAsXElement(),
                    root,
                    "User"
                );

            matter.Present("Name", "string", () => TakeContent._(AsBytes._("Bob").Bytes()));
            matter.Present("Friends", "string", () => TakeContent._(AsBytes._("Jay").Bytes()));

            Assert.Equal(
                "<Root><User><Name>Bob</Name><Friends>Jay</Friends></User></Root>",
                root.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void OpensSubBlock()
        {
            var root = new XElement("Root");
            new XmlBlock(
                new MatterOrigin(new BytesAsXElement()),
                new BytesAsXElement(),
                root,
                "User"
            ).Open("block", "Skills");

            Assert.Equal(
                "<Root><User><Skills /></User></Root>",
                root.ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}

