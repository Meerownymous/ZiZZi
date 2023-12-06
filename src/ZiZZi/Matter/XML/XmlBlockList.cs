using System;
using System.IO;
using System.Xml.Linq;
using Tonga;

namespace ZiZZi.Matter.XML
{
    /// <summary>
    /// List of blocks which themselves can contain more properties and sub blocks.
    /// </summary>
    public sealed class XmlBlockList : IMatter<XNode>
    {
        private readonly Lazy<XContainer> container;
        private readonly ISwap<string, XNode, string, IMatter<XNode>> matter;

        /// <summary>
        /// List of blocks which themselves can contain more properties and sub blocks.
        /// </summary>
        public XmlBlockList(
            ISwap<string, XNode, string, IMatter<XNode>> matter,
            XNode parent,
            string arrayName
        )
        {
            this.container = new Lazy<XContainer>(() =>
            {
                var content = new XElement(arrayName);
                (parent as XContainer).Add(content);
                return content;
            });
            this.matter = matter;
        }

        public XNode Content()
        {
            return this.container.Value;
        }

        /// <summary>
        /// Opens a block inside the array. Block name must always be the same because it is an array.
        /// </summary>
        public IMatter<XNode> Open(string contentType, string name)
        {
            var subMedia = this.matter.Flip(contentType, this.container.Value, name);
            subMedia.Content();
            return subMedia;
        }

        public void Put(string name, Func<string> content)
        {
            throw new InvalidOperationException();
        }

        public void Put(string name, string dataType, Func<byte[]> content)
        {
            throw new InvalidOperationException();
        }

        public void Put(string name, string dataType, Func<Stream> content)
        {
            throw new InvalidOperationException();
        }
    }
}

