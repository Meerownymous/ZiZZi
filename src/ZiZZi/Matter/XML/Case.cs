using System;
using System.Xml.Linq;
using Tonga;
using Tonga.Map;
using Tonga.Swap;

namespace ZiZZi.Matter.XML
{
    /// <summary>
    /// Use with swapswitch to swap if given type is matched.
    /// </summary>
    public class Case : PairEnvelope<string, ISwap<string, byte[], XElement>>
    {
        /// <summary>
        /// Use with swapswitch to swap if given type is matched.
        /// </summary>
        public Case(string type, Func<string, byte[], XElement> swap) : base(
            AsPair._<string, ISwap<string, byte[], XElement>>(type, AsSwap._(swap))
        )
        { }

        /// <summary>
        /// Use with swapswitch to swap if given type is matched.
        /// </summary>
        public static IPair<string, ISwap<string, byte[], XElement>> _(
            string type, Func<string, byte[], XElement> swap
        ) =>
            new Case(type, swap);
    }
}

