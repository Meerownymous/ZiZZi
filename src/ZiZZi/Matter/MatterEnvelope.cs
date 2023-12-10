using System;
using System.IO;

namespace ZiZZi.Matter
{
    /// <summary>
    /// Envelope for matters.
    /// </summary>
    public abstract class MatterEnvelope<T> : IMatter<T>
    {
        private readonly Lazy<IMatter<T>> origin;

        /// <summary>
        /// Envelope for matters.
        /// </summary>
        public MatterEnvelope(Func<IMatter<T>> origin)
        {
            this.origin = new Lazy<IMatter<T>>(origin);
        }

        public T Content()
        {
            return this.origin.Value.Content();
        }

        public IMatter<T> Open(string contentType, string name)
        {
            return this.origin.Value.Open(contentType, name);
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            this.origin.Value.Present(name, content);
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            this.origin.Value.Present(name, dataType, content);
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            this.origin.Value.Present(name, dataType, content);
        }
    }
}

