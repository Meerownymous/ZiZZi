using System;
using System.IO;
using BLox.Shape.JSON;
using Newtonsoft.Json.Linq;
using Tonga.Enumerable;
using Tonga.Scalar;

namespace BLox.Shape
{
    /// <summary>
    /// A media in JSON format.
    /// </summary>
    public sealed class JsonMedia2 : IShape<JContainer>
    {
        private readonly JContainer basis;
        private readonly MediaPipe medias;

        /// <summary>
        /// A media in JSON format.
        /// </summary>
        public JsonMedia2()
        {
            this.medias = new MediaPipe(new BytePipe());
            this.basis = new JObject();
        }

        public JContainer Content()
        {
            return Root(this.basis);
        }

        public IShape<JContainer> Open(string contentType, string name)
        {
            if (Length._(this.basis.Children()).Value() > 0)
                throw new InvalidOperationException(
                    $"You are trying to open a {contentType} named {name} on "
                    + $"root level of the media, but it already has a root."
                );
            return this.medias.Flip(contentType, this.basis, name);
        }

        public void Put(string name, string content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of media. " +
                $"You must first open a content type."
            );
        }

        public void Put(string name, string dataType, byte[] content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of media. " +
                $"You must first open a content type."
            );
        }

        public void Put(string name, string dataType, Stream content)
        {
            throw new InvalidOperationException(
                "You cannot put content into the root of media. " +
                $"You must first open a content type."
            );
        }

        private static JContainer Root(JContainer basis)
        {
            return
                First._(
                    NotEmpty._(
                        basis.Children()
                    )
                ).Value() as JContainer;
        }
    }
}
