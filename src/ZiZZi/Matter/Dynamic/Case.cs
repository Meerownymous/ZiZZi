using System;
using System.Xml.Linq;
using Tonga;
using Tonga.Map;
using Tonga.Swap;

namespace ZiZZi.Matter.Dynamic
{
    /// <summary>
    /// Use with swapswitch to swap if given type is matched.
    /// </summary>
    public class Case : PairEnvelope<string, ISwap<string, byte[], object>>
    {
        /// <summary>
        /// Use with swapswitch to swap if given type is matched.
        /// </summary>
        public Case(string type, Func<string, byte[], object> swap) : base(
            AsPair._<string, ISwap<string, byte[], object>>(type, AsSwap._(swap))
        )
        { }

        /// <summary>
        /// Use with swapswitch to swap if given type is matched.
        /// </summary>
        public static IPair<string, ISwap<string, byte[], object>> _(
            string type, Func<string, byte[], object> swap
        ) =>
            new Case(type, swap);
    }
}

