using ZiZZi.Matter.Content;
using Tonga;
using Tonga.Enumerable;
using Tonga.Swap;
using Tonga.Text;
using System.Xml.Linq;
using System.Diagnostics;

namespace ZiZZi.Matter.XML
{
    /// <summary>
    /// Makes XContainer content from name and bytes to print into xml.
    /// </summary>
    public sealed class BytesAsXElement : ISwap<string, string, byte[], XNode>
    {
        private readonly SwapSwitch<string, byte[], XElement> swap;

        /// <summary>
        /// Makes XContainer content from bytes to form xml.
        /// </summary>
        public BytesAsXElement()
        {
            this.swap =
                SwapSwitch._(
                    AsEnumerable._(
                        Case._("double", (name, data) => new XElement(name, AsDouble._(data).Value())),
                        Case._("integer", (name, data) => new XElement(name, AsInteger._(data).Value())),
                        Case._("long", (name, data) => new XElement(name, AsLong._(data).Value())),
                        Case._("bool", (name, data) => new XElement(name, AsBool._(data).Value())),
                        Case._("string", (name, data) => new XElement(name, AsText._(data).AsString()))
                    )
                );
        }

        public XNode Flip(string contentType, string name, byte[] input) =>
            this.swap.Flip(contentType, name, input);
    }
}

