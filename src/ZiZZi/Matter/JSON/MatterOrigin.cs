using Newtonsoft.Json.Linq;
using Tonga;
using Tonga.Swap;

namespace ZiZZi.Matter.JSON
{
    /// <summary>
    /// Delivers matter of given contentype  which is rooted in the given JToken.
    /// </summary>
    public sealed class MatterOrigin : ISwap<string, JContainer, string, IMatter<JContainer>>
    {
        private readonly ISwap<string, JContainer, string, IMatter<JContainer>> swap;

        /// <summary>
        /// Delivers matter of given contentype  which is rooted in the given JToken.
        /// </summary>
        public MatterOrigin(BytesAsToken bytesAsToken)
        {
            this.swap =
                SwapSwitch._(
                    "block",
                    AsSwap._<JContainer, string, IMatter<JContainer>>((parent, name) =>
                        new JsonBlock(this, bytesAsToken, parent, name)
                    ),
                    "value-array",
                    AsSwap._<JContainer, string, IMatter<JContainer>>((parent, name) =>
                        new ListGuard<JContainer>(
                            new JsonValueArray(bytesAsToken, parent, name),
                            name,
                            true
                        )
                    ),
                    "block-array",
                    AsSwap._<JContainer, string, IMatter<JContainer>>((parent, name) =>
                        new ListGuard<JContainer>(
                            new JsonBlockArray(this, parent, name),
                            name,
                            false
                        )
                    )
                );
        }

        public IMatter<JContainer> Flip(string contentType, JContainer parent, string name)
        {
            return this.swap.Flip(contentType, parent, name);
        }
    }
}