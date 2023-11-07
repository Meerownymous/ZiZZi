

using System.Collections.Generic;
using Tonga.Enumerable;
using Tonga.Func;
using Yaapii.Atoms.Enumerable;

namespace ZiZZi
{
    /// <summary>
    /// A list of blocks.
    /// </summary>
    public sealed class BxBlockArray : IBrix
    {
        private readonly string name;
        private readonly string itemName;
        private readonly IEnumerable<IBrix> contents;

        /// <summary>
        /// A list of blocks.
        /// </summary>
        public BxBlockArray(string name, string itemName, params IBrix[] contents) : this(
            name, itemName,
            AsEnumerable._(contents)
        )
        { }

        /// <summary>
        /// A list of blocks.
        /// </summary>
        public BxBlockArray(string name, string itemName, IEnumerable<IBrix> contents)
        {
            this.name = name;
            this.itemName = itemName;
            this.contents = contents;
        }

        public T Print<T>(IMedia<T> media)
        {
            var target = media.Array(this.name, this.itemName);
            Each._(
                block => block.Print(media),
                this.contents
            ).Invoke();
            return media.Content();
        }

        public override string ToString()
        {
            return $"ARRAY:COMPLEX({this.name}/{this.itemName})";
        }
    }
}
