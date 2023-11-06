

using BLox.Shape;
using Xunit;

namespace BLox.Test
{
    public sealed class BxConditionalTests
    {
        [Fact]
        public void PrintsIfConditionMatched()
        {
            var media = new XmlMedia().Block("root");
            new BxConditional(() => true,
                () => new BxProp("Matched", "true")
            ).Print(media);
            Assert.Equal(
                "<root><Matched>true</Matched></root>",
                media.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void DoesntPrintIfConditionNotMatched()
        {
            var media = new XmlMedia().Block("root");
            new BxConditional(() => false,
                () => new BxProp("Matched", "true")
            ).Print(media);
            Assert.Equal(
                "<root />",
                media.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }
    }
}
