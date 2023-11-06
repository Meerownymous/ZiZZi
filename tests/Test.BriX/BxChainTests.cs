

using BLox.Shape;
using System.Xml.Linq;
using Xunit;

namespace BLox.Test
{
    public sealed class BxChainTests
    {
        [Fact]
        public void PrintsBrix()
        {
            var media = new XmlMedia().Block("root");
            new BxChain(
                    new BxProp("firstname", "John"),
                    new BxProp("lastname", "Malkovich")
                )
                .Print(media);

            Assert.Equal(
                "<root><firstname>John</firstname><lastname>Malkovich</lastname></root>",
                media.Content().ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}
