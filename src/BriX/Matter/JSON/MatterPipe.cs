using Newtonsoft.Json.Linq;
using Tonga;
using Tonga.Swap;

namespace ZiZZi.Matter.JSON
{
    /// <summary>
    /// Maps contentype to matter which is rooted in the given JToken.
    /// </summary>
    public sealed class MatterPipe : ISwap<string, JContainer, string, IMatter<JContainer>>
    {
        private readonly ISwap<string, JContainer, string, IMatter<JContainer>> swap;

        /// <summary>
        /// Maps contentype to matter which is rooted in the given JToken.
        /// </summary>
        public MatterPipe(BytesAsToken bytesAsToken)
        {
            this.swap =
                SwapSwitch._(
                    "block",
                    AsSwap._<JContainer, string, IMatter<JContainer>>((parent, name) =>
                        new JsonBlock(this, bytesAsToken, parent, name)
                    ),
                    "value-array",
                    AsSwap._<JContainer, string, IMatter<JContainer>>((parent, name) =>
                        new ArrayGuard<JContainer>(
                            new JsonValueArray(bytesAsToken, parent, name),
                            name,
                            true
                        )
                    ),
                    "block-array",
                    AsSwap._<JContainer, string, IMatter<JContainer>>((parent, name) =>
                        new ArrayGuard<JContainer>(
                            new JsonBlockArray(this, bytesAsToken, parent, name),
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