using System;
using System.Xml.Linq;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Scalar;

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
        public BxTransported(XNode node) : this(node, true)
        { }

        /// <summary>
        /// A brix which has been transported as raw data.
        /// </summary>
        private BxTransported(XNode node, bool isRoot = true) : this(new ScalarOf<XElement>(() =>
            {
                XElement result;
                if (node.NodeType == System.Xml.XmlNodeType.Element)
                {
                    result = isRoot ? node.Document.Root : node as XElement;
                }
                else if(node.NodeType == System.Xml.XmlNodeType.Document)
                {
                    result = (node as XDocument).Root;
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
