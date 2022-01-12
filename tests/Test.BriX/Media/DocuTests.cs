//MIT License

//Copyright (c) 2022 ICARUS Consulting GmbH

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

using Xunit;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Text;

namespace BriX.Media.Test
{
    public sealed class DocuTests
    {
        [Fact(Skip = "For documentation only")]
        public void ComplexExampleForDocu()
        {
            var brix = new BxBlock("rootBlock",
                new BxBlock("Block1",
                    new BxBlockArray("BlockArrayName", "BlockArrayItemName",
                        new BxBlock(
                            new BxProp("Prop-Inside-a-empty-block", "the Value with special chars §$%%&/><|öä#ü'*#\"")
                        )
                    )
                ),
                new BxBlock("Block2",
                    new BxArray("Array", "arrayItemName", "first", "second", "third")
                ),
                new BxBlock("Block3",
                    new BxMap(
                        "BxMap", "thefirstValue",
                        "bxMapKey", "second entry"
                    )
                )
            );
            var json = brix.Print(new JsonMedia()).ToString();
            var xml = brix.Print(new XmlMedia()).ToString();
            var rebuild = new TextOf(new BytesOf(brix.Print(new RebuildMedia()))).AsString();
            var yaml = brix.Print(new YamlMedia()).ToString();
        }
    }
}
