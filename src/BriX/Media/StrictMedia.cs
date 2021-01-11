//MIT License

//Copyright (c) 2021 ICARUS Consulting GmbH

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

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

#pragma warning disable IDE0060 // Remove unused parameter

namespace BriX.Media
{
    /// <summary>
    /// A media in JSON format.
    /// </summary>
    public sealed class StrictMedia<T> : IMedia<T>
    {
        private readonly IMedia<T> origin;
        private readonly bool isRoot;
        private readonly string arrayItemName;
        private readonly string type;
        private readonly IList<string> existingNames;

        /// <summary>
        /// A media in JSON format.
        /// </summary>
        public StrictMedia(IMedia<T> origin) : this(origin, string.Empty, string.Empty, true)
        { }

        /// <summary>
        /// A media in JSON format.
        /// </summary>
        private StrictMedia(IMedia<T> origin, string arrayItemName, string type, bool isRoot = false)
        {
            this.origin = origin;
            this.arrayItemName = arrayItemName;
            this.type = type;
            this.isRoot = isRoot;
            this.existingNames = new List<string>();
        }

        public IMedia<T> Array(string arrayName, string itemName) //array- and itemName is not needed in json
        {
            IMedia<T> result;
            RejectDuplicates(arrayName);
            if (this.isRoot)
            {
                RejectEmpty("array", arrayName);
                RejectDuplicateRoot();
                result = this.origin.Array(arrayName, itemName);
            }
            else
            {
                if (Is("prop"))
                {
                    throw new InvalidOperationException($"You cannot put array '{arrayName}/{itemName}' into a prop. You can create an array as root or inside a block.");
                }
                result = this.origin.Array(arrayName, itemName);
            }
            this.existingNames.Add(arrayName.ToLower());
            return new StrictMedia<T>(result, itemName, "array", false);
        }

        public IMedia<T> Block(string name)
        {
            IMedia<T> result;
            if (this.isRoot)
            {
                RejectEmpty("block", name);
                RejectDuplicateRoot();
                result = this.origin.Block(name);
            }
            else
            {
                if (Is("prop"))
                {
                    RejectEmpty("block", name);
                    RejectDuplicates(name);
                    result = this.origin.Block(name);
                }
                else if (Is("array"))
                {
                    if (name != string.Empty && this.arrayItemName.Length > 0 && !name.Equals(this.arrayItemName, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException($"You are putting block '{name}' into a list - but you gave it another name as you specified for the list items: '{this.arrayItemName}'. Blocks which are put into lists must have the same name.");
                    }
                    result = this.origin.Block(name);
                }
                else if (Is("object"))
                {
                    RejectEmpty("block", name);
                    RejectDuplicates(name);
                    result = this.origin.Block(name);
                }
                else
                {
                    throw new ApplicationException($"Unknown media type '{this.type}'");
                }
            }
            this.existingNames.Add(name.ToLower());
            return new StrictMedia<T>(result, string.Empty, "block", false);
        }

        public T Content()
        {
            return this.origin.Content();
        }

        public IMedia<T> Prop(string name)
        {
            if (isRoot)
            {
                throw new InvalidOperationException($"You cannot put prop '{name}' directly into a media. You must start with a block or an array.");
            }
            RejectDuplicates(name);
            var result = this.origin.Prop(name);
            this.existingNames.Add(name.ToLower());
            return new StrictMedia<T>(result, string.Empty, "prop");
        }

        public IMedia<T> Put(string value)
        {
            if (Is("object"))
            {
                throw new InvalidOperationException($"You cannot put value '{value}' directly into a block.");
            }
            this.origin.Put(value);
            return this;
        }

        private bool Is(string type)
        {
            return this.type == type;
        }

        private void RejectDuplicates(string name)
        {
            if (this.existingNames.Contains(name.ToLower()))
            {
                throw new InvalidOperationException($"Cannot add '{name}' because it already exists.");
            }
        }

        private void RejectDuplicateRoot()
        {
            if (this.existingNames.Count > 0)
            {
                throw new InvalidOperationException($"You cannot put two blocks/arrays into the root, and this already has one.");
            }
        }

        private void RejectEmpty(string type, string name)
        {
            if (name == string.Empty)
            {
                throw new InvalidOperationException($"You are trying to make a {type} without a name into a property or an object. Blocks without a name can only be put into arrays.");
            }
        }
    }
}
