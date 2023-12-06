using System;
using System.IO;
using System.Xml.Linq;
using Tonga;
using Tonga.Bytes;
using Tonga.IO;

namespace ZiZZi.Matter.XML
{
    /// <summary>
    /// Simple value list of items.
    /// </summary>
    public class XmlValueList : IMatter<XNode>
    {
        private readonly Lazy<XContainer> content;
        private readonly ISwap<string, string, byte[], XNode> bytesAsElement;

        /// <summary>
        /// Simple value array of items.
        /// </summary>
        public XmlValueList(
            ISwap<string, string, byte[], XNode> bytesAsElement,
            XNode parent,
            string arrayName
        )
        {
            this.content = new Lazy<XContainer>(() =>
            {
                var content = new XElement(arrayName);
                (parent as XContainer).Add(content);
                return content;
            });
            this.bytesAsElement = bytesAsElement;
        }

        public XNode Content()
        {
            return this.content.Value;
        }

        public IMatter<XNode> Open(string contentType, string name)
        {
            return new Dead<XNode>();
        }

        public void Put(string name, Func<string> content)
        {
            this.content.Value
                .Add(
                    new XElement(name, content())
                );
        }

        public void Put(string name, string dataType, Func<byte[]> content)
        {
            this.content.Value
                .Add(
                    this.bytesAsElement.Flip(dataType, name, content())
                );
        }

        public void Put(string name, string dataType, Func<Stream> content)
        {
            this.Put(name, dataType,
                () => new AsBytes(
                    new AsInput(content())
                ).Bytes()
            );
        }
    }
}

