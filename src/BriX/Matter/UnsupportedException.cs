using System;
using System.Collections.Generic;
using Tonga.Text;

namespace ZiZZi.Matter
{
    /// <summary>
    /// Messages which items are supported out of given list.
    /// </summary>
    public sealed class UnsupportedException : ArgumentException
    {
        /// <summary>
        /// Messages which items are supported out of given list.
        /// </summary>
        public UnsupportedException(string unsupported, IEnumerable<string> supported) : base(
            $"{unsupported} is an unsupported content type. Supported are: {Joined._(",", supported)}."
        )
        { }
    }
}

