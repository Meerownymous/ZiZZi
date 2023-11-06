using System;
using Tonga.Scalar;

namespace BLox.Shape.Content
{
    /// <summary>
    /// Bytes as bool.
    /// </summary>
    public sealed class AsBool : ScalarEnvelope<bool>
    {
        /// <summary>
        /// Bytes as bool.
        /// </summary>
        public AsBool(byte[] data) : base(() =>
            BitConverter.ToBoolean(data, 0)
        )
        { }

        public static AsBool _(byte[] data) => new AsBool(data);
    }
}

