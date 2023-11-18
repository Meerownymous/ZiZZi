using System.Xml.Linq;
using Tonga;
using Tonga.Swap;

namespace ZiZZi.Matter.Dynamic
{
    /// <summary>
    /// Delivers matter of given contentype  which is rooted in the given dynamic object.
    /// </summary>
    public sealed class MatterOrigin : ISwap<string, object, string, IMatter<object>>
    {
        private readonly ISwap<string, dynamic, string, IMatter<object>> matterOrigin;

        /// <summary>
        /// Delivers matter of given contentype  which is rooted in the given dynamic object.
        /// </summary>
        public MatterOrigin(BytesAsTyped bytesAsTyped)
        {
            this.matterOrigin =
                SwapSwitch._<string, dynamic, string, IMatter<object>>(
                    "block",
                    AsSwap._<object, string, IMatter<object>>((parent, name) =>
                        new DynBlock(this, bytesAsTyped, parent, name, false)
                    ),
                    "value-list",
                    AsSwap._<object, string, IMatter<object>>((parent, name) =>
                        new ListGuard<object>(
                            new DynValueList(bytesAsTyped, parent, name),
                            name,
                            true
                        )
                    ),
                    "block-list",
                    AsSwap._<object, string, IMatter<object>>((parent, name) =>
                        new ListGuard<object>(
                            new DynBlockList(this, parent, name),
                            name,
                            false
                        )
                    ),
                    "block-inside-list",
                    AsSwap._<dynamic, string, IMatter<dynamic>>((parent, name) =>
                        new DynBlock(this, bytesAsTyped, parent, name, true)
                    )
                );
        }

        public IMatter<dynamic> Flip(string contentType, dynamic parent, string name)
        {
            return this.matterOrigin.Flip(contentType, parent, name);
        }
    }
}