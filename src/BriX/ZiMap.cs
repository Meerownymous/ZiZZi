using System.Globalization;
using Tonga;
using Tonga.Enumerable;
using Tonga.Map;

namespace ZiZZi
{
    /// <summary>
    /// A blox map (list of <see cref="ZiProp"/>).
    /// </summary>
    public sealed class ZiMap<TValue> : BloxEnvelope
    {
        /// <summary>
        /// A brix map (list of <see cref="ZiProp"/>).
        /// </summary>
        public ZiMap(string key, string value, params string[] pairs) : this(
            AsMap._(key, value, pairs)
        )
        { }

        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public ZiMap(IPair<string,string> pair, params IPair<string,string>[] pairs) : this(
            AsMap._(pair, pairs)
        )
        { }

        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public ZiMap(IMap<string, double> entries) : this(
            AsMap._(
                Mapped._(
                    pair => AsPair._(pair.Key(), new AsBytes()),
                    entries.Pairs()
                )
            )
        )
        { }

        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public ZiMap(IMap<string, T> entries) : base(() =>
            new ZiChain(
                Mapped._(
                    (pair) => new ZiProp(pair, ),
                    entries.Pairs()
                )
            )
        )
        { }
    }
}
