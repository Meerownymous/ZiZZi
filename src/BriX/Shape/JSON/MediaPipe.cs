using Newtonsoft.Json.Linq;
using Tonga;
using Tonga.Swap;

namespace BLox.Shape.JSON
{
    /// <summary>
    /// Maps contentype to media rooted in the given JToken.
    /// </summary>
    public sealed class MediaPipe : ISwap<string, JContainer, string, IShape<JContainer>> 
    {
        private readonly ISwap<string, JContainer, string, IShape<JContainer>> swap;

        /// <summary>
        /// Maps contentype to media rooted in the given JToken.
        /// </summary>
        public MediaPipe(BytePipe content)
        {
            this.swap =
                SwapSwitch._(
                    "block",
                    AsSwap._<JContainer, string, IShape<JContainer>>((parent, name) =>
                        new JsonBlock(this, content, parent, name)
                    ),
                    "array",
                    AsSwap._<JContainer, string, IShape<JContainer>>((parent, name) =>
                        new JsonBlock(this, content, parent, name)
                    )
                );
        }

        public IShape<JContainer> Flip(string contentType, JContainer parent, string name)
        {
            return this.swap.Flip(contentType, parent, name);
        }
    }
}