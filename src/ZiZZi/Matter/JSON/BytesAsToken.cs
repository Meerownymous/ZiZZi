using ZiZZi.Matter.Content;
using Newtonsoft.Json.Linq;
using Tonga;
using Tonga.Enumerable;
using Tonga.Swap;
using Tonga.Text;

namespace ZiZZi.Matter.JSON
{
    /// <summary>
    /// Makes JValue content from bnytes to print into json.
    /// </summary>
    public sealed class BytesAsToken : ISwap<string, byte[], JToken>
    {
        private readonly SwapSwitch<byte[], JValue> swap;

        /// <summary>
        /// Makes JValue content from bnytes to print into json.
        /// </summary>
        public BytesAsToken()
        {
            this.swap =
                SwapSwitch._(
                    AsEnumerable._(
                        Case._("double", data => new JValue(AsDouble._(data).Value())),
                        Case._("integer", data => new JValue(AsInteger._(data).Value())),
                        Case._("long", data => new JValue(AsLong._(data).Value())),
                        Case._("bool", data => new JValue(AsBool._(data).Value())),
                        Case._("string", data => new JValue(AsText._(data).AsString()))
                    )
                );
        }

        public JToken Flip(string contentType, byte[] input) => this.swap.Flip(contentType, input);
    }
}

