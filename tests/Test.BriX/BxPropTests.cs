

using BLox.Shape;
using Xunit;

namespace BLox.Test
{
    public sealed class BxPropTests
    {
        [Fact]
        public void PrintsProp()
        {
            var media = new XmlMedia().Block("root");

            new BxProp("Key", "Value")
                .Print(media);

            Assert.Equal(
                "<root><Key>Value</Key></root>",
                media.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }
    }
}
