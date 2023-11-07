using System.Collections.Generic;
using Tonga.Enumerable;
using Tonga.Func;

namespace ZiZZi
{
    /// <summary>
    /// Multiple contents.
    /// </summary>
    public sealed class BxChain : IBrix
    {
        private readonly IEnumerable<IBrix> printables;

        /// <summary>
        /// Multiple contents.
        /// </summary>
        public BxChain(params IBrix[] more) : this(
            AsEnumerable._(more)
        )
        { }

        /// <summary>
        /// Multiple contents.
        /// </summary>
        public BxChain(IBrix printable, IEnumerable<IBrix> printables) : this(
            Joined._(
                AsEnumerable._(printable),
                printables
            )
        )
        { }

        /// <summary>
        /// Multiple contents.
        /// </summary>
        public BxChain(IEnumerable<IBrix> printables)
        {
            this.printables = printables;
        }

        public T Print<T>(IMedia<T> media)
        {
            Each._(
                printable => printable.Print(media),
                this.printables
            ).Invoke();
            return media.Content();
        }
    }
}
