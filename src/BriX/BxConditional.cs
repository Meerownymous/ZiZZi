

using System;

namespace BLox
{
    /// <summary>
    /// Prints only if the given condition is matched.
    /// Otherwise does nothing.
    /// </summary>
    public sealed class BxConditional : BrixEnvelope
    {
        /// <summary>
        /// Prints only if the given condition is matched.
        /// Otherwise does nothing.
        /// </summary>
        public BxConditional(bool condition, Func<IBrix> consequence) : this(() => condition, consequence)
        { }

        /// <summary>
        /// Prints only if the given condition is matched.
        /// Otherwise does nothing.
        /// </summary>
        public BxConditional(Func<bool> condition, Func<IBrix> consequence) : base(() =>
        {
            IBrix result = new BxNothing();
            if (condition())
            {
                result = consequence();
            }
            return result;
        })
        { }
    }
}
