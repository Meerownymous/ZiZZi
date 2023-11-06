﻿

using System;
using System.Xml.Linq;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace BLox
{
    /// <summary>
    /// A brix which has been transported as raw data.
    /// </summary>
    public sealed class BxRebuilt : IBrix
    {
        private readonly Lazy<IBrix> brix;

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxRebuilt(byte[] bytes) : this(new TextOf(bytes))
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxRebuilt(IInput data) : this(new TextOf(data))
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxRebuilt(string xml) : this(new TextOf(xml))
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxRebuilt(IText content) : this(
            () => XDocument.Parse(content.AsString()),
            () => content.AsString() == "",
            true
        )
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxRebuilt(XNode node) : this(() => node, () => node.ToString() == "", true)
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        private BxRebuilt(XNode node, bool isRoot) : this(() => node,()=> node.ToString() == "", isRoot)
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        private BxRebuilt(Func<XNode> node, Func<bool> isEmpty, bool isRoot = true) : this(new ScalarOf<XElement>(() =>
            {
                var builtNode = node();
                XElement result;
                if (builtNode.NodeType == System.Xml.XmlNodeType.Element)
                {
                    result = isRoot ? builtNode.Document.Root : builtNode as XElement;
                }
                else if (builtNode.NodeType == System.Xml.XmlNodeType.Document)
                {
                    result = (builtNode as XDocument).Root;
                }
                else
                {
                    throw new ArgumentException("A transported brix must be of type XElement or XDocument");
                }
                return result;
            }),
            isEmpty
        )
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        public BxRebuilt(IScalar<XElement> data, Func<bool> isEmpty) : this(
            ()=>
            {
                IBrix result = new BxNothing();
                if (!isEmpty())
                {
                    result =
                        new BxChain(
                            new BxConditional(
                                () => data.Value().Attribute("bx-type").Value == "block",
                                () =>
                                new BxBlock(data.Value().Name.LocalName,
                                    new Mapped<XNode, IBrix>(
                                        elem => new BxRebuilt(elem, false),
                                        data.Value().Elements()
                                    )
                                )
                            ),
                            new BxConditional(
                                () => data.Value().Attribute("bx-type").Value == "array",
                                () =>
                                new Ternary<XNode, IBrix>(
                                    new LengthOf(data.Value().Elements()).Value() > 0 &&
                                    new LengthOf(
                                        new FirstOf<XElement>(data.Value().Elements()).Value().Attributes()
                                    ).Value() > 0,
                                    new BxBlockArray(data.Value().Name.LocalName, data.Value().Attribute("bx-array-item-name").Value,
                                        new Mapped<XNode, IBrix>(
                                            elem => new BxRebuilt(elem, false),
                                            data.Value().Elements()
                                        )
                                    ),
                                    new BxArray(data.Value().Name.LocalName, data.Value().Attribute("bx-array-item-name").Value,
                                        new Mapped<XElement, string>(
                                            elem => elem.Value,
                                            data.Value().Elements()
                                        )
                                    )
                                ).Value()
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
                return result;
            }
        )
        { }

        private BxRebuilt(Func<IBrix> brix)
        {
            this.brix = new Lazy<IBrix>(brix);
        }

        public T Print<T>(IMedia<T> media)
        {
            return this.brix.Value.Print(media);
        }
    }
}
