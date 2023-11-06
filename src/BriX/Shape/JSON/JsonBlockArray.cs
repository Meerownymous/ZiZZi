using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace BLox.Shape.JSON
{
    /// <summary>
    /// Array of blocks which can contain more attributes and sub blocks.
    /// </summary>
    public sealed class JsonBlockArray : IShape<JContainer>
    {
        private readonly Lazy<JArray> container;
        private readonly MediaPipe medias;

        /// <summary>
        /// Array of blocks which can contain more attributes and sub blocks.
        /// </summary>
        public JsonBlockArray(MediaPipe media, JContainer parent, string arrayName)
        {
            this.container = new Lazy<JArray>(() =>
            {
                var content = new JArray();
                parent.Add(new JProperty(arrayName, content));
                return content;
            });
            this.medias = media;
        }

        public JContainer Content()
        {
            return this.container.Value;
        }

        public IShape<JContainer> Open(string contentType, string name)
        {
            var subMedia = this.medias.Flip(contentType, this.container.Value, name);
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

