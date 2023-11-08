

using System.Collections.Generic;
using Tonga.Enumerable;

namespace ZiZZi
{
    /// <summary>
    /// A list of values.
    /// </summary>
    public sealed class ZiValueList : IBlox
    {
        private readonly string name;
        private readonly string itemName;
        private readonly IEnumerable<string> contents;

        /// <summary>
        /// A list of values.
        /// </summary>
        public ZiValueList(string name, string itemName, params string[] contents) : this(
            name, itemName,
            AsEnumerable._(contents)
        )
        { }

        /// <summary>
        /// A list of values.
        /// </summary>
        public ZiValueList(string name, string itemName, IEnumerable<string> contents)
        {
            this.name = name;
            this.itemName = itemName;
            this.contents = contents;
        }

        public T Form<T>(IMatter<T> matter)
        {
            var array = matter.Open("value-list", this.name);
            foreach (var value in this.contents)
            {
                array.Put(this.itemName, value);
            }
            return matter.Content();
        }

        public override string ToString()
        {
            return $"ARRAY:VALUES({this.name}/{itemName}:{string.Join(",", this.contents)})";
        }
    }
}
