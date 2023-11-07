using System;

namespace ZiZZi
{
    /// <summary>
    /// Envelope for brix.
    /// </summary>
    public abstract class BrixEnvelope : IBrix
    {
        private readonly Func<IBrix> brix;

        /// <summary>
        /// Envelope for brix.
        /// </summary>
        public BrixEnvelope(Func<IBrix> brix)
        {
            this.brix = brix;
        }

        public T Print<T>(IMedia<T> media)
        {
            return this.brix().Print(media);
        }
    }
}
