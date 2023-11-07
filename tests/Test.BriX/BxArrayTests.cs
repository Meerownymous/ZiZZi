

using ZiZZi.Matter;
using Xunit;
using Yaapii.Atoms.Enumerable;

namespace ZiZZi.Test
{
    public sealed class BxArrayTests
    {
        [Fact]
        public void WritesArray()
        {
            var media = new XmlMedia();
            new BxValueArray("ItemArray", "Item", new ManyOf<string>("Entry"))
                .Print(media);

            Assert.Equal(
                "<ItemArray><Item>Entry</Item></ItemArray>",
                media.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }
    }
}
