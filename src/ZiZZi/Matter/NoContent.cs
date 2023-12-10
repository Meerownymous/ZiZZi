using System;
using System.IO;

namespace ZiZZi.Matter
{
    /// <summary>
    /// Matter which rejects content dlivery.
    /// Handy when using a matter for building which does not match
    /// the final TResult type.
    /// </summary>
    public sealed class NoContent<TResult> : IMatter<TResult>
    {
        private readonly IMatter<TResult> origin;

        /// <summary>
        /// Matter which rejects content dlivery.
        /// Handy when using a matter for building which does not match
        /// the final TResult type.
        /// </summary>
        public NoContent(IMatter<TResult> origin)
        {
            this.origin = origin;
        }

        public TResult Content()
        {
            throw new InvalidOperationException();
        }

        public IMatter<TResult> Open(string contentType, string name)
        {
            return new NoContent<TResult>(this.origin.Open(contentType, name));
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            this.origin.Present(name, content);
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            this.origin.Present(name, dataType, content);
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            this.origin.Present(name, dataType, content);
        }
    }
}

