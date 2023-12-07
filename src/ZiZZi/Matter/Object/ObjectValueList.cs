using System;
using System.Collections.Generic;
using System.IO;
using Tonga.Bytes;
using Tonga.IO;

namespace ZiZZi.Matter.Object
{
    /// <summary>
    /// Forms a list of values.
    /// Only values of the generic type are accepted, others are rejected.
    /// </summary>
    public sealed class ObjectValueList<TItem> : IMatter<object>
        where TItem : class
    {
        private readonly string name;
        private readonly BytesAsTyped conversion;
        private List<TItem> values;

        /// <summary>
        /// Forms a list of values.
        /// Only values of the generic type are accepted, others are rejected.
        /// </summary>
        public ObjectValueList(string listName, BytesAsTyped conversion)
        {
            this.values = new List<TItem>();
            this.name = listName;
            this.conversion = conversion;
        }

        public dynamic Content()
        {
            return this.values.ToArray();
        }

        public IMatter<dynamic> Open(string contentType, string name)
        {
            throw new InvalidOperationException();
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            if(typeof(TItem) != typeof(string))
                throw new ArgumentException($"Cannot put strings into {this.name} - it is a list of {typeof(TItem).GetType().Name}.");
            this.values.Add(content().Value() as TItem);
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            if (!typeof(TItem).Name.Equals(dataType, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Cannot put {dataType} into {this.name} - it is a list of {typeof(TItem).GetType().Name}.");
            this.values.Add(this.conversion.Flip(dataType, name, content().Value()) as TItem);
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            if (!typeof(TItem).Name.Equals(dataType, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Cannot put {dataType} into {this.name} - it is a list of {typeof(TItem).GetType().Name}.");
            this.values
                .Add(
                    this.conversion
                        .Flip(dataType, name,
                            new AsBytes(new AsInput(content().Value())).Bytes()
                        ) as TItem
                );
        }
    }
}

