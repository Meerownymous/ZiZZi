using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Tonga;
using Tonga.Swap;

namespace ZiZZi.Matter.XML
{
    /// <summary>
    /// Delivers matter of given contentype  which is rooted in the given XNode.
    /// </summary>
    public sealed class MatterOrigin : ISwap<string, XNode, string, IMatter<XNode>>
    {
        private readonly ISwap<string, XNode, string, IMatter<XNode>> matterOrigin;

        /// <summary>
        /// Delivers matter of given contentype  which is rooted in the given JToken.
        /// </summary>
        public MatterOrigin(BytesAsXElement bytesAsElement)
        {
            this.matterOrigin =
                SwapSwitch._(
                    "block",
                    AsSwap._<XNode, string, IMatter<XNode>>((parent, name) =>
                        new XmlBlock(this, bytesAsElement, parent, name)
                    ),
                    "value-list",
                    AsSwap._<XNode, string, IMatter<XNode>>((parent, name) =>
                        new ListGuard<XNode>(
                            new XmlValueList(bytesAsElement, parent, name),
                            name,
                            true
                        )
                    ),
                    "block-list",
                    AsSwap._<XNode, string, IMatter<XNode>>((parent, name) =>
                        new ListGuard<XNode>(
                            new XmlBlockList(this, parent, name),
                            name,
                            false
                        )
                    ),
                    "block-inside-list",
                    AsSwap._<XNode, string, IMatter<XNode>>((parent, name) =>
                        new XmlBlock(this, bytesAsElement, parent, name)
                    )
                );
        }

        public IMatter<XNode> Flip(string contentType, XNode parent, string name)
        {
            return this.matterOrigin.Flip(contentType, parent, name);
        }
    }
}