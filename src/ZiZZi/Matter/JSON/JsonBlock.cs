using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Tonga.Bytes;
using Tonga.IO;

namespace ZiZZi.Matter.JSON
{
    /// <summary>
    /// A json block. It can contain properties, arrays and other blocks.
    /// </summary>
    public class JsonBlock : IMatter<JContainer>
    {
        private readonly MatterOrigin matter;
        private readonly BytesAsToken bytesAsToken;
        private readonly Lazy<JContainer> container;

        public JsonBlock(MatterOrigin matter, BytesAsToken bytesAsToken, JContainer parent, string name)
        {
            this.matter = matter;
            this.bytesAsToken = bytesAsToken;
            this.container =
                new Lazy<JContainer>(() =>
                {
                    var container = new JObject();
                    if (parent.Type == JTokenType.Array)
                    {
                        parent.Add(container);
                    }
                    else if (parent.Type == JTokenType.Object)
                    {
                        parent.Add(new JProperty(name, container));
                    }
                    else
                    {
                        throw new ArgumentException(
                            $"Impossible to open a new block inside a \"{parent.Type}\".@" +
                            " Parent must be array or object."
                        );
                    }
                    return container;
                });
        }

        public JContainer Content()
        {
            return this.container.Value;
        }

        public IMatter<JContainer> Open(string contentType, string name)
        {
            var subMedia = this.matter.Flip(contentType, this.container.Value, name);
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
                        this.bytesAsToken.Flip(dataType, content)
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

