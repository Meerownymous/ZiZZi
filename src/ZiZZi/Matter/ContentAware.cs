using System;
using System.IO;

namespace ZiZZi.Matter
{
    /// <summary>
    /// Matter which looks at presentet content and takes it only
    /// if it is allowed to.
    /// </summary>
    public sealed class ContentAware<T> : IMatter<T>
    {
        private readonly IMatter<T> origin;

        /// <summary>
        /// Matter which looks at presentet content and takes it only
        /// if it is allowed to.
        /// </summary>
        public ContentAware(IMatter<T> origin)
        {
            this.origin = origin;
        }

        public T Content()
        {
            return this.origin.Content();
        }

        public IMatter<T> Open(string contentType, string name)
        {
            return new ContentAware<T>(this.origin.Open(contentType, name));
        }

        public void Present(string name, Func<IContent<string>> content)
        {
            var unwrapped = content();
            if(unwrapped.CanTake())
            {
                this.origin.Present(name, () => unwrapped);
            }
        }

        public void Present(string name, string dataType, Func<IContent<byte[]>> content)
        {
            var unwrapped = content();
            if (unwrapped.CanTake())
            {
                this.origin.Present(name, dataType, () => unwrapped);
            }
        }

        public void Present(string name, string dataType, Func<IContent<Stream>> content)
        {
            var unwrapped = content();
            if (unwrapped.CanTake())
            {
                this.origin.Present(name, dataType, () => unwrapped);
            }
        }
    }
}

