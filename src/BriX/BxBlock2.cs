using System.Collections.Generic;
using Tonga.Enumerable;

namespace BLox
{
    /// <summary>
    /// A block which can be printed to a media.
    /// </summary>
    public sealed class BxBlock2 : IBrix2
    {
        private readonly string name;
        private readonly IBrix2 content;

        /// <summary>
        /// A block which can be printed to an array.
        /// </summary>
        public BxBlock2(IBrix2 content) : this(string.Empty, content)
        { }

        /// <summary>
        /// A block which can be printed to an array.
        /// </summary>
        public BxBlock2(params IBrix2[] content) : this(string.Empty, AsEnumerable._(content))
        { }

        /// <summary>
        /// A block which can be printed to an array.
        /// </summary>
        public BxBlock2(IEnumerable<IBrix2> content) : this(string.Empty, new BxChain2(content))
        { }

        /// <summary>
        /// A block which can be printed to a media.
        /// </summary>
        public BxBlock2(string name, params IBrix2[] content) : this(name, AsEnumerable._(content))
        { }

        /// <summary>
        /// A block which can be printed to a media.
        /// </summary>
        public BxBlock2(string name, IEnumerable<IBrix2> content) : this(name, new BxChain2(content))
        { }

        /// <summary>
        /// A block which can be printed to a media.
        /// </summary>
        public BxBlock2(string name, IBrix2 content)
        {
            this.name = name;
            this.content = content;
        }

        public T Print<T>(IShape<T> media)
        {
            return this.content.Print(media.Open("block", this.name));
        }

        public override string ToString()
        {
            return $"BLOCK({this.name})";
        }
    }
}
