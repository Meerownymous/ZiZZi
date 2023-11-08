using System.Collections.Generic;
using Tonga.Enumerable;

namespace ZiZZi
{
    /// <summary>
    /// A block which can be printed to a media.
    /// </summary>
    public sealed class ZiBlock : IBlox
    {
        private readonly string name;
        private readonly IBlox content;

        /// <summary>
        /// A block which can be printed to an array.
        /// </summary>
        public ZiBlock(IBlox content) : this(string.Empty, content)
        { }

        /// <summary>
        /// A block which can be printed to an array.
        /// </summary>
        public ZiBlock(params IBlox[] content) : this(string.Empty, AsEnumerable._(content))
        { }

        /// <summary>
        /// A block which can be printed to an array.
        /// </summary>
        public ZiBlock(IEnumerable<IBlox> content) : this(string.Empty, new ZiChain(content))
        { }

        /// <summary>
        /// A block which can be printed to a media.
        /// </summary>
        public ZiBlock(string name, params IBlox[] content) : this(name, AsEnumerable._(content))
        { }

        /// <summary>
        /// A block which can be printed to a media.
        /// </summary>
        public ZiBlock(string name, IEnumerable<IBlox> content) : this(name, new ZiChain(content))
        { }

        /// <summary>
        /// A block which can be printed to a media.
        /// </summary>
        public ZiBlock(string name, IBlox content)
        {
            this.name = name;
            this.content = content;
        }

        public T Form<T>(IMatter<T> media)
        {
            return this.content.Form(media.Open("block", this.name));
        }

        public override string ToString()
        {
            return $"BLOCK({this.name})";
        }
    }
}
