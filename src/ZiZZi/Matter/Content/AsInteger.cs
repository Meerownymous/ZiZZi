using System;
using Tonga;
using Tonga.Scalar;

namespace ZiZZi.Matter.Content
{
    /// <summary>
    /// Bytes as double.
    /// </summary>
    public sealed class AsInteger : ScalarEnvelope<int>
    {
        /// <summary>
        /// Bytes as double.
        /// </summary>
        public AsInteger(byte[] data) : base(() =>
            BitConverter.ToInt32(data, 0)
        )
        { }

        public static AsInteger _(byte[] data) => new AsInteger(data);
    }
}

