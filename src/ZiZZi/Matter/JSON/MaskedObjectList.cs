using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ZiZZi.Matter.JSON
{
    /// <summary>
    /// Matter which ignores a properties that do not match a given json mask.
    /// </summary>
    public sealed class MaskedObjectList<TResult> : IMatter<TResult>
        where TResult : class
    {
        private readonly IMatter<TResult> origin;
        private readonly JToken mask;

        /// <summary>
        /// Matter which ignores a properties that do not match a given json mask.
        /// </summary>
        public MaskedObjectList(IMatter<TResult> origin, JToken mask)
        {
            this.origin = origin;
            this.mask = mask;
        }

        public TResult Content() => this.origin.Content();

        public IMatter<TResult> Open(string contentType, string name) =>
            new MaskedBlock<TResult>(
                this.origin.Open(contentType, name), (this.mask as JArray)[0]
            );

        public void Present(string name, Func<IContent<string>> content) =>
            throw new InvalidOperationException();

        public void Present(string name, string dataType, Func<IContent<byte[]>> content) =>
            throw new InvalidOperationException();

        public void Present(string name, string dataType, Func<IContent<Stream>> content) =>
            throw new InvalidOperationException();
    }
}

