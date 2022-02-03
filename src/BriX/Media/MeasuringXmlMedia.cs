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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Map;

namespace BriX.Media
{
    /// <summary>
    /// A media in XML format.
    /// </summary>
    public sealed class MeasuringXmlMedia : IMedia<XNode>
    {
        private readonly bool isRootMedia;
        private readonly IDictionary<string, XNode> nodes;
        private readonly IDictionary<string, string> attributes;
        private readonly IDictionary<string, Stopwatch> stopWatches;

        /// <summary>
        /// A media in XML format.
        /// </summary>
        public MeasuringXmlMedia() : this(new XDocument(), "block", string.Empty, new Stopwatch(), true)
        { }

        /// <summary>
        /// A media in XML format.
        /// </summary>
        private MeasuringXmlMedia(XContainer node, string brixType, string arrayItemName, Stopwatch stopWatch, bool isRoot = false)
        {
            this.isRootMedia = isRoot;
            this.stopWatches =
                new Dictionary<string, Stopwatch>(
                    new MapOf<string, Stopwatch>(
                        new KvpOf<string, Stopwatch>("my", stopWatch),
                        new KvpOf<string, Stopwatch>("sub", new Stopwatch())
                    )
                );

            this.nodes =
                new Dictionary<string, XNode>(
                    new MapOf<string, XNode>(
                        new KvpOf<string, XNode>("my", node),
                        new KvpOf<string, XNode>("sub", node)
                    )
                );
            this.attributes =
                new Dictionary<string, string>(
                    new MapOf(
                        "array-item-name", arrayItemName,
                        "brix-type", brixType
                    )
                ); ;
        }

        /// <summary>
        /// A media in XML format.
        /// </summary>
        public IMedia<XNode> Array(string arrayName, string itemName)
        {
            EnsureRootMeasurement();
            var array = new XElement(arrayName);
            SaveSubMeasurement();
            PrepareSubMeasurement(array);
            RejectDuplicates(arrayName);
            if (this.isRootMedia)
            {
                RejectEmpty("array", arrayName);
                RejectDuplicateRoot();
                this.attributes["brix-type"] = "array";
                this.Node().Add(array);
            }
            else
            {
                if (Is("prop"))
                {
                    throw new InvalidOperationException($"You cannot put array '{arrayName}/{ArrayItemName()}' into a prop. You can create an array as root or inside a block.");
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

            return new MeasuringXmlMedia(array, "array", itemName, StartSubMeasurement());
        }

        /// <summary>
        /// A media in XML format.
        /// </summary>
        public IMedia<XNode> Block(string name)
        {
            EnsureRootMeasurement();
            var block = new XElement("bootstrap");
            SaveSubMeasurement();
            PrepareSubMeasurement(block);
            if (this.isRootMedia)
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
                    var arrayItemName = ArrayItemName();
                    if (name != String.Empty && arrayItemName.Length > 0 && !name.Equals(arrayItemName, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException($"You are putting block '{name}' into a list - but you gave it another name as you specified for the list items: '{ArrayItemName()}'. Blocks which are put into lists must have the same name.");
                    }
                    block.Name = ArrayItemName();
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
            return new MeasuringXmlMedia(block, "block", String.Empty, StartSubMeasurement());
        }

        public XNode Content()
        {
            if (!this.IsRootMeasurement())
            {
                SaveSubMeasurement();
            }
            else
            {
                EndRootMeasurement();
            }
            return this.nodes["my"];
        }

        public IMedia<XNode> Prop(string name)
        {
            EnsureRootMeasurement();
            if (this.isRootMedia)
            {
                throw new InvalidOperationException($"You cannot put prop '{name}' directly into a media. You must start with a block or an array.");
            }
            RejectDuplicates(name);

            var prop = new XElement(name);

            SaveSubMeasurement();
            PrepareSubMeasurement(prop);

            if (Is("block"))
            {
                (this.nodes["my"] as XContainer).Add(prop);
            }
            else if (Is("array"))
            {
                throw new InvalidOperationException($"You cannot put prop '{name}' into an array. Props can only exist in blocks.");
            }
            return new MeasuringXmlMedia(prop, "prop", string.Empty, StartSubMeasurement());
        }

        public IMedia<XNode> Put(string value)
        {
            if (Is("array"))
            {
                (this.Node() as XElement).Add(new XElement(ArrayItemName(), value));
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
            return this.nodes["my"] as XContainer;
        }

        private void RejectDuplicates(string name)
        {
            var existing =
                new ListOf<string>(
                    new Yaapii.Atoms.Enumerable.Mapped<XElement, string>(
                        elem => elem.Name.LocalName.ToLower(),
                        (this.nodes["my"] as XContainer).Elements()
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

        private bool IsRootMeasurement()
        {
            return this.nodes["my"] == this.nodes["my"].Document.Root;
        }

        private bool Is(string brixType)
        {
            return this.attributes["brix-type"] == brixType;
        }

        private string ArrayItemName()
        {
            return this.attributes["array-item-name"];
        }

        private void EnsureRootMeasurement()
        {
            if (this.IsRootMeasurement())
            {
                this.stopWatches["my"].Start();
            }
        }

        private void PrepareSubMeasurement(XNode xnode)
        {
            if (xnode.NodeType == System.Xml.XmlNodeType.Element)
            {
                (xnode as XElement).SetAttributeValue("time", "0");
            }
            this.nodes["sub"] = xnode;
        }

        private void SaveSubMeasurement()
        {
            if (this.nodes["sub"] != this.nodes["my"])
            {
                this.stopWatches["sub"].Stop();

                (this.nodes["sub"] as XElement).SetAttributeValue("time", this.stopWatches["sub"].ElapsedMilliseconds);
            }
        }

        private Stopwatch StartSubMeasurement()
        {
            var newStopwatch = new Stopwatch();
            newStopwatch.Start();
            this.stopWatches["sub"] = newStopwatch;
            return this.stopWatches["sub"];
        }

        private void EndRootMeasurement()
        {
            this.nodes["my"].Document.Root.SetAttributeValue("time", this.stopWatches["my"].ElapsedMilliseconds);
        }
    }
}
