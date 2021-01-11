using BriX.Media;
using Xunit;
using Yaapii.Xml;

namespace BriX.Test
{
    public sealed class BxTransportedTests
    {
        [Fact]
        public void RebuildsEmptyRootBlock()
        {
            Assert.Equal(
                1,
                new XMLCursor(
                    new BxTransported(
                        new BxBlock("root").Print(new TransportMedia())
                    ).Print(new XmlMedia())
                ).Nodes("/root")
                .Count
            );
        }

        [Fact]
        public void RebuildsEmptyRootArray()
        {
            Assert.Equal(
                1,
                new XMLCursor(
                    new BxTransported(
                        new BxArray("root", "sub").Print(new TransportMedia())
                    ).Print(new XmlMedia())
                ).Nodes("/root")
                .Count
            );
        }

        [Fact]
        public void RebuildsSubBlock()
        {
            Assert.Equal(
                1,
                new XMLCursor(
                    new BxTransported(
                        new BxBlock("root",
                            new BxBlock("sub")
                        ).Print(new TransportMedia())
                    ).Print(new XmlMedia())
                ).Nodes("/root/sub")
                .Count
            );
        }

        [Fact]
        public void RebuildsSimpleArray()
        {
            Assert.Equal(
                1,
                new XMLCursor(
                    new BxTransported(
                        new BxBlock("root",
                            new BxArray("items", "item", "content")
                        ).Print(new TransportMedia())
                    ).Print(new XmlMedia())
                ).Nodes("/root/items/item[text() = 'content']")
                .Count
            );
        }

        [Fact]
        public void RebuildsComplexArray()
        {
            Assert.Equal(
                1,
                new XMLCursor(
                    new BxBlock("root",
                        new BxBlockArray("items", "item",
                            new BxBlock(new BxProp("name", "I-Tem"))
                        )
                    ).Print(new TransportMedia())
                ).Nodes("/root/items/item[name/text() = 'I-Tem']")
                .Count
            );
        }

        [Fact]
        public void RebuildsProp()
        {
            Assert.Equal(
                1,
                new XMLCursor(
                    new BxBlock("root",
                        new BxProp("of", "all evil")
                    ).Print(new TransportMedia())
                ).Nodes("/root/of[text() = 'all evil']")
                .Count
            );
        }
    }
}
