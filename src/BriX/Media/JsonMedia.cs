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

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BriX.Media
{
    /// <summary>
    /// A media in JSON format.
    /// </summary>
    public sealed class JsonMedia : IMedia<JToken>
    {
        private readonly JToken[] token;
        private readonly bool isRoot;
        private readonly string arrayItemName;
        private readonly IList<string> existingNames;

        /// <summary>
        /// A media in JSON format.
        /// </summary>
        public JsonMedia() : this(new JObject(), string.Empty, true)
        { }

        /// <summary>
        /// A media in JSON format.
        /// </summary>
        private JsonMedia(JToken token, string arrayItemName, bool isRoot = false)
        {
            this.token = new JToken[1] { token };
            this.arrayItemName = arrayItemName;
            this.isRoot = isRoot;
            this.existingNames = new List<string>();
        }

        public IMedia<JToken> Array(string arrayName, string itemName) //array- and itemName is not needed in json
        {
            var array = new JArray();
            RejectDuplicates(arrayName);
            if (this.isRoot)
            {
                RejectEmpty("array", arrayName);
                RejectDuplicateRoot();
                this.token[0] = array;
            }
            else
            {
                var token = this.Token();
                if (Is(JTokenType.Property))
                {
                    throw new InvalidOperationException($"You cannot put array '{arrayName}/{itemName}' into a prop. You can create an array as root or inside a block.");
                }
                else if (Is(JTokenType.Array))
                {
                    (token as JArray).Add(array);
                }
                else if (Is(JTokenType.Object))
                {
                    var prop = new JProperty(arrayName, array);
                    (token as JObject).Add(prop);
                }
            }
            this.existingNames.Add(arrayName.ToLower());
            return new JsonMedia(array, itemName);
        }

        public IMedia<JToken> Block(string name)
        {
            var obj = new JObject();
            if (this.isRoot)
            {
                RejectEmpty("block", name);
                RejectDuplicateRoot();
                obj = (this.Token() as JObject);
            }
            else
            {
                if (Is(JTokenType.Property))
                {
                    RejectEmpty("block", name);
                    RejectDuplicates(name);
                    (this.Token() as JProperty).Value = obj;
                }
                else if (Is(JTokenType.Array))
                {
                    if (name != string.Empty && this.arrayItemName.Length > 0 && !name.Equals(this.arrayItemName, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException($"You are putting block '{name}' into a list - but you gave it another name as you specified for the list items: '{this.arrayItemName}'. Blocks which are put into lists must have the same name.");
                    }
                    (this.Token() as JArray).Add(obj);
                }
                else if (Is(JTokenType.Object))
                {
                    RejectEmpty("block", name);
                    RejectDuplicates(name);
                    (this.Token() as JObject).Add(new JProperty(name, obj));
                }
            }
            this.existingNames.Add(name.ToLower());
            return new JsonMedia(obj, string.Empty);
        }

        public JToken Content()
        {
            return this.Token();
        }

        public IMedia<JToken> Prop(string name)
        {
            if (isRoot)
            {
                throw new InvalidOperationException($"You cannot put prop '{name}' directly into a media. You must start with a block or an array.");
            }
            RejectDuplicates(name);

            var prop = new JProperty(name, string.Empty);
            if (Is(JTokenType.Object))
            {
                (this.Token() as JObject).Add(prop);
            }
            else if (Is(JTokenType.Array))
            {
                throw new InvalidOperationException($"You cannot put prop '{name}' into an array. Props can only exist in blocks.");
            }
            this.existingNames.Add(name.ToLower());
            return new JsonMedia(prop, string.Empty);
        }

        public IMedia<JToken> Put(string value)
        {
            if (Is(JTokenType.Array))
            {
                (this.Token() as JArray).Add(value);
            }
            else if (Is(JTokenType.Property))
            {
                (this.Token() as JProperty).Value = value;
            }
            else if (Is(JTokenType.Object))
            {
                throw new InvalidOperationException($"You cannot put value '{value}' directly into a block.");
            }
            return this;
        }

        private JToken Token()
        {
            return this.token[0];
        }

        private bool Is(JTokenType type)
        {
            return this.Token().Type == type;
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
