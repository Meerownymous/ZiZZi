using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Tonga.Bytes;
using Tonga.IO;

namespace BLox.Shape.JSON
{
    /// <summary>
    /// A json block. It can contain properties, arrays and other blocks.
    /// </summary>
    public class JsonBlock : IShape<JContainer>
    {
        private readonly MediaPipe medias;
        private readonly BytePipe content;
        private readonly Lazy<JObject> container;

        public JsonBlock(MediaPipe medias, BytePipe content, JContainer parent, string name)
        {
            this.medias = medias;
            this.content = content;
            this.container = new Lazy<JObject>(() =>
            {
                var container = new JObject();
                parent.Add(new JProperty(name, container));
                return container;
            });
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
            this.container.Value.Add(new JProperty(name, content));
        }

        public void Put(string name, string dataType, byte[] content)
        {
            this.container
                .Value
                .Add(
                    new JProperty(
                        name,
                        this.content.Flip(dataType, content)
                    )
                );
        }

        public void Put(string name, string dataType, Stream content)
        {
            this.Put(
                name, dataType,
                new AsBytes(
                    new AsInput(content)
                ).Bytes()
            );
        }
    }
}

