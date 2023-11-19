using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Tonga.Bytes;
using Tonga.IO;

namespace ZiZZi.Matter.Dynamic
{
    /// <summary>
    /// A dynamic block. It can contain properties, arrays and other blocks.
    /// </summary>
    public class DynBlock : IMatter<object>
    {
        private readonly string name;
        private readonly MatterOrigin matter;
        private readonly BytesAsTyped bytesAsTyped;
        private readonly Lazy<IDictionary<string, object>> container;

        /// <summary>
        /// A dynamic block. It can contain properties, arrays and other blocks.
        /// </summary>
        public DynBlock(
            MatterOrigin matter,
            BytesAsTyped bytesAsTyped,
            dynamic parent,
            string name,
            bool parentIsArray
        )
        {
            this.name = name;
            this.matter = matter;
            this.bytesAsTyped = bytesAsTyped;
            this.container =
                new Lazy<IDictionary<string, object>>(() =>
                {
                    dynamic block = new ExpandoObject();
                    if (parentIsArray)
                    {
                        ((IList<object>)parent).Add(block);
                    }
                    else
                    {
                        ((IDictionary<string, object>)parent)[name] = block;
                    }
                    return block;
                });
        }

        public dynamic Content()
        {
            return this.container.Value;
        }

        public IMatter<dynamic> Open(string contentType, string name)
        {
            var subMedia = this.matter.Flip(contentType, this.container.Value, name);
            subMedia.Content();
            return subMedia;
        }

        public void Put(string name, string content)
        {
            this.container.Value[name] = content;
        }

        public void Put(string name, string dataType, byte[] content)
        {
            this.container
                .Value[name] = this.bytesAsTyped.Flip(dataType, name, content);
        }

        public void Put(string name, string dataType, Stream content)
        {
            this.Put(
                name, dataType,
                new AsBytes(
                    new AsInput(content)
                ).Bytes()
            );
        }
    }
}

