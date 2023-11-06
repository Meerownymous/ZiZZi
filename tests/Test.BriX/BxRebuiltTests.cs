

using BLox.Shape;
using Xunit;
using Yaapii.Xml;

namespace BLox.Test
{
    public sealed class BxRebuiltTests
    {
        [Fact]
        public void RebuildsEmptyRootBlock()
        {
            Assert.Equal(
                1,
                new XMLCursor(
                    new BxRebuilt(
                        new BxBlock("root").Print(new RebuildMedia())
                    ).Print(new XmlMedia())
                ).Nodes("/root")
                .Count
            );
        }

        [Fact]
        public void WorksWithNothing()
        {
            Assert.Empty(
                new XMLCursor(
                    new BxBlock("root",
                        new BxRebuilt(new BxNothing().Print(new RebuildMedia()))
                    ).Print(new XmlMedia())
                ).Nodes("/root/*")
            );
        }

        [Fact]
        public void RebuildsEmptyRootArray()
        {
            Assert.Equal(
                1,
                new XMLCursor(
                    new BxRebuilt(
                        new BxArray("root", "sub").Print(new RebuildMedia())
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
                    new BxRebuilt(
                        new BxBlock("root",
                            new BxBlock("sub")
                        ).Print(new RebuildMedia())
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
                    new BxRebuilt(
                        new BxBlock("root",
                            new BxArray("items", "item", "content")
                        ).Print(new RebuildMedia())
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
                    new BxRebuilt(
                        new BxBlock("root",
                            new BxBlockArray("items", "item",
                                new BxBlock(new BxProp("name", "I-Tem"))
                            )
                        ).Print(new RebuildMedia())
                    ).Print(new XmlMedia())
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
                    new BxRebuilt(
                        new BxBlock("root",
                            new BxProp("of", "all evil")
                        ).Print(new RebuildMedia())
                    ).Print(new XmlMedia())
                ).Nodes("/root/of[text() = 'all evil']")
                .Count
            );
        }
    }
}
