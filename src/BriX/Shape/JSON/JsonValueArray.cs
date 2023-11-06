using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Tonga;
using Tonga.Bytes;
using Tonga.IO;

namespace BLox.Shape.JSON
{
    /// <summary>
    /// Simple value array of items.
    /// </summary>
    public class JsonValueArray : IShape<JContainer>
    {
        private readonly Lazy<JArray> content;
        private readonly ISwap<string, byte[], JToken> pipe;

        /// <summary>
        /// Simple value array of items.
        /// </summary>
        public JsonValueArray(JContainer parent, ISwap<string, byte[], JToken> pipe, string arrayName)
        {
            this.content = new Lazy<JArray>(() =>
            {
                var content = new JArray();
                parent.Add(new JProperty(arrayName, content));
                return content;
            });
            this.pipe = pipe;
        }

        public JContainer Content()
        {
            return this.content.Value;
        }

        public IShape<JContainer> Open(string contentType, string name)
        {
            return new Dead<JContainer>();
        }

        public void Put(string name, string content)
        {
            this.content.Value.Add(content);
        }

        public void Put(string name, string dataType, byte[] content)
        {
            this.content
                .Value
                .Add(
                    this.pipe.Flip(dataType, content)
                );
        }

        public void Put(string name, string dataType, Stream content)
        {
            this.Put(name, dataType,
                new AsBytes(
                    new AsInput(content)
                ).Bytes()
            );
        }
    }
}

