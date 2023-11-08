

using ZiZZi.Matter;
using System.Xml.Linq;
using Xunit;
using ZiZZi.Matter.XML;

namespace ZiZZi.Test
{
    public sealed class ZiChainTests
    {
        [Fact]
        public void ChainsContent()
        {
            var matter = new XmlMatter().Open("block", "root");
            new ZiChain(
                new ZiProp("firstname", "John"),
                new ZiProp("lastname", "Malkovich")
            )
            .Form(matter);

            Assert.Equal(
                "<root><firstname>John</firstname><lastname>Malkovich</lastname></root>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}
