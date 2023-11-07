


using System;
using ZiZZi.Matter;

namespace ZiZZi.Fake
{
    /// <summary>
    /// Fake brix.
    /// </summary>
    public sealed class FkBrix : IBrix
    {
        private readonly Action print;

        /// <summary>
        /// Fake brix.
        /// </summary>
        public FkBrix(): this(() => { })
        { }

        /// <summary>
        /// Fake brix.
        /// </summary>
        public FkBrix(Action print)
        {
            this.print = print;
        }

        public TMedia Print<TMedia>(IMedia<TMedia> media)
        {
            this.print();
            return media.Content();
        }
    }
}
