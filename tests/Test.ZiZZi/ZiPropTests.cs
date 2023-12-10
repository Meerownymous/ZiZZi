

using ZiZZi.Matter;
using Xunit;
using ZiZZi.Matter.XML;
using System.Xml.Linq;

namespace ZiZZi.Test
{
    public sealed class ZiPropTests
    {
        [Fact]
        public void ExpressesProp()
        {
            var matter = new XmlMatter().Open("block", "root");
            new ZiProp("Key", "Value").Form(matter);

            Assert.Equal(
                "<root><Key>Value</Key></root>",
                matter.Content().ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}
