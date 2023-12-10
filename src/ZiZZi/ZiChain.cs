using System.Collections.Generic;
using Tonga.Enumerable;
using Tonga.Func;

namespace ZiZZi
{
    /// <summary>
    /// Multiple contents.
    /// </summary>
    public sealed class ZiChain : IBlox
    {
        private readonly IEnumerable<IBlox> information;

        /// <summary>
        /// Multiple contents.
        /// </summary>
        public ZiChain(params IBlox[] more) : this(
            AsEnumerable._(more)
        )
        { }

        /// <summary>
        /// Multiple contents.
        /// </summary>
        public ZiChain(IBlox printable, IEnumerable<IBlox> printables) : this(
            Joined._(
                AsEnumerable._(printable),
                printables
            )
        )
        { }

        /// <summary>
        /// Multiple contents.
        /// </summary>
        public ZiChain(IEnumerable<IBlox> printables)
        {
            this.information = printables;
        }

        public T Form<T>(IMatter<T> media)
            where T : class
        {
            foreach (var p in this.information)
                p.Form(media);

            //Each._(
            //    printable => printable.Form(media),
            //    this.information
            //).Invoke();
            return media.Content();
        }
    }
}
