using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tonga.Enumerable;

namespace ZiZZi.Matter.Object
{
    /// <summary>
    /// A list of blocks in shape of the given generic parameter.
    /// </summary>
    public sealed class ObjectBlockList<TBlock> : IMatter<object>
    {
        private readonly TBlock blueprint;
        private readonly BytesAsTyped conversion;
        private readonly IList<dynamic> items;

        /// <summary>
        /// A list of blocks in shape of the given generic parameter.
        /// </summary>
        public ObjectBlockList(TBlock blueprint, BytesAsTyped conversion)
        {
            this.blueprint = blueprint;
            this.conversion = conversion;
            this.items = new List<dynamic>();
        }

        public dynamic Content()
        {
            return
                Mapped._(
                    item => (item as IMatter<dynamic>).Content(),
                    this.items
                ).ToArray();
        }

        public IMatter<dynamic> Open(string contentType, string name)
        {
            dynamic sub =
                Activator.CreateInstance(
                    typeof(ObjectBlock2<>)
                        .MakeGenericType(typeof(TBlock)),
                        this.blueprint,
                        this.conversion
                );
            this.items.Add(sub);
            return sub;
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            throw new InvalidOperationException();
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            throw new InvalidOperationException();
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            throw new InvalidOperationException();
        }
    }
}

