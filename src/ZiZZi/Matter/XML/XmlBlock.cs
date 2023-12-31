﻿using System;
using System.IO;
using System.Xml.Linq;
using Tonga.Bytes;
using Tonga.IO;

namespace ZiZZi.Matter.XML
{
    /// <summary>
    /// A XML block. It can contain properties, arrays and other blocks.
    /// </summary>
    public class XmlBlock : IMatter<XNode>
    {
        private readonly MatterOrigin matter;
        private readonly BytesAsXElement bytesAsElement;
        private readonly Lazy<XElement> container;

        /// <summary>
        /// A XML block. It can contain properties, arrays and other blocks.
        /// </summary>
        public XmlBlock(
            MatterOrigin matter,
            BytesAsXElement bytesAsElement,
            XNode parent,
            string name
        )
        {
            this.matter = matter;
            this.bytesAsElement = bytesAsElement;
            this.container =
                new Lazy<XElement>(() =>
                {
                    var container = new XElement(name);
                    (parent as XContainer).Add(container);
                    return container;
                });
        }

        public XNode Content()
        {
            return this.container.Value;
        }

        public IMatter<XNode> Open(string contentType, string name)
        {
            var subMedia = this.matter.Flip(contentType, this.container.Value, name);
            subMedia.Content();
            return subMedia;
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            this.container.Value.Add(new XElement(name, content().Value()));
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            this.container
                .Value
                .Add(
                    this.bytesAsElement.Flip(dataType, name, content().Value())
                );
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            this.container
                .Value
                .Add(
                    this.bytesAsElement
                        .Flip(dataType, name,
                            new AsBytes(
                                new AsInput(content().Value())
                            ).Bytes()
                        )
                );
        }
    }
}

