using System;
using Tonga;
using Tonga.Scalar;

namespace BLox.Shape.Content
{
    /// <summary>
    /// Bytes as double.
    /// </summary>
    public sealed class AsDouble : ScalarEnvelope<double>
    {
        /// <summary>
        /// Bytes as double.
        /// </summary>
        public AsDouble(byte[] data) : base(() =>
            BitConverter.ToDouble(data, 0)
        )
        { }

        public static AsDouble _(byte[] data) => new AsDouble(data);
    }
}

