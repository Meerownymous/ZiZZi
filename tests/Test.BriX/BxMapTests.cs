

using BLox.Shape;
using Tonga.Map;
using Xunit;
using Yaapii.Atoms.Map;

namespace BLox.Test
{
    public sealed class BxMapTests
    {
        [Fact]
        public void WritesProps()
        {
            var media = new XmlMedia().Block("map");
            new BxMap(
                AsMap._(
                    "firstname", "Förster",
                    "lastname", "Laster"
                )
            ).Print(media);

            Assert.Equal(
                "<map><firstname>Förster</firstname><lastname>Laster</lastname></map>",
                media.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }
    }
}
