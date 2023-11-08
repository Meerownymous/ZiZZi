using System.Collections.Generic;
using System.Globalization;
using Tonga;
using Tonga.Enumerable;
using Tonga.Map;

namespace ZiZZi
{
    /// <summary>
    /// A blox map (list of <see cref="ZiProp"/>).
    /// </summary>
    public sealed class ZiMap : BloxEnvelope
    {
        /// <summary>
        /// A blox map (list of <see cref="ZiProp"/>).
        /// </summary>
        public ZiMap(IPair<string, long> firstPair, params IPair<string, long>[] pairs) : this(
            Mapped._(pair => new ZiProp(pair.Key(), pair.Value()),
                Joined._(
                    Single._(firstPair),
                    pairs
                )
            )
        )
        { }

        /// <summary>
        /// A blox map (list of <see cref="ZiProp"/>).
        /// </summary>
        public ZiMap(IMap<string, bool> props) : this(
            Mapped._(
                entry => new ZiProp(entry.Key(), entry.Value()),
                AsEnumerable._(props.Pairs)
            )
        )
        { }

        /// <summary>
        /// A blox map (list of <see cref="ZiProp"/>).
        /// </summary>
        public ZiMap(params string[] props) : this(
            Mapped._(
                entry => new ZiProp(entry.Key(), entry.Value()),
                AsMap._<string, string>().Pairs()
            )
        )
        { }

        /// <summary>
        /// A blox map (list of <see cref="ZiProp"/>).
        /// </summary>
        public ZiMap(IMap<string,string> props) : this(
            Mapped._(
                entry => new ZiProp(entry.Key(), entry.Value()),
                AsEnumerable._(props.Pairs)
            )
        )
        { }

        /// <summary>
        /// A blox map (list of <see cref="ZiProp"/>).
        /// </summary>
        public ZiMap(IMap<string, double> entries) : this(
            Mapped._(
                entry => new ZiProp(entry.Key(), entry.Value()),
                AsEnumerable._(entries.Pairs)
            )
        )
        { }

        /// <summary>
        /// A blox map (list of <see cref="ZiProp"/>).
        /// </summary>
        private ZiMap(IEnumerable<IBlox> props) : base(() =>
            new ZiChain(props)
        )
        { }
    }
}
