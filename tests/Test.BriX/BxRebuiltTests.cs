//MIT License

//Copyright (c) 2021 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using BriX.Media;
using Xunit;
using Yaapii.Xml;

namespace BriX.Test
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
