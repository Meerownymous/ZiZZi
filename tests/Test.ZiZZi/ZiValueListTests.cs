using Xunit;
using Tonga.Enumerable;
using ZiZZi.Matter.XML;

namespace ZiZZi.Test
{
    public sealed class ZiValueListTests
    {
        [Fact]
        public void MakesList()
        {
            var matter = new XmlMatter();
            new ZiValueList("ItemArray", "Item", AsEnumerable._("Entry"))
                .Form(matter);

            Assert.Equal(
                "<ItemArray><Item>Entry</Item></ItemArray>",
                matter.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }
    }
}
