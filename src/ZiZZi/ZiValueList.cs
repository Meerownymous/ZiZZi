

using System;
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
        private readonly Lazy<IEnumerator<string>> contents;

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
        public ZiValueList(string name, string itemName, IEnumerable<string> contents) : this(
            name, itemName, () => contents
        )
        { }

        /// <summary>
        /// A list of values.
        /// </summary>
        public ZiValueList(string name, string itemName, Func<IEnumerable<string>> contents)
        {
            this.name = name;
            this.itemName = itemName;
            this.contents = new Lazy<IEnumerator<string>>(() => contents().GetEnumerator());
        }

        public T Form<T>(IMatter<T> matter)
        {
            var array = matter.Open("value-list", this.name);
            var contentTaken = true;
            while (contentTaken)
            {
                contentTaken = false;
                array.Present(
                    this.itemName,
                    () =>
                    {
                        contentTaken = this.contents.Value.MoveNext();
                        return contentTaken
                        ?
                        TakeContent._(this.contents.Value.Current)
                        :
                        new NoContent<string>();
                    }
                );
            }
            return matter.Content();
        }

        public override string ToString()
        {
            return $"ARRAY:VALUES({this.name}/{itemName}:{string.Join(",", this.contents)})";
        }
    }
}
