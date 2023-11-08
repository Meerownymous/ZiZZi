using System;
using Newtonsoft.Json.Linq;
using Tonga;
using Tonga.Map;
using Tonga.Swap;

namespace ZiZZi.Matter.JSON
{
    /// <summary>
    /// Use with swapswitch to swap if given type is matched.
    /// </summary>
    public class Case : PairEnvelope<string, ISwap<byte[], JValue>>
    {
        /// <summary>
        /// Use with swapswitch to swap if given type is matched.
        /// </summary>
        public Case(string type, Func<byte[], JValue> swap) : base(
            AsPair._<string, ISwap<byte[], JValue>>(type, AsSwap._(swap))
        )
        { }

        /// <summary>
        /// Use with swapswitch to swap if given type is matched.
        /// </summary>
        public static IPair<string, ISwap<byte[], JValue>> _(
            string type, Func<byte[], JValue> swap
        ) =>
            new Case(type, swap);
    }
}

