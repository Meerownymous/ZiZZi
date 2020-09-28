//MIT License

//Copyright (c) 2020 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections.Generic;
using System.Globalization;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
using BriX.Media;

namespace BriX
{
    /// <summary>
    /// A brix map (list of <see cref="BxProp"/>).
    /// </summary>
    public sealed class BxMap : BrixEnvelope
    {
        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public BxMap(params string[] keyValuePairs) : this(
            new MapOf(keyValuePairs)
        )
        { }

        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public BxMap(IKvp entry, params IKvp[] entries) : this(
            new MapOf(entry, entries)
        )
        { }

        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public BxMap(IDictionary<string, double> entries) : this(
            new MapOf(
                new Mapped<string, IKvp>(
                    key => new KvpOf(key, entries[key].ToString(CultureInfo.InvariantCulture)),
                    entries.Keys
                )
            )
        )
        { }

        /// <summary>
        /// A brix map (list of <see cref="BxProp"/>).
        /// </summary>
        public BxMap(IDictionary<string, string> entries) : base(() =>
            new BxChain(
                new Mapped<string, IBrix>(
                    (key) => new BxProp(key, entries[key]),
                    entries.Keys
                )
            )
        )
        { }
    }
}
