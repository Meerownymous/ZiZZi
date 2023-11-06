using System.Globalization;
using Tonga;
using Tonga.Enumerable;
using Tonga.Map;

namespace BLox
{
    /// <summary>
    /// A brix map (list of <see cref="BxProp"/>).
    /// </summary>
    public sealed class BxMap : BrixEnvelope
    {
        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public BxMap(string key, string value, params string[] pairs) : this(
            AsMap._(key, value, pairs)
        )
        { }

        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public BxMap(IPair<string,string> pair, params IPair<string,string>[] pairs) : this(
            AsMap._(pair, pairs)
        )
        { }

        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public BxMap(IMap<string, double> entries) : this(
            AsMap._(
                Mapped._(
                    pair => AsPair._(pair.Key(), pair.Value().ToString(CultureInfo.InvariantCulture)),
                    entries.Pairs()
                )
            )
        )
        { }

        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public BxMap(IMap<string, string> entries) : base(() =>
            new BxChain(
                Mapped._(
                    (pair) => new BxProp(pair),
                    entries.Pairs()
                )
            )
        )
        { }
    }
}
