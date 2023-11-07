using System;
using Tonga.Scalar;

namespace ZiZZi.Matter.Content
{
    /// <summary>
    /// Bytes as long.
    /// </summary>
    public sealed class AsLong : ScalarEnvelope<long>
    {
        /// <summary>
        /// Bytes as long.
        /// </summary>
        public AsLong(byte[] data) : base(() =>
            BitConverter.ToInt64(data, 0)
        )
        { }

        public static AsLong _(byte[] data) => new AsLong(data);
    }
}

