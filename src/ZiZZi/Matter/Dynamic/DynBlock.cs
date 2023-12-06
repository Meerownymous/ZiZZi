using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Tonga.Bytes;
using Tonga.IO;

namespace ZiZZi.Matter.Object
{
    /// <summary>
    /// A dynamic block. It can contain properties, arrays and other blocks.
    /// </summary>
    public class DynBlock : IMatter<object>
    {
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

        public void Present(string name, Func<IContent<string>> content)
        {
            this.container.Value[name] = content().Value();
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            this.container
                .Value[name] = this.bytesAsTyped.Flip(dataType, name, content().Value());
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            this.container
                .Value[name] = this.bytesAsTyped.Flip(dataType, name,
                    new AsBytes(
                        new AsInput(content().Value())
                    ).Bytes()
                );
        }
    }
}

