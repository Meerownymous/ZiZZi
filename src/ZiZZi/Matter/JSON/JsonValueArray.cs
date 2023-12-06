using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Tonga;
using Tonga.Bytes;
using Tonga.IO;

namespace ZiZZi.Matter.JSON
{
    /// <summary>
    /// Simple value array of items.
    /// </summary>
    public class JsonValueArray : IMatter<JContainer>
    {
        private readonly Lazy<JArray> content;
        private readonly ISwap<string, byte[], JToken> pipe;

        /// <summary>
        /// Simple value array of items.
        /// </summary>
        public JsonValueArray(ISwap<string, byte[], JToken> bytesAsToken, JContainer parent, string arrayName)
        {
            this.content = new Lazy<JArray>(() =>
            {
                var content = new JArray();
                parent.Add(new JProperty(arrayName, content));
                return content;
            });
            this.pipe = bytesAsToken;
        }

        public JContainer Content()
        {
            return this.content.Value;
        }

        public IMatter<JContainer> Open(string contentType, string name)
        {
            return new Dead<JContainer>();
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            this.content.Value.Add(content().Value());
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            this.content
                .Value
                .Add(
                    this.pipe.Flip(dataType, content().Value())
                );
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            this.content
                .Value
                .Add(
                    this.pipe
                        .Flip(
                            dataType,
                            new AsBytes(
                                new AsInput(content().Value())
                        ).Bytes()
                    )
                );
        }
    }
}

