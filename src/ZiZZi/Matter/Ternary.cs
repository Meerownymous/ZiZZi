using System;
namespace ZiZZi.Matter
{
    /// <summary>
    /// Ternary matter based on condition.
    /// </summary>
    public sealed class Ternary<T> : MatterEnvelope<T>
    {
        /// <summary>
        /// Ternary matter based on condition.
        /// </summary>
        public Ternary(Func<bool> condition, Func<IMatter<T>> yes, Func<IMatter<T>> no) : base(() =>
            condition() ? yes() : no()
        )
        { }
    }
}

