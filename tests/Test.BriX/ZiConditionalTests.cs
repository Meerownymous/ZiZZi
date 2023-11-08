

using ZiZZi.Matter;
using Xunit;
using ZiZZi.Matter.XML;

namespace ZiZZi.Test
{
    public sealed class ZiConditionalTests
    {
        [Fact]
        public void PrintsIfConditionMatched()
        {
            var matter = new XmlMatter().Open("block", "root");
            new ZiConditional(
                () => true,
                () => new ZiProp("Matched", "true")
            ).Form(matter);
            Assert.Equal(
                "<root><Matched>true</Matched></root>",
                matter.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void DoesntPrintIfConditionNotMatched()
        {
            var matter = new XmlMatter().Open("block", "root");
            new ZiConditional(
                () => false,
                () => new ZiProp("Matched", "true")
            ).Form(matter);

            Assert.Equal(
                "<root />",
                matter.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }
    }
}
