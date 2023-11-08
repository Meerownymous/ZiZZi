using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Tonga;

namespace ZiZZi.Matter.JSON
{
    /// <summary>
    /// Array of blocks which can contain more attributes and sub blocks.
    /// </summary>
    public sealed class JsonBlockArray : IMatter<JContainer>
    {
        private readonly Lazy<JArray> container;
        private readonly ISwap<string, JContainer, string, IMatter<JContainer>> matter;
        private readonly ISwap<string, byte[], JToken> bytesAsToken;

        /// <summary>
        /// Array of blocks which can contain more attributes and sub blocks.
        /// </summary>
        public JsonBlockArray(
            ISwap<string, JContainer, string, IMatter<JContainer>> matter,
            ISwap<string, byte[], JToken> bytesAsToken,
            JContainer parent,
            string arrayName
        )
        {
            this.container = new Lazy<JArray>(() =>
            {
                var content = new JArray();
                parent.Add(new JProperty(arrayName, content));
                return content;
            });
            this.matter = matter;
        }

        public JContainer Content()
        {
            return this.container.Value;
        }

        /// <summary>
        /// Opens a block inside the array. Block name must always be the same.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMatter<JContainer> Open(string contentType, string name)
        {
            var subMedia = this.matter.Flip(contentType, this.container.Value, name);
            subMedia.Content();
            return subMedia;
        }

        public void Put(string name, string content)
        {
            throw new InvalidOperationException();
        }

        public void Put(string name, string dataType, byte[] content)
        {
            throw new InvalidOperationException();
        }

        public void Put(string name, string dataType, Stream content)
        {
            throw new InvalidOperationException();
        }
    }
}

