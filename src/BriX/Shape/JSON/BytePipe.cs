using System;
using BLox.Shape.Content;
using Newtonsoft.Json.Linq;
using Tonga;
using Tonga.Enumerable;
using Tonga.Map;
using Tonga.Swap;
using Tonga.Text;

namespace BLox.Shape.JSON
{
    /// <summary>
    /// Makes JValue content from bnytes to print into json.
    /// </summary>
    public sealed class BytePipe : ISwap<string, byte[], JToken>
    {
        private readonly SwapSwitch<byte[], JValue> swap;

        /// <summary>
        /// Makes JValue content from bnytes to print into json.
        /// </summary>
        public BytePipe()
        {
            this.swap =
                SwapSwitch._(
                    AsEnumerable._(
                        Case._("double", data => new JValue(AsDouble._(data).Value())),
                        Case._("integer", data => new JValue(AsInteger._(data).Value())),
                        Case._("bool", data => new JValue(AsBool._(data).Value())),
                        Case._("string", data => new JValue(AsText._(data).ToString()))
                    )
                );
        }

        public JToken Flip(string contentType, byte[] input) => this.swap.Flip(contentType, input);
    }
}

