using ZiZZi.Matter.Content;
using Tonga;
using Tonga.Enumerable;
using Tonga.Swap;
using Tonga.Text;
using System.Diagnostics;

namespace ZiZZi.Matter.Object
{
    /// <summary>
    /// Makes typed content from name and bytes to print into dynamic objects.
    /// </summary>
    public sealed class BytesAsTyped : ISwap<string, string, byte[], object>
    {
        private readonly SwapSwitch<string, byte[], dynamic> swap;

        /// <summary>
        /// Makes typed content from name and bytes to print into dynamic objects.
        /// </summary>
        public BytesAsTyped()
        {
            this.swap =
                SwapSwitch._(
                    AsEnumerable._(
                        Case._("double", (name, data) => AsDouble._(data).Value()),
                        Case._("int32", (name, data) => AsInteger._(data).Value()),
                        Case._("int64", (name, data) => AsLong._(data).Value()),
                        Case._("boolean", (name, data) => AsBool._(data).Value()),
                        Case._("string", (name, data) => AsText._(data).AsString())
                    )
                );
        }

        public dynamic Flip(string contentType, string name, byte[] input) =>
            this.swap.Flip(contentType, name, input);
    }
}

