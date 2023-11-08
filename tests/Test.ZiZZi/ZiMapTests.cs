using ZiZZi.Matter;
using Tonga.Map;
using Xunit;
using ZiZZi.Matter.XML;

namespace ZiZZi.Test
{
    public sealed class ZiMapTests
    {
        [Fact]
        public void WritesProps()
        {
            var matter = new XmlMatter().Open("block", "map");
            new ZiMap(
                AsMap._(
                    "firstname", "Förster",
                    "lastname", "Laster"
                )
            ).Form(matter);

            Assert.Equal(
                "<map><firstname>Förster</firstname><lastname>Laster</lastname></map>",
                matter.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }
    }
}
