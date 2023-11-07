

using System;
using System.Xml.Linq;
using Yaapii.Atoms.Bytes;
using Yaapii.Atoms.Error;
using Yaapii.Atoms.List;

namespace ZiZZi.Matter
{
    /// <summary>
    /// A media which can be used to rebuild a brix.
    /// </summary>
    public sealed class RebuildMedia : IMedia<XNode>
    {
        private readonly XContainer[] node;
        private readonly string arrayItemName;
        private readonly bool isRoot;
        private readonly string[] brixType;

        /// <summary>
        /// A media which can be used to rebuild a brix.
        /// </summary>
        public RebuildMedia() : this(new XDocument(), "block", string.Empty, true)
        { }

        /// <summary>
        /// A media which can be used to rebuild a brix.
        /// </summary>
        private RebuildMedia(XContainer node, string brixType, string arrayItemName, bool isRoot = false)
        {
            this.node = new XContainer[1] { node };
            this.brixType = new string[1] { brixType };
            this.arrayItemName = arrayItemName;
            this.isRoot = isRoot;
        }

        /// <summary>
        /// A media which can be used to rebuild a brix.
        /// </summary>
        public IMedia<XNode> Array(string arrayName, string itemName)
        {
            var array = new XElement(arrayName);
            array.SetAttributeValue("bx-type", "array");
            array.SetAttributeValue("bx-array-item-name", itemName);
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
            return new RebuildMedia(array, "array", itemName);
        }

        /// <summary>
        /// A media which can be used to rebuild a brix.
        /// </summary>
        public IMedia<XNode> Block(string name)
        {
            var block = new XElement("bootstrap");
            block.SetAttributeValue("bx-type", "block");
            if (this.isRoot)
            {
                RejectDuplicateRoot();
                RejectEmpty("block", name);
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
            return new RebuildMedia(block, "block", String.Empty);
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
            prop.SetAttributeValue("bx-type", "prop");

            if (Is("block"))
            {
                this.node[0].Add(prop);
            }
            else if (Is("array"))
            {
                throw new InvalidOperationException($"You cannot put prop '{name}' into an array. Props can only exist in blocks.");
            }
            return new RebuildMedia(prop, "prop", string.Empty, false);
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
