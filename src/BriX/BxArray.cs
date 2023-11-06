

using System.Collections.Generic;
using Tonga.Enumerable;
using Yaapii.Atoms.Enumerable;

namespace BLox
{
    /// <summary>
    /// A list of values.
    /// </summary>
    public sealed class BxArray : IBrix
    {
        private readonly string name;
        private readonly string itemName;
        private readonly IEnumerable<string> contents;

        /// <summary>
        /// A list of values.
        /// </summary>
        public BxArray(string name, string itemName, params string[] contents) : this(
            name, itemName,
            AsEnumerable._(contents)
        )
        { }

        /// <summary>
        /// A list of values.
        /// </summary>
        public BxArray(string name, string itemName, IEnumerable<string> contents)
        {
            this.name = name;
            this.itemName = itemName;
            this.contents = contents;
        }

        public T Print<T>(IMedia<T> media)
        {
            var array = media.Array(this.name, this.itemName);
            foreach (var value in this.contents)
            {
                array.Put(value);
            }
            return media.Content();
        }

        public override string ToString()
        {
            return $"ARRAY:SIMPLE({this.name}/{itemName}:{string.Join(",", this.contents)})";
        }
    }
}
