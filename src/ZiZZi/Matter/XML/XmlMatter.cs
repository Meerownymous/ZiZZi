using System;
using System.IO;
using System.Xml.Linq;
using Tonga.Scalar;

namespace ZiZZi.Matter.XML
{
    /// <summary>
    /// XML matter.
    /// </summary>
    public sealed class XmlMatter : IMatter<XNode>
    {
        private readonly XNode document;
        private readonly MatterOrigin matters;

        /// <summary>
        /// XML matter.
        /// </summary>
        public XmlMatter()
        {
            this.matters = new MatterOrigin(new BytesAsXElement());
            this.document = new XDocument();
        }

        public XNode Content()
        {
            return this.document;
        }

        public IMatter<XNode> Open(string contentType, string name)
        {
            if (Length._(this.document.Document.Nodes()).Value() > 1)
                throw new InvalidOperationException(
                    $"You are trying to open a {contentType} named {name} on "
                    + $"root level of the xml, but it already has a root."
                );
            return this.matters.Flip(contentType, this.document, name);
        }

        public void Put(string name, Func<string> content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of xml. " +
                $"You must first open a block or array."
            );
        }

        public void Put(string name, string dataType, Func<byte[]> content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of xml. " +
                $"You must first open a block or array."
            );
        }

        public void Put(string name, string dataType, Func<Stream> content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of xml. " +
                $"You must first open a block or array."
            );
        }
    }
}
