using System;
using System.Collections.Generic;
using System.IO;
using Tonga;
using Tonga.Bytes;
using Tonga.IO;

namespace ZiZZi.Matter.Dynamic
{
    /// <summary>
    /// Simple value list of items.
    /// </summary>
    public class DynValueList : IMatter<object>
    {
        private readonly Lazy<IList<object>> list;
        private readonly ISwap<string, string, byte[], object> bytesAsTyped;

        /// <summary>
        /// Simple value array of items.
        /// </summary>
        public DynValueList(
            ISwap<string, string, byte[], object> bytesAsTyped,
            dynamic parent,
            string arrayName
        )
        {
            this.list = new Lazy<IList<object>>(() =>
            {
                var list = new List<object>();
                ((IDictionary<string, object>)parent)[arrayName] = list;
                return list;
            });
            this.bytesAsTyped = bytesAsTyped;
        }

        public dynamic Content()
        {
            return this.list.Value;
        }

        public IMatter<object> Open(string contentType, string name)
        {
            return new Dead<object>();
        }

        public void Put(string name, string content)
        {
            this.list.Value
                .Add(content);
        }

        public void Put(string name, string dataType, byte[] content)
        {
            this.list.Value
                .Add(
                    this.bytesAsTyped.Flip(dataType, name, content)
                );
        }

        public void Put(string name, string dataType, Stream content)
        {
            this.Put(name, dataType,
                new AsBytes(
                    new AsInput(content)
                ).Bytes()
            );
        }
    }
}

