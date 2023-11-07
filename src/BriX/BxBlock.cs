using System.Collections.Generic;
using Tonga.Enumerable;

namespace ZiZZi
{
    /// <summary>
    /// A block which can be printed to a media.
    /// </summary>
    public sealed class BxBlock : IBrix
    {
        private readonly string name;
        private readonly IBrix content;

        /// <summary>
        /// A block which can be printed to an array.
        /// </summary>
        public BxBlock(IBrix content) : this(string.Empty, content)
        { }

        /// <summary>
        /// A block which can be printed to an array.
        /// </summary>
        public BxBlock(params IBrix[] content) : this(string.Empty, AsEnumerable._(content))
        { }

        /// <summary>
        /// A block which can be printed to an array.
        /// </summary>
        public BxBlock(IEnumerable<IBrix> content) : this(string.Empty, new BxChain(content))
        { }

        /// <summary>
        /// A block which can be printed to a media.
        /// </summary>
        public BxBlock(string name, params IBrix[] content) : this(name, AsEnumerable._(content))
        { }

        /// <summary>
        /// A block which can be printed to a media.
        /// </summary>
        public BxBlock(string name, IEnumerable<IBrix> content) : this(name, new BxChain(content))
        { }

        /// <summary>
        /// A block which can be printed to a media.
        /// </summary>
        public BxBlock(string name, IBrix content)
        {
            this.name = name;
            this.content = content;
        }

        public T Print<T>(IMedia<T> media)
        {
            return this.content.Print(media.Block(this.name));
        }

        public override string ToString()
        {
            return $"BLOCK({this.name})";
        }
    }
}
