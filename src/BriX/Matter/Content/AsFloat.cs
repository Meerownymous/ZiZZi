using System;
using Tonga;
using Tonga.Scalar;

namespace ZiZZi.Matter.Content
{
    /// <summary>
    /// Bytes as float.
    /// </summary>
    public sealed class AsFloat : ScalarEnvelope<float>
    {
        /// <summary>
        /// Bytes as float.
        /// </summary>
        public AsFloat(byte[] data) : base(() =>
            BitConverter.ToSingle(data, 0)
        )
        { }

        public static AsFloat _(byte[] data) => new AsFloat(data);
    }
}

