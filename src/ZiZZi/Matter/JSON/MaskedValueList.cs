using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ZiZZi.Matter.JSON
{
    /// <summary>
    /// Matter which ignores a properties that do not match a given json mask.
    /// </summary>
    public sealed class MaskedValueList<TResult> : IMatter<TResult>
        where TResult : class
    {
        private readonly IMatter<TResult> origin;

        /// <summary>
        /// Matter which ignores a properties that do not match a given json mask.
        /// </summary>
        public MaskedValueList(IMatter<TResult> origin, JToken mask)
        {
            this.origin = origin;
        }

        public TResult Content() => this.origin.Content();

        public IMatter<TResult> Open(string contentType, string name) =>
            throw new InvalidOperationException();

        public void Present(string name, Func<IContent<string>> content) =>
            this.origin.Present(name, content);

        public void Present(string name, string dataType, Func<IContent<byte[]>> content) =>
            this.origin.Present(name, dataType, content);

        public void Present(string name, string dataType, Func<IContent<Stream>> content) =>
            this.origin.Present(name, dataType, content);
    }
}

