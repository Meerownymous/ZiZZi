using System;

namespace ZiZZi
{
    /// <summary>
    /// Envelope for blox.
    /// </summary>
    public abstract class BloxEnvelope : IBlox
    {
        private readonly Func<IBlox> blox;

        /// <summary>
        /// Envelope for blox.
        /// </summary>
        public BloxEnvelope(Func<IBlox> blox)
        {
            this.blox = blox;
        }

        public T Form<T>(IMatter<T> matter)
            where T : class
        {
            return this.blox().Form(matter);
        }
    }
}
