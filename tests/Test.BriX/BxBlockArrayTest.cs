//MIT License

//Copyright (c) 2020 ICARUS Consulting GmbH

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
using System;
using Xunit;

namespace BriX.Test
{
    public sealed class BxBlocksTests
    {
        [Fact]
        public void WritesBlockArray()
        {
            var media = new XmlMedia();

            new BxBlockArray("H-Blockx", "H-Item",
                new BxBlock("H-Item", new BxProp("move", "ya")),
                new BxBlock("H-Item", new BxProp("move-move", "ya"))
            ).Print(media);

            Assert.Equal(
                "<H-Blockx><H-Item><move>ya</move></H-Item><H-Item><move-move>ya</move-move></H-Item></H-Blockx>",
                media.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void WritesBlockArrayUnnamed()
        {
            var media = new XmlMedia();

            new BxBlockArray("H-Blockx", "H-Item",
                new BxBlock(new BxProp("move", "ya")),
                new BxBlock(new BxProp("move-move", "ya"))
            ).Print(media);

            Assert.Equal(
                "<H-Blockx><H-Item><move>ya</move></H-Item><H-Item><move-move>ya</move-move></H-Item></H-Blockx>",
                media.Content().ToString(System.Xml.Linq.SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void RejectsBlockWithDifferentName()
        {
            Assert.Throws<InvalidOperationException>(() =>
                    new BxBlockArray("H-Blockx", "H-Item",
                    new BxBlock("H-Item", new BxProp("move", "ya")),
                    new BxBlock("X-Item", new BxProp("move-move", "ya"))
                ).Print(new XmlMedia())
            );
        }
    }
}
