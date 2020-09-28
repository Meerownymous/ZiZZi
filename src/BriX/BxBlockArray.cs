using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;

namespace BriX.Media
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
            new ManyOf<IBrix>(contents)
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
            var array = media.Array(this.name, this.itemName);
            foreach (var block in this.contents)
            {
                block.Print(array);
            }
            return media.Content();
        }

        public override string ToString()
        {
            return $"ARRAY:COMPLEX({this.name}/{this.itemName})";
        }
    }
}
