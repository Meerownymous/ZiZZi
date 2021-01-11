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

using System;
using System.Xml.Linq;
using Yaapii.Atoms.List;

namespace BriX.Media
{
    /// <summary>
    /// A media in XML format.
    /// </summary>
    public sealed class XmlMedia : IMedia<XNode>
    {
        private readonly XContainer[] node;
        private readonly string arrayItemName;
        private readonly bool isRoot;
        private readonly string[] brixType;

        /// <summary>
        /// A media in XML format.
        /// </summary>
        public XmlMedia() : this(new XDocument(), "block", string.Empty, true)
        { }

        /// <summary>
        /// A media in XML format.
        /// </summary>
        private XmlMedia(XContainer node, string brixType, string arrayItemName, bool isRoot = false)
        {
            this.node = new XContainer[1] { node };
            this.brixType = new string[1] { brixType };
            this.arrayItemName = arrayItemName;
            this.isRoot = isRoot;
        }

        /// <summary>
        /// A media in XML format.
        /// </summary>
        public IMedia<XNode> Array(string arrayName, string itemName)
        {
            var array = new XElement(arrayName);
            RejectDuplicates(arrayName);
            if (this.isRoot)
            {
                RejectEmpty("array", arrayName);
                RejectDuplicateRoot();
                this.brixType[0] = "array";
                this.Node().Add(array);
            }
            else
            {
                if (Is("prop"))
                {
                    throw new InvalidOperationException($"You cannot put array '{arrayName}/{arrayItemName}' into a prop. You can create an array as root or inside a block.");
                }
                else if (Is("array"))
                {
                    this.Node().Add(array);
                }
                else if (Is("block"))
                {
                    this.Node().Add(array);
                }
            }
            return new XmlMedia(array, "array", itemName);
        }

        /// <summary>
        /// A media in XML format.
        /// </summary>
        public IMedia<XNode> Block(string name)
        {
            var block = new XElement("bootstrap");
            if (this.isRoot)
            {
                RejectDuplicateRoot();
                block.Name = name;
                this.Node().Add(block);
            }
            else
            {
                if (Is("prop"))
                {
                    RejectEmpty("block", name);
                    RejectDuplicates(name);
                    block.Name = name;
                    this.Node().Add(block);
                }
                else if (Is("array"))
                {
                    if (name != String.Empty && this.arrayItemName.Length > 0 && !name.Equals(this.arrayItemName, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException($"You are putting block '{name}' into a list - but you gave it another name as you specified for the list items: '{this.arrayItemName}'. Blocks which are put into lists must have the same name.");
                    }
                    block.Name = this.arrayItemName;
                    this.Node().Add(block);
                }
                else
                {
                    RejectEmpty("block", name);
                    RejectDuplicates(name);
                    block.Name = name;
                    this.Node().Add(block);
                }
            }
            return new XmlMedia(block, "block", String.Empty);
        }

        public XNode Content()
        {
            return this.node[0] as XNode;
        }

        public IMedia<XNode> Prop(string name)
        {
            if (isRoot)
            {
                throw new InvalidOperationException($"You cannot put prop '{name}' directly into a media. You must start with a block or an array.");
            }

            RejectDuplicates(name);

            var prop = new XElement(name);

            if (Is("block"))
            {
                this.node[0].Add(prop);
            }
            else if (Is("array"))
            {
                throw new InvalidOperationException($"You cannot put prop '{name}' into an array. Props can only exist in blocks.");
            }
            return new XmlMedia(prop, "prop", string.Empty, false);
        }

        public IMedia<XNode> Put(string value)
        {
            if (Is("array"))
            {
                (this.Node() as XElement).Add(new XElement(this.arrayItemName, value));
            }
            else if (Is("prop"))
            {
                (this.Node() as XElement).Value = value;
            }
            else if (Is("block"))
            {
                throw new InvalidOperationException($"You cannot put value '{value}' directly into a block.");
            }
            return this;
        }

        private XContainer Node()
        {
            return this.node[0];
        }

        private bool Is(string brixType)
        {
            return this.brixType[0] == brixType;
        }

        private void RejectDuplicates(string name)
        {
            var existing =
                new ListOf<string>(
                    new Yaapii.Atoms.Enumerable.Mapped<XElement, string>(
                        elem => elem.Name.LocalName.ToLower(),
                        this.node[0].Elements()
                    )
                );

            if (
                existing.Contains(name.ToLower())
            )
            {
                throw new InvalidOperationException($"Cannot add '{name}' because it already exists.");
            }
        }

        private void RejectDuplicateRoot()
        {
            if (this.Node().Elements().GetEnumerator().MoveNext())
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
