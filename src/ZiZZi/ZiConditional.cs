

using System;

namespace ZiZZi
{
    /// <summary>
    /// Forms matter only if the given condition is matched.
    /// Otherwise does nothing.
    /// </summary>
    public sealed class ZiConditional : BloxEnvelope
    {
        /// <summary>
        /// Forms matter only if the given condition is matched.
        /// Otherwise does nothing.
        /// </summary>
        public ZiConditional(bool condition, Func<IBlox> consequence) : this(() => condition, consequence)
        { }

        /// <summary>
        /// Forms matter only if the given condition is matched.
        /// Otherwise does nothing.
        /// </summary>
        public ZiConditional(Func<bool> condition, Func<IBlox> consequence) : base(() =>
        {
            IBlox result = new ZiNothing();
            if (condition())
            {
                result = consequence();
            }
            return result;
        })
        { }
    }
}
