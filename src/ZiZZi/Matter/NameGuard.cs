using System;
using System.IO;
using Tonga.Scalar;

namespace ZiZZi.Matter
{
    /// <summary>
    /// When opening a new matter that has no name, that matter's name will be the given one.
    /// </summary>
    public sealed class NameGuard<T> : IMatter<T>
    {
        private readonly string allowedName;
        private readonly IMatter<T> origin;

        /// <summary>
        /// When opening a new matter that has no name, that matter's name will be the given one.
        /// </summary>
        public NameGuard(string allowedName, IMatter<T> origin)
        {
            this.allowedName = allowedName;
            this.origin = origin;
        }

        public T Content()
        {
            return this.origin.Content();
        }

        public IMatter<T> Open(string contentType, string name)
        {
            if (name != this.allowedName && name != String.Empty)
                throw new ArgumentException($"The name of the {contentType} must either be empty or \"{this.allowedName}\"");
            return
                this.origin.Open(contentType, this.allowedName);
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

