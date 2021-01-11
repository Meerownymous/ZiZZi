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

using System;
using System.Xml.Linq;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace BriX
{
    /// <summary>
    /// A brix which has been transported as raw data.
    /// </summary>
    public sealed class BxTransported : IBrix
    {
        private readonly IBrix brix;

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxTransported(IInput data) : this(() => XDocument.Parse(new TextOf(data).AsString()), true)
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxTransported(string xml) : this(() => XDocument.Parse(xml), true)
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxTransported(byte[] bytes) : this(() => XDocument.Parse(new TextOf(bytes).AsString()), true)
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxTransported(XNode node) : this(() => node, true)
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        private BxTransported(XNode node, bool isRoot) : this(() => node, isRoot)
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        private BxTransported(Func<XNode> node, bool isRoot = true) : this(new ScalarOf<XElement>(() =>
            {
                var builtNode = node();
                XElement result;
                if (builtNode.NodeType == System.Xml.XmlNodeType.Element)
                {
                    result = isRoot ? builtNode.Document.Root : builtNode as XElement;
                }
                else if(builtNode.NodeType == System.Xml.XmlNodeType.Document)
                {
                    result = (builtNode as XDocument).Root;
                }
                else
                {
                    throw new ArgumentException("A transported brix must be of type XElement or XDocument");
                }
                return result;
            })
        )
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxTransported(IScalar<XElement> data)
        {
            this.brix =
                new BxChain(
                    new BxConditional(
                        () => data.Value().Attribute("bx-type").Value == "block",
                        () =>
                        new BxBlock(data.Value().Name.LocalName,
                            new Mapped<XNode, IBrix>(
                                elem => new BxTransported(elem, false),
                                data.Value().Elements()
                            )
                        )
                    ),
                    new BxConditional(
                        () => data.Value().Attribute("bx-type").Value == "array",
                        () =>
                        new BxArray(data.Value().Name.LocalName, data.Value().Attribute("bx-array-item-name").Value,
                            new Mapped<XElement, string>(
                                elem => elem.Value,
                                data.Value().Elements()
                            )
                        )
                    ),
                    new BxConditional(
                        () => data.Value().Attribute("bx-type").Value == "prop",
                        () =>
                        new BxProp(data.Value().Name.LocalName,
                            data.Value().Value
                        )
                    )
                );
        }

        public T Print<T>(IMedia<T> media)
        {
            return this.brix.Print(media);
        }
    }
}
