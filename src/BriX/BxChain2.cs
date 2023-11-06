using System.Collections.Generic;
using Tonga.Enumerable;
using Tonga.Func;

namespace BLox
{
    /// <summary>
    /// Multiple contents.
    /// </summary>
    public sealed class BxChain2 : IBrix2
    {
        private readonly IEnumerable<IBrix2> printables;

        /// <summary>
        /// Multiple contents.
        /// </summary>
        public BxChain2(params IBrix2[] more) : this(
            AsEnumerable._(more)
        )
        { }

        /// <summary>
        /// Multiple contents.
        /// </summary>
        public BxChain2(IBrix2 printable, IEnumerable<IBrix2> printables) : this(
            Joined._(
                AsEnumerable._(printable),
                printables
            )
        )
        { }

        /// <summary>
        /// Multiple contents.
        /// </summary>
        public BxChain2(IEnumerable<IBrix2> printables)
        {
            this.printables = printables;
        }

        public T Print<T>(IShape<T> media)
        {
            Each._(
                printable => printable.Print(media),
                this.printables
            ).Invoke();
            return media.Content();
        }
    }
}
