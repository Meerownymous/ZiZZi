//MIT License

//Copyright (c) 2022 ICARUS Consulting GmbH

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
using Yaapii.Atoms.Enumerable;

namespace BriX
{
    /// <summary>
    /// A list of blocks.
    /// </summary>
    public sealed class BxBlockArray : IBrix
    {
        private readonly string name;
        private readonly string itemName;
        private readonly IEnumerable<IBrix> contents;

        /// <summary>
        /// A list of blocks.
        /// </summary>
        public BxBlockArray(string name, string itemName, params IBrix[] contents) : this(
            name, itemName,
            new ManyOf<IBrix>(contents)
        )
        { }

        /// <summary>
        /// A list of blocks.
        /// </summary>
        public BxBlockArray(string name, string itemName, IEnumerable<IBrix> contents)
        {
            this.name = name;
            this.itemName = itemName;
            this.contents = contents;
        }

        public T Print<T>(IMedia<T> media)
        {
            var array = media.Array(this.name, this.itemName);
            foreach (var block in this.contents)
            {
                block.Print(array);
            }
            return media.Content();
        }

        public override string ToString()
        {
            return $"ARRAY:COMPLEX({this.name}/{this.itemName})";
        }
    }
}
