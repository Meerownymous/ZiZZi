using System.Collections.Generic;
using Tonga.Enumerable;
using Tonga.Func;
using ZiZZi.Matter;

namespace ZiZZi
{
    /// <summary>
    /// A list of blocks.
    /// </summary>
    public sealed class ZiBlockList : IBlox
    {
        private readonly string listName;
        private readonly string itemName;
        private readonly IEnumerable<IBlox> contents;

        /// <summary>
        /// A list of blocks.
        /// </summary>
        public ZiBlockList(string listName, string itemName, params IBlox[] contents) : this(
            listName,
            itemName,
            AsEnumerable._(contents)
        )
        { }

        /// <summary>
        /// A list of blocks.
        /// </summary>
        public ZiBlockList(string listName, string itemName, IEnumerable<IBlox> contents)
        {
            this.listName = listName;
            this.itemName = itemName;
            this.contents = contents;
        }

        public T Form<T>(IMatter<T> matter)
        {
            var target =
                new NameGuard<T>(
                    this.itemName,
                    matter.Open("block-list", this.listName)
                );

            Each._(
                block => block.Form(target),
                this.contents
            ).Invoke();
            return matter.Content();
        }

        public override string ToString()
        {
            return $"LIST:BLOCKS({this.listName}/{this.itemName})";
        }
    }
}
